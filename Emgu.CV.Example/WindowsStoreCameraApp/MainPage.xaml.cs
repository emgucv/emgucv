using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
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

      private async void Button_Click_1(object sender, RoutedEventArgs e)
      {
         InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
         {
            await _mediaCapture.CapturePhotoToStreamAsync(Windows.Media.MediaProperties.ImageEncodingProperties.CreateJpeg(), stream);
            stream.Seek(0);

            WriteableBitmap bmp = new WriteableBitmap(1, 1);
            System.IO.Stream streamTmp = stream.AsStreamForRead();
            MemoryStream ms = new MemoryStream();
            {
               streamTmp.CopyTo(ms);
               using (Image<Bgr, Byte> img = Image<Bgr, Byte>.FromJpegData(ms.ToArray()))
                  CvInvoke.cvNot(img, img);

            }
            
            //await bmp.SetSourceAsync(stream);
            //ImageView.Source = bmp; 
         }
      }
   }
}
