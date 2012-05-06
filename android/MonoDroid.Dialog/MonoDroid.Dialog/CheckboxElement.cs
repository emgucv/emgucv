using Android.Content;
using Android.Views;
using Android.Widget;

namespace MonoDroid.Dialog
{
    public class CheckboxElement : Element
    {
        private CheckBox _cb;
        public bool Value;
        public string Group;

        public CheckboxElement(string caption)
            : base(caption)
        {
        }

        public CheckboxElement(string caption, bool value)
            : this(caption)
        {
            Value = value;
        }

        public CheckboxElement(string caption, bool value, string group)
            : this(caption, value)
        {
            Group = group;
        }

		public override View GetView(Context context, View convertView, ViewGroup parent)
        {
            _cb = new CheckBox(context) {Checked = Value, Text = Caption};
            return _cb;
        }

		//public override void Selected()
		//{
		//    Value = !Value;
		//    _cb.Checked = Value;
		//}
    }
}