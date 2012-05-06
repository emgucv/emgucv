using System;

using Android.Content;
using Android.Views;

using MonoDroid.Dialog;

namespace Android.NUnitLite.UI {
	
	public class ActivityElement : StringElement {
		
		Type activity;
		
		public ActivityElement (string name, Type type) : base (name)
		{
			activity = type;
			Value = ">"; // hint there's something more to show
		}
		
		public override View GetView (Context context, View convertView, ViewGroup parent)
		{
			View view = base.GetView (context, convertView, parent);
			view.Click += delegate {
				Intent intent = new Intent (context, activity);
				intent.AddFlags (ActivityFlags.NewTask);			
				context.StartActivity (intent);
			};
			return view;
		}
	}
}

