using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace RealtimeCamera
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            textBlock.Text = String.Empty;
            Window.Current.VisibilityChanged += (sender, args) =>
            {
                WinrtInvoke.WinrtOnVisibilityChanged(args.Visible);
            };
            //render the captured image in the bottom image view
            WinrtInvoke.WinrtSetFrameContainer(this.image2);

            WinrtInvoke.WinrtStartMessageLoop(Process);

        }

        private Mat _cameraMatrix;
        private Mat _distCoeffs;
        private Matrix<float> mapx, mapy;

        private VideoCapture _capture;

        public void Process()
        {
            Mat m = new Mat(new System.Drawing.Size(640, 480), DepthType.Cv8U, 3);
            Mat mProcessed = new Mat();
            while (true)
            {

                if (_captureEnabled)
                {
                    try
                    {
                        

                        //Read the camera data to the mat
                        //Must use VideoCapture.Read function for UWP to read image from capture.
                        //Note that m is in 3 channel RGB color space, 
                        //our default color space is BGR for 3 channel Mat
                        bool grabbed = _capture.Grab();

                        bool retrieved = _capture.Retrieve(m);
                        //_capture.Read(m);

                        WinrtInvoke.WinrtImshow();
                        if (!m.IsEmpty)
                        {
                            if (_cameraMatrix == null || _distCoeffs == null)
                            {
                                //Create a dummy camera calibration matrix for testing
                                //Use your own if you have calibrated your camera
                                _cameraMatrix = new Mat(new System.Drawing.Size(3, 3), DepthType.Cv32F, 1);
                                   
                                int centerY = m.Width >> 1;
                                int centerX = m.Height >> 1;
                                //CvInvoke.SetIdentity(_cameraMatrix, new MCvScalar(1.0));
                                _cameraMatrix.SetTo(new float[]
                                {
                                    1f, 0f, (float)centerY,
                                    0f, 1f, (float)centerX,
                                    0f, 0f, 1f
                                });

                                _distCoeffs = new Mat(new System.Drawing.Size(5, 1), DepthType.Cv32F, 1);
                                _distCoeffs.SetTo(new float[] { -0.000003f, 0f, 0f, 0f, 0f });
                                mapx = new Matrix<float>(m.Height, m.Width);
                                mapy = new Matrix<float>(m.Height, m.Width);
                                CvInvoke.InitUndistortRectifyMap(
                                    _cameraMatrix, 
                                    _distCoeffs, 
                                    null, 
                                    _cameraMatrix, 
                                    m.Size,
                                    DepthType.Cv32F, 
                                    mapx,
                                    mapy);

                            }

              

                            m.CopyTo(mProcessed);
                            CvInvoke.Undistort(m, mProcessed, _cameraMatrix, _distCoeffs );

                            //mProcess is in the same color space as m, which is RGB, 
                            //needed to change to BGR
                            CvInvoke.CvtColor(mProcessed, mProcessed, ColorConversion.Rgb2Bgr);
                            
                            
                            //Can apply simple image processing to the captured image, let just invert the pixels
                            //CvInvoke.BitwiseNot(m, m);

                            //render the processed image on the top image view
                            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                async () =>
                                {
                                    var wb = mProcessed.ToWritableBitmap();
                                    image1.Source = wb;
                                });

                            //The data in the mat that is read from the camera will 
                            //be drawn to the Image control
                            WinrtInvoke.WinrtImshow();
                        }
                    }
                    catch (Exception e)
                    {
                        SetText(e.Message);
                    }
                }
                else
                {
                    if (_capture != null)
                    {
                        _capture.Dispose();
                        _capture = null;
                    }

                    Task t = Task.Delay(100);
                    t.Wait();
                }
            }
        }

        private void SetText(String msg)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    textBlock.Text = msg;
                });
        }

        private bool _captureEnabled = false;

        private void captureButton_Click(object sender, RoutedEventArgs e)
        {
            _captureEnabled = !_captureEnabled;

            if (_captureEnabled)
            {
                if (_capture == null)
                {
                    _capture = new VideoCapture();
                    if (!_capture.IsOpened)
                    {
                        //Stop the capture
                        captureButton_Click(this, null);
                        _captureEnabled = !_captureEnabled;
                        return;
                    }
                }
                captureButton.Content = "Stop";
            }
            else
            {
                captureButton.Content = "Start Capture";
            }

        }
    }
}
