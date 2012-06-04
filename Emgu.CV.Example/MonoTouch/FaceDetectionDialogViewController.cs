//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FaceDetection;

namespace Emgu.CV.Example.MonoTouch
{
    public class FaceDetectionDialogViewController : DialogViewController
    {
        public FaceDetectionDialogViewController()
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
            long processingTime;
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>("lena.jpg"))
            {
               DetectFace.DetectAndDraw(image, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml", out processingTime);
               using (Image<Bgr, Byte> resized =image.Resize((int) View.Frame.Width, (int)View.Frame.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN, true))
               {
                  imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
                  imageView.Image = resized.ToUIImage();
               }
            }
            messageElement.Value = String.Format("Processing Time: {0} milliseconds.", processingTime);
            messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);

            imageView.SetNeedsDisplay();
         }
         )});
         root.Add(new Section() {messageElement});
         root.Add(new Section() {imageView});
      }
    }
}

