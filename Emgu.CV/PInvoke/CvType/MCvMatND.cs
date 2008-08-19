using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Managed structure equivalent to CvMat
   /// </summary>
   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
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
      /// 
      /// </summary>
      public int hdr_refcount;
      /// <summary>
      /// data pointers 
      /// </summary>
      public IntPtr data;
      /// <summary>
      /// The odd indexed values are number of elements, the even indexed values are steps
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)CvEnum.GENERAL.CV_MAX_DIM)]
      public Dimension[] dim;

      /// <summary>
      /// the Dimension of MCvMatND 
      /// </summary>
      public struct Dimension
      {
         /// <summary>
         /// number of elements
         /// </summary>
         public int size;

         /// <summary>
         /// size of element in byte
         /// </summary>
         public int step;
      }
   }
}
