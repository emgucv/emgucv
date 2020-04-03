//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

using PedestrianDetection;

namespace Emgu.CV.XamarinForms
{
    public class PedestrianDetectionPage : ButtonTextImagePage
    {
        public PedestrianDetectionPage()
           : base()
        {
            var button = this.GetButton();
            button.Text = "Perform Pedestrian Detection";

            button.Clicked += OnButtonClicked;
        }


        public void ProcessImage(Mat m)
        {
            Rectangle[] pedestrians;
            if (m.NumberOfChannels == 4)
            {
                //if the png file is loaded with alpha channel
                using (Mat bgr = new Mat())
                {
                    CvInvoke.CvtColor(m, bgr, ColorConversion.Bgra2Bgr);
                    pedestrians = FindPedestrian.Find(bgr);
                }
            }
            else
                pedestrians = FindPedestrian.Find(m);

            foreach (Rectangle rect in pedestrians)
            {
                CvInvoke.Rectangle(m, rect, new MCvScalar(0, 0, 255), 2);
            }

        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            Mat[] images = await LoadImages(new String[] { "pedestrian.png" });
            if (images == null || images[0] == null)
                return;
            SetMessage("please wait...");
            SetImage(null);

            Stopwatch watch = Stopwatch.StartNew();
            await Task.Run(() => ProcessImage(images[0]));
            watch.Stop();

            String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
            SetMessage(String.Format("Detection completed with {1} in {0} milliseconds.", computeDevice, watch.ElapsedMilliseconds));
            SetImage(images[0]);
        }
    }
}

