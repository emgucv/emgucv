//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvMat
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvHistogram
   {
      /// <summary>
      /// The type of histogram
      /// </summary>
      public int type;

      /// <summary>
      /// Pointer to CvArr
      /// </summary>
      public IntPtr bins;

      /// <summary>
      /// For uniform histograms 
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)CvEnum.GENERAL.CV_MAX_DIM)]
      public RangeF[] thresh;

      /// <summary>
      /// For non-uniform histograms
      /// </summary>
      public IntPtr thresh2;

      /// <summary>
      /// Embedded matrix header for array histograms
      /// </summary>
      public MCvMatND mat;
   }
}
