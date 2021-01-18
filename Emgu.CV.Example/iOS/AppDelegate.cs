//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV;
using Foundation;
using UIKit;

namespace Example.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow _window;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            CvInvokeIOS.Init();

            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            UINavigationController navControl = new UINavigationController();
            navControl.PushViewController(new ExamplesDialogViewController(), false);

            _window.RootViewController = navControl;
            _window.MakeKeyAndVisible();

            return true;
        }

        public static bool iOS7Plus
        {
            get
            {
                System.Version version = new Version(UIDevice.CurrentDevice.SystemVersion);
                return version.Major >= 7;
            }
        }
    }
}

