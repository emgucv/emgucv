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
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using LicensePlateRecognition;

namespace AndroidExamples
{
   [Activity(Label = "License Plate Recognition")]
   public class LicensePlateRecognitionActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.LicensePlateRecognition);

         // Get our button from the layout resource,
         // and attach an event to it
         Button button = FindViewById<Button>(Resource.Id.RecognizeLicensePlateButton);
         ImageView imageView = FindViewById<ImageView>(Resource.Id.LicensePlateRecognitionImageView);
         TextView messageView = FindViewById<TextView>(Resource.Id.LicensePlateRecognitionMessageView);

         String[] files = Assets.List(".");

         button.Click += delegate
         {
            using (AndroidPermanantFileAsset a8 = new AndroidPermanantFileAsset(this, "tessdata/eng.traineddata", "tmp", true))
            using (AndroidPermanantFileAsset a0 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.bigrams", "tmp", true))
            using (AndroidPermanantFileAsset a1 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.fold", "tmp", true))
            using (AndroidPermanantFileAsset a2 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.lm", "tmp", true))
            using (AndroidPermanantFileAsset a3 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.nn", "tmp", true))
            using (AndroidPermanantFileAsset a4 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.params", "tmp", true))
            using (AndroidPermanantFileAsset a5 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.size", "tmp", true))
            using (AndroidPermanantFileAsset a6 = new AndroidPermanantFileAsset(this, "tessdata/eng.cube.word-freq", "tmp", true))
            using (AndroidPermanantFileAsset a7 = new AndroidPermanantFileAsset(this, "tessdata/eng.tesseract_cube.nn", "tmp", true))
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
               messageView.Text = builder.ToString();

               foreach (MCvBox2D box in licenseBoxList)
               {
                  image.Draw(box, new Bgr(Color.Red), 2);
               }

               imageView.SetImageBitmap(image.ToBitmap());
            }
         };
      }
   }
}