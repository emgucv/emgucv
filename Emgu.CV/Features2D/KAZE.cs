//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
﻿using Emgu.CV.Features2D;
﻿using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped KAZE detector
   /// </summary>
   public class KAZE : Feature2D
   {
      /// <summary>
      /// The diffusivity
      /// </summary>
      public enum Diffusivity
      {
         /// <summary>
         /// PM G1
         /// </summary>
         PmG1 = 0,
         /// <summary>
         /// PM G2
         /// </summary>
         PmG2 = 1,
         /// <summary>
         /// Weickert
         /// </summary>
         Weickert = 2,
         /// <summary>
         /// Charbonnier
         /// </summary>
         Charbonnier = 3 
      }

      /// <summary>
      /// Create KAZE using the specific values
      /// </summary>
      public KAZE(bool extended, bool upright, float threshold = 0.001f, int octaves = 4, int sublevels = 4, Diffusivity diffusivity = Diffusivity.PmG2)
      {
         _ptr = CvInvoke.cveKAZEDetectorCreate(extended, upright, threshold, octaves, sublevels, diffusivity,
            ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveKAZEDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveKAZEDetectorCreate(
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool extended, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool upright, 
         float threshold,
         int octaves, int sublevels, KAZE.Diffusivity diffusivity,
         ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveKAZEDetectorRelease(ref IntPtr detector);
   }
}
