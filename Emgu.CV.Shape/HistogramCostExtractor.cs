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
   /// Abstract base class for histogram cost algorithms.
   /// </summary>
   public abstract class HistogramCostExtractor : UnmanagedObject
   {
      /// <summary>
      /// Release the histogram cost extractor
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ShapeInvoke.cvHistogramCostExtractorRelease(ref _ptr);
         }
      }


   }

   /// <summary>
   /// A norm based cost extraction.
   /// </summary>
   public class NormHistogramCostExtractor : HistogramCostExtractor
   {
      /// <summary>
      /// Create a norm based cost extraction.
      /// </summary>
      /// <param name="flag">Distance type</param>
      /// <param name="nDummies">Number of dummies</param>
      /// <param name="defaultCost">Default cost</param>
      public NormHistogramCostExtractor(CvEnum.DistType flag = CvEnum.DistType.L2, int nDummies = 25, float defaultCost = 0.2f)
      {
         _ptr = ShapeInvoke.cvNormHistogramCostExtractorCreate(flag, nDummies, defaultCost);
      }
   }

   /// <summary>
   /// An EMD based cost extraction.
   /// </summary>
   public class EMDHistogramCostExtractor : HistogramCostExtractor
   {
      /// <summary>
      /// Create an EMD based cost extraction.
      /// </summary>
      /// <param name="flag">Distance type</param>
      /// <param name="nDummies">Number of dummies</param>
      /// <param name="defaultCost">Default cost</param>
      public EMDHistogramCostExtractor(CvEnum.DistType flag = CvEnum.DistType.L2, int nDummies = 25, float defaultCost = 0.2f)
      {
         _ptr = ShapeInvoke.cvEMDHistogramCostExtractorCreate(flag, nDummies, defaultCost);
      }
   }

   /// <summary>
   /// An Chi based cost extraction.
   /// </summary>
   public class ChiHistogramCostExtractor : HistogramCostExtractor
   {
      /// <summary>
      /// Create an Chi based cost extraction.
      /// </summary>
      /// <param name="nDummies">Number of dummies</param>
      /// <param name="defaultCost">Default cost</param>
      public ChiHistogramCostExtractor(int nDummies = 25, float defaultCost = 0.2f)
      {
         _ptr = ShapeInvoke.cvChiHistogramCostExtractorCreate(nDummies, defaultCost);
      }      
   }

   /// <summary>
   /// An EMD-L1 based cost extraction.
   /// </summary>
   public class EMDL1HistogramCostExtractor : HistogramCostExtractor
   {
      /// <summary>
      /// Create an EMD-L1 based cost extraction.
      /// </summary>
      /// <param name="nDummies">Number of dummies</param>
      /// <param name="defaultCost">Default cost</param>
      public EMDL1HistogramCostExtractor(int nDummies = 25, float defaultCost = 0.2f)
      {
         _ptr = ShapeInvoke.cvEMDL1HistogramCostExtractorCreate(nDummies, defaultCost);
      }
   }

   /// <summary>
   /// Library to invoke functions that belongs to the shape module
   /// </summary>
   public static partial class ShapeInvoke
   {
      static ShapeInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvNormHistogramCostExtractorCreate(CvEnum.DistType flag, int nDummies, float defaultCost);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvEMDHistogramCostExtractorCreate(CvEnum.DistType flag, int nDummies, float defaultCost);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvChiHistogramCostExtractorCreate(int nDummies, float defaultCost);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvHistogramCostExtractorRelease(ref IntPtr extractor);
   }
}
