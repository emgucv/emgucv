//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using PedestrianDetection;

namespace Emgu.CV.Example.MonoTouch
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
            using (Image<Bgr, byte> image = new Image<Bgr, byte>("pedestrian.png"))
            {
               Rectangle[] pedestrians = FindPedestrian.Find(
                        image.Mat, false, 
                        out processingTime
               );
               foreach (Rectangle rect in pedestrians)
               {
                  image.Draw(rect, new Bgr(Color.Red), 1);
               }
               Size frameSize = FrameSize;
               using (Image<Bgr, Byte> resized = image.Resize(frameSize.Width, frameSize.Height, Emgu.CV.CvEnum.Inter.Nearest, true))
               {
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

