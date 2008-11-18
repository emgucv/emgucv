using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Managed structure equivalent to CvMatND
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvMatND
   {
      /// <summary>
      /// CvMatND signature (CV_MATND_MAGIC_VAL), element type and flags
      /// </summary>
      public int type;
      /// <summary>
      /// number of array dimensions
      /// </summary>
      public int dims;
      /// <summary>
      /// underlying data reference counter
      /// </summary>
      public IntPtr refcount;
      /// <summary>
      /// data pointers
      /// </summary>
      public IntPtr data;

      /// <summary>
      /// pairs (number of elements, distance between elements in bytes) for every dimension
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * (int)CvEnum.GENERAL.CV_MAX_DIM)]
      public int[] dim;
   }
}
