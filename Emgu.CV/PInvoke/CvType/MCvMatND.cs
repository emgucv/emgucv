//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
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
      /// Header reference count
      /// </summary>
      public int hdr_refcount;

      /// <summary>
      /// data pointers
      /// </summary>
      public IntPtr data;

      /// <summary>
      /// pairs (number of elements, distance between elements in bytes) for every dimension
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)CvEnum.GENERAL.CV_MAX_DIM)]
      public Dimension[] dim;

      /// <summary>
      /// The MatND Dimension
      /// </summary>
      [StructLayout(LayoutKind.Sequential)]
      public struct Dimension
      {
         private int _size;
         private int _step;

         /// <summary>
         /// Number of elements in this dimension
         /// </summary>
         public int Size
         {
            get { return _size; }
            set { _size = value; }
         }

         /// <summary>
         /// distance between elements in bytes for this dimension
         /// </summary>
         public int Step
         {
            get { return _step; }
            set { _step = value; }
         }
      }
   }
}
