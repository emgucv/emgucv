using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace PedestrianDetection
{
   [Activity(Label = "Pedestrian Detection", MainLauncher = true, Icon = "@drawable/icon")]
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
            long time;
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>(Assets, "pedestrian.png"))
            using (Image<Bgr, Byte> result = FindPedestrian.Find(image, out time))
            {
               messageView.Text = String.Format("Detection completed in {0} milliseconds.", time);
               imageView.SetImageBitmap(result.ToBitmap());
            }
         };
      }
   }
}

