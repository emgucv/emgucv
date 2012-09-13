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

         _topLayer = new ProcessedCameraPreview(this, cameraPreviewCallbackWithBuffer);
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

         //JpegCallback cameraCallback = new JpegCallback();

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

   public enum ViewMode
   {
      Preview,
      Canny,
      ColorMap, 
      Distor
   }

}

