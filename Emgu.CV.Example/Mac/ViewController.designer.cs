// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Emgu.CV.Example.Mac
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton helloWorldButton { get; set; }

		[Outlet]
		AppKit.NSImageView mainImageView { get; set; }

		[Outlet]
		AppKit.NSTextField messageLabel { get; set; }

		[Action ("CameraCaptureClicked:")]
		partial void CameraCaptureClicked (Foundation.NSObject sender);

		[Action ("faceDetectionClicked:")]
		partial void faceDetectionClicked (Foundation.NSObject sender);

		[Action ("featureMatchingClicked:")]
		partial void featureMatchingClicked (Foundation.NSObject sender);

		[Action ("helloWorldClicked:")]
		partial void helloWorldClicked (Foundation.NSObject sender);

		[Action ("pedestrianDetectionClicked:")]
		partial void pedestrianDetectionClicked (Foundation.NSObject sender);

		[Action ("plannarSubdivisionClicked:")]
		partial void plannarSubdivisionClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (helloWorldButton != null) {
				helloWorldButton.Dispose ();
				helloWorldButton = null;
			}

			if (mainImageView != null) {
				mainImageView.Dispose ();
				mainImageView = null;
			}

			if (messageLabel != null) {
				messageLabel.Dispose ();
				messageLabel = null;
			}
		}
	}
}
