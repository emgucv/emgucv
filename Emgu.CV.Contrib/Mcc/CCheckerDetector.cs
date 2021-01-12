//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// A class to find the positions of the ColorCharts in the image.
    /// </summary>
    public partial class CCheckerDetector : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a new CCheckerDetector.
        /// </summary>
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

        /// <summary>
        /// Get the best color checker. By the best it means the one
        /// detected with the highest confidence.
        /// </summary>
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
            if (_sharedPtr != IntPtr.Zero)
            {
                MccInvoke.cveCCheckerDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Find the ColorCharts in the given image. The found charts are not returned but instead stored in the
        /// detector, these can be accessed later on using BestColorChecker
        /// and GetListColorChecker()
        /// </summary>
        /// <param name="image">Image in color space BGR</param>
        /// <param name="chartType">Type of the chart to detect</param>
        /// <param name="nc">Number of charts in the image, if you don't know the exact then keeping this number high helps.</param>
        /// <param name="useNet">If it is true, the network provided using the setNet() is used for preliminary search for regions where chart
        /// could be present, inside the regionsOfInterest provided.</param>
        /// <param name="p">Parameters of the detection system</param>
        /// <returns>true if at least one chart is detected, otherwise false</returns>
        public bool Process(
            IInputArray image,
            CChecker.TypeChart chartType,
            int nc = 1,
            bool useNet = false, 
            DetectorParameters p = null
            )
        {
            using (InputArray iaImage = image.GetInputArray())
            {
               return  MccInvoke.cveCCheckerDetectorProcess(
                    _ptr,
                    iaImage,
                    chartType,
                    nc,
                    useNet,
                    p ?? IntPtr.Zero);
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
