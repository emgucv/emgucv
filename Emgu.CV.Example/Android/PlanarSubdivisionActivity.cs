using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using PlanarSubdivisionExample;

namespace AndroidExamples
{
   [Activity(Label = "Planar Subdivision", MainLauncher = true, Icon = "@drawable/icon")]
   public class PlanarSubdivisionActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.PlanarSubdivision);

         Button redrawButton = FindViewById<Button>(Resource.Id.RedrawSubdivisionButton);
         //display the image
         ImageView imageView = FindViewById<ImageView>(Resource.Id.PlanarSubdivisionImageView);

         redrawButton.Click += (sender, e) =>
            {
               using (Image<Bgr, Byte> img = DrawSubdivision.Draw(600, 20))
               {
                  imageView.SetImageBitmap(img.ToBitmap());
               }
            };

         redrawButton.PerformClick();
      }

   }
}

