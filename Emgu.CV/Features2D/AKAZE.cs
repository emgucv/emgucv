//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   /// Wrapped AKAZE detector
   /// </summary>
   public class AKAZE : Feature2D
   {
      public enum DescriptorType
      {
         KazeUpright = 2,
         Kaze = 3, 
         MldbUpright = 4,
         Mldb = 5
      }

      /// <summary>
      /// Create AKAZE using the specific values
      /// </summary>
      public AKAZE(DescriptorType descriptorType = DescriptorType.Mldb, int descriptorSize = 0, int descriptorChannels = 3,
         float threshold = 0.001f, int octaves = 4, int sublevels = 4, KAZE.Diffusivity diffusivity = KAZE.Diffusivity.PmG2)
      {
         _ptr = CvInvoke.cveAKAZEDetectorCreate(
            descriptorType, descriptorSize, descriptorChannels, 
            threshold, octaves, sublevels, diffusivity,
            ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveAKAZEDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveAKAZEDetectorCreate(
         AKAZE.DescriptorType descriptorType, int descriptorSize, int descriptorChannels,
         float threshold, int octaves, int sublevels, KAZE.Diffusivity diffusivity,
         ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAKAZEDetectorRelease(ref IntPtr detector);
   }
}
