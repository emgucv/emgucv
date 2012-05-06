using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MonoDroid.Dialog
{
    public class ImageElement : Element
    {
		// Height for rows
		const int dimx = 48;
		const int dimy = 44;
		
		// radius for rounding
		const int roundPx = 12;
		
		int color = 0xff4242;
		
		public ImageView Value;
		ImageView scaled;
		
		public ImageElement(ImageView image) : base("")
        {
        	if (image == null){
				Value = MakeEmpty ();
				scaled = Value;
			} 
			else {
				Value = image;			
				scaled = Scale (Value);
			}
		}
		
		ImageView MakeEmpty()
		{
			return new ImageView(null);	
		}
		
		ImageView Scale (ImageView source)
		{ 
			BitmapDrawable drawable = (BitmapDrawable) source.Drawable;
			Bitmap bitmap =  drawable.Bitmap;
	       	Bitmap bMapScaled = Bitmap.CreateScaledBitmap(bitmap, dimx, dimy, true);
        	source.SetImageBitmap(bMapScaled);
			return source;
		}
		
		protected override void Dispose (bool disposing)
		{
			if (disposing){
				scaled.Dispose ();
				Value.Dispose ();
			}
			base.Dispose (disposing);
		}
		
		public override View GetView (Context context, View convertView, ViewGroup parent)
		{
			var view = convertView as RelativeLayout;
			
			if (view == null)
				view = new RelativeLayout(context);
						
            var parms = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                                                        ViewGroup.LayoutParams.WrapContent);
            parms.SetMargins(5, 2, 5, 2);
            parms.AddRule(LayoutRules.AlignParentLeft);
			
			view.AddView(scaled,parms);

            return view;
		}
	}
}