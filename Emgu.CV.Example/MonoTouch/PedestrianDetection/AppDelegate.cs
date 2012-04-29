//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PedestrianDetection
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
			
         RootElement root = new RootElement("");
         UIImageView imageView = new UIImageView(window.Frame);
         StringElement messageElement = new StringElement("");

         root.Add(new Section()
                 { new StyledStringElement("Process", delegate {
            long processingTime;
            using (Image<Bgr, Byte> image = FindPedestrian.Find("pedestrian.png",  out processingTime))
            using (Image<Bgr, Byte> resized =image.Resize((int)window.Frame.Width, (int)window.Frame.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN, true))
            {
               imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
               imageView.Image = resized.ToUIImage();
            }
            messageElement.Value = String.Format("Detection Time: {0} milliseconds.", processingTime);
            messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);

            imageView.SetNeedsDisplay();
         }
         )});
         root.Add(new Section() {messageElement});
         root.Add(new Section() {imageView});

         DialogViewController viewController = new DialogViewController(root);

         window.RootViewController = viewController;
			
         // make the window visible
         window.MakeKeyAndVisible();
			
         return true;
      }
   }
}

