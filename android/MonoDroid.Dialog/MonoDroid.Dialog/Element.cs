using System;
using Android.Views;
using Android.Content;

namespace MonoDroid.Dialog
{
    public class Element : IDisposable
    {
        /// <summary>
        ///  The caption to display for this given element
        /// </summary>
        public string Caption;

        /// <summary>
        ///  Handle to the container object.
        /// </summary>
        /// <remarks>
        /// For sections this points to a RootElement, for every
        /// other object this points to a Section and it is null
        /// for the root RootElement.
        /// </remarks>
        public Element Parent;

		public Action Click;
		public Action LongClick;

        /// <summary>
        ///  Initializes the element with the given caption.
        /// </summary>
        /// <param name="caption">
        /// The caption.
        /// </param>
        public Element(string caption)
        {
            Caption = caption;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Returns a summary of the value represented by this object, suitable 
        /// for rendering as the result of a RootElement with child objects.
        /// </summary>
        /// <returns>
        /// The return value must be a short description of the value.
        /// </returns>
        public virtual string Summary()
        {
            return "";
        }

        public virtual View GetView(Context context, View convertView, ViewGroup parent)
        {
			var view = new View(context);
			return view;
        }

		//public virtual void Selected()
		//{
		//}
				
        public virtual bool Matches(string text)
        {
            if (Caption == null)
                return false;
            return Caption.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
        }
    }
}