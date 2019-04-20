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

    public class QualityBRISQUE : SharedPtrObject, IQualityBase
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

        public QualityBRISQUE(
            String modelFilePath,
            String rangeFilePath)
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
            }
        }

    }


    /// <summary>
    /// Class that contains entry points for the Quality module.
    /// </summary>
    public static partial class QualityInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQualityBRISQUERelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveQualityBRISQUECreate(
            IntPtr modelFilePath,
            IntPtr rangeFilePath,
            ref IntPtr qualityBase,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);


    }


}
