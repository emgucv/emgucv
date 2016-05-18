//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Preferences;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using FaceDetection;
using FeatureMatchingExample;

namespace Emgu.CV.XamarinForms
{
   public class FeatureMatchingPage : ButtonTextImagePage
   {
      public FeatureMatchingPage()
         : base()
      {
         var button = this.GetButton();
         button.Text = "Perform Feature Matching";
         button.Clicked += OnButtonClicked;

         OnImagesLoaded += async (sender, images) =>
         {
            GetLabel().Text = "Please wait...";
            SetImage(null);
            Task<Tuple<Mat, long>> t = new Task<Tuple<Mat, long>>(
               () =>
               {
                  long time;
                  Mat matchResult = DrawMatches.Draw(images[0], images[1], out time);
                  return new Tuple<Mat,long>(matchResult, time);
               });
            t.Start();

            var result = await t;
            foreach (var img in images)
               img.Dispose();
            
            SetImage(t.Result.Item1);
            String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
            GetLabel().Text = String.Format("Detected with {1} in {0} milliseconds.", t.Result.Item2, computeDevice);
         };
      }

      private void OnButtonClicked(Object sender, EventArgs args)
      {
         LoadImages(new String[] {"box.png", "box_in_scene.png"}, new string[] {"Pick a model image from", "Pick a observed image from"});
      }
   }
}
