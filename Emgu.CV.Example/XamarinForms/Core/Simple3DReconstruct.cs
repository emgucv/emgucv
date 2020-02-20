using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Emgu.CV.XamarinForms
{
    public static class Simple3DReconstruct
    {
        public static Viz3d GetViz3d(Mat left, Mat right)
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
            long disparityComputationTime = watch.ElapsedMilliseconds;

            Mat pointsArray = points.Reshape(points.NumberOfChannels, points.Rows * points.Cols);
            Mat colorArray = left.Reshape(left.NumberOfChannels, left.Rows * left.Cols);
            Mat colorArrayFloat = new Mat();
            colorArray.ConvertTo(colorArrayFloat, DepthType.Cv32F);
            WCloud cloud = new WCloud(pointsArray, colorArray);

            Emgu.CV.Viz3d v = new Emgu.CV.Viz3d("Simple stereo reconstruction");
            WText wtext = new WText("3d point cloud", new System.Drawing.Point(20, 20), 20, new MCvScalar(255, 255, 255));
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
        private static void Computer3DPointsFromStereoPair(IInputArray left, IInputArray right, Mat outputDisparityMap, Mat points)
        {
            System.Drawing.Size size;
            using (InputArray ia = left.GetInputArray())
                size = ia.GetSize();

            using (StereoBM stereoSolver = new StereoBM())
            {
                stereoSolver.Compute(left, right, outputDisparityMap);

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

                    CvInvoke.ReprojectImageTo3D(outputDisparityMap, points, q, false, DepthType.Cv32F);

                }
                //points = PointCollection.ReprojectImageTo3D(outputDisparityMap, q);
            }
        }
    }
}
