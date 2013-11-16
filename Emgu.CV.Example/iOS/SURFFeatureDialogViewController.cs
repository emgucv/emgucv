//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SURFFeatureExample;

namespace Emgu.CV.Example.MonoTouch
{
   public class SURFFeatureDialogViewController : ButtonMessageImageDialogViewController
   {
      public SURFFeatureDialogViewController()
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
            using (Image<Gray, byte> modelImage = new Image<Gray, byte>("box.png"))
            using (Image<Gray, byte> observedImage = new Image<Gray, byte>("box_in_scene.png"))
            using (Image<Bgr, Byte> image = DrawMatches.Draw(modelImage, observedImage, out processingTime))
            using (Image<Bgr, Byte> resized =image.Resize(frameSize.Width, frameSize.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN, true))
            {
               MessageText = String.Format("Matching Time: {0} milliseconds.", processingTime);
               SetImage(resized);
            }
         };
      }
   }
}

