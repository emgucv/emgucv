using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MonoDroid.Dialog;

namespace Android.NUnitLite.UI {
	
	// can't really name it HtmlSection wrt HtmlElement ;-)
	public class FormattedSection : Section {
			
		public FormattedSection (string html) 
			: base (html)
		{
		}
		
		public override View GetView (Context context, View convertView, ViewGroup parent)
		{
			TextView tv = new TextView (context);
			tv.TextSize = 20f;
			tv.SetText (Android.Text.Html.FromHtml (Caption), TextView.BufferType.Spannable);

			var parms = new RelativeLayout.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			parms.AddRule (LayoutRules.CenterHorizontal);
			
			RelativeLayout view = new RelativeLayout (context, null, Android.Resource.Attribute.ListSeparatorTextViewStyle);
			view.AddView (tv, parms);
			return view;
		}
	}
}

