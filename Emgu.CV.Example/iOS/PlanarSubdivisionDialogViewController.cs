//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
using PlanarSubdivisionExample;

namespace Example.iOS
{
    public class PlanarSubdivisionDialogViewController : DialogViewController
    {
        public PlanarSubdivisionDialogViewController()
         : base(new RootElement(""), true)
        {
        }

      public override void ViewDidLoad()
      {
         base.ViewDidLoad();
           RootElement root = Root;
         root.UnevenRows = true;
            UIImageView imageView = new UIImageView(View.Frame);
            //StringElement messageElement = new StringElement("");
         Section imageSection = new Section() {imageView};
            root.Add(new Section()
                 { new StyledStringElement("Process", delegate {

            using (Mat resized = DrawSubdivision.Draw(Math.Min( (int) View.Frame.Width, (int) View.Frame.Height) , 20))
            //using (Image<Bgr, Byte> resized = result.Resize((int)View.Frame.Width, (int)View.Frame.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN, true))
            {
               imageView.Frame = new RectangleF(PointF.Empty, resized.Size);
               imageView.Image = resized.ToUIImage();
            }
            //messageElement.Value = String.Format("Detection Time: {0} milliseconds.", processingTime);
            //messageElement.GetImmediateRootElement().Reload(messageElement, UITableViewRowAnimation.Automatic);

            imageView.SetNeedsDisplay();
               this.ReloadData();
            }
            )});
            //root.Add(new Section() {messageElement});

            root.Add(imageSection);
      }
    }
}

