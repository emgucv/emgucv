//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using FeatureMatchingExample;

namespace Example.iOS
{
   public class FeatureMatchingDialogViewController : ButtonMessageImageDialogViewController
   {
      public FeatureMatchingDialogViewController()
         : base()
      {
      }

      public override void ViewDidLoad()
      {
         base.ViewDidLoad();
         ButtonText = "Match";
         base.OnButtonClick +=
            delegate
         {
            long processingTime;
            Size frameSize = FrameSize;
            using (Mat modelImage = CvInvoke.Imread("box.png", Emgu.CV.CvEnum.ImreadModes.Grayscale))
            using (Mat observedImage = CvInvoke.Imread("box_in_scene.png", Emgu.CV.CvEnum.ImreadModes.Grayscale))
            using (Mat image = DrawMatches.Draw(modelImage, observedImage, out processingTime))
            using (Mat resized = new Mat())
            {
               double dx = ((double) frameSize.Width) / image.Width ;
               double dy = ((double) frameSize.Height) / image.Height;
               double min = Math.Min(dx, dy);
               CvInvoke.Resize(image, resized, Size.Empty, min, min); 
               //image.Resize(frameSize.Width, frameSize.Height, Emgu.CV.CvEnum.Inter.Nearest, true)
               MessageText = String.Format("Matching Time: {0} milliseconds.", processingTime);
               SetImage(resized);
            }
         };
      }
   }
}

