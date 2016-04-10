//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
﻿using System.Runtime.InteropServices;
using System.Text;
﻿using Emgu.CV.Features2D;
﻿using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped AGAST detector
   /// </summary>
   public class AgastFeatureDetector : Feature2D
   {
      /// <summary>
      /// Agast feature type
      /// </summary>
      public enum Type
      {
         /// <summary>
         /// AGAST_5_8
         /// </summary>
         AGAST_5_8 = 0,
         /// <summary>
         /// AGAST_7_12d
         /// </summary>
         AGAST_7_12d = 1,
         /// <summary>
         /// AGAST_7_12s
         /// </summary>
         AGAST_7_12s = 2,
         /// <summary>
         /// OAST_9_16
         /// </summary>
         OAST_9_16 = 3,
      }

      /// <summary>
      /// Create AGAST using the specific values
      /// </summary>
      public AgastFeatureDetector(int threshold, bool nonmaxSuppression, Type type)
      {
         _ptr = CvInvoke.cveAgastFeatureDetectorCreate(threshold, nonmaxSuppression, type,
            ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveAgastFeatureDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveAgastFeatureDetectorCreate(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmaxSuppression, 
         Emgu.CV.Features2D.AgastFeatureDetector.Type type,
         ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAgastFeatureDetectorRelease(ref IntPtr detector);
   }
}
