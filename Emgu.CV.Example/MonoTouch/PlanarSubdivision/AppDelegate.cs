//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PlanarSubdivisionExample
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
      public override bool FinishedLaunching(UIApplication app, NSDictionary options)
      {
         // create a new window instance based on the screen size
         window = new UIWindow(UIScreen.MainScreen.Bounds);
			
         UIViewController viewController = new UIViewController();

         UIImageView imageView = new UIImageView(window.Frame);
         viewController.Add(imageView);

         Image<Bgr, Byte> image = DrawSubdivision.Draw(600, 20);
         imageView.Image = image.ToUIImage();

         window.RootViewController = viewController;
			
         // make the window visible
         window.MakeKeyAndVisible();
			
         return true;
      }
   }
}

