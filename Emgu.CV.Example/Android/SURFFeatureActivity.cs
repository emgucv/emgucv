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
using Emgu.CV.Util;
using Emgu.CV.Structure;
using SURFFeatureExample;

namespace AndroidExamples
{
   [Activity(Label = "SURF Feature")]
   public class SURFFeatureActivity : ButtonMessageImageActivity
   {
      public SURFFeatureActivity()
         : base("Match SURF Features")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         OnButtonClick += delegate
         {
            long time;
            using (Image<Gray, Byte> box = new Image<Gray, byte>(Assets, "box.png"))
            using (Image<Gray, Byte> boxInScene = new Image<Gray, byte>(Assets, "box_in_scene.png"))
            using (Image<Bgr, Byte> result = DrawMatches.Draw(box, boxInScene, out time))
            {
               SetImageBitmap(result.ToBitmap());
               SetMessage(String.Format("Matched in {0} milliseconds.", time));
            }
         };
      }
   }
}

