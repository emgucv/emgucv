//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
    public class SURFFeatureDialogViewController : DialogViewController
    {
        public SURFFeatureDialogViewController()
         : base(new RootElement("SURF Feature"), true)
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
            using (Image<Gray, byte> modelImage = new Image<Gray, byte>("box.png"))
            using (Image<Gray, byte> observedImage = new Image<Gray, byte>("box_in_scene.png"))
            using (Image<Bgr, Byte> image = DrawMatches.Draw(modelImage, observedImage, out processingTime))
            using (Image<Bgr, Byte> resized =image.Resize((int) View.Frame.Width, (int) View.Frame.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN, true))
            {
               imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
               imageView.Image  = resized.ToUIImage();

            }
            messageElement.Value = String.Format("Matching Time: {0} milliseconds.", processingTime);
            messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);

            imageView.SetNeedsDisplay();
         }
         )}
            );
            root.Add(new Section() {messageElement});
            root.Add(new Section() {imageView});
        }
    }
}

