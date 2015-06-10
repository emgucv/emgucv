//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
   /// <summary>
   /// Wrapped SIFT detector
   /// </summary>
   public class SIFT : Feature2D
   {
      /// <summary>
      /// Create a SIFT using the specific values
      /// </summary>
      /// <param name="nFeatures">The desired number of features. Use 0 for un-restricted number of features</param>
      /// <param name="nOctaveLayers">The number of octave layers. Use 3 for default</param>
      /// <param name="contrastThreshold">Contrast threshold. Use 0.04 as default</param>
      /// <param name="edgeThreshold">Detector parameter. Use 10.0 as default</param>
      /// <param name="sigma">Use 1.6 as default</param>
      public SIFT(
         int nFeatures = 0, int nOctaveLayers = 3,
         double contrastThreshold = 0.04, double edgeThreshold = 10.0,
         double sigma = 1.6)
      {
         _ptr = ContribInvoke.cveSIFTCreate(nFeatures, nOctaveLayers, contrastThreshold, edgeThreshold, sigma, ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            ContribInvoke.cveSIFTRelease(ref _ptr);
         base.DisposeObject();
      }
   }

   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveSIFTCreate(
         int nFeatures, int nOctaveLayers,
         double contrastThreshold, double edgeThreshold,
         double sigma, ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveSIFTRelease(ref IntPtr detector);
   }
}