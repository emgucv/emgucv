//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TrafficSignRecognition
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
            using (Image<Bgr, byte> stopSignModel = new Image<Bgr, byte>("stop-sign-model.png"))
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>("stop-sign.jpg"))
            {
               Stopwatch watch = Stopwatch.StartNew(); // time the detection process

               List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, byte>>();
               List<Rectangle> stopSignBoxList = new List<Rectangle>();
               StopSignDetector detector = new StopSignDetector(stopSignModel);
               detector.DetectStopSign(image, stopSignList, stopSignBoxList);

               watch.Stop(); //stop the timer
               messageElement.Value = String.Format("Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);
               messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);
               foreach (Rectangle rect in stopSignBoxList)
               {
                  image.Draw(rect, new Bgr(Color.Red), 2);
               }
               imageView.Image = image.ToUIImage();
               imageView.SetNeedsDisplay();
            }
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

