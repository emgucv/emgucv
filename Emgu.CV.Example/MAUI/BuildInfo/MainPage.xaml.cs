using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Platform.Maui.UI;
using Emgu.CV.Structure;
using Emgu.Util;
using Point = System.Drawing.Point;

namespace BuildInfo;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();

        Emgu.CV.Platform.Maui.MauiInvoke.Init();

#if DEBUG
        CvInvoke.LogLevel = LogLevel.Verbose; //LogLevel.Debug;
#endif

        //If you swap out the mini runtime nuget package with the full runtime package, you will be able to test Face Landmark detection using DNN module.
        var openCVConfigDict = CvInvoke.ConfigDict;
        bool haveDNN = (openCVConfigDict["HAVE_OPENCV_DNN"] != 0);
        FaceDetectBtn.IsVisible = haveDNN;

        String aboutIcon = null;
        ToolbarItem aboutItem = new ToolbarItem("About", aboutIcon,
                            () =>
                            {
                                this.Navigation.PushAsync(new AboutPage());
                            }
        );
        this.ToolbarItems.Add(aboutItem);
        
        Mat helloWorldImage = new Mat(
            new System.Drawing.Size(640, 480), 
            DepthType.Cv8U, 
            3);
        helloWorldImage.SetTo(new MCvScalar(0,0,0)); //Set to black background
        CvInvoke.PutText(helloWorldImage, "Hello, world!", new Point(100, 100), FontFace.HersheyDuplex, 2.0, new MCvScalar(0,0,255));
        
        this.HelloWorldImageView.SetImage(helloWorldImage);
    }


    private async void OnFaceDetectClicked(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                return;
            }
        }

        ProcessAndRenderPage faceLandmarkDetectionPage = new ProcessAndRenderPage(
            new FaceAndLandmarkDetector(),
            "Perform Face Landmark Detection",
            "lena.jpg",
            "");
        await this.Navigation.PushAsync(faceLandmarkDetectionPage);
    }

    private async void OnInvertClicked(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                return;
            }
        }

        ProcessAndRenderPage cameraFeedPage = new ProcessAndRenderPage(
            new ImageInvert(),
            "Invert image",
            "lena.jpg",
            "Will perform a simple image inversion.");
        await this.Navigation.PushAsync(cameraFeedPage);
    }

    public class ImageInvert : DisposableObject, IProcessAndRenderModel
    {
        protected override void DisposeObject()
        {
            Clear();
        }

        public void Clear()
        {
            // If the model need to be disposed, do it here.
        }

        public Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged, object initOptions)
        {
            //Initialize the model here.
            //e.g. Download and load the DNN model needed.
            //Simply run a delay if no model is needed to be loaded.
            return Task.Delay(1);
        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            //Do a simple inversion image operation here
            CvInvoke.BitwiseNot(imageIn, imageOut);
            
            //return any message you want to display
            return "Image inverted.";
        }

        public RenderType RenderMethod
        {
            get { return RenderType.Overwrite; }
        }
        public bool Initialized
        {
            get { return true; }
        }
    }
}

