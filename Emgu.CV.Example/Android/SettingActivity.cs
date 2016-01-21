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
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace AndroidExamples
{
   [Activity(
      Label = "@string/menu_option_settings",
      Theme = "@android:style/Theme.NoTitleBar")]
   public class SettingActivity : PreferenceActivity
   {
      protected override void OnCreate(Bundle bundle)
      {
         //RequestWindowFeature(WindowFeatures.NoTitle);

         base.OnCreate(bundle);
         
         // Create your application here
         AddPreferencesFromResource(Resource.Layout.Setting);

      }
   }
}