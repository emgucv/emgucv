//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace VideoSurveilance
{
   public partial class VideoSurveilance : Form
   {
      
      private static VideoCapture _cameraCapture;
      
      private static IBackgroundSubtractor _fgDetector;
      private static Emgu.CV.Cvb.CvBlobDetector _blobDetector;
      private static Emgu.CV.Cvb.CvTracks _tracker;

      public VideoSurveilance()
      {
         InitializeComponent();
         Run();
      }

      void Run()
      {
         try
         {
            _cameraCapture = new VideoCapture();
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
            return;
         }

         _fgDetector = new BackgroundSubtractorMOG2();
         _blobDetector = new CvBlobDetector();
         _tracker = new CvTracks();

         Application.Idle += ProcessFrame;
      }

      void ProcessFrame(object sender, EventArgs e)
      {
         Mat frame = _cameraCapture.QueryFrame();
         Mat smoothedFrame = new Mat();
         CvInvoke.GaussianBlur(frame, smoothedFrame, new Size(3, 3), 1); //filter out noises
         //frame._SmoothGaussian(3); 

         #region use the BG/FG detector to find the forground mask
         Mat forgroundMask = new Mat();
         _fgDetector.Apply(smoothedFrame, forgroundMask);
         #endregion

         CvBlobs blobs = new CvBlobs();
         _blobDetector.Detect(forgroundMask.ToImage<Gray, byte>(), blobs);
         blobs.FilterByArea(100, int.MaxValue);

         float scale = (frame.Width + frame.Width)/2.0f;
         _tracker.Update(blobs, 0.01 * scale, 5, 5);
        
         foreach (var pair in _tracker)
         {
            CvTrack b = pair.Value;
            CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
            CvInvoke.PutText(frame,  b.Id.ToString(), new Point((int)Math.Round(b.Centroid.X), (int)Math.Round(b.Centroid.Y)), FontFace.HersheyPlain, 1.0, new MCvScalar(255.0, 255.0, 255.0));
         }

         imageBox1.Image = frame;
         imageBox2.Image = forgroundMask;
      }
   }
}