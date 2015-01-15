//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace CameraCapture
{
   public partial class CameraCapture : Form
   {
      private Capture _capture = null;
      private bool _captureInProgress;

      public CameraCapture()
      {
         InitializeComponent();
         CvInvoke.UseOpenCL = false;
         try
         {
            _capture = new Capture();
            _capture.ImageGrabbed += ProcessFrame;
         }
         catch (NullReferenceException excpt)
         {
            MessageBox.Show(excpt.Message);
         }
      }

      private void ProcessFrame(object sender, EventArgs arg)
      {
         Mat frame = new Mat();
         _capture.Retrieve(frame, 0);
         Mat grayFrame = new Mat();
         CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);
         Mat smallGrayFrame = new Mat();
         CvInvoke.PyrDown(grayFrame, smallGrayFrame);
         Mat smoothedGrayFrame = new Mat();
         CvInvoke.PyrUp(smallGrayFrame, smoothedGrayFrame);
         
         //Image<Gray, Byte> smallGrayFrame = grayFrame.PyrDown();
         //Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
         Mat cannyFrame = new Mat();
         CvInvoke.Canny(smoothedGrayFrame, cannyFrame, 100, 60);

         //Image<Gray, Byte> cannyFrame = smoothedGrayFrame.Canny(100, 60);

         captureImageBox.Image = frame;
         grayscaleImageBox.Image = grayFrame;
         smoothedGrayscaleImageBox.Image = smoothedGrayFrame;
         cannyImageBox.Image = cannyFrame;
      }

      private void captureButtonClick(object sender, EventArgs e)
      {
         if (_capture != null)
         {
            if (_captureInProgress)
            {  //stop the capture
               captureButton.Text = "Start Capture";
               _capture.Pause();
            }
            else
            {
               //start the capture
               captureButton.Text = "Stop";
               _capture.Start();
            }

            _captureInProgress = !_captureInProgress;
         }
      }

      private void ReleaseData()
      {
         if (_capture != null)
            _capture.Dispose();
      }

      private void FlipHorizontalButtonClick(object sender, EventArgs e)
      {
         if (_capture != null) _capture.FlipHorizontal = !_capture.FlipHorizontal;
      }

      private void FlipVerticalButtonClick(object sender, EventArgs e)
      {
         if (_capture != null) _capture.FlipVertical = !_capture.FlipVertical;
      }
   }
}
