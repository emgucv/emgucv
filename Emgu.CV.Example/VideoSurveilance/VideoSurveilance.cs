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
         Image<Bgr, Byte> frame = _cameraCapture.QueryFrame();
         frame._SmoothGaussian(3); //filter out noises

         #region use the BG/FG detector to find the forground mask
         _detector.Update(frame);
         Image<Gray, Byte> forgroundMask = _detector.ForegroundMask;
         #endregion

         _tracker.Process(frame, forgroundMask);

         foreach (MCvBlob blob in _tracker)
         {
            frame.Draw((Rectangle)blob, new Bgr(255.0, 255.0, 255.0), 2);
            frame.Draw(blob.ID.ToString(), Point.Round(blob.Center), FontFace.HersheyPlain, 1.0, new Bgr(255.0, 255.0, 255.0));
         }

         imageBox1.Image = frame;
         imageBox2.Image = forgroundMask;
      }
   }
}