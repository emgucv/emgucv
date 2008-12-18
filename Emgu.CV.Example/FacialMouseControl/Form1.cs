using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using System.Threading;
using System.Runtime.InteropServices;

//define alias
using MCvPoint = System.Drawing.Point;

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
            catch (NullReferenceException excpt)
            {
               MessageBox.Show(excpt.Message);
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
      private static bool PointInRectangle(PointF point, System.Drawing.Rectangle rect)
      {
         return
             (point.X > rect.X) &&
             (point.X < rect.X + rect.Width) &&
             (point.Y > rect.Y) &&
             (point.X < rect.Y + rect.Height);
      }

      public void ProcessImage(Image<Bgr, Byte> frame)
      {
         Image<Gray, Byte> grayImage = frame.Convert<Gray, Byte>();
         grayImage._EqualizeHist();

         System.Drawing.Rectangle imageArea = grayImage.ROI;

         System.Drawing.Rectangle mouseStableArea =
            new System.Drawing.Rectangle((int)(imageArea.Width * 0.4), (int)(imageArea.Height * 0.4), (int)(imageArea.Width * 0.2), (int)(imageArea.Height * 0.2));

         //draw the stable area where the face will not trigger a movement;
         frame.Draw(mouseStableArea, new Bgr(255, 0, 0), 1);

         System.Drawing.Rectangle[] faces = grayImage.DetectHaarCascade(_face)[0];
         if (faces.Length > 0)
         {   //if there is at least one face

            #region find the biggest face
            System.Drawing.Rectangle biggestFace = faces[0];
            for (int i = 1; i < faces.Length; i++)
            {
               if (faces[i].Width * faces[i].Height > biggestFace.Width * biggestFace.Height)
                  biggestFace = faces[i];
            }
            #endregion

            //draw a yellow rectangle around the face
            frame.Draw(biggestFace, new Bgr(255, 255, 0.0), 1);

            PointF biggestFaceCenter = new PointF(biggestFace.X + biggestFace.Width / 2.0f, biggestFace.Y + biggestFace.Height / 2.0f);
            PointF imageAreaCenter = new PointF(imageArea.X + imageArea.Width / 2.0f, imageArea.Y + imageArea.Height / 2.0f);
            //draw a green cross at the center of the biggest face
            frame.Draw(
                new Cross2DF(biggestFaceCenter, biggestFace.Width * 0.1f, biggestFace.Height * 0.1f),
                new Bgr(0, 255, 0), 1);

            if (!PointInRectangle(biggestFaceCenter, mouseStableArea))
            {   //the point is far enough from the center to triger a movement

               //horizontal fraction is a value in [-0.5, 0.5] where
               //-0.5 refer to the far left and 
               //0.5 refer to the far right
               double horizontalFraction = (biggestFaceCenter.X - imageAreaCenter.X) / imageArea.Width;
               //do the same for vertical fraction
               double verticalFraction = (biggestFaceCenter.Y = imageAreaCenter.Y) / imageArea.Height;

               Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
               int maxMouseSpeed = rect.Width / 20;
               MCvPoint p;
               GetCursorPos(out p);
               p.X = Math.Min(Math.Max(0, p.X + (int)(maxMouseSpeed / 2.0 * horizontalFraction)), rect.Width);
               p.Y = Math.Min(Math.Max(0, p.Y - (int)(maxMouseSpeed / 2.0 * verticalFraction)), rect.Height);
               SetCursorPos(p.X, p.Y);
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