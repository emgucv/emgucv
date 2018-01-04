//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using TrafficSignRecognition;

namespace Example.iOS
{
   public class TrafficSignRecognitionDialogViewController : ButtonMessageImageDialogViewController
   {
      public TrafficSignRecognitionDialogViewController()
         : base()
      {
      }

      public override void ViewDidLoad()
      {
         base.ViewDidLoad();
         ButtonText = "Detect Stop Sign";
         OnButtonClick +=
         delegate
         {
            using (Mat stopSignModel = new Mat("stop-sign-model.png"))
            using (Mat image = new Mat("stop-sign.jpg"))
            {
               Stopwatch watch = Stopwatch.StartNew(); // time the detection process

               List<Mat> stopSignList = new List<Mat>();
               List<Rectangle> stopSignBoxList = new List<Rectangle>();
               StopSignDetector detector = new StopSignDetector(stopSignModel);
               detector.DetectStopSign(image, stopSignList, stopSignBoxList);

               watch.Stop(); //stop the timer
               foreach (Rectangle rect in stopSignBoxList)
               {
                  CvInvoke.Rectangle(image, rect, new MCvScalar(0, 0, 255), 2);
               }
               Size frameSize = FrameSize;
               using (Mat resized = new Mat())
               {
                  CvInvoke.ResizeForFrame(image, resized, frameSize);
                  MessageText = String.Format("Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);
                  SetImage(resized);
               }

            }
         };

      }
   }
}

