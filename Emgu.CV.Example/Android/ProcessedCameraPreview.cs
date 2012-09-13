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

      private ImageFilter _imageFilter;

      Activity _activity;

      private bool _cameraPreviewCallbackWithBuffer;

      private ViewMode _mode = ViewMode.Preview;

      public ViewMode Mode
      {
         get
         {
            return _mode;
         }
         set
         {
            if (_mode != value)
            {
               _mode = value;

               lock (typeof(ImageFilter))
               {
                  if (_imageFilter != null)
                     _imageFilter.Dispose();

                  if (value == ViewMode.Canny)
                  {
                     _imageFilter = new CannyFilter(100, 60, 3);

                  }
                  else if (value == ViewMode.ColorMap)
                  {
                     _imageFilter = new ColorMapFilter(Emgu.CV.CvEnum.ColorMapType.Summer);

                  }
                  else //if (Mode == ViewMode.Distor)
                  {
                     _imageFilter = new DistorFilter(0.5, 0.5, -1.5);
                  }
               }
            }


         }
      }

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
         if (_imageFilter != null)
            _imageFilter.Dispose();
      }

      private static FileInfo SaveBitmap(Context context, Android.Graphics.Bitmap bmp, MediaScannerConnection.IOnScanCompletedListener onScanCompleteListener)
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

      private static Android.Graphics.Bitmap GetThumbnail(Android.Graphics.Bitmap bmp, int maxSize)
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

      public void OnPictureTaken(byte[] data, Camera camera)
      {
         if (data != null)
         {
            try
            {
               Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);

               Android.Graphics.Bitmap thumbnail = null;
               int maxThumbnailSize = 96;
               if (_imageFilter == null)
               {
                  SaveBitmap(_activity, bmp, this);
                  thumbnail = GetThumbnail(bmp, maxThumbnailSize);
                  bmp.Dispose();

               }
               else
               {
                  Image<Bgr, Byte> buffer1 = new Image<Bgr, byte>(bmp);
                  bmp.Dispose();
                  Image<Bgr, Byte> buffer2 = new Image<Bgr, byte>(buffer1.Size);
                  ImageFilter filter = _imageFilter.Clone() as ImageFilter;

                  filter.ProcessData(buffer1, buffer2);
                  buffer1.Dispose();
                  filter.Dispose();

                  using (Android.Graphics.Bitmap result = buffer2.ToBitmap())
                  {
                     buffer2.Dispose();
                     thumbnail = GetThumbnail(result, maxThumbnailSize);
                     SaveBitmap(_activity, result, this);
                  }
               }
            }
            catch (Exception excpt)
            {
               _activity.RunOnUiThread(() =>
               {
                  while (excpt.InnerException != null)
                     excpt = excpt.InnerException;
                  AlertDialog.Builder alert = new AlertDialog.Builder(_activity);
                  alert.SetTitle("Error saving file");
                  alert.SetMessage(excpt.Message);
                  alert.SetPositiveButton("OK", (s, er) => { });
                  alert.Show();
               });
               return;
            }
            /*
         catch (FileNotFoundException e)
         {
            Android.Util.Log.Debug("Emgu.CV", e.Message);
         }
         catch (IOException e)
         {
            Android.Util.Log.Debug("Emgu.CV", e.Message);
         } */

            /*
            try
            {
               ExifInterface exif = new ExifInterface(f.FullName);
               // Read the camera model and location attributes
               exif.GetAttribute(ExifInterface.TagModel);
               float[] latLng = new float[2];
               exif.GetLatLong(latLng);
               // Set the camera make
               exif.SetAttribute(ExifInterface.TagMake, "My Phone");
               exif.SetAttribute(ExifInterface.TagDatetime, System.DateTime.Now.ToString());
            }
            catch (IOException e)
            {
               Android.Util.Log.Debug("Emgu.CV", e.Message);
            }*/


            Toast.MakeText(_activity, "File Saved.", ToastLength.Short).Show();
            camera.StartPreview();
         }
         else
         {
            Android.Util.Log.Debug("Emgu.CV", "No image");
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

                     lock (typeof(ImageFilter))
                        _imageFilter.ProcessData(bgr, image);

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
}