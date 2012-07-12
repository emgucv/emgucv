using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

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
               List<Rectangle> faces = new List<Rectangle>();
               List<Rectangle> eyes = new List<Rectangle>();
               DetectFace.Detect(image, faceXml.FileFullPath, eyeXml.FileFullPath, faces, eyes, out time);
               SetMessage( String.Format("Detected in {0} milliseconds.", time) );

               Bitmap bmp = null;
               using (Bitmap tmp = image.ToBitmap())
                  bmp = tmp.Copy(Bitmap.Config.Argb8888, true);
               using (Canvas c = new Canvas(bmp))
               using (Paint p = new Paint())
               {
                  p.Color = Android.Graphics.Color.Red;
                  p.StrokeWidth = 2;
                  p.SetStyle(Paint.Style.Stroke);
                  foreach (Rectangle rect in faces)
                     c.DrawRect(new Rect(rect.Left, rect.Top, rect.Right, rect.Bottom), p);

                  p.Color = Android.Graphics.Color.Blue;
                  foreach (Rectangle rect in eyes)
                     c.DrawRect(new Rect(rect.Left, rect.Top, rect.Right, rect.Bottom), p);
               }

               SetImageBitmap(bmp);
               SetImageBitmap(image.ToBitmap());
            }
         };
      }
   }
}

