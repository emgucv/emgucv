//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using Emgu.CV.Models;

namespace Example.iOS
{
    public class FaceDetectionDialogViewController : ButtonMessageImageDialogViewController
    {
        public FaceDetectionDialogViewController()
           : base()
        {
        }



        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ButtonText = "Detect Face & Eyes";
            OnButtonClick += async delegate
            {
                //Read the files as an 8-bit Bgr image  
                using (Mat image = CvInvoke.Imread("lena.jpg", ImreadModes.Color))
                {
                    Emgu.CV.Models.FaceAndLandmarkDetector detector = new Emgu.CV.Models.FaceAndLandmarkDetector();
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


