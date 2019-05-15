//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

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

#if !(NETFX_CORE || __ANDROID__ || __IOS__ || __MACOS__)
            //Button viz3dButton = new Button();
            //viz3dButton.Text = "Viz3D";
            //buttonList.Add(viz3dButton);
            //viz3dButton.Clicked += (sender, args) =>
            //{
            //    MainPage.Navigation.PushAsync(new Viz3dPage());
            //};
#endif

            StackLayout buttonsLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
            };
            foreach (View b in buttonList)
                buttonsLayout.Children.Add(b);

            // The root page of your application
            ContentPage page =
              new ContentPage
              {
                  Content = buttonsLayout
              };

#if NETFX_CORE
		    String aboutIcon = "questionmark.png";
#else
            String aboutIcon = null;
#endif

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



#if NETFX_CORE
            //ocrButton.IsVisible = false;
            dnnButton.IsVisible = false;
            faceLandmarkDetectionButton.IsVisible = false;
#else
            dnnButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new DnnPage()); };
            faceLandmarkDetectionButton.Clicked += (sender, args) => { MainPage.Navigation.PushAsync(new FaceLandmarkDetectionPage()); };
#endif
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
