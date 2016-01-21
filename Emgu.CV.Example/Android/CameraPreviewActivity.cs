//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

//
// Camera preview based on monodroid API-samples
// https://github.com/xamarin/monodroid-samples/blob/master/ApiDemo/Graphics/CameraPreview.cs
//



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
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Paint = Android.Graphics.Paint;

namespace AndroidExamples
{
   [Activity(Label = "Android Camera")]
   public class CameraPreviewActivity : Activity
   {
      private CameraPreview _preview;
      private ProcessedCameraPreview _topLayer;

      private ImageButton[] _previewButtons;
      private ImageFilter[] _previewFilters;
      
      private ImageBufferFactory<Android.Graphics.Bitmap> _previewBitmapBuffers;
      
      private ImageButton _lastCapturedImageButton;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);
         RequestWindowFeature(WindowFeatures.NoTitle);
         bool cameraPreviewCallbackWithBuffer = false;
         SetContentView(Resource.Layout.CameraPreviewLayout);

         _bgrBuffers = new ImageBufferFactory<Mat>(size => new Mat(size.Height, size.Width, DepthType.Cv8U, 3));
         _previewBitmapBuffers = new ImageBufferFactory<Android.Graphics.Bitmap>(s => Android.Graphics.Bitmap.CreateBitmap(s.Width, s.Height, Android.Graphics.Bitmap.Config.Rgb565));

         _topLayer = new ProcessedCameraPreview(this, cameraPreviewCallbackWithBuffer);
         _topLayer.PictureTaken += this.PictureTaken;
         _topLayer.ImagePreview += this.ImagePreview;
         
         _preview = new CameraPreview(this, _topLayer, cameraPreviewCallbackWithBuffer);

         RelativeLayout mainLayout = FindViewById<RelativeLayout>(Resource.Id.CameraPreiewRelativeLayout);
         
