//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

//
// Camera preview based on monodroid API-samples
// https://github.com/xamarin/monodroid-samples/blob/master/ApiDemo/Graphics/CameraPreview.cs
//

//#define GL_VIEW

using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Paint = Android.Graphics.Paint;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Emgu.Util;

namespace AndroidExamples
{
   [Activity(Label = "Android Camera")]
   public class CameraPreviewActivity : Activity
   {
      private Preview _preview;
      private TopLayer _topLayer;

      private IMenuItem _menuCanny;
      private IMenuItem _menuColorMap;
      private IMenuItem _menuPreview;
      private IMenuItem _menuDistor;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);
         RequestWindowFeature(WindowFeatures.NoTitle);
         bool cameraPreviewCallbackWithBuffer = false;
         SetContentView(Resource.Layout.CameraPreviewLayout);

         _topLayer = new TopLayer(this, cameraPreviewCallbackWithBuffer);
         _preview = new Preview(this, _topLayer, cameraPreviewCallbackWithBuffer);

         RelativeLayout mainLayout = FindViewById<RelativeLayout>(Resource.Id.CameraPreiewRelativeLayout);
         mainLayout.AddView(_preview, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));
         mainLayout.AddView(_topLayer, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));
         //AddContentView(_topLayer, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent ));
#if GL_VIEW
         _topLayer.SetZOrderOnTop(true);
#endif
         RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;

         ImageButton switchCameraButton = FindViewById<ImageButton>(Resource.Id.CameraPreviewSwitchCameraImageButton);
         if (Camera.NumberOfCameras <= 1)
            switchCameraButton.Visibility = ViewStates.Invisible;
         else
         {
            switchCameraButton.BringToFront();
         }
         switchCameraButton.Click += delegate
         {
            _preview.SwitchCamera();
         };

      }

      public override bool OnCreateOptionsMenu(IMenu menu)
      {
         _menuPreview = menu.Add("Preview");
         _menuCanny = menu.Add("Canny");
         _menuColorMap = menu.Add("Color Map");
         _menuDistor = menu.Add("Distor");
         return base.OnCreateOptionsMenu(menu);
      }

      public override bool OnOptionsItemSelected(IMenuItem item)
      {
         if (item == _menuCanny)
         {
            _topLayer.Mode = ViewMode.Canny;
         }
         else if (item == _menuColorMap)
         {
            _topLayer.Mode = ViewMode.ColorMap;
         }
         else if (item == _menuPreview)
         {
            _topLayer.Mode = ViewMode.Preview;
         }
         else
         {
            _topLayer.Mode = ViewMode.Distor;
         }
         return base.OnOptionsItemSelected(item);
      }
   }

   enum ViewMode
   {
      Preview,
      Canny,
      ColorMap, 
      Distor
   }

   class TopLayer : 
#if GL_VIEW
      GLImageView,
#else
      View, 
#endif
      Camera.IPreviewCallback
   {
      private Stopwatch _watch;
      Paint _paint;
      Size _imageSize;

      //Image<Bgr, Byte> _image;
      ImageBufferFactory<Bgr> _bgrBuffers;
      private CannyFilter _cannyFilter;
      private ColorMapFilter _colorMapFilter;
      private DistorFilter _distorFilter;
      Activity _activity;

      private bool _cameraPreviewCallbackWithBuffer;

      public ViewMode Mode = ViewMode.Preview;

      public TopLayer(Activity activity, bool cameraPreviewCallbackWithBuffer)
         : base(activity)
      {
         _activity = activity;
         _cameraPreviewCallbackWithBuffer = cameraPreviewCallbackWithBuffer;

         _paint = new Paint();
         _paint.SetStyle(Paint.Style.Stroke);
         _paint.SetARGB(255, 255, 0, 0);
         _paint.TextSize = 25;

         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(4, 8))
         {
         }
         _watch = Stopwatch.StartNew();
         _bgrBuffers = new ImageBufferFactory<Bgr>();
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         _bgrBuffers.Dispose();
         if (_cannyFilter != null)
            _cannyFilter.Dispose();
      }

