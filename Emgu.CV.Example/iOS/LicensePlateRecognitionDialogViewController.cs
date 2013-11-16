//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using LicensePlateRecognition;

namespace Emgu.CV.Example.MonoTouch
{
   public class LicensePlateRecognitionDialogViewController :DialogViewController
   {
      public LicensePlateRecognitionDialogViewController()
         : base(new RootElement(""), true)
      {

      }

      public Size FrameSize
      {
         get
         {
            int width = 0, height = 0;
            InvokeOnMainThread(delegate
            {
               width = (int)View.Frame.Width;
               height = (int)View.Frame.Height;
            });
            return new Size(width, height);
         }
      }

      public override void ViewDidLoad()
      {
         base.ViewDidLoad();
         RootElement root = Root;
         root.UnevenRows = true;
         UIImageView imageView = new UIImageView(View.Frame);
         StringElement messageElement = new StringElement("");
         StringElement licenseElement = new StringElement("");

         root.Add(new Section()
                 { new StyledStringElement("Process", delegate {

            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>("license-plate.jpg"))
            {
               LicensePlateDetector detector = new LicensePlateDetector(".");
               Stopwatch watch = Stopwatch.StartNew(); // time the detection process

               List<Image<Gray, Byte>> licensePlateImagesList = new List<Image<Gray, byte>>();
               List<Image<Gray, Byte>> filteredLicensePlateImagesList = new List<Image<Gray, byte>>();
               List<MCvBox2D> licenseBoxList = new List<MCvBox2D>();
               List<string> words = detector.DetectLicensePlate(
                  image,
                  licensePlateImagesList,
                  filteredLicensePlateImagesList,
                  licenseBoxList);

               watch.Stop(); //stop the timer
               messageElement.Value = String.Format("{0} milli-seconds", watch.Elapsed.TotalMilliseconds);

               StringBuilder builder = new StringBuilder();
               foreach (String w in words)
                  builder.AppendFormat("{0} ", w);
               licenseElement.Value = builder.ToString();

               messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);
               licenseElement.GetImmediateRootElement().Reload(licenseElement, UITableViewRowAnimation.Automatic);
               foreach (MCvBox2D box in licenseBoxList)
               {
                  image.Draw(box, new Bgr(Color.Red), 2);
               }
                  Size frameSize = FrameSize;
                  using (Image<Bgr, byte> resized = image.Resize( frameSize.Width, frameSize.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true))
                  {
                     imageView.Image = resized.ToUIImage();
                     imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
                  }
               imageView.SetNeedsDisplay();
                  ReloadData();
            }
         }
         )});
         root.Add(new Section("Recognition Time") {messageElement});
         root.Add(new Section("License Plate") { licenseElement});
         root.Add(new Section() {imageView});
      }
   }
}

