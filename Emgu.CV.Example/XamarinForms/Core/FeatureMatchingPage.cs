//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;
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

            var p = this.Picker;
            p.Title = "Feature2D type";
            p.IsVisible = true;
            p.Items.Add("KAZE");
            p.Items.Add("SIFT");
        }

        private async void OnButtonClicked(Object sender, EventArgs args)
        {
            Mat[] images = await LoadImages(new String[] { "box.png", "box_in_scene.png" }, new string[] { "Pick a model image from", "Pick a observed image from" });
            if (images == null || images[0] == null || images[1] == null)
                return;
            SetMessage("Please wait...");
            SetImage(null);
            Task<Tuple<Mat, long>> t = new Task<Tuple<Mat, long>>(
                () =>
                {
                    long time;
                    Emgu.CV.Features2D.Feature2D featureDetectorExtrator;
                    String pickedFeature2D = this.Picker.SelectedItem.ToString();
                    if (pickedFeature2D.Equals("SIFT"))
                    {
                        featureDetectorExtrator = new SIFT();
                    }
                    else
                    {
                        featureDetectorExtrator = new KAZE();
                    }

                    Mat matchResult = DrawMatches.Draw(images[0], images[1], featureDetectorExtrator, out time);
                    featureDetectorExtrator.Dispose();
                    return new Tuple<Mat, long>(matchResult, time);
                });
            t.Start();

            var result = await t;
            foreach (var img in images)
                img.Dispose();

            SetImage(t.Result.Item1);
            String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
            SetMessage(String.Format("Detected with {1} in {0} milliseconds.", t.Result.Item2, computeDevice));
        }
    }
}
