//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim0;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim1;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim2;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim3;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim4;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim5;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim6;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim7;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim8;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim9;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim10;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim11;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim12;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim13;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim14;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim15;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim16;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim17;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim18;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim19;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim20;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim21;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim22;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim23;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim24;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim25;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim26;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim27;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim28;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim29;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim30;
      /// <summary>
      /// pairs (number of elements, distance between elements in bytes)
      /// </summary>
      public Dimension dim31;

      /// <summary>
      /// pairs (number of elements, distance between elements in bytes) for every dimension
      /// </summary>
      public Dimension[] dim
      {
         get
         {
            return new Dimension[] { dim0,dim1,dim2,dim3,dim4,dim5,dim6,dim7,dim8,dim9,dim10,dim11,dim12,dim13,dim14,dim15,dim16,dim17,dim18,dim19,dim20,dim21,dim22,dim23,dim24,dim25,dim26,dim27,dim28,dim29,dim30,dim31};
         }
      }

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
