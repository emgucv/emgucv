//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of cv::Mat
   /// </summary>
   public class VectorOfMat : Emgu.Util.UnmanagedObject
   {
      /// <summary>
      /// Create an empty standard vector of Mat
      /// </summary>
      public VectorOfMat()
      {
         _ptr = CvInvoke.VectorOfMatCreate();
      }

      /// <summary>
      /// Creat a standard vector of Mats
      /// </summary>
      /// <param name="values">The values to be pushed into the vector</param>
      public VectorOfMat(Mat[] values)
         :this()
      {
         Push(values);
      }

      /// <summary>
      /// Push a Mat into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(Mat value)
      {
         CvInvoke.VectorOfMatPush(_ptr, value.Ptr);
      }

      /// <summary>
      /// Push multiple Mat into the standard vector
      /// </summary>
      /// <param name="values">The values to be pushed to the vector</param>
      public void Push(Mat[] values)
      {
         foreach (Mat value in values)
            Push(value);
      }

      /// <summary>
      /// Convert a CvArray to cv::Mat and push it into the vector
      /// </summary>
      /// <typeparam name="TDepth">The type of depth of the cvArray</typeparam>
      /// <param name="cvArray">The cvArray to be pushed into the vector</param>
      public void Push<TDepth>(CvArray<TDepth> cvArray) where TDepth : new()
      {
         using (Mat m = new Mat())
         {
            m.From(cvArray);
            Push(m);
         }
      }

      /// <summary>
      /// Convert a group of CvArray to cv::Mat and push them into the vector
      /// </summary>
      /// <typeparam name="TDepth">The type of depth of the cvArray</typeparam>
      /// <param name="values">The values to be pushed to the vector</param>
      public void Push<TDepth>(CvArray<TDepth>[] values) where TDepth : new()
      {
         foreach (CvArray<TDepth> value in values)
            Push(value);
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return CvInvoke.VectorOfMatGetSize(_ptr);
         }
      }

      /// <summary>
      /// Get the Mat from the specific place in the vector 
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The mat</returns>
      public Mat this[int index]
      {
         get
         {
            return new Mat(CvInvoke.VectorOfMatGetItem(_ptr, index), false);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         CvInvoke.VectorOfMatClear(_ptr);
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.VectorOfMatRelease(_ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfMatCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfMatCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfMatRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfMatGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfMatPush(IntPtr v, IntPtr value);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfMatClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfMatGetItem(IntPtr v, int index);
   }
}