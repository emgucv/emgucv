using System.Reflection;

using Android.App;
using Android.OS;
using Android.NUnitLite;
using Android.NUnitLite.UI;

using Android.Content.Res;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using Android.Graphics;

namespace Emgu.CV.Test
{
   [Activity(Label = "Emgu.CV.Test", MainLauncher = true)]
   public class MainActivity : RunnerActivity
   {
      protected override void OnCreate(Bundle bundle)
      {
         // tests can be inside the main assembly
         Add(Assembly.GetExecutingAssembly());
         // or in any reference assemblies			
         //Add (typeof (m4a.tests.RunnerTest).Assembly);
         // or in any assembly that you load (since JIT is available)


         AssetsUtil.Assets = this.Assets;
#if false
			// you can use the default or set your own custom writer (e.g. save to web site and tweet it ;-)
			Runner.Writer = new TcpTextWriter ("10.0.1.2", 16384);
			// start running the test suites as soon as the application is loaded
			Runner.AutoStart = true;
			// crash the application (to ensure it's ended) and return to springboard
			Runner.TerminateAfterExecution = true;
#endif
         // you cannot add more assemblies once calling base
         base.OnCreate(bundle);
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