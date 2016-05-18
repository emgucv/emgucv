//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing;
using Android.Graphics;
using Emgu.CV.CvEnum;
using PedestrianDetection;

namespace AndroidExamples
{
   [Activity(Label = "Pedestrian Detection")]
   public class PedestrianDetectionActivity : ButtonMessageImageActivity
   {
      public PedestrianDetectionActivity()
         : base("Detect Pedestrian")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         OnImagePicked += (sender, image) =>
         {
            if (image == null)
               return;
            AppPreference appPreference = new AppPreference();
            CvInvoke.UseOpenCL = appPreference.UseOpenCL;
            String oclDeviceName = appPreference.OpenClDeviceName;
            if (!String.IsNullOrEmpty(oclDeviceName) && CvInvoke.UseOpenCL)
            {
               CvInvoke.OclSetDefaultDevice(oclDeviceName);
            }

            long time;
            Rectangle[] pedestrians;
            using (UMat umat = image.GetUMat(AccessType.ReadWrite))
               pedestrians = FindPedestrian.Find(image, out time);

            String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Emgu.CV.Ocl.Device.Default.Name : "CPU";
            SetMessage(String.Format("Detection completed with {1} in {0} milliseconds.", time, computeDevice));
            foreach (Rectangle rect in pedestrians)
            {
               CvInvoke.Rectangle(image, rect, new Bgr(System.Drawing.Color.Red).MCvScalar, 2);
            }

            SetImageBitmap(image.ToBitmap());
            image.Dispose();
         };

         OnButtonClick += (sender, args) =>
         {
            PickImage("pedestrian.png");
         };
      }
   }
}

