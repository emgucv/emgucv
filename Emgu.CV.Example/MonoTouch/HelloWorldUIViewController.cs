using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

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

            MCvFont font = new MCvFont(
                Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN,
                1.0,
                1.0
            );
            using (Image<Bgr, Byte> image = new Image<Bgr, Byte>(320, 240))
            {
                image.SetValue(new Bgr(255, 255, 255));
                image.Draw(
                    "Hello, world",
                    ref font,
                    new Point(30, 30),
                    new Bgr(0, 255, 0)
                );

                UIImageView imageView = new UIImageView(image.ToUIImage());
                Add(imageView);
            }
        }
    }
}

