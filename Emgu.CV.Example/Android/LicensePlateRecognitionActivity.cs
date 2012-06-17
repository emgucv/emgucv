using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using LicensePlateRecognition;

namespace AndroidExamples
{
   [Activity(Label = "License Plate Recognition")]
   public class LicensePlateRecognitionActivity : ButtonMessageImageActivity
   {
      public LicensePlateRecognitionActivity()
         : base("Detect License Plate")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         OnButtonClick += delegate
         {
            AndroidPermanantFileAsset.OverwriteMethod overwriteMethod = AndroidPermanantFileAsset.OverwriteMethod.NeverOverwrite;
            using (AndroidPermanantFileAsset a8 = new AndroidPermanantFileAsset(this, "tessdata/eng.traineddata", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a0 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.bigrams", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a1 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.fold", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a2 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.lm", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a3 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.nn", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a4 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.params", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a5 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.size", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a6 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.word-freq", "tmp", overwriteMethod))
            using (AndroidPermanantFileAsset a7 = new AndroidPermanantFileAsset(this, "tessdata/eng.tesseract_cube.nn", "tmp", overwriteMethod))
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(Assets, "license-plate.jpg"))
            {
               LicensePlateDetector detector = new LicensePlateDetector( System.IO.Path.Combine( a0.DirectoryName, "..") + System.IO.Path.DirectorySeparatorChar );
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

               StringBuilder builder = new StringBuilder();
               builder.Append(String.Format("{0} milli-seconds. ", watch.Elapsed.TotalMilliseconds));
               foreach (String w in words)
                  builder.AppendFormat("{0} ", w);
               SetMessage(builder.ToString());

               Bitmap bmp = null;
               using (Bitmap tmp = image.ToBitmap())
                  bmp = tmp.Copy(Bitmap.Config.Argb8888, true);
               using (Canvas c = new Canvas(bmp))
               using (Paint p = new Paint())
               {
                  p.Color = Android.Graphics.Color.Red;
                  p.StrokeWidth = 2;
                  p.SetStyle(Paint.Style.Stroke);

                  foreach (MCvBox2D box in licenseBoxList)
                  {
                     Rectangle rect = box.MinAreaRect();
                     c.DrawRect(new Rect(rect.Left, rect.Top, rect.Right, rect.Bottom), p);
                  }
               }

               SetImageBitmap(bmp);
            }
         };
      }
   }
}