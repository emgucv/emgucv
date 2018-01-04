//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
            using (Mat image = CvInvoke.Imread ("pedestrian.png", ImreadModes.Color))
            {
               Rectangle[] pedestrians = FindPedestrian.Find(
                        image, 
                        out processingTime
               );

               foreach (Rectangle rect in pedestrians)
               {
                  CvInvoke.Rectangle (
                     image,
                     rect,
                     new MCvScalar (0, 0, 255),
                     1);
               }
               Size frameSize = FrameSize;
              
               using (Mat resized = new Mat())
               {
                  CvInvoke.ResizeForFrame(image, resized, frameSize);
                  MessageText = String.Format(
                            "Detection Time: {0} milliseconds.",
                            processingTime
                  );
                  SetImage(resized);
               }
            }
         };
           
      }
   }
}

