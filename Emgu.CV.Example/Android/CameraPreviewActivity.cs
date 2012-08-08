//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

//
// Camera preview based on monodroid API-samples
// https://github.com/xamarin/monodroid-samples/blob/master/ApiDemo/Graphics/CameraPreview.cs
//

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

namespace AndroidExamples
{
   [Activity(Label = "MonoAndroidCamera")]
   public class CameraPreviewActivity : Activity
   {
      private Preview _preview;
      private TopLayer _topLayer;

      private IMenuItem _menuCanny;
      private IMenuItem _menuColorMap;
      private IMenuItem _menuPreview;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         bool cameraPreviewCallbackWithBuffer = false;
         _topLayer = new TopLayer(this, cameraPreviewCallbackWithBuffer);
         _preview = new Preview(this, _topLayer, cameraPreviewCallbackWithBuffer);
         SetContentView(_preview);
         AddContentView(_topLayer, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent ));

         RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
      }

      public override bool OnCreateOptionsMenu(IMenu menu)
      {
         _menuPreview = menu.Add("Preview");
         _menuCanny = menu.Add("Canny");
         _menuColorMap = menu.Add("Color Map");
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
         } else
         {
            _topLayer.Mode = ViewMode.Preview;
         }
         return base.OnOptionsItemSelected(item);
      }
   }

   enum ViewMode
   {
      Preview,
      Canny,
      ColorMap
   }

   class TopLayer : View, Camera.IPreviewCallback
   {
      private Stopwatch _watch;
      Paint _paint;
      Android.Graphics.Bitmap _bmp;
      int[] _bgraData;
      byte[] _bgrData;

      private bool _cameraPreviewCallbackWithBuffer;

      public ViewMode Mode = ViewMode.Preview;

      public TopLayer(Context context, bool cameraPreviewCallbackWithBuffer)
         : base(context)
      {
         _cameraPreviewCallbackWithBuffer = cameraPreviewCallbackWithBuffer;

         _paint = new Paint();
         _paint.SetStyle(Paint.Style.Stroke);
         _paint.SetARGB(255, 255, 0, 0);
         _paint.TextSize = 25;

         Image<Bgr, Byte> img = new Image<Bgr, byte>(4, 8);

         _watch = Stopwatch.StartNew();
      }

      protected override void OnDraw(Android.Graphics.Canvas canvas)
      {
         base.OnDraw(canvas);

         lock (this)
         {
            if (_bmp != null && canvas != null)
            {
               Stopwatch w = Stopwatch.StartNew();
               canvas.DrawBitmap(_bmp, 0, 0, null);
               w.Stop();

               _watch.Stop();
               canvas.DrawText(String.Format("{0:F2} FPS; {1}x{2}; Render Time: {3} ms", 
                  1.0 / _watch.ElapsedMilliseconds * 1000, 
                  _bmp.Width,
                  _bmp.Height,
                  w.ElapsedMilliseconds), 20, 20, _paint);
               _watch.Reset();
               _watch.Start();
            }
         }
      }

      private bool _busy;

      public void OnPreviewFrame(byte[] data, Camera camera)
      {
         if (!_busy)
            try
            {
               _busy = true;
               Camera.Size size = camera.GetParameters().PreviewSize;
               GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

               if (Mode == ViewMode.Canny)
               {
                  if (_bgrData == null || _bgrData.Length < size.Width * size.Height)
                     _bgrData = new byte[size.Width * size.Height * 3];
                  GCHandle bgrHandle = GCHandle.Alloc(_bgrData, GCHandleType.Pinned);
                  using (Image<Gray, Byte> grey = new Image<Gray, byte>(size.Width, size.Height, size.Width, handle.AddrOfPinnedObject()))
                  using (Image<Gray, Byte> canny = new Image<Gray,byte>(size.Width, size.Height, size.Width, bgrHandle.AddrOfPinnedObject()))
                  { 
                     CvInvoke.cvCanny(grey, canny, 100, 60, 3);

                     if (_bgraData == null || _bgraData.Length < size.Width * size.Height)
                        _bgraData = new int[size.Width * size.Height];

                     GCHandle bgraHandle = GCHandle.Alloc(_bgraData, GCHandleType.Pinned);
                     using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, bgraHandle.AddrOfPinnedObject()))
                        CvInvoke.cvCvtColor(canny, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGRA);
                     bgraHandle.Free();
                  }
               }
               else if (Mode == ViewMode.Preview)
               {
                  if (_bgraData == null || _bgraData.Length < size.Width * size.Height)
                     _bgraData = new int[size.Width * size.Height];
                  GCHandle bgraHandle = GCHandle.Alloc(_bgraData, GCHandleType.Pinned);
                  using (Image<Gray, Byte> yuv420sp = new Image<Gray, byte>(size.Width, (size.Height >> 1) * 3, size.Width, handle.AddrOfPinnedObject()))
                  using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, bgraHandle.AddrOfPinnedObject()))
                     CvInvoke.cvCvtColor(yuv420sp, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGRA);
                  bgraHandle.Free();
               }
               else
               {
                  if (_bgraData == null || _bgraData.Length < size.Width * size.Height)
                     _bgraData = new int[size.Width * size.Height];
                  if (_bgrData == null || _bgrData.Length < size.Width * size.Height * 3)
                     _bgrData = new byte[size.Width * size.Height * 3];

                  GCHandle bgraHandle = GCHandle.Alloc(_bgraData, GCHandleType.Pinned);
                  GCHandle bgrHandle = GCHandle.Alloc(_bgrData, GCHandleType.Pinned);
                  using (Image<Gray, Byte> yuv420sp = new Image<Gray, byte>(size.Width, (size.Height >> 1) * 3, size.Width, handle.AddrOfPinnedObject()))
                  using (Image<Bgr, Byte> bgr = new Image<Bgr, byte>(size.Width, size.Height, size.Width * 3, bgrHandle.AddrOfPinnedObject()))
                  using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, bgraHandle.AddrOfPinnedObject()))
                  {
                     CvInvoke.cvCvtColor(yuv420sp, bgr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGR);
                     CvInvoke.ApplyColorMap(bgr, bgr, Emgu.CV.CvEnum.ColorMapType.Summer);
                     CvInvoke.cvCvtColor(bgr, bgra, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2BGRA);
                  }
                  bgraHandle.Free();
                  bgrHandle.Free();
               }
               handle.Free();

               lock (this)
               {
                  if (_bmp != null && (_bmp.Width != size.Width || _bmp.Height != size.Height))
                  {
                     _bmp.Dispose();
                     _bmp = null;
                  }
                  if (_bmp == null)
                     _bmp = Android.Graphics.Bitmap.CreateBitmap(size.Width, size.Height, Android.Graphics.Bitmap.Config.Argb8888);

                  _bmp.SetPixels(_bgraData, 0, size.Width, 0, 0, size.Width, size.Height);
               }

               this.Invalidate();
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
      ISurfaceHolder surface_holder;
      Camera camera;
      TopLayer _topLayer;

      private bool _cameraPreviewCallbackWithBuffer;

      public Preview(Context context, TopLayer topLayer, bool cameraPreviewCallbackWithBuffer)
         : base(context)
      {
         _cameraPreviewCallbackWithBuffer = cameraPreviewCallbackWithBuffer;

         // Install a SurfaceHolder.Callback so we get notified when the
         // underlying surface is created and destroyed.
         surface_holder = Holder;
         surface_holder.AddCallback(this);

         _topLayer = topLayer;
      }

      public void SurfaceCreated(ISurfaceHolder holder)
      {
         // The Surface has been created, acquire the camera and tell it where
         // to draw.

         int numberOfCameras = Camera.NumberOfCameras;
         if (numberOfCameras > 0)
         {
            camera = Camera.Open(0);

            try
            {
               camera.SetPreviewDisplay(holder);
            }
            catch (Exception)
            {
               camera.Release();
               camera = null;
               // TODO: add more exception handling logic here
            }
         }
      }

      public void SurfaceDestroyed(ISurfaceHolder holder)
      {
         // Surface will be destroyed when we return, so stop the preview.
         // Because the CameraDevice object is not a shared resource, it's very
         // important to release it when the activity is paused.
         if (camera != null)
         {
            camera.StopPreview();
            if (_cameraPreviewCallbackWithBuffer)
               camera.SetPreviewCallbackWithBuffer(null);
            else
               camera.SetPreviewCallback(null);
            camera.Release();
            camera = null;
         }
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

      public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h)
      {
         int maxWidth = 640, maxHeight = 480;

         // Now that the size is known, set up the camera parameters and begin
         // the preview.
         Camera.Parameters parameters = camera.GetParameters();

         IList<Camera.Size> sizes = parameters.SupportedPreviewSizes;
         Camera.Size optimalSize = GetOptimalPreviewSize(sizes, w, h, maxWidth, maxHeight);

         parameters.SetPreviewSize(optimalSize.Width, optimalSize.Height);
         
         camera.SetParameters(parameters);

         //set for protrait mode
         //camera.SetDisplayOrientation(90);

         if (_cameraPreviewCallbackWithBuffer)
         {
            int bufferSize = optimalSize.Width * (optimalSize.Height >> 1) * 3;
            camera.SetPreviewCallbackWithBuffer(_topLayer);
            for (int i = 0; i < 1; ++i)
               camera.AddCallbackBuffer(new byte[bufferSize]);
         }
         else
            camera.SetPreviewCallback(_topLayer);

         camera.StartPreview();

         Layout(0, 0, optimalSize.Width, optimalSize.Height);
      }
   }
}

