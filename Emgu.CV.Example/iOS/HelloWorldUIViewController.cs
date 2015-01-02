using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Emgu.CV.Example.MonoTouch
{
    public class HelloWorldUIViewController : UIViewController
    {
        public HelloWorldUIViewController()
         : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
         if (AppDelegate.iOS7Plus)
            EdgesForExtendedLayout = UIRectEdge.None;

         /*
            MCvFont font = new MCvFont(
                Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN,
                1.0,
                1.0
            );*/

         using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(320, 240))
         {
            image.SetValue(new Bgr(255, 255, 255));
            image.Draw(
               "Hello, world",
               new Point(30, 30),
               CvEnum.FontFace.HersheyPlain,
               1.0,
               new Bgr(0, 255, 0)
            );

            UIImageView imageView = new UIImageView(image.ToUIImage());
            Add(imageView);
         }
        }
    }
}

