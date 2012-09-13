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

namespace AndroidExamples
{
   public class ProcessedCameraPreview :
#if GL_VIEW
      GLImageView,
#else
 View,
#endif
 Camera.IPreviewCallback,
      Camera.IPictureCallback,
      MediaScannerConnection.IOnScanCompletedListener
   {
      private Stopwatch _watch;
      Paint _paint;
      Size _imageSize;

      ImageBufferFactory<Bgr> _bgrBuffers;

      Activity _activity;

      private bool _cameraPreviewCallbackWithBuffer;

      public ProcessedCameraPreview(Activity activity, bool cameraPreviewCallbackWithBuffer)
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

      }

      public static FileInfo SaveBitmap(Context context, Android.Graphics.Bitmap bmp, MediaScannerConnection.IOnScanCompletedListener onScanCompleteListener)
      {
         String fileName = DateTime.Now.ToString().Replace('/', '-').Replace(':', '-').Replace(' ', '-') + ".jpg";
         FileInfo f = new FileInfo(System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), "EMGU", fileName));
         if (!f.Directory.Exists)
            f.Directory.Create();

         using (System.IO.FileStream writer = new System.IO.FileStream(
                           f.FullName,
                           System.IO.FileMode.Create))
         {
            bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, writer);
         }

         MediaScannerConnection.ScanFile(context, new String[] { f.FullName }, null, onScanCompleteListener);
         return f;
      }

      /// <summary>
      /// Function to call when the image is scanned by the media scanner
      /// </summary>
      /// <param name="path">The path to the file that has been scanned.</param>
      /// <param name="uri">The Uri for the file if the scanning operation succeeded and the file was added to the media database, or null if scanning failed.</param>
      public void OnScanCompleted(string path, Android.Net.Uri uri)
      {
      }

      public static Android.Graphics.Bitmap GetThumbnail(Android.Graphics.Bitmap bmp, int maxSize)
      {
         int width = maxSize;
         int height = maxSize;
         if (bmp.Width > bmp.Height)
         {
            height = maxSize * bmp.Height / bmp.Width;
         }
         else
         {
            width = maxSize * bmp.Width / bmp.Height;
         }
         return Android.Media.ThumbnailUtils.ExtractThumbnail(bmp, width, height);
      }


      public class PictureTakenEventArgs
      {
         public Android.Graphics.Bitmap Bitmap;
         public Camera Camera;

         public PictureTakenEventArgs(Android.Graphics.Bitmap bmp, Camera camera)
         {
            Bitmap = bmp;
            Camera = camera;
         }
      }
      public delegate void PictureTakenEventHandler(object sender, PictureTakenEventArgs eventArgs);
      public event PictureTakenEventHandler PictureTaken;

      public void OnPictureTaken(byte[] data, Camera camera)
      {
         if (data != null && PictureTaken != null)
         {
            Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
            PictureTaken(this, new PictureTakenEventArgs(bmp, camera));
            

         }
         else
         {
            Android.Util.Log.Debug("Emgu.CV", "No image or PictureTake is not registered");
         }
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
                  _bmpImage.Dispose();
                  _bmpImage = null;
                  _bmp.Dispose();
                  _bmp = null;
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

      public class ImagePreviewEventArgs 
      {
         public Image<Gray, Byte> Yuv420sp;
         public Image<Bgr, Byte> Result;

         public ImagePreviewEventArgs(Image<Gray, Byte> yuv420sp, Image<Bgr, Byte> result)
         {
            Yuv420sp = yuv420sp;
            Result = result;
         }
      }

      public delegate void ImagePreviewEventHandler(Object sender, ImagePreviewEventArgs e);

      public event ImagePreviewEventHandler ImagePreview;

      public void OnPreviewFrame(byte[] data, Camera camera)
      {
         if (!_busy && ImagePreview != null)
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
                  ImagePreview(this, new ImagePreviewEventArgs(yuv420sp, image));

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
}