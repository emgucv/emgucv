//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;

using AppKit;
using Foundation;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Collections.Generic;

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

		void RunHelloWorld()
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

		void RunPlannarSubdivision()
		{
			Mat img = PlanarSubdivisionExample.DrawSubdivision.Draw(400, 20);
			mainImageView.Image = img.ToNSImage();
		}

		void RunDetectFace()
		{
			//Read the files as an 8-bit Bgr image  
			NSImage nsImage = NSImage.ImageNamed("lena.jpg");
			UMat image = new UMat(nsImage); //UMat version
											//image = new Mat("lena.jpg", ImreadModes.Color); //CPU version

			long detectionTime;
			List<Rectangle> faces = new List<Rectangle>();
			List<Rectangle> eyes = new List<Rectangle>();

			FaceDetection.DetectFace.Detect(
			  image, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
			  faces, eyes,
			  out detectionTime);

			foreach (Rectangle face in faces)
				CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
			foreach (Rectangle eye in eyes)
				CvInvoke.Rectangle(image, eye, new Bgr(Color.Blue).MCvScalar, 2);

			mainImageView.Image = image.ToNSImage();
		}

		void RunFeatureMatching()
		{
			long matchTime;
			using (Mat modelImage = CvInvoke.Imread("box.png", ImreadModes.Grayscale))
			using (Mat observedImage = CvInvoke.Imread("box_in_scene.png", ImreadModes.Grayscale))
			{
				Mat result = FeatureMatchingExample.DrawMatches.Draw(modelImage, observedImage, out matchTime);
				//ImageViewer.Show(result, String.Format("Matched in {0} milliseconds", matchTime));
				mainImageView.Image = result.ToNSImage();
			}

		}

		void RunPedestrianDetection()
		{
			using (Mat image = new Mat("pedestrian.png"))
			{

				long processingTime;
				Rectangle[] results;


				using (UMat uImage = image.GetUMat(AccessType.ReadWrite))
					results = PedestrianDetection.FindPedestrian.Find(uImage, out processingTime);


				foreach (Rectangle rect in results)
				{
					CvInvoke.Rectangle(image, rect, new Bgr(Color.Red).MCvScalar);
				}
				mainImageView.Image = image.ToNSImage();
			}
		}

		private VideoCapture _capture = null;
		private Mat _frame = null;
		private bool _captureInProgress = false;

		private void ProcessFrame(object sender, EventArgs arg)
		{
			if (_capture != null && _capture.Ptr != IntPtr.Zero)
			{
				_capture.Retrieve(_frame, 0);
				var nsImage = _frame.ToNSImage();
				InvokeOnMainThread(() =>
				{
					mainImageView.Image = nsImage;
				});
			}
		}

		void RunCameraCapture()
		{
			if (_capture == null)
			{
				_capture = new VideoCapture();
				if (_capture == null || !_capture.IsOpened)
				{
					InvokeOnMainThread(() =>
					{
						messageLabel.StringValue = "unable to create capture";
					});
					_capture = null;
					return;
				}
				_capture.ImageGrabbed += ProcessFrame;
				_frame = new Mat();

			}
			if (_captureInProgress)
			{  //stop the capture
			   //captureButton.Text = "Start Capture";
				_capture.Pause();
			}
			else
			{
				//start the capture
				//captureButton.Text = "Stop";
				_capture.Start();
			}

			_captureInProgress = !_captureInProgress;

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
			RunHelloWorld();
		}

		partial void plannarSubdivisionClicked(NSObject sender)
		{
			RunPlannarSubdivision();
		}

		partial void faceDetectionClicked(NSObject sender)
		{
			RunDetectFace();
		}

		partial void featureMatchingClicked(Foundation.NSObject sender)
		{
			RunFeatureMatching();
		}

		partial void pedestrianDetectionClicked(Foundation.NSObject sender)
		{
			RunPedestrianDetection();
		}

		partial void CameraCaptureClicked(NSObject sender)
		{
			RunCameraCapture();
		}
	}
}