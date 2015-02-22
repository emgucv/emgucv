using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Core;
using Emgu.CV;
using Emgu.CV.CvEnum;
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
using Nokia.Graphics.Imaging;

namespace WindowsStoreCameraApp
{
   /// <summary>
   /// An empty page that can be used on its own or navigated to within a Frame.
   /// </summary>
   public sealed partial class MainPage : Page
   {
      //private MediaCapture _mediaCapture;

      public MainPage()
      {
         
         this.InitializeComponent();
         CaptureButton.Content = "Capture and Canny";
         /*
         _mediaCapture = new MediaCapture();

         MediaCaptureInitializationSettings setttings = new MediaCaptureInitializationSettings();
         setttings.AudioDeviceId = String.Empty;
         setttings.VideoDeviceId = String.Empty;
         setttings.StreamingCaptureMode = StreamingCaptureMode.Video;
         setttings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;

         _mediaCapture.InitializeAsync(setttings);*/
      }

      /// <summary>
      /// Invoked when this page is about to be displayed in a Frame.
      /// </summary>
      /// <param name="e">Event data that describes how this page was reached.  The Parameter
      /// property is typically used to configure the page.</param>
      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
      }

      private CameraPreviewImageSource _cameraPreviewImageSource;
      private WriteableBitmap _writeableBitmap;
      private FilterEffect _effect;
      private WriteableBitmapRenderer _writeableBitmapRenderer;

      /// <summary>
      /// Initialize and start the camera preview
      /// </summary>
      public async Task InitializeAsync()
      {
         if (CaptureButton.Content.ToString().Equals("Capture and Canny"))
         {
            LoadButton.Visibility = Visibility.Collapsed;
            CaptureButton.Content = "Stop";
            
            // Create a camera preview image source (from Imaging SDK)
            if (_cameraPreviewImageSource == null)
            {
               _cameraPreviewImageSource = new CameraPreviewImageSource();
               await _cameraPreviewImageSource.InitializeAsync(string.Empty);
            }

            var properties = await _cameraPreviewImageSource.StartPreviewAsync();

            // Create a preview bitmap with the correct aspect ratio
            var width = 640.0;
            var height = (width/properties.Width)*properties.Height;
            var bitmap = new WriteableBitmap((int) width, (int) height);

            _writeableBitmap = bitmap;

            // Create a filter effect to be used with the source (no filters yet)
            _effect = new FilterEffect(_cameraPreviewImageSource);
            _writeableBitmapRenderer = new WriteableBitmapRenderer(_effect, _writeableBitmap);

            ImageView.Source = _writeableBitmap;

            // Attach preview frame delegate
            _cameraPreviewImageSource.PreviewFrameAvailable += OnPreviewFrameAvailable;
         }
         else
         {
            if (CaptureButton.Content.ToString().Equals("Stop"))
            {
               await  _cameraPreviewImageSource.StopPreviewAsync();
               _cameraPreviewImageSource.Dispose();
               _cameraPreviewImageSource = null;
            }
            CaptureButton.Content = "Capture and Canny";
            LoadButton.Visibility = Visibility.Visible;
            ImageView.Source = null;
         }
      }

      private bool _isRendering = false;
      private byte[] _buffer;
      private async void OnPreviewFrameAvailable(IImageSize imageSize)
      {
         if (!_isRendering)
         {
            _isRendering = true;
            await _writeableBitmapRenderer.RenderAsync();
            await
               Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                  CoreDispatcherPriority.High,
                  () =>
                  {
                     int bufferSize = _writeableBitmap.PixelWidth * _writeableBitmap.PixelHeight * 4;
                     if (_buffer == null || _buffer.Length != bufferSize)
                        _buffer = new byte[bufferSize];
                     IBuffer pixelBuffer = _writeableBitmap.PixelBuffer;
                     pixelBuffer.CopyTo(_buffer);

                     GCHandle handle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
                     using (
                        Mat m = new Mat(_writeableBitmap.PixelHeight, _writeableBitmap.PixelWidth,
                           DepthType.Cv8U, 4,
                           handle.AddrOfPinnedObject(), _writeableBitmap.PixelWidth * 4))
                     using (Mat gray = new Mat())
                     using (Mat canny = new Mat())
                     {
                        CvInvoke.CvtColor(m, gray, ColorConversion.Bgr2Gray);
                        CvInvoke.Canny(gray, canny, 40, 60);
                        CvInvoke.CvtColor(canny, m, ColorConversion.Gray2Bgra);
                     }
                     handle.Free();
                     
                     using (Stream s = pixelBuffer.AsStream())
                     {
                        s.Write(_buffer, 0, _buffer.Length);
                     }
                    
                     _writeableBitmap.Invalidate();
                  });
            _isRendering = false; 
         }

      }
      private async void CaptureAndProcessButtonClick(object sender, RoutedEventArgs e)
      {

         await InitializeAsync();
         
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
