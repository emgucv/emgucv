//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;

namespace VideoSurveilance
{
   public partial class VideoSurveilance : Form
   {
      //private static MCvFont _font = new MCvFont(Emgu.CV.CvEnum.FontType.HersheyPlain, 1.0, 1.0);
      private static Capture _cameraCapture;
      private static BlobTrackerAuto<Bgr> _tracker;
      private static IBGFGDetector<Bgr> _detector;

      public VideoSurveilance()
      {
         InitializeComponent();
         Run();
      }

      void Run()
      {
         try
         {
            _cameraCapture = new Capture();
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message);
            return;
         }
         
         _detector = new FGDetector<Bgr>(ForgroundDetectorType.Fgd);

         _tracker = new BlobTrackerAuto<Bgr>();

         Application.Idle += ProcessFrame;
      }

      void ProcessFrame(object sender, EventArgs e)
      {
         Mat frame = _cameraCapture.QueryFrame();
         Image<Bgr, Byte> smoothedFrame = new Image<Bgr, byte>(frame.Size);
         CvInvoke.GaussianBlur(frame, smoothedFrame, new Size(3, 3), 1); //filter out noises
         //frame._SmoothGaussian(3); 

         #region use the BG/FG detector to find the forground mask
         _detector.Update(smoothedFrame);
         Image<Gray, Byte> forgroundMask = _detector.ForegroundMask;
         #endregion

         _tracker.Process(smoothedFrame, forgroundMask);

         foreach (MCvBlob blob in _tracker)
         {
            CvInvoke.Rectangle(frame, (Rectangle)blob, new MCvScalar(255.0, 255.0, 255.0), 2);
            CvInvoke.PutText(frame, blob.ID.ToString(), Point.Round(blob.Center), FontFace.HersheyPlain, 1.0, new MCvScalar(255.0, 255.0, 255.0));
         }

         imageBox1.Image = frame;
         imageBox2.Image = forgroundMask;
      }
   }
}