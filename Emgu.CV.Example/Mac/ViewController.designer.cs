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
		AppKit.NSImageView mainImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (mainImageView != null) {
				mainImageView.Dispose ();
				mainImageView = null;
			}
		}
	}
}
