//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;

using Size = System.Drawing.Size;
using Point = System.Drawing.Point;
using Color = System.Drawing.Color;


namespace Emgu.CV.Models
{
    public class ShapeDetector : DisposableObject, IProcessAndRenderModel
    {

        /// <summary>
        /// The rendering method
        /// </summary>
        public RenderType RenderMethod
        {
            get
            {
                return RenderType.Overwrite;
            }
        }

        private Mat _gray = new Mat();
        private Mat _cannyEdges = new Mat();

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_gray != null)
            {
                _gray.Dispose();
                _gray = null;
            }

            if (_cannyEdges != null)
            {
                _cannyEdges.Dispose();
                _cannyEdges = null;
            }
        }

        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// Run the detection pipeline (grayscale → blur → Canny → Hough → contours) and
        /// return the detected geometry. Shared by <see cref="ProcessAndRender"/> and
        /// <see cref="Detect"/> so the OpenCV pipeline lives in one place.
        /// </summary>
        private void ComputeShapes(
            IInputArray imageIn,
            out CircleF[] circles,
            out LineSegment2D[] lines,
            out List<Triangle2DF> triangleList,
            out List<RotatedRect> boxList)
        {
            #region Pre-processing
            //Convert the image to grayscale and filter out the noise
            CvInvoke.CvtColor(imageIn, _gray, ColorConversion.Bgr2Gray);
            //Remove noise
            CvInvoke.GaussianBlur(_gray, _gray, new Size(3, 3), 1);
            double cannyThreshold = 180.0;
            double cannyThresholdLinking = 120.0;
            CvInvoke.Canny(_gray, _cannyEdges, cannyThreshold, cannyThresholdLinking);
            #endregion

            #region circle detection
            double circleAccumulatorThreshold = 120;
            circles = CvInvoke.HoughCircles(_gray, HoughModes.Gradient, 2.0, 20.0, cannyThreshold,
                circleAccumulatorThreshold, 5);
            #endregion

            #region Edge detection
            // Use the IOutputArray overload + manual parsing instead of the
            // LineSegment2D[] overload: that overload pins a managed array and
            // hands the pointer to native code, which aborts when run off the UI
            // thread and is unsafe when zero lines are found.
            using (Mat lineMat = new Mat())
            {
                CvInvoke.HoughLinesP(
                    _cannyEdges,
                    lineMat,
                    1, //Distance resolution in pixel-related units
                    Math.PI / 45.0, //Angle resolution measured in radians.
                    20, //threshold
                    30, //min Line width
                    10); //gap between lines
                lines = ParseLineSegments(lineMat);
            }
            #endregion

            #region Find triangles and rectangles
            triangleList = new List<Triangle2DF>();
            boxList = new List<RotatedRect>(); //a box is a rotated rectangle
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(_cannyEdges, contours, null, RetrType.List,
                    ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05,
                            true);
                        if (CvInvoke.ContourArea(approxContour, false) > 250
                        ) //only consider contours with area greater than 250
                        {
                            if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
                            {
                                System.Drawing.Point[] pts = approxContour.ToArray();
                                triangleList.Add(new Triangle2DF(
                                    pts[0],
                                    pts[1],
                                    pts[2]
                                ));
                            }
                            else if (approxContour.Size == 4) //The contour has 4 vertices.
                            {
                                #region determine if all the angles in the contour are within [80, 100] degree

                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(
                                        edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }

                                #endregion

                                if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                            }
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Convert the Nx1 CV_32SC4 output of HoughLinesP into line segments.
        /// Returns an empty array when no lines were found.
        /// </summary>
        private static LineSegment2D[] ParseLineSegments(Mat lineMat)
        {
            if (lineMat == null || lineMat.IsEmpty)
                return Array.Empty<LineSegment2D>();

            int count = lineMat.Rows * lineMat.Cols;
            if (count == 0)
                return Array.Empty<LineSegment2D>();

            int[] data = new int[count * 4];
            Marshal.Copy(lineMat.DataPointer, data, 0, data.Length);

            LineSegment2D[] result = new LineSegment2D[count];
            for (int i = 0; i < count; i++)
                result[i] = new LineSegment2D(
                    new Point(data[i * 4], data[i * 4 + 1]),
                    new Point(data[i * 4 + 2], data[i * 4 + 3]));
            return result;
        }

        /// <summary>
        /// Run the pipeline and return each detection stage as its own image
        /// (original, triangles &amp; rectangles, circles, lines) drawn on a clean
        /// white background, suitable for the bespoke Shape Detection page.
        /// </summary>
        public ShapeDetectionResult Detect(IInputArray imageIn)
        {
            Stopwatch watch = Stopwatch.StartNew();
            ComputeShapes(imageIn, out CircleF[] circles, out LineSegment2D[] lines,
                out List<Triangle2DF> triangleList, out List<RotatedRect> boxList);
            watch.Stop();

            Size size = _gray.Size;
            MCvScalar white = new MCvScalar(255, 255, 255);

            Mat original = new Mat();
            using (InputArray iaImageIn = imageIn.GetInputArray())
                iaImageIn.CopyTo(original);

            Mat trianglesRectangles = new Mat(size, DepthType.Cv8U, 3);
            trianglesRectangles.SetTo(white);
            foreach (Triangle2DF triangle in triangleList)
                CvInvoke.Polylines(trianglesRectangles,
                    Array.ConvertAll(triangle.GetVertices(), Point.Round),
                    true, new Bgr(Color.OrangeRed).MCvScalar, 3);
            foreach (RotatedRect box in boxList)
                CvInvoke.Polylines(trianglesRectangles, Array.ConvertAll(box.GetVertices(), Point.Round),
                    true, new Bgr(Color.RoyalBlue).MCvScalar, 3);

            Mat circleImage = new Mat(size, DepthType.Cv8U, 3);
            circleImage.SetTo(white);
            Color[] circleColors = { Color.SeaGreen, Color.MediumPurple, Color.Goldenrod };
            for (int i = 0; i < circles.Length; i++)
                CvInvoke.Circle(circleImage, Point.Round(circles[i].Center), (int)circles[i].Radius,
                    new Bgr(circleColors[i % circleColors.Length]).MCvScalar, 3);

            Mat lineImage = new Mat(size, DepthType.Cv8U, 3);
            lineImage.SetTo(white);
            foreach (LineSegment2D line in lines)
                CvInvoke.Line(lineImage, line.P1, line.P2, new Bgr(Color.Black).MCvScalar, 3);

            return new ShapeDetectionResult(
                original, trianglesRectangles, circleImage, lineImage,
                triangleList.Count, boxList.Count, circles.Length, lines.Length,
                watch.ElapsedMilliseconds);
        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();

            ComputeShapes(imageIn, out CircleF[] circles, out LineSegment2D[] lines,
                out List<Triangle2DF> triangleList, out List<RotatedRect> boxList);

            watch.Stop();

            using (Mat triangleRectangleImage = new Mat(_gray.Size, DepthType.Cv8U, 3)) //image to draw triangles and rectangles on
            using (Mat circleImage = new Mat(_gray.Size, DepthType.Cv8U, 3)) //image to draw circles on
            using (Mat lineImage = new Mat(_gray.Size, DepthType.Cv8U, 3)) //image to draw lines on
            {
                #region draw triangles and rectangles

                triangleRectangleImage.SetTo(new MCvScalar(0));
                foreach (Triangle2DF triangle in triangleList)
                {
                    CvInvoke.Polylines(triangleRectangleImage,
                        Array.ConvertAll(triangle.GetVertices(), Point.Round),
                        true, new Bgr(Color.DarkBlue).MCvScalar, 2);
                }

                foreach (RotatedRect box in boxList)
                {
                    CvInvoke.Polylines(triangleRectangleImage, Array.ConvertAll(box.GetVertices(), Point.Round),
                        true,
                        new Bgr(Color.DarkOrange).MCvScalar, 2);
                }

                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(triangleRectangleImage,
                    new Rectangle(Point.Empty,
                        new Size(triangleRectangleImage.Width - 1, triangleRectangleImage.Height - 1)),
                    new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(triangleRectangleImage, "Triangles and Rectangles", new Point(20, 20),
                    HersheyFonts.Duplex, 0.5, new MCvScalar(120, 120, 120));

                #endregion

                #region draw circles

                circleImage.SetTo(new MCvScalar(0));
                foreach (CircleF circle in circles)
                    CvInvoke.Circle(circleImage, Point.Round(circle.Center), (int)circle.Radius,
                        new Bgr(Color.Brown).MCvScalar, 2);

                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(circleImage,
                    new Rectangle(Point.Empty, new Size(circleImage.Width - 1, circleImage.Height - 1)),
                    new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(circleImage, "Circles", new Point(20, 20), HersheyFonts.Duplex, 0.5,
                    new MCvScalar(120, 120, 120));

                #endregion

                #region draw lines

                lineImage.SetTo(new MCvScalar(0));
                foreach (LineSegment2D line in lines)
                    CvInvoke.Line(lineImage, line.P1, line.P2, new Bgr(Color.Green).MCvScalar, 2);
                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(lineImage,
                    new Rectangle(Point.Empty, new Size(lineImage.Width - 1, lineImage.Height - 1)),
                    new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(lineImage, "Lines", new Point(20, 20), HersheyFonts.Duplex, 0.5,
                    new MCvScalar(120, 120, 120));

                #endregion


                using (InputArray iaImageIn = imageIn.GetInputArray())
                using (Mat imageInMat = iaImageIn.GetMat())
                    CvInvoke.VConcat(new Mat[] { imageInMat, triangleRectangleImage, circleImage, lineImage }, imageOut);

            }
            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);

        }

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                return true;
            }
        }

        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
        {
            //do nothing, no need to initialize
            await Task.Delay(0);
        }


    }

    /// <summary>
    /// The result of <see cref="ShapeDetector.Detect"/>: each detection stage as its
    /// own image plus the per-shape counts and timing. Owns the four images and
    /// disposes them when disposed.
    /// </summary>
    public sealed class ShapeDetectionResult : DisposableObject
    {
        /// <summary>The unmodified input image.</summary>
        public Mat Original { get; private set; }
        /// <summary>Detected triangle and rectangle outlines on a white background.</summary>
        public Mat TrianglesRectangles { get; private set; }
        /// <summary>Detected circle outlines on a white background.</summary>
        public Mat Circles { get; private set; }
        /// <summary>Detected line segments on a white background.</summary>
        public Mat Lines { get; private set; }

        /// <summary>Number of triangles detected.</summary>
        public int TriangleCount { get; }
        /// <summary>Number of rectangles detected.</summary>
        public int RectangleCount { get; }
        /// <summary>Number of circles detected.</summary>
        public int CircleCount { get; }
        /// <summary>Number of line segments detected.</summary>
        public int LineCount { get; }
        /// <summary>Time spent running the detection pipeline, in milliseconds.</summary>
        public long ElapsedMilliseconds { get; }

        internal ShapeDetectionResult(
            Mat original, Mat trianglesRectangles, Mat circles, Mat lines,
            int triangleCount, int rectangleCount, int circleCount, int lineCount,
            long elapsedMilliseconds)
        {
            Original = original;
            TrianglesRectangles = trianglesRectangles;
            Circles = circles;
            Lines = lines;
            TriangleCount = triangleCount;
            RectangleCount = rectangleCount;
            CircleCount = circleCount;
            LineCount = lineCount;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        protected override void DisposeObject()
        {
            Original?.Dispose(); Original = null;
            TrianglesRectangles?.Dispose(); TrianglesRectangles = null;
            Circles?.Dispose(); Circles = null;
            Lines?.Dispose(); Lines = null;
        }
    }
}
