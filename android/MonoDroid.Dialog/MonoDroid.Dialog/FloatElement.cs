using Android.Widget;

namespace MonoDroid.Dialog
{
	public class FloatElement : Element
	{
		public bool ShowCaption;
		public float Value;
		public float MinValue, MaxValue;
	    public ImageView Left;
	    public ImageView Right;

	    public FloatElement(string caption) : base(caption)
		{
		}

        public FloatElement(ImageView left, ImageView right, float value)
            : base(null)
        {
            Left = left;
            Right = right;
            MinValue = 0;
            MaxValue = 1;
            Value = value;
        }
	}
}