//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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
using Android.Preferences;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using LicensePlateRecognition;
using System.IO;

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

         OnImagePicked += (sender, image) =>
         {
            if (image == null)
               return;

            ISharedPreferences preference = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            String appVersion = PackageManager.GetPackageInfo(PackageName, Android.Content.PM.PackageInfoFlags.Activities).VersionName;
            if (!preference.Contains("tesseract-data-version") || !preference.GetString("tesseract-data-version", null).Equals(appVersion)
               || !preference.Contains("tesseract-data-path"))
            {
               AndroidFileAsset.OverwriteMethod overwriteMethod = AndroidFileAsset.OverwriteMethod.AlwaysOverwrite;

               FileInfo a8 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.traineddata", "tmp", overwriteMethod);
               FileInfo a0 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.cube.bigrams", "tmp", overwriteMethod);
               FileInfo a1 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.cube.fold", "tmp", overwriteMethod);
               FileInfo a2 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.cube.lm", "tmp", overwriteMethod);
               FileInfo a3 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.cube.nn", "tmp", overwriteMethod);
               FileInfo a4 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.cube.params", "tmp", overwriteMethod);
               FileInfo a5 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.cube.size", "tmp", overwriteMethod);
               FileInfo a6 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.cube.word-freq", "tmp", overwriteMethod);
               FileInfo a7 = AndroidFileAsset.WritePermanantFileAsset(this, "tessdata/eng.tesseract_cube.nn", "tmp", overwriteMethod);

               //save tesseract data path
               ISharedPreferencesEditor editor = preference.Edit();
               editor.PutString("tesseract-data-version", appVersion);
               editor.PutString("tesseract-data-path", System.IO.Path.Combine(a0.DirectoryName, "..") + System.IO.Path.DirectorySeparatorChar);
               editor.Commit();
            }

            LicensePlateDetector detector = new LicensePlateDetector(preference.GetString("tesseract-data-path", null));
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

            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("{0} milli-seconds. ", watch.Elapsed.TotalMilliseconds));
            foreach (String w in words)
               builder.AppendFormat("{0} ", w);
            SetMessage(builder.ToString());

            foreach (RotatedRect box in licenseBoxList)
            {
               Rectangle rect = box.MinAreaRect();
               CvInvoke.Rectangle(image, rect, new Bgr(System.Drawing.Color.Red).MCvScalar, 2);
            }

            SetImageBitmap(image.ToBitmap());
            image.Dispose();
         };


         OnButtonClick += delegate
         {
            PickImage("license-plate.jpg");

         };
      }
   }
}