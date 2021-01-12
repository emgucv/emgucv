//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Cascade Classifier for object detection using Cuda
    /// </summary>
    public partial class CudaCascadeClassifier : SharedPtrObject
    {

        /// <summary>
        /// Create a Cuda cascade classifier using the specific file
        /// </summary>
        /// <param name="fileName">The file to create the classifier from</param>
        public CudaCascadeClassifier(String fileName)
        {
            Debug.Assert(File.Exists(fileName), String.Format("The Cascade file {0} does not exist.", fileName));
            using (CvString s = new CvString(fileName))
                _ptr = CudaInvoke.cudaCascadeClassifierCreate(s, ref _sharedPtr);
        }

        /// <summary>
        /// Create a Cuda cascade classifier using the specific file storage
        /// </summary>
        /// <param name="fs">The file storage to create the classifier from</param>
        public CudaCascadeClassifier(FileStorage fs)
        {
            _ptr = CudaInvoke.cudaCascadeClassifierCreateFromFileStorage(fs, ref _sharedPtr);
        }

        /// <summary>
        /// Detects objects of different sizes in the input image.
        /// </summary>
        /// <param name="image">Matrix of type CV_8U containing an image where objects should be detected.</param>
        /// <param name="objects">Buffer to store detected objects (rectangles).</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void DetectMultiScale(IInputArray image, IOutputArray objects, Stream stream = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaObjects = objects.GetOutputArray())
                CudaInvoke.cudaCascadeClassifierDetectMultiScale(_ptr, iaImage, oaObjects,
                   stream == null ? IntPtr.Zero : stream.Ptr);
        }

        /// <summary>
        /// Converts objects array from internal representation to standard vector.
        /// </summary>
        /// <param name="objects">Objects array in internal representation.</param>
        /// <returns>Resulting array.</returns>
        public Rectangle[] Convert(IOutputArray objects)
        {
            using (OutputArray oaObjects = objects.GetOutputArray())
            using (VectorOfRect vr = new VectorOfRect())
            {
                CudaInvoke.cudaCascadeClassifierConvert(_ptr, oaObjects, vr);
                return vr.ToArray();
            }
        }

        /// <summary>
        /// Release all unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaCascadeClassifierRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cudaCascadeClassifierCreate(IntPtr filename, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cudaCascadeClassifierCreateFromFileStorage(IntPtr filestorage, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaCascadeClassifierRelease(ref IntPtr classified);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static int cudaCascadeClassifierDetectMultiScale(IntPtr classifier, IntPtr image, IntPtr objects, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaCascadeClassifierConvert(IntPtr classifier, IntPtr gpuObjects, IntPtr objects);

    }
}
