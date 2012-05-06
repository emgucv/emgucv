using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using MonoDroid.Dialog;
using NUnitLite;

namespace Android.NUnitLite.UI {

	[Activity (Label = "Tests")]			
	public class TestSuiteActivity : Activity {
		
		string test_suite;
		TestSuite suite;
		Section main;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			test_suite = Intent.GetStringExtra ("TestSuite");
			suite = AndroidRunner.Suites [test_suite];

			var menu = new RootElement (String.Empty);
			
			main = new Section (test_suite);
			foreach (ITest test in suite.Tests) {
				TestSuite ts = test as TestSuite;
				if (ts != null)
					main.Add (new TestSuiteElement (ts));
				else
					main.Add (new TestCaseElement (test as TestCase));
			}
			menu.Add (main);

			Section options = new Section () {
				new ActionElement ("Run all", Run),
			};
			menu.Add (options);

			var da = new DialogAdapter (this, menu);
			var lv = new ListView (this) {
				Adapter = da
			};
			SetContentView (lv);
		}
		
		public void Run ()
		{
			AndroidRunner runner = AndroidRunner.Runner;
			if (!runner.OpenWriter ("Run " + test_suite, this))
				return;
			
			try {
				foreach (ITest test in suite.Tests) {
					test.Run (runner);
				}
			}
			finally {
				runner.CloseWriter ();
			}
			
			foreach (TestElement te in main) {
				te.Update ();
			}
		}
	}
}