//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
using Foundation;
using UIKit;
using LicensePlateRecognition;

namespace Example.iOS
{
    public class LicensePlateRecognitionDialogViewController : DialogViewController
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

        private static void TesseractDownloadLangFile(String folder, String lang)
        {
            String subfolderName = "tessdata";
            String folderName = System.IO.Path.Combine(folder, subfolderName);
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
            String dest = System.IO.Path.Combine(folderName, String.Format("{0}.traineddata", lang));
            if (!System.IO.File.Exists(dest))
                using (System.Net.WebClient webclient = new System.Net.WebClient())
                {
                    String source =
                        String.Format("https://github.com/tesseract-ocr/tessdata/blob/4592b8d453889181e01982d22328b5846765eaad/{0}.traineddata?raw=true", lang);

                    Console.WriteLine(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
                    webclient.DownloadFile(source, dest);
                    Console.WriteLine(String.Format("Download completed"));
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

            messageElement.Value = String.Format("Checking Tesseract language data...");
            try
            {
                TesseractDownloadLangFile(".", "eng");
                TesseractDownloadLangFile(".", "osd");
            }
            catch (System.Net.WebException webExcpt)
            {
                messageElement.Value =
                    String.Format(
                        "Failed to download Tesseract language file, please check internet connection.");
                return;
            }
            catch (Exception e)
            {
                messageElement.Value = e.Message;
                return;
            }
            messageElement.Value = String.Format("Checking Tesseract language data...");

            using (Image<Bgr, Byte> image = new Image<Bgr, byte>( "license-plate.jpg"))
            {
               LicensePlateDetector detector = new LicensePlateDetector(".");
               Stopwatch watch = Stopwatch.StartNew(); // time the detection process

               List<IInputOutputArray> licensePlateImagesList = new List<IInputOutputArray>();
               List<IInputOutputArray> filteredLicensePlateImagesList = new List<IInputOutputArray>();
               List<RotatedRect> licenseBoxList = new List<RotatedRect>();
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
               foreach (RotatedRect box in licenseBoxList)
               {

                  image.Draw(box, new Bgr(Color.Red), 2);
               }
               Size frameSize = FrameSize;
               using (Mat resized = new Mat())
                  {
                     CvInvoke.ResizeForFrame(image, resized, frameSize);
                     imageView.Image = resized.ToUIImage();
                     imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
                  }
               imageView.SetNeedsDisplay();
                  ReloadData();
            }
         }
         )});
            root.Add(new Section("Recognition Time") { messageElement });
            root.Add(new Section("License Plate") { licenseElement });
            root.Add(new Section() { imageView });
        }
    }
}

