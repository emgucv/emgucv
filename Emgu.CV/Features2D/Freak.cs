//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Freak Descriptor
   /// </summary>
   public class Freak : UnmanagedObject, IDescriptorExtractor<Gray, Byte>
   {
      /// <summary>
      /// Create a Freak descriptor extractor.
      /// </summary>
      /// <param name="orientationNormalized">Enable orientation normalization, use true for default.</param>
      /// <param name="scaleNormalized">Enable scale normalization, use true for default.</param>
      /// <param name="patternScale">Scaling of the description pattern, use 22.0f for default.</param>
      /// <param name="nOctaves">Number of octaves covered by the detected keypoints, use 4 for default.</param>
      public Freak(bool orientationNormalized, bool scaleNormalized, float patternScale, int nOctaves)
      {
         _ptr = CvInvoke.CvFreakCreate(orientationNormalized, scaleNormalized, patternScale, nOctaves);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with BRIEF
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvFreakRelease(ref _ptr);
      }

      IntPtr IDescriptorExtractor<Gray, Byte>.DescriptorExtratorPtr
      {
         get { return _ptr; }
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFreakCreate(
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool orientationNormalized,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool scaleNormalized,
         float patternScale,
         int nOctaves);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFreakRelease(ref IntPtr extractor);
   }
}
