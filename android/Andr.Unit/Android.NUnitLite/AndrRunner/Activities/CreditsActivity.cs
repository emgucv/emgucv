using System;

using Android.App;
using Android.OS;

using MonoDroid.Dialog;

namespace Android.NUnitLite.UI {
	
	[Activity (Label = "Credits")]
	public class CreditsActivity : DialogActivity {
		
		const string notice = "<br><b>Andr.Unit Runner</b><br>Copyright &copy; 2011 Xamarin Inc.<br>All rights reserved.<br><br>Author: Sebastien Pouliot<br>";

      public CreditsActivity()
         : base(null)
      {
      }

		protected override void OnCreate (Bundle bundle)
		{
			Root = new RootElement (String.Empty) {
				new FormattedSection (notice) {
					new HtmlElement ("About Xamarin", "http://xamarin.com"),
					new HtmlElement ("About Mono for Android", "http://android.xamarin.com"),
					new HtmlElement ("About MonoDroid.Dialog", "https://github.com/spouliot/MonoDroid.Dialog"),
					new HtmlElement ("About NUnitLite", "http://www.nunitlite.org")
				}
			};
			
			base.OnCreate (bundle);
		}
	}
}