//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
        /// <param name="nc">Number of charts in the image, if you don't know the exact then keeping this number high helps.</param>
        /// <param name="regionOfInterest">Regions of image to look for the chart, if it is empty, charts are looked for in the entire image</param>
        /// <returns>true if at least one chart is detected, otherwise false</returns>
        public bool Process(
            IInputArray image, 
            VectorOfRect regionOfInterest = null,
            int nc = 1)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
               return  MccInvoke.cveCCheckerDetectorProcess(
                    _ptr,
                    iaImage,
                    regionOfInterest ?? IntPtr.Zero,
                    nc);
            }
        }

        /// <summary>
        /// Draws the specified color checker on the provided image.
        /// </summary>
        /// <param name="pChecker">
        /// The <see cref="CChecker"/> object representing the color checker to be drawn.
        /// </param>
        /// <param name="img">
        /// The image on which the color checker will be drawn. This parameter must implement <see cref="IInputOutputArray"/>.
        /// </param>
        /// <param name="color">
        /// The color of the drawn checker, specified as an <see cref="MCvScalar"/>.
        /// </param>
        /// <param name="thickness">
        /// The thickness of the lines used to draw the checker.
        /// </param>
        public void Draw(
            CChecker pChecker,
            IInputOutputArray img,
            MCvScalar color,
            int thickness)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
            {
                MccInvoke.cveCCheckerDetectorDraw(_ptr, pChecker, ioaImg, ref color, thickness);
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
            IntPtr regionOfInterest,
            int nc);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerDetectorDraw(
            IntPtr detector,
            IntPtr pChecker,
            IntPtr img,
            ref MCvScalar color,
            int thickness);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCCheckerDetectorGetBestColorChecker(IntPtr detector);
        

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerDetectorRelease(ref IntPtr sharedPtr);

    }
}
