using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidExamples
{
   [Activity(Label = "Emgu CV Examples", MainLauncher = true)]
   public class AndroidExamplesActivity : Activity
   {
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

         Button surfFeatureButton = FindViewById<Button>(Resource.Id.GotoSURFFeaturesButton);
         surfFeatureButton.Click += delegate 
         {
            StartActivity(typeof(SURFFeatureActivity));
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
   }
}

