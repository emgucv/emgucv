//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Example.MonoTouch
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
            using (Image<Bgr, byte> stopSignModel = new Image<Bgr, byte>("stop-sign-model.png"))
            using (Mat image = CvInvoke.Imread("stop-sign.jpg", Emgu.CV.CvEnum.LoadImageType.AnyColor))
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
               using (Image<Bgr, byte> resized = image.ToImage<Bgr, Byte>().Resize(frameSize.Width, frameSize.Height, Emgu.CV.CvEnum.Inter.Cubic, true))
               {
                  MessageText = String.Format("Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);
                  SetImage(resized);
               }

            }
         };

      }
   }
}

