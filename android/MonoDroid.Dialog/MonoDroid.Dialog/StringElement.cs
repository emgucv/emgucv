using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class StringElement : Element
    {
        public string Value;
        private TextView _caption;
        private TextView _text;

        public StringElement(string caption) : base(caption)
        {
        }

        public StringElement(string caption, string value)
            : base(caption)
        {
            Value = value;
        }

        public StringElement(string caption, Action click)
            : base(caption)
        {
			this.Click = click;
        }

        public object Alignment;

		public override View GetView(Context context, View convertView, ViewGroup parent)
        {
			var view = convertView as RelativeLayout;
			
			if (view == null)
				view = new RelativeLayout(context);
						
            var parms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                        ViewGroup.LayoutParams.WrapContent);
            parms.SetMargins(5, 3, 5, 0);
            parms.AddRule( LayoutRules.AlignParentLeft);

            _caption = new TextView(context) {Text = Caption, TextSize = 16f};
            view.AddView(_caption, parms);

            if (!string.IsNullOrEmpty(Value))
            {
                var tparms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                             ViewGroup.LayoutParams.WrapContent);
                tparms.SetMargins(5, 3, 5, 0);
                tparms.AddRule( LayoutRules.AlignParentRight);

                _text = new TextView(context) {Text = Value, TextSize = 16f};
                view.AddView(_text, tparms);
            }

            return view;
        }

        public override string Summary()
        {
            return Caption;
        }

        public override bool Matches(string text)
        {
            return (Value != null ? Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 : false) ||
                   base.Matches(text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _caption.Dispose();
                _caption = null;
                _text.Dispose();
                _text = null;
            }
        }
    }
}