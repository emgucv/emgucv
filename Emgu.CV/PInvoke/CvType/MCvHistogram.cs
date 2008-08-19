using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Managed structure equivalent to CvMat
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvHistogram
   {
      /// <summary>
      /// 
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
      public Range[] thresh;

      /// <summary>
      /// For non-uniform histograms
      /// </summary>
      public IntPtr thresh2;

      /// <summary>
      /// Embedded matrix header for array histograms
      /// </summary>
      public MCvMatND mat;

      /// <summary>
      /// A range
      /// </summary>
      [StructLayout(LayoutKind.Sequential)]
      public struct Range
      {
         /// <summary>
         /// The min value of this bin
         /// </summary>
         public float min;
         /// <summary>
         /// The max value of this bin
         /// </summary>
         public float max;
      }
   }
}
