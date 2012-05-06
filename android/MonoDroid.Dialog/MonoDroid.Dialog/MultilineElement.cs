using Android.Content;

namespace MonoDroid.Dialog
{
    public class MultilineElement : StringElement, IElementSizing
    {
        public MultilineElement(string caption, string value) : base(caption)
        {
        }

        public float GetHeight()
        {
            return 0.0f;
        }
    }
}