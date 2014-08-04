//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// An interface for feature detector
   /// </summary>
   public interface IFeatureDetector : IAlgorithm
   {
      /// <summary>
      /// Get the pointer to the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr FeatureDetectorPtr { get; }

   }
}

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveAlgorithmFromFeatureDetector(IntPtr detector);
   }
}
