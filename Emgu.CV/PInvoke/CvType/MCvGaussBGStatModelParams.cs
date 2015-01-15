/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapper to the CvBGStatModel
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvGaussBGStatModelParams
   {
      /// <summary>
      /// Learning rate; alpha = 1/win_size. (default = 200)
      /// </summary>
      public int win_size;
      /// <summary>
      /// Number of Gaussians in mixture. (default = 5, Max = 500)
      /// </summary>
      public int n_gauss;
      /// <summary>
      /// Threshold sum of weights for background test. (default = 0.7)
      /// </summary>
      public double bg_threshold;
      /// <summary>
      /// Lambda=2.5 is 99% (default = 2.5)
      /// </summary>
      public double std_threshold;
      /// <summary>
      /// Discard foreground blobs whose bounding box is smaller than this threshold. (default = 15.f)
      /// </summary>
      public double minArea;
      /// <summary>
      /// Default = 0.05
      /// </summary>
      public double weight_init;
      /// <summary>
      /// Default = 30*30
      /// </summary>
      public double variance_init;
   }
}*/