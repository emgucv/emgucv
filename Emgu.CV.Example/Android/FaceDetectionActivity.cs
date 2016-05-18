//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Preferences;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using FaceDetection;

namespace AndroidExamples
{
   [Activity(Label = "Face Detection")]
   public class FaceDetectionActivity : ButtonMessageImageActivity
   {
      public FaceDetectionActivity()
         : base("Detect Face")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         OnImagePicked += (sender, image) =>
         {
            if (image == null)
               return;

            AppPreference appPreference = new AppPreference();
            CvInvoke.UseOpenCL = appPreference.UseOpenCL;
            String oclDeviceName = appPreference.OpenClDeviceName;
            if (!String.IsNullOrEmpty(oclDeviceName))
            {
               CvInvoke.OclSetDefaultDevice(oclDeviceName);
            }


            ISharedPreferences preference = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            String appVersion = PackageManager.GetPackageInfo(PackageName, Android.Content.PM.PackageInfoFlags.Activities).VersionName;
            if (!preference.Contains("cascade-data-version") || !preference.GetString("cascade-data-version", null).Equals(appVersion)
               || !(preference.Contains("cascade-eye-data-path") || preference.Contains("cascade-face-data-path")))
            {
               AndroidFileAsset.OverwriteMethod overwriteMethod = AndroidFileAsset.OverwriteMethod.AlwaysOverwrite;

               FileInfo eyeFile = AndroidFileAsset.WritePermanantFileAsset(this, "haarcascade_eye.xml", "cascade", overwriteMethod);
               FileInfo faceFile = AndroidFileAsset.WritePermanantFileAsset(this, "haarcascade_frontalface_default.xml", "cascade", overwriteMethod);

               //save tesseract data path
               ISharedPreferencesEditor editor = preference.Edit();
               editor.PutString("cascade-data-version", appVersion);
               editor.PutString("cascade-eye-data-path", eyeFile.FullName);
               editor.PutString("cascade-face-data-path", faceFile.FullName);
               editor.Commit();
            }

            string eyeXml = preference.GetString("cascade-eye-data-path", null);
            string faceXml = preference.GetString("cascade-face-data-path", null);
            long time;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();


            DetectFace.Detect(image, faceXml, eyeXml, faces, eyes, out time);

            String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Emgu.CV.Ocl.Device.Default.Name : "CPU";
            SetMessage(String.Format("Detected with {1} in {0} milliseconds.", time, computeDevice));

            foreach (Rectangle rect in faces)
               CvInvoke.Rectangle(image, rect, new Bgr(System.Drawing.Color.Red).MCvScalar, 2);
               
            foreach (Rectangle rect in eyes)
               CvInvoke.Rectangle(image, rect, new Bgr(System.Drawing.Color.Blue).MCvScalar, 2);
               
            SetImageBitmap(image.ToBitmap());
            image.Dispose();

            if (CvInvoke.UseOpenCL)
               CvInvoke.OclFinish();
         };

         OnButtonClick += (sender, args) =>
         {
            PickImage("lena.jpg");
         };
      }
   }
}

