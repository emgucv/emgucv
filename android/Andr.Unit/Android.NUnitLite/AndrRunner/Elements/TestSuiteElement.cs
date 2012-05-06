using System;

using Android.App;
using Android.Content;
using Android.Views;

using NUnitLite;

namespace Android.NUnitLite.UI {
	
	class TestSuiteElement : TestElement {

		public TestSuiteElement (TestSuite suite) : base (suite)
		{
			if (Suite.TestCaseCount > 0)
				Indicator = ">"; // hint there's more
		}
		
		public TestSuite Suite {
			get { return Test as TestSuite; }
		}
		
		protected override string GetCaption ()
		{
			int count = Suite.TestCaseCount;
			string caption = String.Format ("<b>{0}</b><br>", Suite.Name);
			if (count == 0) {
				caption += "<font color='#ff7f00'>no test was found inside this suite</font>";
			} else if (Result == null) {
				caption += String.Format ("<font color='green'><b>{0}</b> test case{1}, <i>{2}</i></font>", 
					count, count == 1 ? String.Empty : "s", Suite.RunState);
			} else {
				int error = 0;
				int failure = 0;
				int success = 0;
				foreach (TestResult tr in Result.Results) {
					if (tr.IsError)
						error++;
					else if (tr.IsFailure)
						failure++;
					else if (tr.IsSuccess)
						success++;
				}
				
				if (Result.IsSuccess) {
					caption += String.Format ("<font color='green'><b>Success!</b> {0} test{1}</font>",
						success, success == 1 ? String.Empty : "s");
				} else if (Result.Executed) {
					caption += String.Format ("<font color='green'>{0} success,</font> <font color='red'>{1} failure{2}, {3} error{4}</font>", 
						success, failure, failure > 1 ? "s" : String.Empty,
						error, error > 1 ? "s" : String.Empty);
				}
			}
			return caption;
		}

		public override View GetView (Context context, View convertView, ViewGroup parent)
		{
			View view = base.GetView (context, convertView, parent);
			// if there are test cases inside this suite then create an activity to show them
			if (Suite.TestCaseCount > 0) {		
				view.Click += delegate {
					Intent intent = new Intent(context, typeof (TestSuiteActivity));
					intent.PutExtra ("TestSuite", Name);
					intent.AddFlags (ActivityFlags.NewTask);			
					context.StartActivity (intent);
				};
			}
			return view;
		}
	}
}