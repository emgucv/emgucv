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
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;
using Android.Util;

namespace AndroidExamples
{
   public class LicenseDialogPreference : DialogPreference
   {
      public LicenseDialogPreference(Context context,  IAttributeSet attrs)
         :base(context, attrs )
      {

      }

      protected override View OnCreateDialogView()
      {
         //return base.OnCreateDialogView();
         LayoutInflater inflator = LayoutInflater.FromContext(this.Context);
         View dialog = inflator.Inflate(Resource.Layout.LicenseDisplay, null);
         TextView opencvLicenseTextView = dialog.FindViewById<TextView>(Resource.Id.openCVLicenseTextView);
         
         using (System.IO.StreamReader reader = new StreamReader(Context.Resources.OpenRawResource(Resource.Raw.LICENSE)))
         {
            opencvLicenseTextView.Text = reader.ReadToEnd();
         }
         return dialog;
      }
   }
}