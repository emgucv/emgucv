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

namespace PlanarSubdivisionExample
{
   [Activity(Label = "Planar Subdivision", MainLauncher = true, Icon = "@drawable/icon")]
   public class PlanarSubdivisionActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.Main);

         Button redrawButton = FindViewById<Button>(Resource.Id.RedrawButton);
         redrawButton.Click += new EventHandler(redrawButton_Click);

         redrawButton_Click(this, null);
      }

      void redrawButton_Click(object sender, EventArgs e)
      {
         //display the image
         ImageView imageView = FindViewById<ImageView>(Resource.Id.MyImage);
         using (Image<Bgr, Byte> img = DrawSubdivision.Draw(600, 20))
         {
            imageView.SetImageBitmap(img.ToBitmap());
         }
      }
   }
}

