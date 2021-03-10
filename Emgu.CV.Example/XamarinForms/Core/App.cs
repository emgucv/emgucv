//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Models;
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

            Button sceneTextDetectionButton = new Button();
            sceneTextDetectionButton.Text = "Scene Text detection (DNN Module)";

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

            Button licensePlateRecognitionButton = new Button();
            licensePlateRecognitionButton.Text = "License Plate Recognition (DNN Module)";

            List<View> buttonList = new List<View>()
            {
                helloWorldButton,
                planarSubdivisionButton,
                faceDetectionButton,
                faceLandmarkDetectionButton,
                sceneTextDetectionButton,
                featureDetectionButton,
                shapeDetectionButton,
                pedestrianDetectionButton,
                ocrButton,
                maskRcnnButton,
                stopSignDetectionButton,
                yoloButton,
                licensePlateRecognitionButton
            };

            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveViz = (openCVConfigDict["HAVE_OPENCV_VIZ"] != 0);
            bool haveDNN = (openCVConfigDict["HAVE_OPENCV_DNN"] != 0);
            bool haveFreetype = (openCVConfigDict["HAVE_OPENCV_FREETYPE"] != 0);

            bool hasInferenceEngine = false;
            if (haveDNN)
            {
                var dnnBackends = DnnInvoke.AvailableBackends;
                hasInferenceEngine = Array.Exists(dnnBackends, dnnBackend =>
                    (dnnBackend.Backend == Dnn.Backend.InferenceEngine
                     || dnnBackend.Backend == Dnn.Backend.InferenceEngineNgraph
                     || dnnBackend.Backend == Dnn.Backend.InferenceEngineNnBuilder2019));
            }

            if (haveViz)
            {
                Button viz3dButton = new Button();
                viz3dButton.Text = "Simple 3D reconstruction";

                buttonList.Add(viz3dButton);

                viz3dButton.Clicked += (sender, args) =>
                {
                    using (Mat left = CvInvoke.Imread("imL.png", ImreadModes.Color))
                    using (Mat right = CvInvoke.Imread("imR.png", ImreadModes.Color))
                    using(Mat points = new Mat())
                    using (Mat colors = new Mat())
                    {
                        Simple3DReconstruct.GetPointAndColor(left, right, points, colors);
                        Viz3d v = Simple3DReconstruct.GetViz3d(points, colors);
                        v.Spin();
                    }
                };
            }

            if (haveFreetype)
            {
                Button freetypeButton = new Button();
                freetypeButton.Text = "Free Type";

                buttonList.Add(freetypeButton);

                freetypeButton.Clicked += (sender, args) =>
                {
                    MainPage.Navigation.PushAsync(new FreetypePage());
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

            NavigationPage navigationPage = new NavigationPage(page);
            MainPage = navigationPage;

            //Fix for UWP navigation text
            if (Device.RuntimePlatform == Device.WPF)
                navigationPage.BarTextColor = Color.Green;
            

            ToolbarItem aboutItem = new ToolbarItem("About", aboutIcon,
                () =>
                {
                    MainPage.Navigation.PushAsync(new AboutPage());
                    //page.DisplayAlert("Emgu CV Examples", "App version: ...", "Ok");
                }
            );
            page.ToolbarItems.Add(aboutItem);

            helloWorldButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new HelloWorldPage()); };

            planarSubdivisionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new PlanarSubdivisionPage());
            };

            faceDetectionButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage faceAndEyeDetectorPage = new ProcessAndRenderPage(
                    new CascadeFaceAndEyeDetector(),
                    "Face and eye detection (Cascade classifier)",
                    "lena.jpg",
                    "Cascade classifier");
                MainPage.Navigation.PushAsync(faceAndEyeDetectorPage);
            };

            shapeDetectionButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage shapeDetectionPage = new ProcessAndRenderPage(
                    new ShapeDetector(),
                    "Shape detection",
                    "pic3.png",
                    "Shape detection");
                MainPage.Navigation.PushAsync(shapeDetectionPage);
            };

            pedestrianDetectionButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage pedestrianDetectorPage = new ProcessAndRenderPage(
                    new PedestrianDetector(),
                    "Pedestrian detection",
                    "pedestrian.png",
                    "HOG pedestrian detection");
                MainPage.Navigation.PushAsync(pedestrianDetectorPage);
            };

            featureDetectionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new FeatureMatchingPage());
            };

            licensePlateRecognitionButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage vehicleLicensePlateDetectorPage = new ProcessAndRenderPage(
                    new VehicleLicensePlateDetector(),
                    "Perform License Plate Recognition",
                    "cars_license_plate.png",
                    "This demo is based on the security barrier camera demo in the OpenVino model zoo. The models is trained with BIT-vehicle dataset. License plate is trained based on Chinese license plate that has white character on blue background. You will need to re-train your own model if you intend to use this in other countries.");
                MainPage.Navigation.PushAsync(vehicleLicensePlateDetectorPage);
            };

            maskRcnnButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage maskRcnnPage = new ProcessAndRenderPage(
                    new MaskRcnn(),
                    "Mask-rcnn Detection",
                    "dog416.png",
                    "");
                MainPage.Navigation.PushAsync(maskRcnnPage);
            };

            faceLandmarkDetectionButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage faceLandmarkDetectionPage = new ProcessAndRenderPage(
                    new FaceAndLandmarkDetector(),
                    "Perform Face Landmark Detection",
                    "lena.jpg",
                    "");
                MainPage.Navigation.PushAsync(faceLandmarkDetectionPage);
            };
            sceneTextDetectionButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage sceneTextDetectionPage = new ProcessAndRenderPage(
                    new SceneTextDetector(),
                    "Perform Scene Text Detection",
                    "cars_license_plate.png",
                    "This model is trained on MSRA-TD500, so it can detect both English and Chinese text instances.");
                MainPage.Navigation.PushAsync(sceneTextDetectionPage);
            };
            stopSignDetectionButton.Clicked += (sender, args) =>
            {
                MaskRcnn model = new MaskRcnn();
                model.ObjectsOfInterest = new string[] { "stop sign" };
                ProcessAndRenderPage stopSignDetectionPage = new ProcessAndRenderPage(
                    model,
                    "Mask-rcnn Detection",
                    "stop-sign.jpg",
                    "Stop sign detection using Mask RCNN");

                MainPage.Navigation.PushAsync(stopSignDetectionPage);
            };
            yoloButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage yoloPage = new ProcessAndRenderPage(
                    new Yolo(),
                    "Yolo Detection",
                    "dog416.png",
                    "");
                MainPage.Navigation.PushAsync(yoloPage);
            };

            ocrButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new OcrPage());
            };

            maskRcnnButton.IsVisible = haveDNN;
            faceLandmarkDetectionButton.IsVisible = haveDNN;
            stopSignDetectionButton.IsVisible = haveDNN;
            yoloButton.IsVisible = haveDNN;
            sceneTextDetectionButton.IsVisible = haveDNN && haveFreetype;
            licensePlateRecognitionButton.IsVisible = hasInferenceEngine;

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
