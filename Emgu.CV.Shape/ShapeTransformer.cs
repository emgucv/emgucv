//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Shape
{
   /// <summary>
   /// Abstract base class for shape transformation algorithms.
   /// </summary>
   public abstract class ShapeTransformer : UnmanagedObject
   {
      /// <summary>
      /// Release the unmanaged memory associated with this ShapeTransformer object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ShapeInvoke.cvShapeTransformerRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Definition of the transformation ocupied in the paper “Principal Warps: Thin-Plate Splines and Decomposition of Deformations”, by F.L. Bookstein (PAMI 1989).
   /// </summary>
   public class ThinPlateSplineShapeTransformer : ShapeTransformer
   {
      /// <summary>
      /// Create a thin plate spline shape transformer
      /// </summary>
      /// <param name="regularizationParameter">The regularization parameter for relaxing the exact interpolation requirements of the TPS algorithm.</param>
      public ThinPlateSplineShapeTransformer(double regularizationParameter = 0)
      {
         _ptr = ShapeInvoke.cvThinPlateSplineShapeTransformerCreate(regularizationParameter);
      }
   }

   /// <summary>
   /// Wrapper class for the OpenCV Affine Transformation algorithm.
   /// </summary>
   public class AffineTransformer : ShapeTransformer
   {
      /// <summary>
      /// Create an affine transformer
      /// </summary>
      /// <param name="fullAffine">Full affine</param>
      public AffineTransformer(bool fullAffine)
      {
         _ptr = ShapeInvoke.cvAffineTransformerCreate(fullAffine);
      }
   }

   public static partial class ShapeInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvShapeTransformerRelease(ref IntPtr transformer);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvAffineTransformerCreate(
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool fullAffine);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvThinPlateSplineShapeTransformerCreate(double regularizationParameter);
   }
}
