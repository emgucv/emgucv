//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of standard vector of Point.
   /// </summary>
   public class VectorOfVectorOfPoint : Emgu.Util.UnmanagedObject
   {
      /// <summary>
      /// Create an empty standard vector of standard of Point
      /// </summary>
      public VectorOfVectorOfPoint()
      {
         _ptr = CvInvoke.VectorOfVectorOfPointCreate();
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return CvInvoke.VectorOfVectorOfPointGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         CvInvoke.VectorOfVectorOfPointClear(_ptr);
      }

      /// <summary>
      /// Get the item in the specific index
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The item in the specific index</returns>
      public VectorOfPoint this[int index]
      {
         get
         {
            return new VectorOfPoint(CvInvoke.VectorOfVectorOfPointGetItem(_ptr, index), false);  
         }
      }

      /// <summary>
      /// Convert the standard vector to arrays of Point
      /// </summary>
      /// <returns>Arrays of Point</returns>
      public Point[][] ToArray()
      {
         int size = Size;
         Point[][] res = new Point[size][];
         for (int i = 0; i < size; i++)
         {
            VectorOfPoint v = this[i];
            res[i] = v.ToArray();
         }
         return res;
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.VectorOfVectorOfPointRelease(_ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfPointCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfPointRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfVectorOfPointGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfPointClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfPointGetItem(IntPtr points, int index);
   }
}