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
using System.Drawing;
using Android.Graphics;
using PedestrianDetection;

namespace AndroidExamples
{
   [Activity(Label = "Pedestrian Detection")]
   public class PedestrianDetectionActivity : ButtonMessageImageActivity
   {
      public PedestrianDetectionActivity()
         : base("Detect Pedestrian")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         OnButtonClick += delegate
         {
            long time;
            using (Image<Bgr, Byte> image = PickImage("pedestrian.png"))
            {
               if (image == null)
                  return;
               Rectangle[] pedestrians = FindPedestrian.Find(image.Mat, out time);

               SetMessage(String.Format("Detection completed in {0} milliseconds.", time));
               foreach (Rectangle rect in pedestrians)
               {
                  image.Draw(rect, new Bgr(System.Drawing.Color.Red), 2);
               }

               SetImageBitmap(image.ToBitmap());
            }
         };
      }
   }
}

