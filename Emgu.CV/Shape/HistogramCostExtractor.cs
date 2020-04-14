//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;
using Emgu.CV.Util;

namespace Emgu.CV.Shape
{
    /// <summary>
    /// Abstract base class for histogram cost algorithms.
    /// </summary>
    public abstract class HistogramCostExtractor : SharedPtrObject
    {

        /// <summary>
        /// Release the histogram cost extractor
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                ShapeInvoke.cveHistogramCostExtractorRelease(ref _sharedPtr);
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
            _ptr = ShapeInvoke.cveNormHistogramCostExtractorCreate(flag, nDummies, defaultCost, ref _sharedPtr);
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
            _ptr = ShapeInvoke.cveEMDHistogramCostExtractorCreate(flag, nDummies, defaultCost, ref _sharedPtr);
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
            _ptr = ShapeInvoke.cveChiHistogramCostExtractorCreate(nDummies, defaultCost, ref _sharedPtr);
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
            _ptr = ShapeInvoke.cveEMDL1HistogramCostExtractorCreate(nDummies, defaultCost, ref _sharedPtr);
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
        internal extern static IntPtr cveNormHistogramCostExtractorCreate(CvEnum.DistType flag, int nDummies, float defaultCost, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveEMDHistogramCostExtractorCreate(CvEnum.DistType flag, int nDummies, float defaultCost, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveChiHistogramCostExtractorCreate(int nDummies, float defaultCost, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveEMDL1HistogramCostExtractorCreate(int nDummies, float defaultCost, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHistogramCostExtractorRelease(ref IntPtr sharedPtr);
    }
}
