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
    /// Structural similarity algorithm
    /// </summary>
    public class QualitySSIM : SharedPtrObject, IQualityBase
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
        /// Create an object which calculates quality via mean square error.
        /// </summary>
        /// <param name="refImgs">input image(s) to use as the source for comparison</param>
        public QualitySSIM(IInputArrayOfArrays refImgs)
        {
            using (InputArray iaRefImgs = refImgs.GetInputArray())
                _ptr = QualityInvoke.cveQualitySSIMCreate(
                    iaRefImgs,
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
                QualityInvoke.cveQualitySSIMRelease(ref _sharedPtr);
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
        internal extern static IntPtr cveQualitySSIMCreate(
            IntPtr refImgs,
            ref IntPtr qualityBase,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQualitySSIMRelease(ref IntPtr sharedPtr);
    }


}
