//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// A HOG descriptor
    /// </summary>
    public partial class CudaHOG : SharedPtrObject
    {
        /// <summary>
        /// The descriptor format
        /// </summary>
        public enum DescrFormat
        {
            /// <summary>
            /// Row by row
            /// </summary>
            RowByRow,
            /// <summary>
            /// Col by col
            /// </summary>
            ColByCol
        }

        /// <summary>
        /// Create a new HOGDescriptor using the specific parameters
        /// </summary>
        /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
        /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
        /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
        /// <param name="nbins">Number of bins.</param>
        /// <param name="winSize">Detection window size. Must be aligned to block size and block stride. Must match the size of the training image. Use (64, 128) for default.</param>
        public CudaHOG(
           Size winSize,
           Size blockSize,
           Size blockStride,
           Size cellSize,
           int nbins = 9)
        {
            _ptr = CudaInvoke.cudaHOGCreate(
               ref winSize,
               ref blockSize,
               ref blockStride,
               ref cellSize,
               nbins,
               ref _sharedPtr);
        }

        /// <summary>
        /// Returns coefficients of the classifier trained for people detection (for default window size).
        /// </summary>
        /// <returns>The default people detector</returns>
        public Mat GetDefaultPeopleDetector()
        {
            Mat m = new Mat();
            CudaInvoke.cudaHOGGetDefaultPeopleDetector(_ptr, m);
            return m;
        }

        /// <summary>
        /// Set the SVM detector 
        /// </summary>
        /// <param name="detector">The SVM detector</param>
        public void SetSVMDetector(IInputArray detector)
        {
            using (InputArray iaDetector = detector.GetInputArray())
            {
                CudaInvoke.cudaHOGSetSVMDetector(_ptr, iaDetector);
            }
        }

        /// <summary>
        /// Performs object detection with increasing detection window.
        /// </summary>
        /// <param name="image">The CudaImage to search in</param>
        /// <returns>The regions where positives are found</returns>
        public MCvObjectDetection[] DetectMultiScale(IInputArray image)
        {
            using (Util.VectorOfRect vr = new VectorOfRect())
            using (Util.VectorOfDouble vd = new VectorOfDouble())
            {
                DetectMultiScale(image, vr, vd);
                Rectangle[] location = vr.ToArray();
                double[] weight = vd.ToArray();
                MCvObjectDetection[] result = new MCvObjectDetection[location.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    MCvObjectDetection od = new MCvObjectDetection();
                    od.Rect = location[i];
                    od.Score = (float)weight[i];
                    result[i] = od;
                }
                return result;
            }
        }

        /// <summary>
        /// Performs object detection with a multi-scale window.
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="objects">Detected objects boundaries.</param>
        /// <param name="confident">Optional output array for confidences.</param>
        public void DetectMultiScale(IInputArray image, VectorOfRect objects, VectorOfDouble confident = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
                CudaInvoke.cudaHOGDetectMultiScale(_ptr, iaImage, objects, confident);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this HOGDescriptor
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaHOGRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaHOGGetDefaultPeopleDetector(IntPtr hog, IntPtr detector);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cudaHOGCreate(
           ref Size winSize,
           ref Size blockSize,
           ref Size blockStride,
           ref Size cellSize,
           int nbins,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaHOGRelease(ref IntPtr descriptor);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaHOGDetectMultiScale(
           IntPtr descriptor,
           IntPtr img,
           IntPtr foundLocations,
           IntPtr confidents);

    }
}
