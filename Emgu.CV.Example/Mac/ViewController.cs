using System;

using AppKit;
using Foundation;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Emgu.CV.Example.Mac
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			messageLabel.StringValue = String.Empty;

			// Do any additional setup after loading the view.
			//HelloWorld();

		}

		void HelloWorld()
		{
			Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
			img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

			//Draw "Hello, world." on the image using the specific font
			CvInvoke.PutText(
			   img,
			   "Hello, world",
			   new System.Drawing.Point(10, 80),
			   FontFace.HersheyComplex,
			   1.0,
			   new Bgr(0, 255, 0).MCvScalar);
			mainImageView.Image = img.ToNSImage();
		}

		void PlannarSubdivision()
		{
			Mat img = PlanarSubdivisionExample.DrawSubdivision.Draw(400, 20);
			mainImageView.Image = img.ToNSImage();
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

		partial void helloWorldClicked(NSObject sender)
		{
			HelloWorld();
		}

		partial void plannarSubdivisionClicked(NSObject sender)
		{
			PlannarSubdivision();
		}
	}
}
