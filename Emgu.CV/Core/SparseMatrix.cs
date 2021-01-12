//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Create a sparse matrix
   /// </summary>
   /// <typeparam name="TDepth">The type of elements in this matrix</typeparam>
   public class SparseMatrix<TDepth> : UnmanagedObject
            where TDepth : new()
   {
      private int[] _dimension;

      /// <summary>
      /// Create a sparse matrix of the specific dimension
      /// </summary>
      /// <param name="dimension">The dimension of the sparse matrix</param>
      public SparseMatrix(int[] dimension)
      {
         _dimension = new int[dimension.Length];
         Array.Copy(dimension, _dimension, dimension.Length);
         GCHandle handle =  GCHandle.Alloc(_dimension, GCHandleType.Pinned);
         _ptr = CvInvoke.cvCreateSparseMat(_dimension.Length, handle.AddrOfPinnedObject(), CvInvoke.GetDepthType(typeof(TDepth)));
         handle.Free();
      }

      /// <summary>
      /// Get or Set the value in the specific <paramref name="row"/> and <paramref name="col"/>
      /// </summary>
      /// <param name="row">the row of the element</param>
      /// <param name="col">the col of the element</param>
      /// <returns>The element on the specific <paramref name="row"/> and <paramref name="col"/></returns>
      public double this[int row, int col]
      {
         get
         {
            return CvInvoke.cvGetReal2D(Ptr, row, col);
         }
         set
         {
            CvInvoke.cvSet2D(Ptr, row, col, new MCvScalar(value));
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this sparse matrix
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseSparseMat(ref _ptr);
      }
   }
}
