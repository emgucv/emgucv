//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
   /// Abstract base class for shape distance algorithms.
   /// </summary>
   public abstract class ShapeDistanceExtractor : UnmanagedObject
   {
      /// <summary>
      /// Compute the shape distance between two shapes defined by its contours.
      /// </summary>
      /// <param name="contour1">Contour defining first shape</param>
      /// <param name="contour2">Contour defining second shape</param>
      /// <returns>The shape distance between two shapes defined by its contours.</returns>
      public float ComputeDistance(Point[] contour1, Point[] contour2)
      {
         using (Emgu.CV.Util.VectorOfPoint c1 = new Util.VectorOfPoint(contour1))
         using (Emgu.CV.Util.VectorOfPoint c2 = new Util.VectorOfPoint(contour2))
         {
            return ShapeInvoke.cvShapeDistanceExtractorComputeDistance(_ptr, c1, c2);
         }
      }
   }

   public class ShapeContextDistanceExtractor : ShapeDistanceExtractor
   {
      public ShapeContextDistanceExtractor(int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations, HistogramCostExtractor comparer, ShapeTransformer transformer)
      {
         _ptr = ShapeInvoke.cvShapeContextDistanceExtractorCreate(nAngularBins, nRadialBins, innerRadius, outerRadius, iterations, comparer, transformer);
      }

      protected override void DisposeObject()
      {
         ShapeInvoke.cvShapeContextDistanceExtractorRelease(ref _ptr);
      }
   }

   public static partial class ShapeInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvShapeContextDistanceExtractorCreate(
         int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
         IntPtr comparer, IntPtr transformer);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvShapeContextDistanceExtractorRelease(ref IntPtr extractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvHausdorffDistanceExtractorCreate(int distanceFlag, float rankProp);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvHausdorffDistanceExtractorRelease(ref IntPtr extractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static float cvShapeDistanceExtractorComputeDistance(IntPtr extractor, IntPtr contour1, IntPtr contour2);
   }
}
