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
    /// Face Recognizer
    /// </summary>
    public abstract class FaceRecognizer : SharedPtrObject
    {
        /// <summary>
        /// The native pointer to the FaceRecognizer object 
        /// </summary>
        protected IntPtr _faceRecognizerPtr;

        /// <summary>
        /// Train the face recognizer with the specific images and labels
        /// </summary>
        /// <param name="images">The images used in the training. This can be a VectorOfMat</param>
        /// <param name="labels">The labels of the images. This can be a VectorOfInt</param>
        public void Train(IInputArray images, IInputArray labels)
        {
            using (InputArray iaImage = images.GetInputArray())
            using (InputArray iaLabels = labels.GetInputArray())
                FaceInvoke.cveFaceRecognizerTrain(_faceRecognizerPtr, iaImage, iaLabels);
        }

        /// <summary>
        /// Train the face recognizer with the specific images and labels
        /// </summary>
        /// <param name="images">The images used in the training.</param>
        /// <param name="labels">The labels of the images.</param>
        public void Train(Mat[] images, int[] labels)
        {
            using (VectorOfMat imgVec = new VectorOfMat())
            using (VectorOfInt labelVec = new VectorOfInt(labels))
            {
                imgVec.Push(images);
                Train(imgVec, labelVec);
            }
        }

        /// <summary>
        /// Predict the label of the image
        /// </summary>
        /// <param name="image">The image where prediction will be based on</param>
        /// <returns>The prediction label</returns>
        public PredictionResult Predict(IInputArray image)
        {
            int label = -1;
            double distance = -1;
            using (InputArray iaImage = image.GetInputArray())
                FaceInvoke.cveFaceRecognizerPredict(_faceRecognizerPtr, iaImage, ref label, ref distance);
            return new PredictionResult() { Label = label, Distance = distance };
        }

        /// <summary>
        /// The prediction result
        /// </summary>
        public struct PredictionResult
        {
            /// <summary>
            /// The label
            /// </summary>
            public int Label;

            /// <summary>
            /// The distance
            /// </summary>
            public double Distance;
        }

        /// <summary>
        /// Save the FaceRecognizer to a file
        /// </summary>
        /// <param name="fileName">The file name to be saved to</param>
        public void Write(String fileName)
        {
            using (CvString s = new CvString(fileName))
                FaceInvoke.cveFaceRecognizerWrite(_faceRecognizerPtr, s);
        }

        /// <summary>
        /// Load the FaceRecognizer from the file
        /// </summary>
        /// <param name="fileName">The file where the FaceRecognizer will be loaded from</param>
        public void Read(String fileName)
        {
            using (CvString s = new CvString(fileName))
                FaceInvoke.cveFaceRecognizerRead(_faceRecognizerPtr, s);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this FaceRecognizer
        /// </summary>
        protected override void DisposeObject()
        {
            _faceRecognizerPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Class that contains entry points for the Face module.
    /// </summary>
    public static partial class FaceInvoke
    {
        static FaceInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFaceRecognizerTrain(IntPtr recognizer, IntPtr images, IntPtr labels);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFaceRecognizerPredict(IntPtr recognizer, IntPtr image, ref int label, ref double distance);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFaceRecognizerWrite(
           IntPtr recognizer,
           IntPtr fileName);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFaceRecognizerRead(
           IntPtr recognizer,
           IntPtr fileName);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFaceRecognizerUpdate(IntPtr recognizer, IntPtr images, IntPtr labels);
    }


}
