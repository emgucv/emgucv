//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// A wrapper for the CvStructuringElementEx structure in opencv
   /// </summary>
   public class StructuringElementEx : UnmanagedObject
   {
      private int[,] _values;
      private GCHandle _handle;

      /// <summary>
      /// Create a custome shape Structuring Element
      /// </summary>
      /// <param name="values">The structuring element data, a plane array, representing row-by-row scanning of the element matrix. Non-zero values indicate points that belong to the element.</param>
      /// <param name="anchorX">Relative horizontal offset of the anchor point</param>
      /// <param name="anchorY">Relative vertical offset of the anchor point</param>
      public StructuringElementEx(int[,] values, int anchorX, int anchorY)
      {
         _values = values;
         _handle = GCHandle.Alloc(_values, GCHandleType.Pinned);
         _ptr = CvInvoke.cvCreateStructuringElementEx(
                   values.GetLength(1),
                   values.GetLength(0),
                   anchorX,
                   anchorY,
                   CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_CUSTOM,
                   _handle.AddrOfPinnedObject());
      }

      /// <summary>
      /// Create a structuring element of the specific type
      /// </summary>
      /// <param name="cols">Number of columns in the structuring element</param>
      /// <param name="rows">Number of rows in the structuring element</param>
      /// <param name="anchorX">Relative horizontal offset of the anchor point</param>
      /// <param name="anchorY">Relative vertical offset of the anchor point</param>
      /// <param name="shape">Shape of the structuring element</param>
      public StructuringElementEx(int cols, int rows, int anchorX, int anchorY, CvEnum.CV_ELEMENT_SHAPE shape)
      {
         Debug.Assert(shape != Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_CUSTOM, "For custome shape please use a different constructor");
         _ptr = CvInvoke.cvCreateStructuringElementEx(cols, rows, anchorX, anchorY, shape, IntPtr.Zero);
      }

      /// <summary>
      /// Release the unmanaged memory associated to this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseStructuringElement(ref _ptr);
         if (_handle.IsAllocated)
            _handle.Free();
      }
   }
}
