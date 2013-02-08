using System;
using System.IO;
using System.Diagnostics;
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

using TrafficSignRecognition;

namespace AndroidExamples
{
   [Activity(Label = "Traffic Sign Recognition")]
   public class TrafficSignRecognitionActivity : ButtonMessageImageActivity
   {
      public TrafficSignRecognitionActivity()
         : base("Find Stop Sign")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         OnButtonClick += delegate 
         { 
            using (Image<Bgr, byte> stopSignModel = new Image<Bgr, byte>(Assets, "stop-sign-model.png"))
            using (Image<Bgr, Byte> image = PickImage("stop-sign.jpg"))
            {             
               if (image == null)
                  return;

               Stopwatch watch = Stopwatch.StartNew(); // time the detection process

               List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, byte>>();
               List<Rectangle> stopSignBoxList = new List<Rectangle>();
               StopSignDetector detector = new StopSignDetector(stopSignModel);
               detector.DetectStopSign(image, stopSignList, stopSignBoxList);

               watch.Stop(); //stop the timer
               SetMessage(String.Format("Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds));

               Bitmap bmp = image.ToBitmap();
               using (Canvas c = new Canvas(bmp))
               using (Paint p = new Paint())
               {
                  p.Color = Android.Graphics.Color.Red;
                  p.StrokeWidth = 2;
                  p.SetStyle(Paint.Style.Stroke);
                 
                  foreach (Rectangle rect in stopSignBoxList)
                  {
                     c.DrawRect(new Rect(rect.Left, rect.Top, rect.Right, rect.Bottom), p);
                  }
               }
               SetImageBitmap(bmp);
            }
         };
      }
   }
}

