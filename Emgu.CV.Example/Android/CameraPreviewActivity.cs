//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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

         _bgrBuffers = new ImageBufferFactory<Bgr>();

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
      }

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

      private ImageFilter _imageFilter;
      private ImageBufferFactory<Bgr> _bgrBuffers;

      public void ImagePreview(Object sender, ProcessedCameraPreview.ImagePreviewEventArgs e)
      {
         Image<Gray, Byte> yuv420sp = e.Yuv420sp;
         Image<Bgr, Byte> image = e.Result;
         if (Mode == ViewMode.Preview)
         {
            CvInvoke.cvCvtColor(yuv420sp, image, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGR);
         }
         else
         {
            Image<Bgr, Byte> bgr = _bgrBuffers.GetBuffer(image.Size, 0);
            CvInvoke.cvCvtColor(yuv420sp, bgr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_YUV420sp2BGR);

            lock (typeof(ImageFilter))
               _imageFilter.ProcessData(bgr, image);
         }
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
               ProcessedCameraPreview.SaveBitmap(this, bmp, _topLayer);
               thumbnail = ProcessedCameraPreview.GetThumbnail(bmp, maxThumbnailSize);
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
                  thumbnail = ProcessedCameraPreview.GetThumbnail(result, maxThumbnailSize);
                  ProcessedCameraPreview.SaveBitmap(this, result, _topLayer);
               }
            }
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
            Mode = ViewMode.Canny;
         }
         else if (item == _menuColorMap)
         {
            Mode = ViewMode.ColorMap;
         }
         else if (item == _menuPreview)
         {
            Mode = ViewMode.Preview;
         }
         else
         {
            Mode = ViewMode.Distor;
         }
         return base.OnOptionsItemSelected(item);
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

