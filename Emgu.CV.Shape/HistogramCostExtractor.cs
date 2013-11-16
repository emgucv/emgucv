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
   public abstract class HistogramCostExtractor : UnmanagedObject
   {
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ShapeInvoke.cvHistogramCostExtractorRelease(ref _ptr);
         }
      }
   }

   public class NormHistogramCostExtractor : HistogramCostExtractor
   {
      public NormHistogramCostExtractor(CvEnum.DIST_TYPE flag, int nDummies, float defaultCost)
      {
         _ptr = ShapeInvoke.cvNormHistogramCostExtractorCreate(flag, nDummies, defaultCost);
      }
   }

   public class EMDHistogramCostExtractor : HistogramCostExtractor
   {
      public EMDHistogramCostExtractor(CvEnum.DIST_TYPE flag, int nDummies, float defaultCost)
      {
         _ptr = ShapeInvoke.cvEMDHistogramCostExtractorCreate(flag, nDummies, defaultCost);
      }
   }

   
   public class ChiHistogramCostExtractor : HistogramCostExtractor
   {
      public ChiHistogramCostExtractor(int nDummies, float defaultCost)
      {
         _ptr = ShapeInvoke.cvChiHistogramCostExtractorCreate(nDummies, defaultCost);
      }
   }

   public class EMDL1HistogramCostExtractor : HistogramCostExtractor
   {
      public EMDL1HistogramCostExtractor(int nDummies, float defaultCost)
      {
         _ptr = ShapeInvoke.cvEMDL1HistogramCostExtractorCreate(nDummies, defaultCost);
      }
   }

   public static partial class ShapeInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvNormHistogramCostExtractorCreate(CvEnum.DIST_TYPE flag, int nDummies, float defaultCost);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvEMDHistogramCostExtractorCreate(CvEnum.DIST_TYPE flag, int nDummies, float defaultCost);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvChiHistogramCostExtractorCreate(int nDummies, float defaultCost);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvHistogramCostExtractorRelease(ref IntPtr extractor);
   }
}
