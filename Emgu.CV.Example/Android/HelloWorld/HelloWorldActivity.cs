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


namespace HelloWorld
{
   [Activity(Label = "Hello World", MainLauncher = true, Icon = "@drawable/icon")]
   public class HelloWorldActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.Main);
         
         Image<Bgr, Byte> image = new Image<Bgr, byte>(480, 320);
         MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 4.0, 4.0);

         image.SetValue(new Bgr(Color.White));
         image.Draw("Hello, World!", ref font, new Point(50, 50), new Bgr(Color.Blue));

         //Emgu.Util.TypeEnum.OS os = Emgu.Util.Platform.OperationSystem;
         //Image<Bgr, byte> subrect = image.GetSubRect(new Rectangle(0, 0, 50, 60));
         ImageView imageView = FindViewById<ImageView>(Resource.Id.MyImage);
         imageView.SetImageBitmap(image.ToBitmap());         
      }
   }
}

