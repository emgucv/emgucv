using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using System.Threading;
using System.Runtime.InteropServices;

namespace FacialMouseControl
{
    public partial class Form1 : Form
    {
        private Capture _capture;

        private Thread _captureThread;

        //private bool _flipHorizontal;
        private HaarCascade _face;

        public Form1()
        {
            InitializeComponent();

            //Read the HaarCascade object
            _face = new HaarCascade("haarcascades/haarcascade_frontalface_alt2.xml");

            if (_capture == null)
            {
                try
                {
                    _capture = new Capture();
                }
                catch (Emgu.PrioritizedException excpt)
                {
                    excpt.Alert(true);
                    return;
                }
            }
            _captureThread = new Thread(
                delegate()
                {
                    while (true)
                    {
                        Image<Bgr, Byte> frame = _capture.QueryFrame();

                        ProcessImage(frame);
                    }
                }
                );

            _captureThread.Start();
        }

        /// <summary>
        /// Returns true if the point is in the rectangle
        /// </summary>
        /// <param name="point">the point to test</param>
        /// <param name="rect">the rectangle area</param>
        private static bool PointInRectangle(Point2D<double> point, Rectangle<double> rect)
        {
            return
                (point.X > rect.Center.X - rect.Width / 2.0) &&
                (point.X < rect.Center.X + rect.Width / 2.0) &&
                (point.Y > rect.Center.Y - rect.Height / 2.0) &&
                (point.X < rect.Center.Y + rect.Height / 2.0);
        }

        public void ProcessImage(Image<Bgr, Byte> frame)
        {
            Image<Gray, Byte> grayImage = frame.Convert<Gray, Byte>();
            grayImage._EqualizeHist();

            Rectangle<double> imageArea = grayImage.ROI;
            Rectangle<double> mouseStableArea = new Rectangle<double>(imageArea.Center, imageArea.Width * 0.1, imageArea.Height * 0.1);
            
            //draw the stable area where the face will not trigger a movement;
            frame.Draw(mouseStableArea, new Bgr(255, 0,0), 1);

            Rectangle<double>[] faces = grayImage.DetectHaarCascade(_face)[0];
            if (faces.Length > 0)
            {   //if there is at least one face

                #region find the biggest face
                Rectangle<double> biggestFace = faces[0];
                for (int i = 1; i < faces.Length; i++)
                {
                    if (faces[i].Area > biggestFace.Area)
                        biggestFace = faces[i];
                }
                #endregion 

                //draw a yellow rectangle around the face
                frame.Draw(biggestFace, new Bgr(255, 255, 0.0), 1);

                //draw a green cross at the center of the biggest face
                frame.Draw( 
                    new Cross2D<double>(biggestFace.Center, biggestFace.Width * 0.1, biggestFace.Height* 0.1), 
                    new Bgr(0, 255, 0), 1);

                if (! PointInRectangle(biggestFace.Center, mouseStableArea))
                {   //the point is far enough from the center to triger a movement
                    
                    //horizontal fraction is a value in [-0.5, 0.5] where
                    //-0.5 refer to the far left and 
                    //0.5 refer to the far right
                    double horizontalFraction = (biggestFace.Center.X - imageArea.Center.X) / imageArea.Width;
                    //do the same for vertical fraction
                    double verticalFraction = (biggestFace.Center.Y = imageArea.Center.Y) / imageArea.Height;

                    Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                    int maxMouseSpeed = rect.Width / 20 ; 
                    MCvPoint p;
                    GetCursorPos(out p);
                    p.x = Math.Min(Math.Max(0, p.x + (int)(maxMouseSpeed /2.0 * horizontalFraction )), rect.Width);
                    p.y = Math.Min(Math.Max(0, p.x - (int)(maxMouseSpeed /2.0 * verticalFraction)), rect.Height);
                    SetCursorPos(p.x, p.y);
                }
            }

            imageBox1.Image = frame;
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out MCvPoint lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        public void ReleaseData()
        {
            if (_captureThread != null)
                _captureThread.Abort();

            if (_capture != null)
                _capture.Dispose();
        }

        private void flipHorizontalButton_Click(object sender, EventArgs e)
        {
            if (_capture != null) _capture.FlipHorizontal = !_capture.FlipHorizontal;
        }
    }
}