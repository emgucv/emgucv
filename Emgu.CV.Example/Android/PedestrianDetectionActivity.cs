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
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>(Assets, "pedestrian.png"))
            {
               Rectangle[] pedestrians = FindPedestrian.Find(image, out time);

               SetMessage(String.Format("Detection completed in {0} milliseconds.", time));

               Bitmap bmp = null;
               using (Bitmap tmp = image.ToBitmap())
                  bmp = tmp.Copy(Bitmap.Config.Argb8888, true);
               using (Canvas c = new Canvas(bmp))
               using (Paint p = new Paint())
               {
                  p.Color = Android.Graphics.Color.Red;
                  p.StrokeWidth = 2;
                  p.SetStyle(Paint.Style.Stroke);

                  foreach (Rectangle rect in pedestrians)
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

