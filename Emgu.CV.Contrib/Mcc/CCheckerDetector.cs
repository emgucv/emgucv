//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Mcc
{
    public partial class CCheckerDetector : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        public CCheckerDetector()
        {
            _ptr = MccInvoke.cveCCheckerDetectorCreate(ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Pointer to the native algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        public CChecker BestColorChecker
        {
            get
            {
                IntPtr ptr = MccInvoke.cveCCheckerDetectorGetBestColorChecker(_ptr);
                if (ptr == IntPtr.Zero)
                    return null;
                return new CChecker(ptr, false);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                MccInvoke.cveCCheckerDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        public bool Process(
            IInputArray image,
            CChecker.TypeChart chartType,
            int nc,
            bool useNet)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
               return  MccInvoke.cveCCheckerDetectorProcess(
                    _ptr,
                    iaImage,
                    chartType,
                    nc,
                    useNet,
                    IntPtr.Zero);
            }
        }
    }


    /// <summary>
    /// Class that contains entry points for the Mcc module.
    /// </summary>
    public static partial class MccInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCCheckerDetectorCreate(ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveCCheckerDetectorProcess(
            IntPtr detector,
            IntPtr image,
            CChecker.TypeChart chartType,
            int nc,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useNet,
            IntPtr param);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCCheckerDetectorGetBestColorChecker(IntPtr detector);
        

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerDetectorRelease(ref IntPtr sharedPtr);

    }
}
