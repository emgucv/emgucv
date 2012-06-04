//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TrafficSignRecognition;

namespace Emgu.CV.Example.MonoTouch
{
    public class TrafficSignRecognitionDialogViewController : DialogViewController
    {
        public TrafficSignRecognitionDialogViewController()
         : base(new RootElement(""), true)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            RootElement root = Root;
            UIImageView imageView = new UIImageView(View.Frame);

            StringElement messageElement = new StringElement("");


            root.Add(new Section()
                 { new StyledStringElement("Process", delegate {
            using (Image<Bgr, byte> stopSignModel = new Image<Bgr, byte>("stop-sign-model.png"))
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>("stop-sign.jpg"))
            {
               Stopwatch watch = Stopwatch.StartNew(); // time the detection process

               List<Image<Gray, Byte>> stopSignList = new List<Image<Gray, byte>>();
               List<Rectangle> stopSignBoxList = new List<Rectangle>();
               StopSignDetector detector = new StopSignDetector(stopSignModel);
               detector.DetectStopSign(image, stopSignList, stopSignBoxList);

               watch.Stop(); //stop the timer
               messageElement.Value = String.Format("Detection time: {0} milli-seconds", watch.Elapsed.TotalMilliseconds);
               messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);
               foreach (Rectangle rect in stopSignBoxList)
               {
                  image.Draw(rect, new Bgr(Color.Red), 2);
               }
              using (Image<Bgr, byte> resized = image.Resize((int)View.Frame.Width, (int)View.Frame.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true))
              {
                 imageView.Image = resized.ToUIImage();
                 imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
              }
               imageView.SetNeedsDisplay();
            }
         }
         )}
            );
            root.Add(new Section() {messageElement});
            root.Add(new Section() {imageView});
        }
    }
}

