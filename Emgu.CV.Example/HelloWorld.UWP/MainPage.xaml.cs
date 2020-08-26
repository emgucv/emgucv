using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorld.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            using (Mat img = new Mat(200, 400, DepthType.Cv8U, 3)) //Create a 3 channel image of 400x200
            {
                img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

                //Draw "Hello, world." on the image using the specific font
                CvInvoke.PutText(
                    img,
                    "Hello, world",
                    new System.Drawing.Point(10, 80),
                    FontFace.HersheyComplex,
                    1.0,
                    new Bgr(0, 255, 0).MCvScalar);
                imageControl.Source = img.ToWritableBitmap();
            }
        }
    }
}
