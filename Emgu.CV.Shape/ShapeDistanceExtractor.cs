//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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

   /// <summary>
   /// Implementation of the Shape Context descriptor and matching algorithm proposed by Belongie et al. in “Shape Matching and Object Recognition Using Shape Contexts” (PAMI 2002). 
   /// </summary>
   public class ShapeContextDistanceExtractor : ShapeDistanceExtractor
   {
      /// <summary>
      /// Create a shape context distance extractor
      /// </summary>
      /// <param name="comparer">The histogram cost extractor</param>
      /// <param name="transformer">The shape transformer</param>
      /// <param name="nAngularBins">Establish the number of angular bins for the Shape Context Descriptor used in the shape matching pipeline.</param>
      /// <param name="nRadialBins">Establish the number of radial bins for the Shape Context Descriptor used in the shape matching pipeline.</param>
      /// <param name="innerRadius">Set the inner radius of the shape context descriptor.</param>
      /// <param name="outerRadius">Set the outer radius of the shape context descriptor.</param>
      /// <param name="iterations">Iterations</param>
      public ShapeContextDistanceExtractor(
         HistogramCostExtractor comparer, ShapeTransformer transformer,
         int nAngularBins = 12, 
         int nRadialBins = 4, 
         float innerRadius = 0.2f, 
         float outerRadius = 3, 
         int iterations = 3)
      {
         _ptr = ShapeInvoke.cvShapeContextDistanceExtractorCreate(nAngularBins, nRadialBins, innerRadius, outerRadius, iterations, comparer, transformer);
      }

      /// <summary>
      /// Release the memory associated with this shape context distance extractor
      /// </summary>
      protected override void DisposeObject()
      {
         ShapeInvoke.cvShapeContextDistanceExtractorRelease(ref _ptr);
      }
   }

   /// <summary>
   /// A simple Hausdorff distance measure between shapes defined by contours, according to the paper “Comparing Images using the Hausdorff distance.” by D.P. Huttenlocher, G.A. Klanderman, and W.J. Rucklidge. (PAMI 1993).
   /// </summary>
   public class HausdorffDistanceExtractor : ShapeDistanceExtractor
   {
      /// <summary>
      /// Create Hausdorff distance extractor
      /// </summary>
      /// <param name="distanceFlag">Rhe norm used to compute the Hausdorff value between two shapes. It can be L1 or L2 norm.</param>
      /// <param name="rankProp">The rank proportion (or fractional value) that establish the Kth ranked value of the partial Hausdorff distance. Experimentally had been shown that 0.6 is a good value to compare shapes.</param>
      public HausdorffDistanceExtractor(CvEnum.DistType distanceFlag = CvEnum.DistType.L2, float rankProp = 0.6f)
      {
         _ptr = ShapeInvoke.cvHausdorffDistanceExtractorCreate(distanceFlag, rankProp);
      }

      /// <summary>
      /// Release the memory associated with this Hausdorff distance extrator
      /// </summary>
      protected override void DisposeObject()
      {
         ShapeInvoke.cvHausdorffDistanceExtractorRelease(ref _ptr);
      }
   }

   public static partial class ShapeInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvShapeContextDistanceExtractorCreate(
         int nAngularBins, int nRadialBins, float innerRadius, float outerRadius, int iterations,
         IntPtr comparer, IntPtr transformer);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvShapeContextDistanceExtractorRelease(ref IntPtr extractor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvHausdorffDistanceExtractorCreate(CvEnum.DistType distanceFlag, float rankProp);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvHausdorffDistanceExtractorRelease(ref IntPtr extractor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static float cvShapeDistanceExtractorComputeDistance(IntPtr extractor, IntPtr contour1, IntPtr contour2);
   }
}
