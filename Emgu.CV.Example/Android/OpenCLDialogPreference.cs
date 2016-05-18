//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Emgu.CV;
using Emgu.CV.Util;

namespace AndroidExamples
{
   public class OpenCLDialogPreference: DialogPreference
   {
      public OpenCLDialogPreference(Context context,  IAttributeSet attrs)
         :base(context, attrs )
      {

      }

      private RadioGroup _openCLRadioGroup;

      protected override View OnCreateDialogView()
      {
         
         LayoutInflater inflator = LayoutInflater.FromContext(this.Context);
         View dialog = inflator.Inflate(Resource.Layout.opencl_preference, null);

         _openCLRadioGroup = dialog.FindViewById<RadioGroup>(Resource.Id.opencl_preference_radio_group);

         AppPreference preference = new AppPreference();

         RadioButton checkedButton = null;
         RadioButton cpuButton = new RadioButton(this.Context);
         cpuButton.Text = "CPU";

         _openCLRadioGroup.AddView(cpuButton);
         //int selectedIdx = -1;
         if (preference.UseOpenCL == false)
         {
            checkedButton = cpuButton;
         }
         cpuButton.Click += (sender, args) =>
         {
            preference.UseOpenCL = false;
            //Toast.MakeText(this.Context, "cpu clicked", ToastLength.Short).Show();
         };

         String selectedDeviceName = preference.OpenClDeviceName;
         if (selectedDeviceName == null && CvInvoke.HaveOpenCL && preference.UseOpenCL)
         {
            selectedDeviceName = Emgu.CV.Ocl.Device.Default.Name;
         }
         //int counter = 1;
         using (VectorOfOclPlatformInfo oclPlatformInfos = Emgu.CV.Ocl.OclInvoke.GetPlatformsInfo())
         {
            if (oclPlatformInfos.Size > 0)
            {
               for (int i = 0; i < oclPlatformInfos.Size; i++)
               {
                  Emgu.CV.Ocl.PlatformInfo platformInfo = oclPlatformInfos[i];

                  for (int j = 0; j < platformInfo.DeviceNumber; j++)
                  {
                     Emgu.CV.Ocl.Device device = platformInfo.GetDevice(j);
                     RadioButton deviceButton = new RadioButton(this.Context);
                     deviceButton.Text = "OpenCL: " + device.Name;

                     if (preference.UseOpenCL == true && device.Name.Equals(selectedDeviceName))
                     {
                        checkedButton = deviceButton;
                     }
                     _openCLRadioGroup.AddView(deviceButton);

                     //counter++;
                     deviceButton.Click += (sender, args) =>
                     {
                        preference.UseOpenCL = true;
                        preference.OpenClDeviceName = device.Name;
                        //Toast.MakeText(this.Context, device.Name + " clicked", ToastLength.Short).Show();
                     };

                  }
               }
            }
         }
         if (checkedButton != null)
            _openCLRadioGroup.Check(checkedButton.Id);
         //_openCLRadioGroup.in
         /*
            _openCLToggleButton.Checked = preference.UseOpenCL;

            _openCLToggleButton.CheckedChange += (sender, args) =>
            {
               bool isChecked = args.IsChecked;

               if (isChecked && !CvInvoke.HaveOpenCL)
               {
                  _openCLToggleButton.Checked = false;
                  Toast.MakeText(Context, "No OpenCL compatible device found.", ToastLength.Long).Show();
                  isChecked = false;
               }

               preference.UseOpenCL = isChecked;
            };
            */
         return dialog;
      }


   }
}