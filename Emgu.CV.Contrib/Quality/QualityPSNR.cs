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
    /// Peak signal to noise ratio (PSNR) algorithm
    /// </summary>
    public class QualityPSNR : SharedPtrObject, IQualityBase
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
        /// Create an instance of peak signal to noise ratio (PSNR) algorithm
        /// </summary>
        /// <param name="refImgs">Input image(s) to use as the source for comparison</param>
        /// <param name="maxPixelValue">maximum per-channel value for any individual pixel; eg 255 for uint8 image</param>
        public QualityPSNR(IInputArrayOfArrays refImgs, double maxPixelValue = 255.0)
        {
            using (InputArray iaRefImgs = refImgs.GetInputArray())
                _ptr = QualityInvoke.cveQualityPSNRCreate(
                    iaRefImgs,
                    maxPixelValue,
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
                QualityInvoke.cveQualityPSNRRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
                _qualityBasePtr = IntPtr.Zero;
            }
        }

    }


    /// <summary>
    /// Class that contains entry points for the Quality module.
    /// </summary>
    public static partial class QualityInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveQualityPSNRCreate(
            IntPtr refImgs,
            double maxPixelValue,
            ref IntPtr qualityBase,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQualityPSNRRelease(ref IntPtr sharedPtr);
    }


}
