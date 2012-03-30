using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.CoreGraphics;

namespace HelloWorld
{
   public partial class HelloWorldViewController : UIViewController
   {
      static bool UserInterfaceIdiomIsPhone
      {
         get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
      }

      public HelloWorldViewController()
         : base (UserInterfaceIdiomIsPhone ? "HelloWorldViewController_iPhone" : "HelloWorldViewController_iPad", null)
      {
      }
      
      public override void DidReceiveMemoryWarning()
      {
         // Releases the view if it doesn't have a superview.
         base.DidReceiveMemoryWarning();
         
         // Release any cached data, images, etc that aren't in use.
      }
      
      public override void ViewDidLoad()
      {
         base.ViewDidLoad();
         
         // Perform any additional setup after loading the view, typically from a nib.
         MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
         using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(320, 240))
         {
            image.Draw("Hello, world", ref font, new Point(30, 30), new Bgr(255, 255, 255));
            using (Image<Bgra, Byte> imageBgra = image.Convert<Bgra, Byte>())
            {
               CGBitmapContext context = new CGBitmapContext(
               imageBgra.MIplImage.imageData,
               imageBgra.Width, imageBgra.Height,
               8,
               imageBgra.Width * 4,
               CGColorSpace.CreateDeviceRGB(),
               CGImageAlphaInfo.PremultipliedLast);

               UIImageView imageView = new UIImageView(View.Frame);
               View.AddSubview(imageView);
               imageView.Image = UIImage.FromImage(context.ToImage());
            }
         }
      }
      
      public override void ViewDidUnload()
      {
         base.ViewDidUnload();
         
         // Clear any references to subviews of the main view in order to
         // allow the Garbage Collector to collect them sooner.
         //
         // e.g. myOutlet.Dispose (); myOutlet = null;
         
         ReleaseDesignerOutlets();
      }
      
      public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
      {
         // Return true for supported orientations
         if (UserInterfaceIdiomIsPhone)
         {
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
         } else
         {
            return true;
         }
      }
   }
}

