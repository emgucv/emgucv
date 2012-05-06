using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using MonoDroid.Dialog;
using NUnitLite;

namespace Android.NUnitLite.UI {
	
	[Activity (Label = "Results")]			
	public class TestResultActivity : Activity {
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			string test_case = Intent.GetStringExtra ("TestCase");
			
			TestResult result = AndroidRunner.Results [test_case];

			string error = String.Format ("<b>{0}<b><br><font color='grey'>{1}</font>", 
				result.Message, result.StackTrace.Replace (System.Environment.NewLine, "<br>"));
			
			var menu = new RootElement (String.Empty) {
				new Section (test_case) {
					new FormattedElement (error)
				}
			};
			
			var da = new DialogAdapter (this, menu);
			var lv = new ListView (this) {
				Adapter = da
			};
			SetContentView (lv);
		}
	}
}