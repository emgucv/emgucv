//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Threading;
using System.Runtime.InteropServices;

namespace FacialMouseControl
{
   public partial class Form1 : Form
   {
      private Capture _capture;
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

         Application.Idle += ProcessImage;
      }

      public void ProcessImage(object sender, EventArgs e)
      {
         Image<Bgr, Byte> frame = _capture.QueryFrame();
         Image<Gray, Byte> grayImage = frame.Convert<Gray, Byte>();
         grayImage._EqualizeHist();

         System.Drawing.Rectangle imageArea = grayImage.ROI;

         System.Drawing.Rectangle mouseStableArea =
            new System.Drawing.Rectangle((int)(imageArea.Width * 0.4), (int)(imageArea.Height * 0.4), (int)(imageArea.Width * 0.2), (int)(imageArea.Height * 0.2));

         //draw the stable area where the face will not trigger a movement;
         frame.Draw(mouseStableArea, new Bgr(255, 0, 0), 1);

         MCvAvgComp[] faces = grayImage.DetectHaarCascade(_face)[0];
         if (faces.Length > 0)
         {   //if there is at least one face

            #region find the biggest face
            MCvAvgComp biggestFace = faces[0];
            for (int i = 1; i < faces.Length; i++)
            {
               if (faces[i].rect.Width * faces[i].rect.Height > biggestFace.rect.Width * biggestFace.rect.Height)
                  biggestFace = faces[i];
            }
            #endregion

            //draw a yellow rectangle around the face
            frame.Draw(biggestFace.rect, new Bgr(255, 255, 0.0), 1);

            Point biggestFaceCenter = new Point(biggestFace.rect.X + biggestFace.rect.Width / 2, biggestFace.rect.Y + biggestFace.rect.Height / 2);
            Point imageAreaCenter = new Point(imageArea.X + imageArea.Width / 2, imageArea.Y + imageArea.Height / 2);
            //draw a green cross at the center of the biggest face
            frame.Draw(
                new Cross2DF(biggestFaceCenter, biggestFace.rect.Width * 0.1f, biggestFace.rect.Height * 0.1f),
                new Bgr(0, 255, 0), 1);

            if (!mouseStableArea.Contains(biggestFaceCenter))
            {   //the point is far enough from the center to triger a movement

               //horizontal fraction is a value in [-0.5, 0.5] where
               //-0.5 refer to the far left and 
               //0.5 refer to the far right
               double horizontalFraction = (double)(biggestFaceCenter.X - imageAreaCenter.X) / imageArea.Width;
               //do the same for vertical fraction
               double verticalFraction = (double)(biggestFaceCenter.Y - imageAreaCenter.Y) / imageArea.Height;

               Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
               int maxMouseSpeed = rect.Width / 20;
               System.Drawing.Point p;
               GetCursorPos(out p);
               p.X = Math.Min(Math.Max(0,  p.X + (int)( (maxMouseSpeed / 2) * horizontalFraction)), rect.Width);
               p.Y = Math.Min(Math.Max(0, p.Y + (int) ((maxMouseSpeed / 2) * verticalFraction)), rect.Height);
               SetCursorPos(p.X, p.Y);
            }
         }

         imageBox1.Image = frame;
      }

      [DllImport("user32.dll")]
      private static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

      [DllImport("user32.dll")]
      private static extern bool SetCursorPos(int X, int Y);

      public void ReleaseData()
      {
         if (_capture != null)
            _capture.Dispose();
      }

      private void flipHorizontalButton_Click(object sender, EventArgs e)
      {
         if (_capture != null) _capture.FlipHorizontal = !_capture.FlipHorizontal;
      }
   }
}
