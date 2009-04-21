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
      private static MCvFont _font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
      private static Capture _cameraCapture;
      private static BlobTrackerAuto _tracker;
      private static BGCodeBookModel<Bgr> _bgCodeBookModel;

      public VideoSurveilance()
      {
         InitializeComponent();
         Run();
      }

      void Run()
      {
         _cameraCapture = new Capture();
         _bgCodeBookModel = new BGCodeBookModel<Bgr>();

         _tracker = new BlobTrackerAuto();

         Application.Idle += ProcessFrame;
      }

      void ProcessFrame(object sender, EventArgs e)
      {
         Image<Bgr, Byte> frame = _cameraCapture.QueryFrame();
         frame._SmoothGaussian(3); //filter our noises

         #region use the background code book model to find the forground mask
         _bgCodeBookModel.Update(frame, Rectangle.Empty, null);
         Image<Gray, Byte> forgroundMask = new Image<Gray, byte>(frame.Size);
         _bgCodeBookModel.Diff(frame, forgroundMask, Rectangle.Empty);
         #endregion

         _tracker.Process(frame, forgroundMask);

         foreach (MCvBlob blob in _tracker)
         {
            frame.Draw(Rectangle.Round(blob), new Bgr(255.0, 255.0, 255.0), 2);
            frame.Draw(blob.ID.ToString(), ref _font, Point.Round(blob.Center), new Bgr(255.0, 255.0, 255.0));
         }

         imageBox1.Image = frame;
         imageBox2.Image = forgroundMask;

      }
   }
}