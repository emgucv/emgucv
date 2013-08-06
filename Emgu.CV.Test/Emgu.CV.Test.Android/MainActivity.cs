using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;
using Android.Content.Res;
using Emgu.CV.Structure;

namespace Emgu.CV.Test
{
	[Activity (Label = "Emgu.CV.Test.Android", MainLauncher = true)]
	public class MainActivity : TestSuiteActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
//dummy code to load emgucv;
         Image<Bgr, byte> dummy = new Image<Bgr, byte>(2, 2);
         dummy._Not();
         

         AssetsUtil.Assets = this.Assets;
			// tests can be inside the main assembly
			AddTest (Assembly.GetExecutingAssembly ());
			// or in any reference assemblies
			// AddTest (typeof (Your.Library.TestClass).Assembly);

			// Once you called base.OnCreate(), you cannot add more assemblies.
			base.OnCreate (bundle);
		}
	}
   public static class AssetsUtil
   {
      public static AssetManager Assets { get; set; }

      public static Image<TColor, TDepth> LoadImage<TColor, TDepth>(string name)
         where TColor : struct, IColor
         where TDepth : new()
      {
         return new Image<TColor, TDepth>(Assets, name);
      }
   }
}

