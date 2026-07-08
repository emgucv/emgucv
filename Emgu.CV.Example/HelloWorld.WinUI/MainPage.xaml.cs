using Microsoft.UI.Xaml.Controls;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HelloWorld_WinUI;

/// <summary>
/// The main content page displayed inside the application window.
/// Add your UI logic, event handlers, and data binding here.
/// </summary>
public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();

        using (Mat img = new Mat(200, 400, DepthType.Cv8U, 3)) //Create a 3 channel image of 400x200
        {
            img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

            //Draw "Hello, world." on the image using the specific font
            CvInvoke.PutText(
                img,
                "Hello, world",
                new System.Drawing.Point(10, 80),
                HersheyFonts.Complex,
                1.0,
                new Bgr(0, 255, 0).MCvScalar);
            imageControl.Source = img.ToWritableBitmap();
        }
    }
}
