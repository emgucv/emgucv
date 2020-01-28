//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// A HOG descriptor
    /// </summary>
    public class HOGDescriptor : UnmanagedObject
    {
        /// <summary>
        /// Create a new HOGDescriptor
        /// </summary>
        public HOGDescriptor()
        {
            _ptr = CvInvoke.cveHOGDescriptorCreateDefault();
        }

        /// <summary>
        /// Create a new HOGDescriptor using the specific parameters.
        /// </summary>
        /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
        /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
        /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
        /// <param name="gammaCorrection">Do gamma correction preprocessing or not.</param>
        /// <param name="L2HysThreshold">L2-Hys normalization method shrinkage.</param>
        /// <param name="nbins">Number of bins.</param>
        /// <param name="winSigma">Gaussian smoothing window parameter.</param>
        /// <param name="winSize">Detection window size. Must be aligned to block size and block stride. Must match the size of the training image. Use (64, 128) for default.</param>
        /// <param name="derivAperture">Deriv Aperture</param>
        public HOGDescriptor(
           Size winSize,
           Size blockSize,
           Size blockStride,
           Size cellSize,
           int nbins = 9,
           int derivAperture = 1,
           double winSigma = -1,
           double L2HysThreshold = 0.2,
           bool gammaCorrection = true)
        {
            _ptr = CvInvoke.cveHOGDescriptorCreate(
               ref winSize,
               ref blockSize,
               ref blockStride,
               ref cellSize,
               nbins,
               derivAperture,
               winSigma,
               0,
               L2HysThreshold,
               gammaCorrection);
        }


        /// <summary>
        /// Return the default people detector
        /// </summary>
        /// <returns>The default people detector</returns>
        public static float[] GetDefaultPeopleDetector()
        {
            using (Util.VectorOfFloat desc = new VectorOfFloat())
            {
                CvInvoke.cveHOGDescriptorPeopleDetectorCreate(desc);
                return desc.ToArray();
            }
        }

        /// <summary>
        /// Set the SVM detector 
        /// </summary>
        /// <param name="detector">The SVM detector</param>
        public void SetSVMDetector(float[] detector)
        {
            using (VectorOfFloat vec = new VectorOfFloat(detector))
            {
                CvInvoke.cveHOGSetSVMDetector(_ptr, vec);
            }
        }

        /// <summary>
        /// Performs object detection with increasing detection window.
        /// </summary>
        /// <param name="image">The image to search in</param>
        /// <param name="hitThreshold"> Threshold for the distance between features and SVM classifying plane. Usually it is 0 and should be specified in the detector coefficients (as the last free coefficient). But if the free coefficient is omitted (which is allowed), you can specify it manually here.</param>
        /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
        /// <param name="padding">Padding</param>
        /// <param name="scale">Coefficient of the detection window increase.</param>
        /// <param name="finalThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping. Should be an integer if not using meanshift grouping. </param>
        /// <param name="useMeanshiftGrouping">If true, it will use meanshift grouping.</param>
        /// <returns>The regions where positives are found</returns>
        public MCvObjectDetection[] DetectMultiScale(
           IInputArray image,
           double hitThreshold = 0,
           Size winStride = new Size(),
           Size padding = new Size(),
           double scale = 1.05,
           double finalThreshold = 2.0,
           bool useMeanshiftGrouping = false)
        {
            using (Util.VectorOfRect vr = new VectorOfRect())
            using (Util.VectorOfDouble vd = new VectorOfDouble())
            using (InputArray iaImage = image.GetInputArray())
            {
                CvInvoke.cveHOGDescriptorDetectMultiScale(_ptr, iaImage, vr, vd, hitThreshold, ref winStride, ref padding, scale,
                   finalThreshold, useMeanshiftGrouping);
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
        /// Computes HOG descriptors of given image.
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="winStride">Window stride. Must be a multiple of block stride. Use Size.Empty for default</param>
        /// <param name="padding">Padding. Use Size.Empty for default</param>
        /// <param name="locations">Locations for the computation. Can be null if not needed</param>
        /// <returns>The descriptor vector</returns>
        public float[] Compute(IInputArray image, Size winStride = new Size(), Size padding = new Size(),
           Point[] locations = null)
        {
            using (VectorOfFloat desc = new VectorOfFloat())
            using (InputArray iaImage = image.GetInputArray())
            {
                if (locations == null)
                {
                    CvInvoke.cveHOGDescriptorCompute(_ptr, iaImage, desc, ref winStride, ref padding, IntPtr.Zero);
                }
                else
                {
                    using (VectorOfPoint vp = new VectorOfPoint(locations))
                    {
                        CvInvoke.cveHOGDescriptorCompute(_ptr, iaImage, desc, ref winStride, ref padding, vp);
                    }
                }
                return desc.ToArray();
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this HOGDescriptor
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveHOGDescriptorRelease(ref _ptr);
        }

        /// <summary>
        /// Get the size of the descriptor
        /// </summary>
        public uint DescriptorSize
        {
            get { return CvInvoke.cveHOGDescriptorGetDescriptorSize(_ptr); }
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHOGDescriptorPeopleDetectorCreate(IntPtr seq);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveHOGDescriptorCreateDefault();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveHOGDescriptorCreate(
            ref Size winSize,
            ref Size blockSize,
            ref Size blockStride,
            ref Size cellSize,
            int nbins,
            int derivAperture,
            double winSigma,
            int histogramNormType,
            double L2HysThreshold,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool gammaCorrection);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHOGDescriptorRelease(ref IntPtr descriptor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHOGDescriptorDetectMultiScale(
            IntPtr descriptor,
            IntPtr img,
            IntPtr foundLocations,
            IntPtr weights,
            double hitThreshold,
            ref Size winStride,
            ref Size padding,
            double scale,
            double finalThreshold,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useMeanshiftGrouping);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHOGDescriptorCompute(
            IntPtr descriptor,
            IntPtr img,
            IntPtr descriptors,
            ref Size winStride,
            ref Size padding,
            IntPtr locations);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static uint cveHOGDescriptorGetDescriptorSize(IntPtr descriptor);
    }

}