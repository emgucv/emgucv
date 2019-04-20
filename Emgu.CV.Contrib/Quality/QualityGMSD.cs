//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
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

    public class QualityGMSD : SharedPtrObject, IQualityBase
    {
        private IntPtr _qualityBasePtr;
        private IntPtr _algorithmPtr;

        public IntPtr QualityBasePtr
        {
            get { return _qualityBasePtr; }
        }

        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        public QualityGMSD(IInputArray refImgs)
        {
            using (InputArray iaRefImgs = refImgs.GetInputArray())
                _ptr = QualityInvoke.cveQualityGMSDCreate(
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
                QualityInvoke.cveQualityGMSDRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }


    /// <summary>
    /// Class that contains entry points for the Quality module.
    /// </summary>
    public static partial class QualityInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveQualityGMSDCreate(
            IntPtr refImgs,
            ref IntPtr qualityBase,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQualityGMSDRelease(ref IntPtr sharedPtr);
    }


}
