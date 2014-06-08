using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing;
//using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Graphics.Imaging;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsStoreCameraApp
{
   /// <summary>
   /// An empty page that can be used on its own or navigated to within a Frame.
   /// </summary>
   public sealed partial class MainPage : Page
   {
      private MediaCapture _mediaCapture;

      public MainPage()
      {
         this.InitializeComponent();
         _mediaCapture = new MediaCapture();

         MediaCaptureInitializationSettings setttings = new MediaCaptureInitializationSettings();
         setttings.AudioDeviceId = String.Empty;
         setttings.VideoDeviceId = String.Empty;
         setttings.StreamingCaptureMode = StreamingCaptureMode.Video;
         setttings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;

         _mediaCapture.InitializeAsync(setttings);
      }

      /// <summary>
      /// Invoked when this page is about to be displayed in a Frame.
      /// </summary>
      /// <param name="e">Event data that describes how this page was reached.  The Parameter
      /// property is typically used to configure the page.</param>
      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
      }

      private async void CaptureAndProcessButtonClick(object sender, RoutedEventArgs e)
      {
         using (Mat img = await Mat.FromMediaCapture(_mediaCapture))
         {
            CvInvoke.BitwiseNot(img, img);
            ImageView.Source = img.ToWritableBitmap();
         }
      }

      private async void LoadAndProcessButtonClick(object sender, RoutedEventArgs e)
      {
         FileOpenPicker picker = new FileOpenPicker();
         picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
         picker.FileTypeFilter.Add(".png");
         picker.FileTypeFilter.Add(".jpeg");
         picker.FileTypeFilter.Add(".jpg");
         picker.FileTypeFilter.Add(".bmp");

         StorageFile file = await picker.PickSingleFileAsync();
         if (file != null)
         {
            using (Mat img = await Mat.FromStorageFile(file))
            {
               CvInvoke.BitwiseNot(img, img);
               ImageView.Source = img.ToWritableBitmap();
            }
         }
      }
   }
}
