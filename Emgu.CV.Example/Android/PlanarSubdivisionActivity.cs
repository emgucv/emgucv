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
using Android.Graphics;

//using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using PlanarSubdivisionExample;
//using Android.Graphics.Bitmap;
using System.Runtime.InteropServices;

namespace AndroidExamples
{
   [Activity(Label = "Planar Subdivision")]
   public class PlanarSubdivisionActivity : ButtonMessageImageActivity
   {
      public PlanarSubdivisionActivity()
         : base("Planar Subdivision")
      {
      }

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);
         
         OnButtonClick += delegate
         {
            int maxValue = 600, pointCount = 30;

            SetImageBitmap(DrawSubdivision.Draw(maxValue, pointCount).ToBitmap());
         };
      }
   }
}

