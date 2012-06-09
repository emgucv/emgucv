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
using SURFFeatureExample;

namespace AndroidExamples
{
   [Activity(Label = "SURF Feature")]
   public class SURFFeatureActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.SURFFeature);

         // Get our button from the layout resource,
         // and attach an event to it
         Button button = FindViewById<Button>(Resource.Id.MatchButton);
         ImageView imageView = FindViewById<ImageView>(Resource.Id.SURFFeatureImageView);
         TextView messageView = FindViewById<TextView>(Resource.Id.SURFFeatureMessageView);

         button.Click += delegate 
         {
            long time;
            using (Image<Gray, Byte> box = new Image<Gray,byte>(Assets, "box.png"))
            using (Image<Gray, Byte> boxInScene = new Image<Gray, byte>(Assets, "box_in_scene.png"))
            using (Image<Bgr, Byte> result = DrawMatches.Draw(box, boxInScene, out time))
            {
               messageView.Text = String.Format("Matched in {0} milliseconds.", time);
               imageView.SetImageBitmap( result.ToBitmap() );
            }
         };
      }
   }
}

