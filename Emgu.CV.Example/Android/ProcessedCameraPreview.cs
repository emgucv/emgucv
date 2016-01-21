//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

//
// Camera preview based on monodroid API-samples
// https://github.com/xamarin/monodroid-samples/blob/master/ApiDemo/Graphics/CameraPreview.cs
//

#define GL_VIEW

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
   public class ProcessedCameraPreview :
      View,
      Camera.IPreviewCallback,
      Camera.IPictureCallback,
      MediaScannerConnection.IOnScanCompletedListener
   {
      private Stopwatch _watch;
      private Paint _paint;
      private Size _imageSize;
      private ImageBufferFactory<Image<Bgr, Byte>> _bgrBuffers;
      private bool _cameraPreviewCallbackWithBuffer;
      private bool _busy;

      public ProcessedCameraPreview(Activity activity, bool cameraPreviewCallbackWithBuffer)
         : base(activity)
      {
         _cameraPreviewCallbackWithBuffer = cameraPreviewCallbackWithBuffer;

         _paint = new Paint();
         _paint.SetStyle(Paint.Style.Stroke);
         _paint.SetARGB(255, 255, 0, 0);
         _paint.TextSize = 25;

         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(4, 8))
         {
         }
         _watch = Stopwatch.StartNew();
         _bgrBuffers = new ImageBufferFactory<Image<Bgr, byte>>( size => new Image<Bgr, Byte>(size) );
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         _bgrBuffers.Dispose();
      }

      /// <summary>
      /// Function to call when the image is scanned by the media scanner
      /// </summary>
      /// <param name="path">The path to the file that has been scanned.</param>
      /// <param name="uri">The Uri for the file if the scanning operation succeeded and the file was added to the media database, or null if scanning failed.</param>
      public void OnScanCompleted(string path, Android.Net.Uri uri)
      {
      }

      #region static methods
      public static FileInfo SaveBitmap(Context context, Android.Graphics.Bitmap bmp, String folderName, MediaScannerConnection.IOnScanCompletedListener onScanCompleteListener)
      {
         DateTime dateTime = DateTime.Now;
         String fileName = String.Format("IMG_{0}{1}{2}_{3}{4}{5}.jpg", dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
         //String fileName =  DateTime.Now.ToString().Replace('/', '-').Replace(':', '-').Replace(' ', '-') + ".jpg";
         FileInfo f = new FileInfo(System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), folderName, fileName));
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

      public static Android.Graphics.Bitmap GetThumbnail(Android.Graphics.Bitmap bmp, int maxDimension)
      {
         int width = maxDimension;
         int height = maxDimension;
         if (bmp.Width > bmp.Height)
         {
            height = maxDimension * bmp.Height / bmp.Width;
         }
         else
         {
            width = maxDimension * bmp.Width / bmp.Height;
         }
         return Android.Media.ThumbnailUtils.ExtractThumbnail(bmp, width, height);
      }
      #endregion

      #region definitions for events and eventArgs
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
      public delegate void PictureTakenEventHandler(object sender, PictureTakenEventArgs eventArgs);
      public event PictureTakenEventHandler PictureTaken;
      #endregion

      public void OnPictureTaken(byte[] data, Camera camera)
      {
         if (data != null && PictureTaken != null)
         {
            Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
            PictureTaken(this, new PictureTakenEventArgs(bmp, camera));
         }
         else
         {
            if (data == null)
               Report.Error(Context.PackageName, "No image is recevide from PictureTakenEventArgs");
            else
               Report.Error(Context.PackageName, "PictureTaken event handler is not registered");
         }
      }

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
               
#if DEBUG
               canvas.DrawText(String.Format("{0:F2} FPS; {1}x{2}; Render Time: {3} ms",
                  1.0 / _watch.ElapsedMilliseconds * 1000,
                  _imageSize.Width,
                  _imageSize.Height,
                  w.ElapsedMilliseconds), 20, 20, _paint);
#endif
               _watch.Reset();
               _watch.Start();
            }
         }
      }
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

               Invalidate();
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