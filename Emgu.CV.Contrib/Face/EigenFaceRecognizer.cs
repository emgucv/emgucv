//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Face
{
    /// <summary>
    /// Eigen face recognizer
    /// </summary>
    public class EigenFaceRecognizer : BasicFaceRecognizer
    {
        /// <summary>
        /// Create an EigenFaceRecognizer
        /// </summary>
        /// <param name="numComponents">The number of components</param>
        /// <param name="threshold">The distance threshold</param>
        public EigenFaceRecognizer(int numComponents = 0, double threshold = double.MaxValue)
        {
            _ptr = FaceInvoke.cveEigenFaceRecognizerCreate(numComponents, threshold, ref _faceRecognizerPtr, ref _basicFaceRecognizerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this EigenFaceRecognizer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                FaceInvoke.cveEigenFaceRecognizerRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }

            base.DisposeObject();
        }

    }


    /// <summary>
    /// Class that contains entry points for the Face module.
    /// </summary>
    public static partial class FaceInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveEigenFaceRecognizerCreate(
            int numComponents,
            double threshold,
            ref IntPtr faceRecognizerPtr,
            ref IntPtr basicFaceRecognizerPtr,
            ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveEigenFaceRecognizerRelease(ref IntPtr sharedPtr);

    }


}
