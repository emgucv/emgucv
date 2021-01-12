//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Text
{
    /// <summary>
    /// This class provides the functionality of text bounding box detection. This class find bounding boxes of text words given an input image.
    /// </summary>
    public class TextDetectorCNN : SharedPtrObject
    {

        /// <summary>
        /// Creates an instance of the TextDetectorCNN class using the provided parameters.
        /// </summary>
        /// <param name="modelArchFilename">The relative or absolute path to the prototxt file describing the classifiers architecture.</param>
        /// <param name="modelWeightsFilename">The relative or absolute path to the file containing the pretrained weights of the model in caffe-binary form.</param>
        /// <param name="detectionSizes">A list of sizes for multi-scale detection. The values[(300,300),(700,500),(700,300),(700,700),(1600,1600)] are recommended to achieve the best quality.</param>
        public TextDetectorCNN(String modelArchFilename, String modelWeightsFilename, Size[] detectionSizes = null)
        {
            using (CvString csModelArchFilename = new CvString(modelArchFilename))
            using (CvString csModelWeightsFilename = new CvString(modelWeightsFilename))
            {
                if (detectionSizes == null)
                {
                    _ptr = TextInvoke.cveTextDetectorCNNCreate(csModelArchFilename, csModelWeightsFilename,
                        ref _sharedPtr);
                }
                else
                {
                    using (VectorOfSize vs = new VectorOfSize(detectionSizes))
                        _ptr = TextInvoke.cveTextDetectorCNNCreate2(csModelArchFilename, csModelWeightsFilename, vs,
                            ref _sharedPtr);
                }
            }

        }

        /// <summary>
        /// The detect result
        /// </summary>
        public class TextRegion
        {
            /// <summary>
            /// The bounding box
            /// </summary>
            public Rectangle BBox;
            /// <summary>
            /// The confident
            /// </summary>
            public float Confident;
        }

        /// <summary>
        /// Find bounding boxes of text words given an input image.
        /// </summary>
        /// <param name="inputImage">An image expected to be a CV_U8C3 of any size</param>
        /// <returns>The text regions found.</returns>
        public TextRegion[] Detect(IInputArray inputImage)
        {
            using (InputArray iaImage = inputImage.GetInputArray())
            using (VectorOfRect vr = new VectorOfRect())
            using (VectorOfFloat vf = new VectorOfFloat())
            {
                TextInvoke.cveTextDetectorCNNDetect(_ptr, iaImage, vr, vf);
                Rectangle[] bboxes = vr.ToArray();
                float[] confidents = vf.ToArray();
                TextRegion[] regions = new TextRegion[bboxes.Length];
                for (int i = 0; i < regions.Length; i++)
                {
                    TextRegion tr = new TextRegion();
                    tr.BBox = bboxes[i];
                    tr.Confident = confidents[i];
                    regions[i] = tr;
                }
                return regions;
            }
        }

        /// <summary>
        /// Release all the unmanaged memory associate with this TextDetector
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                TextInvoke.cveTextDetectorCNNRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }


    /// <summary>
    /// This class wraps the functional calls to the OpenCV Text modules
    /// </summary>
    public static partial class TextInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTextDetectorCNNCreate(IntPtr modelArchFilename, IntPtr modelWeightsFilename, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTextDetectorCNNCreate2(IntPtr modelArchFilename, IntPtr modelWeightsFilename, IntPtr detectionSizes, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTextDetectorCNNDetect(IntPtr detector, IntPtr inputImage, IntPtr bbox, IntPtr confidence);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTextDetectorCNNRelease(ref IntPtr sharedPtr);

    }

}

