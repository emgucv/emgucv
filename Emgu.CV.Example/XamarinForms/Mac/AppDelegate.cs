using AppKit;
using Foundation;

namespace OSX
{
	[Register("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate()
		{
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			// Insert code here to initialize your application
			global::Xamarin.Forms.Init();

		}

		public override void WillTerminate(NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}
