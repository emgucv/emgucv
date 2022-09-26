using Emgu.CV;

namespace BuildInfo;

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        DetailLabel.Text = CvInvoke.BuildInformation;
    }
}

