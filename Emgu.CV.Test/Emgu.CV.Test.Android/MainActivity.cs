using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;
using Android.Content;
using Android.Content.Res;
using Emgu.CV.Structure;
using Emgu.Util;

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
         AssetsUtil.Context = this;

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
      public static Context Context { get; set; }
      public static Image<TColor, TDepth> LoadImage<TColor, TDepth>(string name)
         where TColor : struct, IColor
         where TDepth : new()
      {
         return new Image<TColor, TDepth>(AssetsUtil.Assets, name);
      }

      public static Mat LoadMat(string name)
      {
         return new Mat(AssetsUtil.Assets, name);
      }

      public static string LoadFile(string assetName)
      {
         AndroidFileAsset.OverwriteMethod method = AndroidFileAsset.OverwriteMethod.AlwaysOverwrite;
         System.IO.FileInfo fi = AndroidFileAsset.WritePermanantFileAsset(Context, assetName, "assetCache", method);
         return fi.FullName;
      }
   }
}

