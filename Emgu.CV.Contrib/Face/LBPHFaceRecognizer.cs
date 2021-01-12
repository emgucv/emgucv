//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// LBPH face recognizer
    /// </summary>
    public class LBPHFaceRecognizer : FaceRecognizer
    {
        /// <summary>
        /// Create a LBPH face recognizer
        /// </summary>
        /// <param name="radius">Radius</param>
        /// <param name="neighbors">Neighbors</param>
        /// <param name="gridX">Grid X</param>
        /// <param name="gridY">Grid Y</param>
        /// <param name="threshold">The distance threshold</param>
        public LBPHFaceRecognizer(int radius = 1, int neighbors = 8, int gridX = 8, int gridY = 8,
           double threshold = Double.MaxValue)
        {
            _ptr = FaceInvoke.cveLBPHFaceRecognizerCreate(
                radius, 
                neighbors, 
                gridX, 
                gridY, 
                threshold, 
                ref _faceRecognizerPtr,
                ref _sharedPtr);
        }

        /// <summary>
        /// Updates a FaceRecognizer with given data and associated labels.
        /// </summary>
        /// <param name="images">The training images, that means the faces you want to learn. The data has to be given as a VectorOfMat.</param>
        /// <param name="labels">The labels corresponding to the images</param>
        public void Update(IInputArray images, IInputArray labels)
        {
            using (InputArray iaImages = images.GetInputArray())
            using (InputArray iaLabels = labels.GetInputArray())
                FaceInvoke.cveFaceRecognizerUpdate(_faceRecognizerPtr, iaImages, iaLabels);
        }

        /// <summary>
        /// Update the face recognizer with the specific images and labels
        /// </summary>
        /// <param name="images">The images used for updating the face recognizer</param>
        /// <param name="labels">The labels of the images</param>
        public void Update(Mat[] images, int[] labels)
        {
            Debug.Assert(images.Length == labels.Length, "The number of labels must equals the number of images");
            using (VectorOfMat imgVec = new VectorOfMat())
            using (VectorOfInt labelVec = new VectorOfInt(labels))
            {
                imgVec.Push(images);
                Update(imgVec, labelVec);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this FisherFaceRecognizer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                FaceInvoke.cveLBPHFaceRecognizerRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }

            base.DisposeObject();
        }

        /// <summary>
        /// Get the histograms
        /// </summary>
        public VectorOfMat Histograms
        {
            get
            {
                VectorOfMat histograms = new VectorOfMat();
                FaceInvoke.cveLBPHFaceRecognizerGetHistograms(_ptr, histograms);
                return histograms;
            }
        }
    }

    /// <summary>
    /// Class that contains entry points for the Face module.
    /// </summary>
    public static partial class FaceInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveLBPHFaceRecognizerCreate(
            int radius, 
            int neighbors, 
            int gridX, 
            int gridY, 
            double threshold,
            ref IntPtr faceRecognizerPtr,
            ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveLBPHFaceRecognizerRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveLBPHFaceRecognizerGetHistograms(IntPtr recognizer, IntPtr histograms);
    }


}
