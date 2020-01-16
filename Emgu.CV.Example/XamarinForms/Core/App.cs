//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
            faceDetectionButton.Text = "Face Detection";

            Button faceLandmarkDetectionButton = new Button();
            faceLandmarkDetectionButton.Text = "Face Landmark Detection";

            Button featureDetectionButton = new Button();
            featureDetectionButton.Text = "Feature Matching";

            Button pedestrianDetectionButton = new Button();
            pedestrianDetectionButton.Text = "Pedestrian Detection";

            Button ocrButton = new Button();
            ocrButton.Text = "OCR";

            Button dnnButton = new Button();
            dnnButton.Text = "DNN";

            List<View> buttonList = new List<View>()
            {
                helloWorldButton,
                planarSubdivisionButton,
                faceDetectionButton,
                faceLandmarkDetectionButton,
                featureDetectionButton,
                pedestrianDetectionButton,
                ocrButton,
                dnnButton
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Emgu.Util.Platform.ClrType != ClrType.NetFxCore)
            {
                Button viz3dButton = new Button();
                viz3dButton.Text = "Viz3D";

                buttonList.Add(viz3dButton);

                viz3dButton.Clicked += (sender, args) =>
                {
                    using (Viz3d viz = new Viz3d("show_simple_widgets"))
                    {
                        viz.SetBackgroundMeshLab();
                        using (WCoordinateSystem coor = new WCoordinateSystem())
                        {
                            viz.ShowWidget("coor", coor);
                            using (WCube cube = new WCube(
                                new MCvPoint3D64f(-.5, -.5, -.5),
                                new MCvPoint3D64f(.5, .5, .5),
                                true,
                                new MCvScalar(255, 255, 255)))
                            {
                                viz.ShowWidget("cube", cube);
                                using (WCube cube0 = new WCube(
                                    new MCvPoint3D64f(-1, -1, -1),
                                    new MCvPoint3D64f(-.5, -.5, -.5),
                                    false,
                                    new MCvScalar(123, 45, 200)))
                                {
                                    viz.ShowWidget("cub0", cube0);
                                    viz.Spin();
                                }
                            }
                        }
                    }
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

            String aboutIcon;
            if (Emgu.Util.Platform.ClrType != ClrType.NetFxCore)
                aboutIcon = "questionmark.png";
            else
                aboutIcon = null;

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
            
            pedestrianDetectionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new PedestrianDetectionPage());
            };

            featureDetectionButton.Clicked += (sender, args) =>
            {
                MainPage.Navigation.PushAsync(new FeatureMatchingPage());
            };

            if (Emgu.Util.Platform.ClrType == ClrType.NetFxCore)
            {
                //No DNN module for UWP apps
                dnnButton.IsVisible = false;
                faceLandmarkDetectionButton.IsVisible = false;
            }
            else
            {
                dnnButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new DnnPage()); };
                faceLandmarkDetectionButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new FaceLandmarkDetectionPage()); };
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
