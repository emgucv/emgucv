using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;

using MonoDroid.Dialog;

namespace Android.NUnitLite.UI {
	
	[Activity (Label = "Options", WindowSoftInputMode = SoftInput.AdjustPan,
		ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation)]
	public class OptionsActivity : DialogActivity {
		BooleanElement remote;
		EntryElement host_name;
		EntryElement host_port;

      public OptionsActivity()
         : base(null)
      {
      }
		
		protected override void OnCreate (Bundle bundle)
		{
			Options options = AndroidRunner.Runner.Options;
			remote = new BooleanElement ("Remote Server", options.EnableNetwork);
			host_name = new EntryElement ("HostName", String.Empty, options.HostName);
			host_port = new EntryElement ("Port", String.Empty, options.HostPort.ToString ());
			
			Root = new RootElement ("Options") {
				new Section () { remote, host_name, host_port }
			};
			
			base.OnCreate (bundle);
		}
		
		int GetPort ()
		{
			int port;
			ushort p;
			if (UInt16.TryParse (host_port.Value, out p))
				port = p;
			else
				port = -1;
			return port;
		}
		
		protected override void OnPause ()
		{
			Options options = AndroidRunner.Runner.Options;
			options.EnableNetwork = remote.Value;
			options.HostName = host_name.Value;
			options.HostPort = GetPort ();
			options.Save (this);
			base.OnPause ();
		}
	}
}