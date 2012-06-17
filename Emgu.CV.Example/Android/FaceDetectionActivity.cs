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

         OnButtonClick += delegate 
         {
            long time;
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>(Assets, "lena.jpg"))
            using (AndroidCacheFileAsset eyeXml = new AndroidCacheFileAsset(this, "haarcascade_eye.xml"))
            using (AndroidCacheFileAsset faceXml = new AndroidCacheFileAsset(this, "haarcascade_frontalface_default.xml"))
            {
               DetectFace.DetectAndDraw(image, faceXml.FileFullPath, eyeXml.FileFullPath, out time);
               SetMessage( String.Format("Detected in {0} milliseconds.", time) );
               SetImageBitmap(image.ToBitmap());
            }
         };
      }
   }
}

