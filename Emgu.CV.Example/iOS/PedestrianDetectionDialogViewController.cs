//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using Emgu.CV.Models;

namespace Example.iOS
{
    public class PedestrianDetectionDialogViewController : ButtonMessageImageDialogViewController
    {
        public PedestrianDetectionDialogViewController()
           : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ButtonText = "Detect Pedestrian";
            OnButtonClick += async delegate
            {
                using (Mat image = CvInvoke.Imread("pedestrian.png", ImreadModes.Color))
                {
                    Emgu.CV.Models.PedestrianDetector detector = new Emgu.CV.Models.PedestrianDetector();
                    await detector.Init(DownloadManager_OnDownloadProgressChanged);
                    SetMessage(detector.ProcessAndRender(image, image));
                    Mat resized = new Mat();

                    CvInvoke.Resize(image, resized, FrameSize, 0, 0, Inter.Linear);
                    SetImage(resized);
                }
            };

        }
    }
}

