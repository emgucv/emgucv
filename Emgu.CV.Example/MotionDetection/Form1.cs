using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using System.Threading;

namespace MotionDetection
{
    public partial class Form1 : Form
    {
        private Capture _capture;
        private Thread _captureThread;
        private MotionHistory _motionHistory;

        public Form1()
        {
            InitializeComponent();
            
            //try to create the capture
            if (_capture == null)
            {
                try
                {
                    _capture = new Capture();
                }
                catch (Emgu.PrioritizedException excpt)
                {   //show errors if there is any
                    excpt.Alert(false);
                }
            }

            if (_capture != null)
            {   //if camera capture successfully created
                _motionHistory = new MotionHistory(
                    6, //number of images to store in buffer, adjust it to fit your camera's frame rate
                    20, //0-255, the amount of pixel intensity change to consider it as motion pixel
                    1.0, //in second, the duration of motion history you wants to keep
                    0.05, //in second, parameter for cvCalcMotionGradient
                    0.5); //in second, parameter for cvCalcMotionGradient

                _captureThread = new Thread(ProcessImage);
                _captureThread.Start();
            }
        }

        public void ProcessImage()
        {
            while (true)
            {
                Image<Bgr, Byte> image = _capture.QuerySmallFrame().PyrUp(); //noise reduced image
                capturedImageBox.Image = image;

                //update the motion history
                _motionHistory.Update(image.Convert<Gray, Byte>());

                #region get a copy of the motion mask and enhance its color 
                Image<Gray, Byte> motionMask = _motionHistory.Mask;
                double[] minValues, maxValues;
                MCvPoint[] minLoc, maxLoc;
                motionMask.MinMax(out minValues, out maxValues, out minLoc, out maxLoc);
                motionMask = motionMask * (255.0 / maxValues[0]);
                #endregion 

                //create the motion image 
                Image<Bgr, Byte> motionImage = new Image<Bgr, byte>(motionMask.Width, motionMask.Height);
                //display the motion pixels in blue
                CvInvoke.cvCvtPlaneToPix(motionMask, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, motionImage);

                //Threshold to define a motion area, reduce it to detect smaller motion
                double minArea = 100;

                Seq<MCvConnectedComp> motionComponents = _motionHistory.MotionComponents;

                //iterate through each of the motion component
                foreach (MCvConnectedComp comp in motionComponents)
                {
                    //reject the components that have small area;
                    if (comp.rect.Area < minArea) continue;

                    // find the angle and motion pixel count of the specific area
                    double angle, motionPixelCount;
                    _motionHistory.MotionInfo(comp.rect, out angle, out motionPixelCount);

                    //reject the area that contains too few motion
                    if (motionPixelCount / comp.area < 0.05) continue;

                    //Draw each individual motion in red
                    DrawMotion(motionImage, comp.rect, angle, new Bgr(0, 0, 255.0));
                }

                
                // find and draw the overall motion angle
                double overallAngle, overallMotionPixelCount;
                _motionHistory.MotionInfo(motionMask.ROI.MCvRect, out overallAngle, out overallMotionPixelCount);
                //DrawMotion(motionImage, motionMask.ROI.MCvRect, overallAngle, new Bgr(0, 255, 0));
                

                //Display the amount of motions found on the current image
                UpdateText(String.Format("Total Motions found: {0}; Motion Pixel count: {1}", motionComponents.Total, overallMotionPixelCount));
                
                motionImageBox.Image = motionImage;

                //The following is optional, it force a garbage collection and free unused memory
                //If removed the program consumes more memory but runs slightly faster
                GC.Collect();
            }
        }

        private void UpdateText(String text)
        {
            if (InvokeRequired)
            {
                this.Invoke( new Action<String>(UpdateText), new object[] { text });
            }
            else
            {
                label3.Text = text;
            }
        }

        private static void DrawMotion(Image<Bgr, Byte> image, MCvRect motionArea, double angle, Bgr color)
        {
            int circleRadius = (int) (motionArea.width + motionArea.height) / 4;

            Rectangle<int> rect = new Rectangle<int>(motionArea);
            Circle<int> circle = new Circle<int>(rect.Center, circleRadius);

            int xDirection = (int)(Math.Cos(angle * Math.PI / 180.0) * circleRadius);
            int yDirection = (int)(Math.Sin(angle * Math.PI / 180.0) * circleRadius);
            Point2D<int> pointOnCircle = new Point2D<int>(
                circle.Center.X + xDirection,
                circle.Center.Y + yDirection);
            LineSegment2D<int> line = new LineSegment2D<int>(circle.Center, pointOnCircle);

            image.Draw(circle, color, 1);
            image.Draw(line, color, 2);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (_captureThread != null) _captureThread.Abort();

            base.Dispose(disposing);
        }
    }
}