#if !GL_VIEW
      private Android.Graphics.Bitmap _bmp;
      private BitmapRgb565Image _bmpImage;

      protected override void OnDraw(Android.Graphics.Canvas canvas)
      {
         base.OnDraw(canvas);

         lock (this)
         {
            Image<Bgr, byte> image = _bgrBuffers.GetBuffer(0);

            if (image != null && !_imageSize.IsEmpty && canvas != null)
            {
               Stopwatch w = Stopwatch.StartNew();

               if ((_bmpImage != null) && (!_imageSize.Equals(_bmpImage.Size)))
               {
                  _bmp.Dispose();
                  _bmpImage.Dispose();
                  _bmp = null;
                  _bmpImage = null;
               }

               if (_bmpImage == null)
               {
                  _bmp = Android.Graphics.Bitmap.CreateBitmap(_imageSize.Width, _imageSize.Height, Android.Graphics.Bitmap.Config.Rgb565);
                  _bmpImage = new BitmapRgb565Image(_bmp);
               }

               _bmpImage.ConvertFrom(image);

               canvas.DrawBitmap(_bmp, 0, 0, _paint);

               w.Stop();

               _watch.Stop();

               canvas.DrawText(String.Format("{0:F2} FPS; {1}x{2}; Render Time: {3} ms", 
                  1.0 / _watch.ElapsedMilliseconds * 1000, 
                  _imageSize.Width,
                  _imageSize.Height,
                  w.ElapsedMilliseconds), 20, 20, _paint);
               _watch.Reset();
               _watch.Start();
            }
         }
      }
