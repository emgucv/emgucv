using System;

using Android.App;
using Android.Content;

namespace Android.NUnitLite {
	
	public class Options {
		
		public Options ()
		{
		}
		
		// Options are not as useful as under iOS since re-installing the
		// application deletes the file containing them.
		internal Options (Activity activity)
		{
			ISharedPreferences prefs = activity.GetSharedPreferences ("options", FileCreationMode.Private);
			EnableNetwork = prefs.GetBoolean ("remote", false);
			HostName = prefs.GetString ("hostName", "0.0.0.0");
			HostPort = prefs.GetInt ("hostPort", -1);
		}
		
		public bool EnableNetwork { get; set; }
		
		public string HostName { get; set; }
		
		public int HostPort { get; set; }
		
		public bool ShowUseNetworkLogger {
			get { return (EnableNetwork && !String.IsNullOrWhiteSpace (HostName) && (HostPort > 0)); }
		}
		
		public void Save (Activity activity)
		{
			ISharedPreferences prefs = activity.GetSharedPreferences ("options", FileCreationMode.Private);
			var edit = prefs.Edit ();
			edit.PutBoolean ("remote", EnableNetwork);
			edit.PutString ("hostName", HostName);
			edit.PutInt ("hostPort", HostPort);
			edit.Commit ();
		}
	}
}