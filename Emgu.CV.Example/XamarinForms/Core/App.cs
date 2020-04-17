//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Xamarin.Forms;
using Emgu.CV.Structure;
using Emgu.Util.TypeEnum;

namespace Emgu.CV.XamarinForms
{
    public class App : Application
    {
        public App()
        {
            Button helloWorldButton = new Button();
            helloWorldButton.Text = "Hello world";

            Button planarSubdivisionButton = new Button();
            planarSubdivisionButton.Text = "Planar Subdivision";

            Button faceDetectionButton = new Button();
            faceDetectionButton.Text = "Face Detection (CascadeClassifier)";

            Button faceLandmarkDetectionButton = new Button();
            faceLandmarkDetectionButton.Text = "Face Landmark Detection (DNN Module)";

            Button featureDetectionButton = new Button();
            featureDetectionButton.Text = "Feature Matching";

            Button shapeDetectionButton = new Button();
            shapeDetectionButton.Text = "Shape Detection";

            Button pedestrianDetectionButton = new Button();
            pedestrianDetectionButton.Text = "Pedestrian Detection";

            Button ocrButton = new Button();
            ocrButton.Text = "OCR";

            Button maskRcnnButton = new Button();
            maskRcnnButton.Text = "Mask RCNN (DNN module)";

            Button yoloButton = new Button();
            yoloButton.Text = "Yolo (DNN module)";

            Button stopSignDetectionButton = new Button();
            stopSignDetectionButton.Text = "Stop Sign Detection (DNN module)";

            List<View> buttonList = new List<View>()
            {
                helloWorldButton,
                planarSubdivisionButton,
                faceDetectionButton,
                faceLandmarkDetectionButton,
                featureDetectionButton,
                shapeDetectionButton,
                pedestrianDetectionButton,
                ocrButton,
                maskRcnnButton,
                stopSignDetectionButton,
                yoloButton,
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Emgu.Util.Platform.ClrType != Emgu.Util.Platform.Clr.NetFxCore)
            {
                Button viz3dButton = new Button();
                viz3dButton.Text = "Simple 3D reconstruction";

                buttonList.Add(viz3dButton);

                viz3dButton.Clicked += (sender, args) =>
                {
                    Mat left = CvInvoke.Imread("imL.png", ImreadModes.Color);
                    Mat right = CvInvoke.Imread("imR.png", ImreadModes.Color);
                    Viz3d v = Simple3DReconstruct.GetViz3d(left, right);
                    v.Spin();
                };
            }

            StackLayout buttonsLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
            };

            foreach (View b in buttonList)
                buttonsLayout.Children.Add(b);

            // The root page of your application
            ContentPage page =
              new ContentPage()
              {
                  Content = new ScrollView()
                  {
                      Content = buttonsLayout,
                  }
              };

            String aboutIcon = null;
            /*
            String aboutIcon;
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.IOS)
            {
                aboutIcon = null;
            }
            else if (Emgu.Util.Platform.ClrType == Emgu.Util.Platform.Clr.NetFxCore)
                aboutIcon = null; 
            else
                aboutIcon = "questionmark.png";*/

            MainPage =
             new NavigationPage(
                page
             );

            ToolbarItem aboutItem = new ToolbarItem("About", aboutIcon,
               () =>
               {
                   MainPage.Navigation.PushAsync(new AboutPage());
                   //page.DisplayAlert("Emgu CV Examples", "App version: ...", "Ok");
               }
            );
            page.ToolbarItems.Add(aboutItem);

            helloWorldButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new HelloWorldPage());
            };

            planarSubdivisionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new PlanarSubdivisionPage());
            };

            faceDetectionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new FaceDetectionPage());
            };
            
            shapeDetectionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new ShapeDetectionPage());
            };

            pedestrianDetectionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new PedestrianDetectionPage());
            };

            featureDetectionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new FeatureMatchingPage());
            };

            if (Emgu.Util.Platform.ClrType == Emgu.Util.Platform.Clr.NetFxCore)
            {
                //No DNN module for UWP apps
                maskRcnnButton.IsVisible = false;
                faceLandmarkDetectionButton.IsVisible = false;
                stopSignDetectionButton.IsVisible = false;
                yoloButton.IsVisible = false;
            }
            else
            {
                maskRcnnButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new MaskRcnnPage()); };
                faceLandmarkDetectionButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new FaceLandmarkDetectionPage()); };
                stopSignDetectionButton.Clicked  += (sender, args) =>
                {
                    MaskRcnnPage stopSignDetectionPage = new MaskRcnnPage();
                    stopSignDetectionPage.DefaultImage = "stop-sign.jpg";
                    stopSignDetectionPage.ObjectsOfInterest = new string[] {"stop sign"};
                    MainPage.Navigation.PushAsync(stopSignDetectionPage);
                };
                yoloButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new YoloPage()); };
            }

            ocrButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new OcrPage());
            };
        }

        public Page CurrentPage
        {
            get
            {
                NavigationPage np = MainPage as NavigationPage;
                return np.CurrentPage;
            }
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }


}
