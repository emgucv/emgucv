using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace HelloWorld
{
   // The UIApplicationDelegate for the application. This class is responsible for launching the 
   // User Interface of the application, as well as listening (and optionally responding) to 
   // application events from iOS.
   [Register ("AppDelegate")]
   public partial class AppDelegate : UIApplicationDelegate
   {
      // class-level declarations
      UIWindow window;

      //
      // This method is invoked when the application has loaded and is ready to run. In this 
      // method you should instantiate the window, load the UI into it and then make the window
      // visible.
      //
      // You have 17 seconds to return from this method, or iOS will terminate your application.
      //
      public override bool FinishedLaunching (UIApplication app, NSDictionary options)
      {
         window = new UIWindow (UIScreen.MainScreen.Bounds);
         UIViewController viewController = new UIViewController ();
         MCvFont font = new MCvFont (Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
         using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(320, 240))
         {
            image.Draw ("Hello, world", ref font, new Point (30, 30), new Bgr (255, 255, 255));

            UIImageView imageView = new UIImageView (window.Frame);
            viewController.Add (imageView);
            imageView.Image = UIImage.FromImage (image.ToCGImage ());
         }

         window.RootViewController = viewController;
         window.MakeKeyAndVisible ();
         
         return true;
      }
   }
}

