//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
using Foundation;
using UIKit;
using FaceDetection;

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
         OnButtonClick += delegate
         {
            long processingTime;
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>("lena.jpg"))
            {
               List<Rectangle> faces = new List<Rectangle>();
               List<Rectangle> eyes = new List<Rectangle>();
               DetectFace.Detect(
                        image.Mat,
                        "haarcascade_frontalface_default.xml",
                        "haarcascade_eye.xml",
                        faces,
                        eyes, 
                        out processingTime
               );
               foreach (Rectangle face in faces)
                  image.Draw(face, new Bgr(Color.Red), 1);
               foreach (Rectangle eye in eyes)
                  image.Draw(eye, new Bgr(Color.Blue), 1);
               Size frameSize = FrameSize;
               using (Image<Bgr, Byte> resized =image.Resize(frameSize.Width, frameSize.Height, Emgu.CV.CvEnum.Inter.Nearest, true))
               {
                  SetImage(resized);
               }
            }
            MessageText = String.Format(
                    "Processing Time: {0} milliseconds.",
                    processingTime
            );
                
         };
      }

   }
}


