//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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

namespace AndroidExamples
{
   public class OpenCLDialogPreference: DialogPreference
   {
      public OpenCLDialogPreference(Context context,  IAttributeSet attrs)
         :base(context, attrs )
      {

      }

      private ToggleButton _openCLToggleButton;

      protected override View OnCreateDialogView()
      {
         
         LayoutInflater inflator = LayoutInflater.FromContext(this.Context);
         View dialog = inflator.Inflate(Resource.Layout.opencl_preference, null);

         _openCLToggleButton = dialog.FindViewById<ToggleButton>(Resource.Id.opencl_preference_toggleButton);

         AppPreference preference = new AppPreference();
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

         return dialog;
      }


   }
}