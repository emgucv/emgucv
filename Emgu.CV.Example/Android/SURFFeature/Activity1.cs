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

namespace SURFFeatureExample
{
   [Activity(Label = "SURFFeature", MainLauncher = true, Icon = "@drawable/icon")]
   public class Activity1 : Activity
   {
      int count = 1;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.Main);

         // Get our button from the layout resource,
         // and attach an event to it
         Button button = FindViewById<Button>(Resource.Id.MatchButton);
         ImageView imageView = FindViewById<ImageView>(Resource.Id.ResultImageView);
         TextView messageView = FindViewById<TextView>(Resource.Id.MessageView);

         button.Click += delegate 
         {
            long time;
            using (Stream boxStream =  Assets.Open("box.png"))
            using (Bitmap boxBmp = BitmapFactory.DecodeStream(boxStream))
            using (Stream boxInSceneStream = Assets.Open("box_in_scene.png"))
            using (Bitmap boxInSceneBmp = BitmapFactory.DecodeStream(boxInSceneStream))
            using (Image<Gray, Byte> box = new Image<Gray,byte>(boxBmp))
            using (Image<Gray, Byte> boxInScene = new Image<Gray,byte>(boxInSceneBmp))
            using (Image<Bgr, Byte> result = DrawMatches.Draw(box, boxInScene, out time))
            {
               messageView.Text = String.Format("Matched in {0} milliseconds.", time);
               imageView.SetImageBitmap( result.ToBitmap() );
            }
         };
      }
   }
}

