using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using FaceDetection;

namespace AndroidExamples
{
   [Activity(Label = "Face Detection", MainLauncher = true, Icon = "@drawable/icon")]
   public class FaceDetectionActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.FaceDetection);

         // Get our button from the layout resource,
         // and attach an event to it
         Button button = FindViewById<Button>(Resource.Id.DetectFaceButton);
         ImageView imageView = FindViewById<ImageView>(Resource.Id.FaceDetectionImageView);
         TextView messageView = FindViewById<TextView>(Resource.Id.FaceDetectionMessageView);

         button.Click += delegate 
         {
            long time;
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>(Assets, "lena.jpg"))
            using (AndroidFileAsset faceXml = new AndroidFileAsset(this, "haarcascade_eye.xml"))
            using (AndroidFileAsset eyeXml = new AndroidFileAsset(this, "haarcascade_frontalface_default.xml"))
            {
               DetectFace.DetectAndDraw(image, faceXml.FileName, eyeXml.FileName, out time);
               messageView.Text = String.Format("Detected in {0} milliseconds.", time);
               imageView.SetImageBitmap(image.ToBitmap());
            }
         };
      }
   }
}

