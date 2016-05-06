using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using Emgu.CV;
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

         Window.Current.VisibilityChanged += (sender, args) =>
         {
            CvInvoke.WinrtOnVisibilityChanged(args.Visible);
         };
         CvInvoke.WinrtSetFrameContainer(this.image);
         CvInvoke.WinrtStartMessageLoop(Process);

      }

      private VideoCapture _capture;
      public void Process()
      {
         Mat m = new Mat();
         while (true)
         {
            if (_captureEnabled)
            {
               if (_capture == null)
                  _capture = new VideoCapture();

               //Read the camera data to the mat
               //Must use VideoCapture.Read function for UWP to read image from capture.
               _capture.Read(m);
               if (!m.IsEmpty)
               {
                  //some simple image processing, let just invert the pixels
                  CvInvoke.BitwiseNot(m, m);

                  //The data in the mat that is read from the camera will 
                  //be drawn to the Image control
                  CvInvoke.WinrtImshow();
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

      private bool _captureEnabled = false;

      private void captureButton_Click(object sender, RoutedEventArgs e)
      {
         _captureEnabled = !_captureEnabled;

         if (_captureEnabled)
         {
            captureButton.Content = "Stop";
         }
         else
         {
            captureButton.Content = "Start Capture";
         }

      }
   }
}
