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

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace TrafficSignRecognition
{
   [Activity(Label = "Traffic Sign Recognition", MainLauncher = true, Icon = "@drawable/icon")]
   public class Activity1 : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.Main);

         // Get our button from the layout resource,
         // and attach an event to it
         Button button = FindViewById<Button>(Resource.Id.DetectButton);
         ImageView imageView = FindViewById<ImageView>(Resource.Id.ResultImageView);
         TextView messageView = FindViewById<TextView>(Resource.Id.MessageView);

         button.Click += delegate 
         { 
            using (Image<Bgr, byte> stopSignModel = new Image<Bgr, byte>(Assets, "stop-sign-model.png"))
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(Assets, "stop-sign.jpg"))
            {
               Stopwatch watch = Stopwatch.StartNew(); // time the detection process

               List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, byte>>();
               List<Rectangle> stopSignBoxList = new List<Rectangle>();
               StopSignDetector detector = new StopSignDetector(stopSignModel);
               detector.DetectStopSign(image, stopSignList, stopSignBoxList);

               watch.Stop(); //stop the timer
               messageView.Text = String.Format("Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);

               foreach (Rectangle rect in stopSignBoxList)
               {
                  image.Draw(rect, new Bgr(Color.Red), 2);
               }
               imageView.SetImageBitmap(image.ToBitmap());
            }
         };
      }
   }
}

