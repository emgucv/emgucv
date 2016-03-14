//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Emgu.CV;
using Emgu.CV.Cuda;

namespace AndroidExamples
{
   [Activity(Label = "Emgu CV Examples", MainLauncher = true)]
   public class AndroidExamplesActivity : Activity
   {
      private IMenuItem _settingsMenu;
      private IMenuItem _aboutUsMenu;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.AndroidExamples);

         /*
         // Get our buttons from the layout resource,
         // and attach events to it
         Button helloWorldButton = FindViewById<Button>(Resource.Id.GotoHelloWorldButton);
         helloWorldButton.Click += delegate
         {
            StartActivity(typeof(HelloWorldActivity));
         };*/

         Button plannarSubdivisionButton = FindViewById<Button>(Resource.Id.GotoPlanarSubdivisionButton);
         plannarSubdivisionButton.Click += delegate
         {
            StartActivity(typeof(PlanarSubdivisionActivity));
         };

         Button featureMatchingButton = FindViewById<Button>(Resource.Id.GotoFeatureMatchingButton);
         featureMatchingButton.Click += delegate 
         {
            StartActivity(typeof(FeatureMatchingActivity));
         };

         Button pedestrianDetectionButton = FindViewById<Button>(Resource.Id.GotoPedestrianDetectionButton);
         pedestrianDetectionButton.Click += delegate
         {
            StartActivity(typeof(PedestrianDetectionActivity));
         };

         Button faceDetectionButton = FindViewById<Button>(Resource.Id.GotoFaceDetectionButton);
         faceDetectionButton.Click += delegate
         {
            StartActivity(typeof(FaceDetectionActivity));
         };

         Button trafficSignRecognitionButton = FindViewById<Button>(Resource.Id.GotoTrafficSignRecognitionButton);
         trafficSignRecognitionButton.Click += delegate
         {
            StartActivity(typeof(TrafficSignRecognitionActivity));
         };

         Button cameraButton = FindViewById<Button>(Resource.Id.GotoCameraButton);
         if (Android.Hardware.Camera.NumberOfCameras > 0)
         {
            cameraButton.Click += delegate
            {
               StartActivity(typeof(CameraPreviewActivity));
            };
         }
         else
         {
            cameraButton.Visibility = ViewStates.Gone;
         }

         Button licensePlateRecognitionButton = FindViewById<Button>(Resource.Id.GotoLicensePlateRecognitionButton);
         licensePlateRecognitionButton.Click += delegate
         {
            StartActivity(typeof(LicensePlateRecognitionActivity));
         };
      }

      public override bool OnCreateOptionsMenu(IMenu menu)
      {
         _aboutUsMenu = menu.Add(Resources.GetString(Resource.String.menu_option_about_us));
         _settingsMenu = menu.Add(Resources.GetString(Resource.String.menu_option_settings));
         return base.OnCreateOptionsMenu(menu);
      }

      public override bool OnOptionsItemSelected(IMenuItem item)
      {
         if (item == _aboutUsMenu)
         {
            Display display = WindowManager.DefaultDisplay;
            Android.Util.DisplayMetrics metrics = new Android.Util.DisplayMetrics();
            display.GetMetrics(metrics);
            int width = (int)Math.Min(display.Width * 0.8, 360 * metrics.Density);
            int height = (int)Math.Min(display.Height * 0.8, 480 * metrics.Density);

            PopupWindow aboutWindow = new PopupWindow(LayoutInflater.Inflate(Resource.Layout.about_us, null, false), width, height);
            TextView appVersionText = aboutWindow.ContentView.FindViewById<TextView>(Resource.Id.AboutUsVersionTextView);
            appVersionText.Text = String.Format("{0}: {1}",
               Resources.GetString(Resource.String.application_version),
               this.PackageManager.GetPackageInfo(PackageName, Android.Content.PM.PackageInfoFlags.Activities).VersionName);

            Button closeButton = aboutWindow.ContentView.FindViewById<Button>(Resource.Id.AboutUsCloseButton);
            closeButton.Click += delegate
            {
               aboutWindow.Dismiss();
            };

            TextView aboutUsNoteTextView =
               aboutWindow.ContentView.FindViewById<TextView>(Resource.Id.AboutUsModuleInfoTextView);
            //Emgu.CV.Util.VectorOfOclPlatformInfo oclInfo = Emgu.CV.OclInvoke.GetPlatformInfo();
            
            String txt = String.Format("Has OpenCL: {0}", CvInvoke.HaveOpenCL);

            if (CvInvoke.HaveOpenCL)
            {
               txt = String.Format("{0}{1}Use OpenCL: {2}{3}{4}{5}",
                  txt, System.Environment.NewLine,
                  CvInvoke.UseOpenCL, System.Environment.NewLine,
                  CvInvoke.OclGetPlatformsSummary(), System.Environment.NewLine);
            }
            txt = String.Format("{0}{1}Has Cuda: {2}", txt, System.Environment.NewLine, CudaInvoke.HasCuda);
            

            aboutUsNoteTextView.Text = txt;

            aboutWindow.ShowAtLocation(this.Window.DecorView, GravityFlags.Center, 0, 0);
         }
         else if (item == _settingsMenu)
         {
            Intent settingsIntent = new Intent(this, typeof(SettingActivity));
            StartActivity(settingsIntent);
         }

         return base.OnOptionsItemSelected(item);
      }
   }
}

