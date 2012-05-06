using System;

using Android.Content;
using Android.Views;

using MonoDroid.Dialog;

namespace Android.NUnitLite.UI {
	
	public class ActionElement : StringElement {
		
		Action action;
		
		public ActionElement (string name, Action action) : base (name)
		{
			this.action = action;
			Value = "..."; // hint some action will take place
		}
		
		public override View GetView (Context context, View convertView, ViewGroup parent)
		{
			View view = base.GetView (context, convertView, parent);
			view.Click += delegate {
				// FIXME: show activity/progress
				action ();
			};
			return view;
		}
	}
}