         mainLayout.AddView(_preview, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));
         mainLayout.AddView(_topLayer, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));

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

         ImageButton captureImageButton = FindViewById<ImageButton>(Resource.Id.CameraPreviewCaptureImageButton);
         captureImageButton.Click += delegate
         {
            Camera camera = _preview.Camera;

            if (camera != null)
            {
               Camera.Parameters p = camera.GetParameters();
               p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
               //p.PictureFormat = Android.Graphics.ImageFormatType.Rgb565;
               camera.SetParameters(p);
               camera.TakePicture(null, null, _topLayer);
            }
         };

         _lastCapturedImageButton = FindViewById<ImageButton>(Resource.Id.capturedImageButton);
         _lastCapturedImageButton.Click += delegate
         {
            if (_lastSavedImageFile != null)
            {
               Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.FromFile(new Java.IO.File(_lastSavedImageFile.FullName)));
               intent.SetType("image/jpeg");
               StartActivity(intent);
            }
         };
         _lastCapturedImageButton.BringToFront();

         _previewButtons = new ImageButton[4];
         _previewFilters = new ImageFilter[4];
         _previewButtons[0] = FindViewById<ImageButton>(Resource.Id.previewImageButton);
         _previewFilters[0] = null;
         _previewButtons[0].Click += delegate
         {
            if (_imageFilter != null)
            {
               _imageFilter.Dispose();
               _imageFilter = null;
            }
         };
         _previewButtons[1] = FindViewById<ImageButton>(Resource.Id.cannyImageButton);
         _previewFilters[1] = new CannyFilter(100, 60, 3);

         _previewButtons[2] = FindViewById<ImageButton>(Resource.Id.colorMapImageButton);
         _previewFilters[2] = new ColorMapFilter(Emgu.CV.CvEnum.ColorMapType.Autumn);
         
         //_previewFilters[3] = new ColorMapFilter(Emgu.CV.CvEnum.ColorMapType.Summer);
         //_previewFilters[3] = null;
         _previewButtons[3] = FindViewById<ImageButton>(Resource.Id.distorImageButton);
         _previewFilters[3] =  new DistorFilter(0.5, 0.5, -1.5);

         for (int i = 1; i < _previewButtons.Length; ++i)
         {
            ImageFilter f = _previewFilters[i];
            _previewButtons[i].Click += delegate
            {
               if (_imageFilter != null)
                  _imageFilter.Dispose();
               _imageFilter = f.Clone() as ImageFilter;
            };
         }
      }

      private ImageFilter _imageFilter;
      private ImageBufferFactory<Mat> _bgrBuffers;
      private FileInfo _lastSavedImageFile;

      public void ImagePreview(Object sender, ProcessedCameraPreview.ImagePreviewEventArgs e)
      {
         Image<Gray, Byte> yuv420sp = e.Yuv420sp;
         Image<Bgr, Byte> image = e.Result;

#region get the thumbnail size buffer
         Size s = image.Size;
         int maxDimension = 96;
         int width = maxDimension;
         int height = maxDimension;
         if (s.Width > s.Height)
         {
            height = maxDimension * s.Height / s.Width;
         }
         else
         {
            width = maxDimension * s.Width / s.Height;
         }
         s.Width = width;
         s.Height = height;
         Mat thumbnail = _bgrBuffers.GetBuffer(s, 1);
#endregion

         if (_imageFilter == null)
         {
            CvInvoke.CvtColor(yuv420sp, image, Emgu.CV.CvEnum.ColorConversion.Yuv420Sp2Bgr);
            CvInvoke.Resize(image, thumbnail, s, 0, 0, Emgu.CV.CvEnum.Inter.Nearest);
         }
         else
         {
            Mat bgr = _bgrBuffers.GetBuffer(image.Size, 0);
            CvInvoke.CvtColor(yuv420sp, bgr, Emgu.CV.CvEnum.ColorConversion.Yuv420Sp2Bgr);
            lock (typeof(ImageFilter))
               _imageFilter.ProcessData(bgr, image.Mat);
            CvInvoke.Resize(bgr, thumbnail, s, 0, 0, Emgu.CV.CvEnum.Inter.Nearest);
         }

         {
            Android.Graphics.Bitmap thumbnailBmp = _previewBitmapBuffers.GetBuffer(s, 0);
            using (BitmapRgb565Image img565 = new BitmapRgb565Image(thumbnailBmp))
               img565.ConvertFrom(thumbnail);
            _previewButtons[0].SetImageBitmap(thumbnailBmp);
  
            int startBufferIndex = 2;
            for (int i = 1; i < _previewFilters.Length; i++)
            {
               Mat buffer = _bgrBuffers.GetBuffer(s, startBufferIndex + i - 1);
               _previewFilters[i].ProcessData(thumbnail, buffer);

               Android.Graphics.Bitmap bmp = _previewBitmapBuffers.GetBuffer(s, i);
               using (BitmapRgb565Image tmp = new BitmapRgb565Image(bmp))
               {
                  tmp.ConvertFrom(buffer);
               }
               _previewButtons[i].SetImageBitmap(bmp);
            }
         } /*
         catch (Exception excpt)
         {
            while (excpt.InnerException != null)
               excpt = excpt.InnerException;
            Android.Util.Log.Debug("Emgu.CV", String.Format("Failed to generate preview: {0}", excpt.Message));
         }*/

      }

      public void PictureTaken(object sender, ProcessedCameraPreview.PictureTakenEventArgs ea)
      {
         Android.Graphics.Bitmap bmp = ea.Bitmap;
         Camera camera = ea.Camera;
         try
         {
            Android.Graphics.Bitmap thumbnail = null;
            int maxThumbnailSize = 96;
            if (_imageFilter == null)
            {
               _lastSavedImageFile = ProcessedCameraPreview.SaveBitmap(this, bmp, PackageName, _topLayer);
               thumbnail = ProcessedCameraPreview.GetThumbnail(bmp, maxThumbnailSize);
               bmp.Dispose();
            }
            else
            {
               Image<Bgr, Byte> buffer1 = new Image<Bgr, byte>(bmp);
               bmp.Dispose();

               using (ImageFilter filter = _imageFilter.Clone() as ImageFilter)
               {
                  if (filter is DistorFilter)
                  {
                     //reduce the image size to half because the distor filter used lots of memory
                     Image<Bgr, Byte> tmp = buffer1.PyrDown();
                     buffer1.Dispose();
                     buffer1 = tmp;
                  }

                  if (filter.InplaceCapable)
                     filter.ProcessData(buffer1.Mat, buffer1.Mat);
                  else
                  {
                     Image<Bgr, Byte> buffer2 = new Image<Bgr, byte>(buffer1.Size);
                     filter.ProcessData(buffer1.Mat, buffer2.Mat);
                     buffer1.Dispose();
                     buffer1 = buffer2;
                  }
               }
               
               using (Android.Graphics.Bitmap result = buffer1.ToBitmap())
               {
                  buffer1.Dispose();
                  _lastSavedImageFile = ProcessedCameraPreview.SaveBitmap(this, result, PackageName, _topLayer);
                  thumbnail = ProcessedCameraPreview.GetThumbnail(result, maxThumbnailSize);
               }
            }

            _lastCapturedImageButton.SetImageBitmap(thumbnail);
            
         }
         catch (Exception excpt)
         {
            this.RunOnUiThread(() =>
            {
               while (excpt.InnerException != null)
                  excpt = excpt.InnerException;
               AlertDialog.Builder alert = new AlertDialog.Builder(this);
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


         Toast.MakeText(this, "File Saved.", ToastLength.Short).Show();
         camera.StartPreview();
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         if (_imageFilter != null)
            _imageFilter.Dispose();
         _bgrBuffers.Dispose();
      }
   }

   public enum ViewMode
   {
      Preview,
      Canny,
      ColorMap,
      Distor
   }

}

