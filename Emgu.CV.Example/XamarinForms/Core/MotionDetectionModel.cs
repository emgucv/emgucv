using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.Util;

namespace Emgu.CV.XamarinForms
{
    public class MotionDetectionModel : IProcessAndRenderModel
    {
        private MotionHistory _motionHistory;
        private IBackgroundSubtractor _forgroundDetector;

        private Mat _segMask = new Mat();
        private Mat _forgroundMask = new Mat();

        public void Dispose()
        {
            Clear();
        }

        public void Clear()
        {
            _motionHistory.Dispose();
            _motionHistory = null;

            if (_forgroundDetector != null)
            {
                IDisposable _fgDetectorDisposable = _forgroundDetector as IDisposable;
                if (_fgDetectorDisposable != null)
                {
                    _fgDetectorDisposable.Dispose();
                }

                _forgroundDetector = null;
            }
        }

        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged, object initOptions)
        {
            _motionHistory = new MotionHistory(
                1.0, //in second, the duration of motion history you wants to keep
                0.05, //in second, maxDelta for cvCalcMotionGradient
                0.5); //in second, minDelta for cvCalcMotionGradient

            if (_forgroundDetector == null)
            {
                _forgroundDetector = new BackgroundSubtractorMOG2();
            }

            await Task.Delay(1);
        }

        private static void DrawMotion(IInputOutputArray image, Rectangle motionRegion, double angle, Bgr color)
        {
            //CvInvoke.Rectangle(image, motionRegion, new MCvScalar(255, 255, 0));
            float circleRadius = (motionRegion.Width + motionRegion.Height) >> 2;
            Point center = new Point(motionRegion.X + (motionRegion.Width >> 1), motionRegion.Y + (motionRegion.Height >> 1));

            CircleF circle = new CircleF(
                center,
                circleRadius);

            int xDirection = (int)(Math.Cos(angle * (Math.PI / 180.0)) * circleRadius);
            int yDirection = (int)(Math.Sin(angle * (Math.PI / 180.0)) * circleRadius);
            Point pointOnCircle = new Point(
                center.X + xDirection,
                center.Y - yDirection);
            LineSegment2D line = new LineSegment2D(center, pointOnCircle);
            CvInvoke.Circle(image, Point.Round(circle.Center), (int)circle.Radius, color.MCvScalar);
            CvInvoke.Line(image, line.P1, line.P2, color.MCvScalar);

        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            if (_forgroundDetector == null)
            {
                _forgroundDetector = new BackgroundSubtractorMOG2();
            }

            _forgroundDetector.Apply(imageIn, _forgroundMask);

            //update the motion history
            _motionHistory.Update(_forgroundMask);

            #region get a copy of the motion mask and enhance its color
            double[] minValues, maxValues;
            Point[] minLoc, maxLoc;
            _motionHistory.Mask.MinMax(out minValues, out maxValues, out minLoc, out maxLoc);
            Mat motionMask = new Mat();
            using (ScalarArray sa = new ScalarArray(255.0 / maxValues[0]))
                CvInvoke.Multiply(_motionHistory.Mask, sa, motionMask, 1, DepthType.Cv8U);
            //Image<Gray, Byte> motionMask = _motionHistory.Mask.Mul(255.0 / maxValues[0]);
            #endregion

            //create the motion image 
            Mat motionImage = new Mat(motionMask.Size.Height, motionMask.Size.Width, DepthType.Cv8U, 3);
            motionImage.SetTo(new MCvScalar(0));
            //display the motion pixels in blue (first channel)
            //motionImage[0] = motionMask;
            CvInvoke.InsertChannel(motionMask, motionImage, 0);

            //Threshold to define a motion area, reduce the value to detect smaller motion
            double minArea = 100;

            Rectangle[] rects = _motionHistory.GetMotionComponents(_segMask);

            //iterate through each of the motion component
            foreach (Rectangle comp in rects)
            {
                int area = comp.Width * comp.Height;
                //reject the components that have small area;
                if (area < minArea) continue;

                // find the angle and motion pixel count of the specific area
                double angle, motionPixelCount;
                _motionHistory.MotionInfo(_forgroundMask, comp, out angle, out motionPixelCount);

                //reject the area that contains too few motion
                if (motionPixelCount < area * 0.05) continue;

                //Draw each individual motion in red
                DrawMotion(motionImage, comp, angle, new Bgr(Color.Red));
            }

            // find and draw the overall motion angle
            double overallAngle, overallMotionPixelCount;

            _motionHistory.MotionInfo(_forgroundMask, new Rectangle(Point.Empty, motionMask.Size), out overallAngle, out overallMotionPixelCount);
            DrawMotion(motionImage, new Rectangle(Point.Empty, motionMask.Size), overallAngle, new Bgr(Color.Green));
            using (InputArray iaImageIn = imageIn.GetInputArray())
            using (Mat m = iaImageIn.GetMat())
            using (Mat forgroundMaskBgr = new Mat())
            {
                CvInvoke.CvtColor(_forgroundMask, forgroundMaskBgr, ColorConversion.Gray2Bgr);
                CvInvoke.VConcat(new Mat[] { m, forgroundMaskBgr, motionImage }, imageOut);
            }

            //Display the amount of motions found on the current image
            return String.Format("Total Motions found: {0}; Motion Pixel count: {1}", rects.Length, overallMotionPixelCount);

        }
    }
}
