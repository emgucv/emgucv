//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
using PedestrianDetection;

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
            OnButtonClick += delegate
            {
                long processingTime;
                using (Mat image = CvInvoke.Imread("pedestrian.png", ImreadModes.Color))
                    using (HOGDescriptor hog = new HOGDescriptor())
                {
                    
                    hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
                    Stopwatch watch = Stopwatch.StartNew();
                    Rectangle[] pedestrians = FindPedestrian.Find(image, hog);
                    watch.Stop();


                    foreach (Rectangle rect in pedestrians)
                    {
                        CvInvoke.Rectangle(
                        image,
                        rect,
                        new MCvScalar(0, 0, 255),
                        1);
                    }
                    Size frameSize = FrameSize;

                    using (Mat resized = new Mat())
                    {
                        CvInvoke.ResizeForFrame(image, resized, frameSize);
                        MessageText = String.Format(
                               "Detection Time: {0} milliseconds.",
                               watch.ElapsedMilliseconds
                     );
                        SetImage(resized);
                    }
                }
            };

        }
    }
}

