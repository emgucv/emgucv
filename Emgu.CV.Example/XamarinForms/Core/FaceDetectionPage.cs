//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Preferences;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using FaceDetection;

namespace Emgu.CV.XamarinForms
{
   public class FaceDetectionPage : ButtonTextImagePage
   {
      public FaceDetectionPage()
         : base()
      {

         var button = this.GetButton();
         button.Text = "Perform Face Detection";
         button.Clicked += OnButtonClicked;

         OnImagesLoaded += async (sender, image) =>
         {
            GetLabel().Text = "Please wait...";
            SetImage(null);
            Task<Tuple<Mat, long>> t = new Task<Tuple<Mat, long>>(
               () =>
               {
                  String faceFile;
                  String eyeFile;
                  bool fileOk = CheckCascadeFile("haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
                     out faceFile,
                     out eyeFile);

                  long time;
                  List<Rectangle> faces = new List<Rectangle>();
                  List<Rectangle> eyes = new List<Rectangle>();

                  using (UMat img = image[0].GetUMat(AccessType.ReadWrite))
                     DetectFace.Detect(img, faceFile, eyeFile, faces, eyes, out time);

                  foreach (Rectangle rect in faces)
                     CvInvoke.Rectangle(image[0], rect, new MCvScalar(0, 0, 255), 2);
                  foreach (Rectangle rect in eyes)
                     CvInvoke.Rectangle(image[0], rect, new MCvScalar(255, 0, 0), 2);

                  return new Tuple<Mat, long>(image[0], time);
               });
            t.Start();

            var result = await t;
            SetImage(t.Result.Item1);
            String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
            GetLabel().Text = String.Format("Detected with {1} in {0} milliseconds.", t.Result.Item2, computeDevice);
         };
      }

      private void OnButtonClicked(Object sender, EventArgs args)
      {
         LoadImages(new string[] { "lena.jpg" });
      }

      bool CheckCascadeFile(String face, String eye, out String faceFile, out String eyeFile)
      {
#if __ANDROID__
         ISharedPreferences preference = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
         String appVersion = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, Android.Content.PM.PackageInfoFlags.Activities).VersionName;
         if (!preference.Contains("cascade-data-version") || !preference.GetString("cascade-data-version", null).Equals(appVersion)
            || !(preference.Contains("cascade-eye-data-path") || preference.Contains("cascade-face-data-path")))
         {
            AndroidFileAsset.OverwriteMethod overwriteMethod = AndroidFileAsset.OverwriteMethod.AlwaysOverwrite;

            FileInfo eyeFileTmp = AndroidFileAsset.WritePermanantFileAsset(Android.App.Application.Context, eye, "cascade", overwriteMethod);
            FileInfo faceFileTmp = AndroidFileAsset.WritePermanantFileAsset(Android.App.Application.Context, face, "cascade", overwriteMethod);

            //save data path
            ISharedPreferencesEditor editor = preference.Edit();
            editor.PutString("cascade-data-version", appVersion);
            editor.PutString("cascade-eye-data-path", eyeFileTmp.FullName);
            editor.PutString("cascade-face-data-path", faceFileTmp.FullName);
            editor.Commit();
         }

         eyeFile = preference.GetString("cascade-eye-data-path", null);
         faceFile = preference.GetString("cascade-face-data-path", null);
         return true;
#else
         faceFile = face;
         eyeFile = eye;
         return true;
#endif
      }
   }
}
