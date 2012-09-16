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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Paint = Android.Graphics.Paint;

namespace Emgu.CV
{
   public class CameraPreview : SurfaceView, ISurfaceHolderCallback
   {
      ISurfaceHolder _surfaceHolder;
      Camera _camera;
      int _cameraIndex;
      Camera.IPreviewCallback _cameraPreviewCallback;

      private bool _cameraPreviewCallbackWithBuffer;

      public CameraPreview(Context context, Camera.IPreviewCallback previewCallback, bool cameraPreviewCallbackWithBuffer)
         : base(context)
      {
         _cameraPreviewCallbackWithBuffer = cameraPreviewCallbackWithBuffer;

         // Install a SurfaceHolder.Callback so we get notified when the
         // underlying surface is created and destroyed.
         _surfaceHolder = Holder;
         _surfaceHolder.AddCallback(this);

         _cameraPreviewCallback = previewCallback;
      }

      public Camera Camera
      {
         get
         {
            return _camera;
         }
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

      private bool CreateCamera(int cameraIndex)
      {
         try
         {
            StopCamera();
            _camera = Camera.Open(cameraIndex);
            _camera.SetPreviewDisplay(_surfaceHolder);
            if (OnCameraCreated != null)
            {
               OnCameraCreated(this, new EventArgs<Camera>(_camera));
            }
         }
         catch (Exception)
         {
            _camera.Release();
            _camera = null;
            return false;
            // TODO: add more exception handling logic here
         }

         return true;
      }

      public event EventHandler<EventArgs<Camera>> OnCameraCreated;

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

      private static Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h, int maxWidth, int maxHeight)
      {
         Camera.Size size = sizes[0];
         double area = size.Width * size.Height;
         double minArea = area;
         int minAreaIdx = 0;

         bool found = false;

         for (int i = 1; i < sizes.Count; i++)
         {
            Camera.Size s = sizes[i];
            double a = s.Width * s.Height;
            if (a < minArea)
            {
               minArea = a;
               minAreaIdx = i;
            }

            if (s.Width <= maxWidth && s.Height <= maxHeight)
            {
               if (!found)
               {
                  found = true;
                  area = a;
                  size = s;
               } else if (a > area)
               {
                  area = a;
                  size = s;
               }
            }
         }

         if (size.Width > maxWidth || size.Height > maxHeight)
         {
            size = sizes[minAreaIdx];
         }
         return size;

         /*
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

         return optimalSize;*/
      }


      private static Camera.Size SetCameraOptimalPreviewSize(Camera camera, int w, int h)
      {
         Camera.Parameters parameters = camera.GetParameters();
         IList<Camera.Size> sizes = parameters.SupportedPreviewSizes;
         int maxWidth = 512, maxHeight = 512;
         Camera.Size optimalSize = GetOptimalPreviewSize(sizes, w, h, maxWidth, maxHeight);
         parameters.SetPreviewSize(optimalSize.Width, optimalSize.Height);
         camera.SetParameters(parameters);
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

         if (_cameraPreviewCallback != null)
         {
            if (_cameraPreviewCallbackWithBuffer)
            {
               int bufferSize = optimalSize.Width * (optimalSize.Height >> 1) * 3;
               _camera.SetPreviewCallbackWithBuffer(_cameraPreviewCallback);
               for (int i = 0; i < 1; ++i)
                  _camera.AddCallbackBuffer(new byte[bufferSize]);
            }
            else
               _camera.SetPreviewCallback(_cameraPreviewCallback);
         }
         _camera.StartPreview();

         Layout(0, 0, optimalSize.Width, optimalSize.Height);
      }
   }
}
