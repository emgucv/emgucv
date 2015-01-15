/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// CvStarDetectorParams
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvStarDetectorParams
   {
      /// <summary>
      /// Maximum size of the features. The following
      /// values of the parameter are supported:
      /// 4, 6, 8, 11, 12, 16, 22, 23, 32, 45, 46, 64, 90, 128
      /// </summary>
      public int MaxSize;
      /// <summary>
      /// Threshold for the approximated laplacian,
      /// used to eliminate weak features. The larger it is,
      /// the less features will be retrieved
      /// </summary>
      public int ResponseThreshold;
      /// <summary>
      /// Another threshold for the laplacian to
      /// eliminate edges.
      /// The larger the threshold, the more points you get.
      /// </summary>
      public int LineThresholdProjected;
      /// <summary>
      /// Another threshold for the feature
      /// size to eliminate edges. 
      /// The larger the threshold, the more points you get.
      /// </summary>
      public int LineThresholdBinarized;
      /// <summary>
      /// 
      /// </summary>
      public int SuppressNonmaxSize;
   }
}
*/