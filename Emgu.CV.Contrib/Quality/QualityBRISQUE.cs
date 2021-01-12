//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Quality
{
    /// <summary>
    /// BRISQUE (Blind/Referenceless Image Spatial Quality Evaluator) is a No Reference Image Quality Assessment (NR-IQA) algorithm.
    /// </summary>
    public class QualityBRISQUE : SharedPtrObject, IQualityBase
    {
        private IntPtr _qualityBasePtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Pointer to the native QualityBase object
        /// </summary>
        public IntPtr QualityBasePtr
        {
            get { return _qualityBasePtr; }
        }

        /// <summary>
        /// Pointer to the native algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Create an object which calculates quality.
        /// </summary>
        /// <param name="modelFilePath">Contains a path to the BRISQUE model data. If empty, attempts to load from ${OPENCV_DIR}/testdata/contrib/quality/brisque_model_live.yml</param>
        /// <param name="rangeFilePath">contains a path to the BRISQUE range data. If empty, attempts to load from ${OPENCV_DIR}/testdata/contrib/quality/brisque_range_live.yml</param>
        public QualityBRISQUE(
            String modelFilePath = "",
            String rangeFilePath = "")
        {
            using (CvString csModelFilePath = new CvString(modelFilePath))
            using (CvString csRangeFilePath = new CvString(rangeFilePath))
                _ptr = QualityInvoke.cveQualityBRISQUECreate(
                    csModelFilePath,
                    csRangeFilePath,
                    ref _qualityBasePtr,
                    ref _algorithmPtr,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                QualityInvoke.cveQualityBRISQUERelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _qualityBasePtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }

    }


    /// <summary>
    /// Class that contains entry points for the Quality module.
    /// </summary>
    public static partial class QualityInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQualityBRISQUERelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveQualityBRISQUECreate(
            IntPtr modelFilePath,
            IntPtr rangeFilePath,
            ref IntPtr qualityBase,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);


    }


}
