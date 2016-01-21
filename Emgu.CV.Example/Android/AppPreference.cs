//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Emgu.CV;

namespace AndroidExamples
{
   public class AppPreference
   {
      protected ISharedPreferences _preference;
      protected Context _context;

      public AppPreference(Context context = null)
      {
         _context = context == null ? Android.App.Application.Context : context;
         _preference = PreferenceManager.GetDefaultSharedPreferences(_context);
      }

      private const String _useOpenCLKey = "use_opencl";
      private const String _opencvDeviceNameKey = "opencl_device_name";
      public bool UseOpenCL
      {
         get
         {
            if (_preference.Contains(_useOpenCLKey))
            {
               return _preference.GetBoolean(_useOpenCLKey, false);
            }
            else
            {
               return CvInvoke.HaveOpenCL;
            }
         }
         set
         {
            _preference.Edit().PutBoolean(_useOpenCLKey, value).Commit();
         }
      }

      public String OpenClDeviceName
      {
         get
         {
            if (_preference.Contains(_opencvDeviceNameKey))
            {
               return _preference.GetString(_opencvDeviceNameKey, null);
            }
            else
            {
               return null;
            }
         }
         set
         {
            _preference.Edit().PutString(_opencvDeviceNameKey, value).Commit();
         }
      }
   }
}