#endif
      private bool _busy;



      public void OnPreviewFrame(byte[] data, Camera camera)
      {
         if (!_busy)
            try
            {
               _busy = true;
               Camera.Size cSize = camera.GetParameters().PreviewSize;
               _imageSize = new Size(cSize.Width, cSize.Height);
               Size size = _imageSize;
               Image<Bgr, Byte> image = _bgrBuffers.GetBuffer(size, 0);

               GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
               using (Image<Gray, Byte> yuv420sp = new Image<Gray, byte>(size.Width, (size.Height >> 1) * 3, size.Width, handle.AddrOfPinnedObject()))
               {
                  if (Mode == ViewMode.Preview)
                  {
                     CvInvoke.cvCvtColor(yuv420sp, image, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGR);
                  }
                  else
                  {
                     Image<Bgr, Byte> bgr = _bgrBuffers.GetBuffer(size, 1);
                     CvInvoke.cvCvtColor(yuv420sp, bgr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGR);

                     if (Mode == ViewMode.Canny)
                     {
                        if (_cannyFilter == null)
                           _cannyFilter = new CannyFilter();

                        _cannyFilter.ProcessData(bgr, image);

                     }
                     else if (Mode == ViewMode.ColorMap)
                     {
                        if (_colorMapFilter == null)
                           _colorMapFilter = new ColorMapFilter(Emgu.CV.CvEnum.ColorMapType.Summer);

                        _colorMapFilter.ProcessData(bgr, image);
                     }
                     else //if (Mode == ViewMode.Distor)
                     {
                        if (_distorFilter == null)
                           _distorFilter = new DistorFilter(0.5, 0.5, -1.5);

                        _distorFilter.ProcessData(bgr, image);
                     }
                  }
               }
               handle.Free();

#if GL_VIEW
               Stopwatch w = Stopwatch.StartNew();

               Image<Bgr, Byte> resized = _bgrBuffers.GetBuffer(new Size(512, 512), 2);
               CvInvoke.cvResize(image, resized, Emgu.CV.CvEnum.INTER.CV_INTER_NN);
               using (Image<Bgra, Byte> texture = resized.Convert<Bgra, Byte>())
               {
                  LoadTextureBGRA(texture.Size, texture.MIplImage.imageData);
                  SetupCamera();
                  RenderCube();
               }
               
               w.Stop();

               _watch.Stop();

               Android.Util.Log.Verbose("Emgu.CV", String.Format("{0:F2} FPS; {1}x{2}; Render Time: {3} ms",
                  1.0 / _watch.ElapsedMilliseconds * 1000,
                  _imageSize.Width,
                  _imageSize.Height,
                  w.ElapsedMilliseconds));
               _watch.Reset();
               _watch.Start();
#else
               Invalidate();
#endif
            }
            finally
            {
               _busy = false;
            }

         if (_cameraPreviewCallbackWithBuffer)
            camera.AddCallbackBuffer(data);
      }
   }

   class Preview : SurfaceView, ISurfaceHolderCallback
   {
      ISurfaceHolder _surfaceHolder;
      Camera _camera;
      int _cameraIndex;
      TopLayer _topLayer;

      private bool _cameraPreviewCallbackWithBuffer;

      public Preview(Context context, TopLayer topLayer, bool cameraPreviewCallbackWithBuffer)
         : base(context)
      {
         _cameraPreviewCallbackWithBuffer = cameraPreviewCallbackWithBuffer;

         // Install a SurfaceHolder.Callback so we get notified when the
         // underlying surface is created and destroyed.
         _surfaceHolder = Holder;
         _surfaceHolder.AddCallback(this);

         _topLayer = topLayer;
      }

      public void SwitchCamera()
      {
         int numberOfCameras = Camera.NumberOfCameras;
         if (numberOfCameras > 1)
         {
            _cameraIndex++;
            _cameraIndex %= numberOfCameras;

            CreateCamera(_cameraIndex);

            SurfaceChanged(
               _surfaceHolder,
               Android.Graphics.Format.Rgb888, //doesn't matter, omitted by the Surface changed function.
               Width,
               Height);
         }
      }

      private void CreateCamera(int cameraIndex)
      {
         StopCamera();
         
         _camera = Camera.Open(cameraIndex);

         try
         {
            _camera.SetPreviewDisplay(_surfaceHolder);
         }
         catch (Exception)
         {
            _camera.Release();
            _camera = null;
            // TODO: add more exception handling logic here
         }
      }

      private void StopCamera()
      {
         if (_camera != null)
         {
            _camera.StopPreview();
            if (_cameraPreviewCallbackWithBuffer)
               _camera.SetPreviewCallbackWithBuffer(null);
            else
               _camera.SetPreviewCallback(null);
            _camera.Release();
            _camera = null;
         }
      }

      public void SurfaceCreated(ISurfaceHolder holder)
      {
         // The Surface has been created, acquire the camera and tell it where
         // to draw.
         if (_surfaceHolder != holder)
         {
            _surfaceHolder = holder;
            _surfaceHolder.AddCallback(this);
         }
         int numberOfCameras = Camera.NumberOfCameras;
         if (numberOfCameras > 0)
         {
            CreateCamera(_cameraIndex);
         }
      }

      public void SurfaceDestroyed(ISurfaceHolder holder)
      {
         // Surface will be destroyed when we return, so stop the preview.
         // Because the CameraDevice object is not a shared resource, it's very
         // important to release it when the activity is paused.
         StopCamera();
      }

      private Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h, int maxWidth, int maxHeight)
      {
         const double ASPECT_TOLERANCE = 0.05;
         double targetRatio = (double)w / h;

         if (sizes == null)
            return null;

         Camera.Size optimalSize = null;
         double minDiff = Double.MaxValue;

         int targetHeight = h;

         // Try to find an size match aspect ratio and size
         for (int i = 0; i < sizes.Count; i++)
         {
            Camera.Size size = sizes[i];

            if (size.Width > maxWidth || size.Height > maxHeight)
               continue;

            double ratio = (double)size.Width / size.Height;

            if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
               continue;

            if (Math.Abs(size.Height - targetHeight) < minDiff)
            {
               optimalSize = size;
               minDiff = Math.Abs(size.Height - targetHeight);
            }
         }

         // Cannot find the one match the aspect ratio, ignore the requirement
         if (optimalSize == null)
         {
            minDiff = Double.MaxValue;
            for (int i = 0; i < sizes.Count; i++)
            {
               Camera.Size size = sizes[i];

               if (Math.Abs(size.Height - targetHeight) < minDiff)
               {
                  optimalSize = size;
                  minDiff = Math.Abs(size.Height - targetHeight);
               }
            }
         }

         return optimalSize;
      }

      
      private Camera.Size SetCameraOptimalPreviewSize(Camera camera, int w, int h)
      {
         Camera.Parameters parameters = _camera.GetParameters();
         IList<Camera.Size> sizes = parameters.SupportedPreviewSizes;
         int maxWidth = 640, maxHeight = 480;
         Camera.Size optimalSize = GetOptimalPreviewSize(sizes, w, h, maxWidth, maxHeight);
         parameters.SetPreviewSize(optimalSize.Width, optimalSize.Height);
         _camera.SetParameters(parameters);
         return optimalSize;
      }

      public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h)
      {
         if (_surfaceHolder != holder)
         {
            _surfaceHolder = holder;
            _surfaceHolder.AddCallback(this);
         }

         // Now that the size is known, set up the camera parameters and begin
         // the preview.
         Camera.Size optimalSize = SetCameraOptimalPreviewSize(_camera, w, h);

         //set for protrait mode
         //camera.SetDisplayOrientation(90);

         if (_cameraPreviewCallbackWithBuffer)
         {
            int bufferSize = optimalSize.Width * (optimalSize.Height >> 1) * 3;
            _camera.SetPreviewCallbackWithBuffer(_topLayer);
            for (int i = 0; i < 1; ++i)
               _camera.AddCallbackBuffer(new byte[bufferSize]);
         }
         else
            _camera.SetPreviewCallback(_topLayer);

         _camera.StartPreview();

         Layout(0, 0, optimalSize.Width, optimalSize.Height);
      }
   }

   public class ImageBufferFactory<TColor> : Emgu.Util.DisposableObject
   where TColor : struct, IColor
   {
      public ImageBufferFactory()
      {
         _sizes = new List<Size>();
         _buffers = new List<Image<TColor, byte>>();
      }

      private List<System.Drawing.Size> _sizes;

      private List<Image<TColor, Byte>> _buffers;

      public Image<TColor, Byte> GetBuffer(int index)
      {
         if (index < _buffers.Count)
            return _buffers[index];
         else
            return null;
      }

      public Image<TColor, Byte> GetBuffer(System.Drawing.Size size, int index)
      {
         for (int i = _buffers.Count; i < index + 1; i++)
         {
            _buffers.Add(null);
            _sizes.Add(Size.Empty);
         }

         if (!_sizes[index].Equals(size))
         {
            if (_buffers[index] == null)
               _buffers[index] = new Image<TColor, byte>(size);
            else
            {
               _buffers[index].Dispose();
               _buffers[index] = new Image<TColor, byte>(size);
            }

            _sizes[index] = size;
         }
         return _buffers[index];
      }

      protected override void DisposeObject()
      {
         for (int i = 0; i < _buffers.Count; i++)
         {
            if (_buffers[i] != null)
               _buffers[i].Dispose();
         }
      }
   }

   public class ImageFilter : Emgu.Util.DisposableObject
   {
      protected ImageBufferFactory<Bgr> _bgrBuffers;
      protected ImageBufferFactory<Gray> _grayBuffers;

      public ImageFilter()
      {
      }

      public Image<Bgr, Byte> GetBufferBgr(Size size, int index)
      {
         if (_bgrBuffers == null)
            _bgrBuffers = new ImageBufferFactory<Bgr>();
         return _bgrBuffers.GetBuffer(size, index);
      }

      public Image<Gray, Byte> GetBufferGray(Size size, int index)
      {
         if (_grayBuffers == null)
            _grayBuffers = new ImageBufferFactory<Gray>();
         return _grayBuffers.GetBuffer(size, index);
      }

      protected override void DisposeObject()
      {
         if (_bgrBuffers != null)
            _bgrBuffers.Dispose();
         if (_grayBuffers != null)
            _grayBuffers.Dispose();
      }
   }

   public class CannyFilter : ImageFilter
   {
      public void ProcessData(Image<Bgr, Byte> sourceImage, Image<Bgr, Byte> dest)
      {
         Size size = sourceImage.Size;

         Image<Gray, Byte> b = GetBufferGray(size, 0);
         Image<Gray, Byte> g = GetBufferGray(size, 1);
         Image<Gray, Byte> r = GetBufferGray(size, 2);
         Image<Gray, Byte> bCanny = GetBufferGray(size, 3);
         Image<Gray, Byte> gCanny = GetBufferGray(size, 4);
         Image<Gray, Byte> rCanny = GetBufferGray(size, 5);
         Image<Bgr, Byte> buffer0 = GetBufferBgr(sourceImage.Size, 0);

         CvInvoke.cvSplit(sourceImage, b, g, r, IntPtr.Zero);
         CvInvoke.cvCanny(b, bCanny, 100, 60, 3);
         CvInvoke.cvCanny(g, gCanny, 100, 60, 3);
         CvInvoke.cvCanny(r, rCanny, 100, 60, 3);
         CvInvoke.cvMerge(bCanny, gCanny, rCanny, IntPtr.Zero, dest);

      }
   }

   public class ColorMapFilter : ImageFilter
   {
      private Emgu.CV.CvEnum.ColorMapType _colorMapType;

      public ColorMapFilter(Emgu.CV.CvEnum.ColorMapType type)
      {
         _colorMapType = type;
      }

      public void ProcessData(Image<Bgr, Byte> sourceImage, Image<Bgr, Byte> dest)
      {
         CvInvoke.ApplyColorMap(sourceImage, dest, _colorMapType);
      }
   }

   public class DistorFilter : ImageFilter
   {
      private double _centerX;
      private double _centerY;
      private double _distorCoeff;

      private Matrix<float> _mapX;
      private Matrix<float> _mapY;

      private Size _size;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="centerWidth">A value between 0 and 1.0, if 0, the center is on the left side of the image, if 1, the center is on the right side of the image</param>
      /// <param name="centerHeight">A value between 0 and 1.0, if 0, the center is on the top of the image, if 1, the center is on the bottom of the image</param>
      /// <param name="distorCoeff"></param>
      public DistorFilter(double centerWidth, double centerHeight, double distorCoeff)
      {
         if (centerWidth < 0 || centerWidth > 1.0 || centerHeight < 0 || centerHeight > 1.0)
         {
            throw new ArgumentException("CenterWidth and CenterHeight must be a number >= 0 and <= 1.0");
         }
         _centerX = centerWidth;
         _centerY = centerHeight;
         _distorCoeff = distorCoeff;
      }

      public void ProcessData(Image<Bgr, Byte> sourceImage, Image<Bgr, Byte> dest)
      {
         if (!sourceImage.Size.Equals(_size))
         {
            if (_mapX != null)
            {
               _mapX.Dispose();
               _mapX = null;
            }
            if (_mapY != null)
            {
               _mapY.Dispose();
               _mapY = null;
            }

            _size = sourceImage.Size;
         }

         if (_mapX == null || _mapY == null)
         {
            IntrinsicCameraParameters p = new IntrinsicCameraParameters(5);
            int centerY = (int)(_size.Width * _centerY);
            int centerX = (int)(_size.Height * _centerX);
            CvInvoke.cvSetIdentity(p.IntrinsicMatrix, new MCvScalar(1.0));
            p.IntrinsicMatrix.Data[0, 2] = centerY;
            p.IntrinsicMatrix.Data[1, 2] = centerX;
            p.IntrinsicMatrix.Data[2, 2] = 1;
            p.DistortionCoeffs.Data[0, 0] = _distorCoeff / (_size.Width * _size.Width);

            p.InitUndistortMap(_size.Width, _size.Height, out _mapX, out _mapY);
         }

         CvInvoke.cvRemap(sourceImage, dest, _mapX, _mapY, (int)Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC | (int)Emgu.CV.CvEnum.WARP.CV_WARP_FILL_OUTLIERS, new MCvScalar());
      }

   }
}

