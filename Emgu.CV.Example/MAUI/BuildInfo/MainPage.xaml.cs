using Emgu.CV;

namespace BuildInfo;

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();

#if __IOS__
        CvInvokeIOS.Init();
#elif __ANDROID__
        CvInvokeAndroid.Init();
#endif
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        DetailLabel.Text = CvInvoke.BuildInformation;
    }
}

