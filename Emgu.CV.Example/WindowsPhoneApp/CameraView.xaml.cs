using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

using Lumia.Imaging;
using Lumia.Imaging.Compositing;
using Lumia.Imaging.Transforms;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;

namespace Emgu.CV.WindowsPhone.App
{
   /// <summary>
   /// An empty page that can be used on its own or navigated to within a Frame.
   /// </summary>
   public sealed partial class CameraView : Page
   {
      public CameraView()
      {
         this.InitializeComponent();
         CaptureButton.Content = "Capture and Canny";
      }

      /// <summary>
      /// Invoked when this page is about to be displayed in a Frame.
      /// </summary>
      /// <param name="e">Event data that describes how this page was reached.
      /// This parameter is typically used to configure the page.</param>
      protected override void OnNavigatedTo(NavigationEventArgs e)
      {
      }

      private CameraPreviewImageSource _cameraPreviewImageSource;
      private WriteableBitmap _writeableBitmap;
      //private FilterEffect _effect;
      private WriteableBitmapRenderer _writeableBitmapRenderer;

      /// <summary>
      /// Initialize and start the camera preview
      /// </summary>
      public async Task InitializeAsync()
      {
         if (CaptureButton.Content.ToString().Equals("Capture and Canny"))
         {
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
            var height = (width / properties.Width) * properties.Height;
            var bitmap = new WriteableBitmap((int)width, (int)height);

            _writeableBitmap = bitmap;

            // Create a filter effect to be used with the source (e.g. used to correct rotation)
            //_effect = new FilterEffect(_cameraPreviewImageSource);
            //_effect.Filters = new IFilter[] { new RotationFilter(90.0) };
            //_writeableBitmapRenderer = new WriteableBitmapRenderer(_effect, _writeableBitmap);

            RotationEffect rotation = new RotationEffect(_cameraPreviewImageSource, 90);

            _writeableBitmapRenderer = new WriteableBitmapRenderer(rotation, _writeableBitmap);
            //_writeableBitmapRenderer.Source = new EffectList() { _cameraPreviewImageSource, rotation };
            //_writeableBitmapRenderer.WriteableBitmap = _writeableBitmap;

            ImageView.Source = _writeableBitmap;

            // Attach preview frame delegate
            
            _cameraPreviewImageSource.PreviewFrameAvailable += OnPreviewFrameAvailable;
         }
         else
         {
            if (CaptureButton.Content.ToString().Equals("Stop"))
            {
               await _cameraPreviewImageSource.StopPreviewAsync();
               _cameraPreviewImageSource.Dispose();
               _cameraPreviewImageSource = null;
            }
            CaptureButton.Content = "Capture and Canny";
            ImageView.Source = null;
         }
      }

      private bool _isRendering = false;
      private byte[] _buffer;
      private async void OnPreviewFrameAvailable(IAsyncImageResource imageResource)
      {
         if (!_isRendering)
         {
            _isRendering = true;
            await _writeableBitmapRenderer.RenderAsync();
            await
               CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
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
                        CvInvoke.CvtColor(m, gray, ColorConversion.Bgra2Gray);
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


   }
}
