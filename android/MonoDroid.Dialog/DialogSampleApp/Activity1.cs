using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MonoDroid.Dialog;

namespace DialogSampleApp
{
    [Activity(Label = "MD.D Sample", MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			
			
			ImageView img = new ImageView(this);
			img.SetImageResource(Resource.Drawable.spadeicon);
			
            var root = new RootElement("Test Root Elem")
                           {
                               new Section("Test Header", "Test Footer")
                                   {
                                       new BooleanElement("Push my button", true),
                                       new BooleanElement("Push this too", false),
                                       new StringElement("Text label", "The Value"),
									   new BooleanElement("Push my button", true),
                                       new BooleanElement("Push this too", false),
                                   },
                               new Section("Part II")
                                   {
                                       new StringElement("This is the String Element", "The Value"),
                                       new CheckboxElement("Check this out", true),
                                       new ImageElement(img),
									   new HtmlElement("Go to Google.com","http://www.google.com")
                                   }
                           };

            var da = new DialogAdapter(this, root);

            var lv = new ListView(this) {Adapter = da};

            SetContentView(lv);
        }
    }
}