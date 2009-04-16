using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;

namespace VideoSurveilance
{
   static class Program
   {
      private static MCvFont _font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
      private static Capture _cameraCapture;
      private static BlobTrackerAuto _tracker;
      private static ImageViewer _viewer;

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Run();
      }

      static void Run()
      {
         _viewer = new ImageViewer();
         
         _cameraCapture = new Capture();
         
         #region create the blob tracker
         BlobTrackerAutoParam param = new BlobTrackerAutoParam();
         //param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BLOB_DETECTOR_TYPE.CC);
         
         param.ForgroundDetector = new ForgroundDetector(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
         
         //param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BLOBTRACKER_TYPE.CCMSPF);
         
         param.FGTrainFrames = 10;
         
         _tracker = new BlobTrackerAuto(param);
         #endregion

         Application.Idle += ProcessFrame;
         
         _viewer.ShowDialog();
      }

      static void ProcessFrame(object sender, EventArgs e)
      {
         _tracker.Process(_cameraCapture.QuerySmallFrame().PyrUp());

         Image<Gray, Byte> img = _tracker.GetForgroundMask();
         
         foreach (MCvBlob blob in _tracker)
         {
            img.Draw(Rectangle.Round(blob), new Gray(255.0), 2);
            img.Draw(blob.ID.ToString(), ref _font, Point.Round(blob.Center), new Gray(255.0));
         }
         _viewer.Image = img;
      }
   }
}
