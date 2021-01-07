using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.XImgproc;

namespace Emgu.CV.XamarinForms
{
    public static class Simple3DReconstruct
    {
        private static MCvPoint3D32f[] GetDisparityPoints(Mat disparityMap)
        {
            MCvPoint3D32f[] pointArrayF = new MCvPoint3D32f[disparityMap.Rows * disparityMap.Cols];
            GCHandle colorHandle = GCHandle.Alloc(pointArrayF, GCHandleType.Pinned);
            using (Mat pointArray = disparityMap.Reshape(disparityMap.NumberOfChannels, disparityMap.Rows * disparityMap.Cols))
            using (Mat pointArrayMat = new Mat(pointArrayF.Length, 1, DepthType.Cv32F, 3, colorHandle.AddrOfPinnedObject(), 3 * 4))
            {
                pointArray.CopyTo(pointArrayMat);
            }
            colorHandle.Free();

            return pointArrayF;
        }
        private static MCvPoint3D32f[] GetBGRColorArray(Mat bgrImg)
        {
            MCvPoint3D32f[] colorArrayF = new MCvPoint3D32f[bgrImg.Rows * bgrImg.Cols];

            GCHandle colorHandle = GCHandle.Alloc(colorArrayF, GCHandleType.Pinned);
            using (Mat colorArray = bgrImg.Reshape(bgrImg.NumberOfChannels, bgrImg.Rows * bgrImg.Cols))
            using (Mat colorArrayFloat = new Mat(colorArrayF.Length, 1, DepthType.Cv32F, 3, colorHandle.AddrOfPinnedObject(), 3 * 4))
            {
                colorArray.ConvertTo(colorArrayFloat, DepthType.Cv32F);
            }
            colorHandle.Free();
            return colorArrayF;
        }

        public static void GetPointAndColor(Mat left, Mat right, Mat pointMat, Mat colorMat)
        {
            Mat disparityMap = new Mat();

            Stopwatch watch = Stopwatch.StartNew();
            UMat leftGray = new UMat();
            UMat rightGray = new UMat();
            CvInvoke.CvtColor(left, leftGray, ColorConversion.Bgr2Gray);
            CvInvoke.CvtColor(right, rightGray, ColorConversion.Bgr2Gray);
            Mat points = new Mat();
            Computer3DPointsFromStereoPair(leftGray, rightGray, disparityMap, points);
            watch.Stop();
            //long disparityComputationTime = watch.ElapsedMilliseconds;

            MCvPoint3D32f[] pointArray = GetDisparityPoints(points);
            MCvPoint3D32f[] colorArrayF = GetBGRColorArray(left);

            List<MCvPoint3D32f> goodPoints = new List<MCvPoint3D32f>();
            List<MCvPoint3D32f> goodColors = new List<MCvPoint3D32f>();
            for (int i = 0; i < pointArray.Length; i++)
            {
                if (pointArray[i].Z < 10)
                {
                    goodPoints.Add(pointArray[i]);
                    goodColors.Add(colorArrayF[i]);
                }
            }

            MCvPoint3D32f[] goodPointArray = goodPoints.ToArray();
            MCvPoint3D32f[] goodColorArray = goodColors.ToArray();

            GCHandle goodPointHandle = GCHandle.Alloc(goodPointArray, GCHandleType.Pinned);
            GCHandle goodColorHandle = GCHandle.Alloc(goodColorArray, GCHandleType.Pinned);

            using (Mat mPoints = new Mat(goodPointArray.Length, 1, DepthType.Cv32F, 3,
                goodPointHandle.AddrOfPinnedObject(), 3 * 4))
            using (Mat mColors = new Mat(goodColorArray.Length, 1, DepthType.Cv32F, 3,
                goodColorHandle.AddrOfPinnedObject(), 3 * 4))
            {
                mPoints.CopyTo(pointMat);
                mColors.ConvertTo(colorMat, DepthType.Cv8U);
            }
            goodPointHandle.Free();
            goodColorHandle.Free();

        }

        public static Viz3d GetViz3d(Mat pointMat, Mat colorMat)
        {
            WCloud cloud = new WCloud(pointMat, colorMat);

            Emgu.CV.Viz3d v = new Emgu.CV.Viz3d("Simple stereo reconstruction");
            WText wtext = new WText("3d point cloud", new System.Drawing.Point(20, 20), 20,
                new MCvScalar(255, 255, 255));
            WCoordinateSystem wCoordinate = new WCoordinateSystem(1.0);
            v.ShowWidget("text", wtext);
            //v.ShowWidget("coordinate", wCoordinate);
            v.ShowWidget("cloud", cloud);
            return v;

        }

        /// <summary>
        /// Given the left and right image, computer the disparity map and the 3D point cloud.
        /// </summary>
        /// <param name="left">The left image</param>
        /// <param name="right">The right image</param>
        /// <param name="outputDisparityMap">The left disparity map</param>
        /// <param name="points">The 3D point cloud within a [-0.5, 0.5] cube</param>
        private static void Computer3DPointsFromStereoPair(IInputArray left, IInputArray right, Mat outputDisparityMap, Mat points, bool handleMissingValues = true)
        {
            System.Drawing.Size size;
            using (InputArray ia = left.GetInputArray())
                size = ia.GetSize();

            using (StereoBM leftMatcher = new StereoBM())
            using (RightMatcher rightMatcher = new RightMatcher(leftMatcher))
            using (Mat leftDisparity = new Mat())
            using (Mat rightDisparity = new Mat())
            using (DisparityWLSFilter wls = new DisparityWLSFilter(leftMatcher))
            {
                leftMatcher.Compute(left, right, leftDisparity);
                rightMatcher.Compute(right, left, rightDisparity);
                wls.Filter(leftDisparity, left, outputDisparityMap, rightDisparity, rightView: right);
                float scale = Math.Max(size.Width, size.Height);

                //Construct a simple Q matrix, if you have a matrix from cvStereoRectify, you should use that instead
                using (Matrix<double> q = new Matrix<double>(
                    new double[,]
                    {
                        {1.0, 0.0, 0.0, -size.Width/2}, //shift the x origin to image center
                        {0.0, -1.0, 0.0, size.Height/2}, //shift the y origin to image center and flip it upside down
                        {0.0, 0.0, -1.0, 0.0}, //Multiply the z value by -1.0, 
                        {0.0, 0.0, 0.0, scale}
                    })) //scale the object's coordinate to within a [-0.5, 0.5] cube
                {

                    CvInvoke.ReprojectImageTo3D(outputDisparityMap, points, q, handleMissingValues, DepthType.Cv32F);
                    //CvInvoke.ReprojectImageTo3D(leftDisparity, points, q, false, DepthType.Cv32F);
                    //CvInvoke.ReprojectImageTo3D(leftDisparity, points, q, handleMissingValues, DepthType.Cv32F);
                }
                //points = PointCollection.ReprojectImageTo3D(outputDisparityMap, q);
            }
        }
    }
}
