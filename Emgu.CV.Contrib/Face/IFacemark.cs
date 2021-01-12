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
    /// Interface to the Facemark class
    /// </summary>
    public interface IFacemark : IAlgorithm
    {

        /// <summary>
        /// Return the pointer to the Facemark object
        /// </summary>
        /// <returns>The pointer to the Facemark object</returns>
        IntPtr FacemarkPtr { get; }
    }

    public static partial class FaceInvoke
    {
        /*
        /// <summary>
        /// Set a user defined face detector for the Facemark algorithm.
        /// </summary>
        /// <param name="facemark">The facemark object</param>
        /// <param name="faceDetect">The user defined face detector function</param>
        /// <returns>True if sucessfull.</returns>
        public static bool SetFaceDetector(this IFacemark facemark, FaceDetectNative faceDetect)
        {
            return cveFacemarkSetFaceDetector(facemark.FacemarkPtr, faceDetect);
        }

        /// <summary>
        /// A native face detect function to be used with the SetFaceDetector function
        /// </summary>
        /// <param name="input">Should be a native pointer of cv::_InputArray*</param>
        /// <param name="output">Should be a native pointer of cv::_OutputArray*</param>
        /// <returns>True if face found</returns>
        [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        public delegate bool FaceDetectNative(IntPtr input, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveFacemarkSetFaceDetector(IntPtr facemark, FaceDetectNative detector);
        */
        /// <summary>
        /// A function to load the trained model before the fitting process.
        /// </summary>
        /// <param name="facemark">The facemark object</param>
        /// <param name="model">A string represent the filename of a trained model.</param>
        public static void LoadModel(this IFacemark facemark, String model)
        {
            using (CvString cs = new CvString(model))
            {
                cveFacemarkLoadModel(facemark.FacemarkPtr, cs);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkLoadModel(IntPtr facemark, IntPtr model);

        /*
        /// <summary>
        /// Default face detector This function is mainly utilized by the implementation of a Facemark Algorithm.
        /// </summary>
        /// <param name="facemark">The facemark object</param>
        /// <param name="image">The input image to be processed.</param>
        /// <param name="faces">Output of the function which represent region of interest of the detected faces. Each face is stored in cv::Rect container.</param>
        /// <returns>True if success</returns>
        public static bool GetFaces(this IFacemark facemark, IInputArray image, IOutputArray faces)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaFaces = faces.GetOutputArray())
            {
                return cveFacemarkGetFaces(facemark.FacemarkPtr, iaImage, oaFaces);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveFacemarkGetFaces(IntPtr facemark, IntPtr image, IntPtr faces);
        */

        /// <summary>
        /// Trains a Facemark algorithm using the given dataset.
        /// </summary>
        /// <param name="facemark">The facemark object</param>
        /// <param name="image">Input image.</param>
        /// <param name="faces">Represent region of interest of the detected faces. Each face is stored in cv::Rect container.</param>
        /// <param name="landmarks">The detected landmark points for each faces.</param>
        /// <returns>True if successful</returns>
        public static bool Fit(this IFacemark facemark, IInputArray image, IInputArray faces, IInputOutputArray landmarks)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaFaces = faces.GetInputArray())
            using (InputOutputArray ioaLandmarks = landmarks.GetInputOutputArray())
            {
                return cveFacemarkFit(facemark.FacemarkPtr, iaImage, iaFaces, ioaLandmarks);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveFacemarkFit(IntPtr facemark, IntPtr image, IntPtr faces, IntPtr landmarks);

        /// <summary>
        /// Utility to draw the detected facial landmark points.
        /// </summary>
        /// <param name="image">The input image to be processed.</param>
        /// <param name="points">Contains the data of points which will be drawn.</param>
        /// <param name="color">The color of points in BGR format </param>
        public static void DrawFacemarks(IInputOutputArray image, IInputArray points, MCvScalar color)
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (InputArray iaPoints = points.GetInputArray())
            {
                cveDrawFacemarks(ioaImage, iaPoints, ref color);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDrawFacemarks(IntPtr image, IntPtr points, ref MCvScalar color);

        /*
        /// <summary>
        /// Add one training sample to the trainer.
        /// </summary>
        /// <param name="facemark">The facemark object</param>
        /// <param name="image">Input image.</param>
        /// <param name="landmarks">The ground-truth of facial landmarks points corresponds to the image.</param>
        /// <returns></returns>
        public static bool AddTraningSample(this IFacemark facemark, IInputArray image, IInputArray landmarks)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaLandmarks = landmarks.GetInputArray())
            {
                return cveFacemarkAddTrainingSample(facemark.FacemarkPtr, iaImage, iaLandmarks);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveFacemarkAddTrainingSample(IntPtr facemark, IntPtr image, IntPtr landmarks);

        /// <summary>
        /// Trains a Facemark algorithm using the given dataset. Before the training process, training samples should be added to the trainer using AddTrainingSample function.
        /// </summary>
        /// <param name="facemark">The facemark object</param>
        public static void Training(this IFacemark facemark)
        {
            cveFacemarkTraining(facemark.FacemarkPtr);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkTraining(IntPtr facemark);
        */
    }
}