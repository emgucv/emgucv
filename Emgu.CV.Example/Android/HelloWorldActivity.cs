using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Emgu.CV;
using Emgu.CV.Structure;

using Bitmap = Android.Graphics.Bitmap;
using System.Drawing;
using System.Runtime.InteropServices;


namespace AndroidExamples
{
   [Activity(Label = "Hello World")]
   public class HelloWorldActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.HelloWorld);

         using (Image<Bgra, Byte> image = new Image<Bgra, byte>(480, 320))
         {
            MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 4.0, 4.0);

            image.SetValue(new Bgra(255,255,255,255));
            image.Draw("Hello, World!", ref font, new Point(50, 50), new Bgra(0,0,255,255));

            ImageView imageView = FindViewById<ImageView>(Resource.Id.HelloWorldImageView);
            imageView.SetImageBitmap(image.ToBitmap());
         }
      }
   }
}

