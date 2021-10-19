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

namespace Emgu.CV
{
    /// <summary>
    /// DNN-based face recognizer
    /// </summary>
    public partial class FaceRecognizerSF : SharedPtrObject
    {
        /// <summary>
        /// Definition of distance used for calculating the distance between two face features
        /// </summary>
        public enum DisType
        {
            /// <summary>
            /// Cosine distance
            /// </summary>
            Cosine = 0, 
            /// <summary>
            /// Norm L2
            /// </summary>
            NormL2 = 1
        };

        /// <summary>
        /// Creates an instance of this class with given parameters.
        /// </summary>
        /// <param name="model">The path of the onnx model used for face recognition</param>
        /// <param name="config">The path to the config file for compability, which is not requested for ONNX models</param>
        /// <param name="backendId">The id of backend</param>
        /// <param name="targetId">The id of target device</param>
        public FaceRecognizerSF(
            String model,
            String config,
            Emgu.CV.Dnn.Backend backendId,
            Emgu.CV.Dnn.Target targetId)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = CvInvoke.cveFaceRecognizerSFCreate(
                    csModel,
                    csConfig,
                    backendId,
                    targetId,
                    ref _sharedPtr
                );
            }
        }

        /// <summary>
        /// Aligning image to put face on the standard position.
        /// </summary>
        /// <param name="srcImg">Input image</param>
        /// <param name="faceBox">The detection result used for indicate face in input image</param>
        /// <param name="alignedImg">Output aligned image</param>
        public void AlignCrop(
            IInputArray srcImg,
            IInputArray faceBox,
            IOutputArray alignedImg)
        {
            using (InputArray iaSrcImg = srcImg.GetInputArray())
            using (InputArray iaFaceBox = faceBox.GetInputArray())
            using (OutputArray oaAlignedImg = alignedImg.GetOutputArray())
            {
                CvInvoke.cveFaceRecognizerSFAlignCrop(_ptr, iaSrcImg, iaFaceBox, oaAlignedImg);
            }
        }

        /// <summary>
        /// Extracting face feature from aligned image.
        /// </summary>
        /// <param name="alignedImg">Input aligned image</param>
        /// <param name="faceFeature">Output face feature</param>
        public void Feature(IInputArray alignedImg, IOutputArray faceFeature)
        {
            using (InputArray iaAlignedImg = alignedImg.GetInputArray())
            using (OutputArray oaFaceFeature = faceFeature.GetOutputArray())
            {
                CvInvoke.cveFaceRecognizerSFFeature(_ptr, iaAlignedImg, oaFaceFeature);
            }
        }

        /// <summary>
        /// Calculating the distance between two face features.
        /// </summary>
        /// <param name="faceFeature1">The first input feature</param>
        /// <param name="faceFeature2">The second input feature of the same size and the same type as <paramref name="faceFeature1"/></param>
        /// <param name="disType">Defining the similarity with optional values</param>
        /// <returns>The distance between two face features.</returns>
        public double Match(
            IInputArray faceFeature1,
            IInputArray faceFeature2,
            DisType disType = DisType.Cosine)
        {
            using (InputArray iaFaceFeature1 = faceFeature1.GetInputArray())
            using (InputArray iaFaceFeature2 = faceFeature2.GetInputArray())
            {
                return CvInvoke.cveFaceRecognizerSFMatch(
                    _ptr,
                    iaFaceFeature1,
                    iaFaceFeature2,
                    disType
                );
            }
        }


        /// <summary>
        /// Release the unmanaged memory associated with this FaceRecognizerSF
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_sharedPtr))
            {
                CvInvoke.cveFaceRecognizerSFRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFaceRecognizerSFCreate(
            IntPtr model,
            IntPtr config,
            Emgu.CV.Dnn.Backend backendId,
            Emgu.CV.Dnn.Target targetId,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFaceRecognizerSFRelease(ref IntPtr faceRecognizer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFaceRecognizerSFAlignCrop(
            IntPtr faceRecognizer,
            IntPtr srcImg,
            IntPtr faceBox,
            IntPtr alignedImg);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFaceRecognizerSFFeature(
            IntPtr faceRecognizer,
            IntPtr alignedImg,
            IntPtr faceFeature);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveFaceRecognizerSFMatch(
            IntPtr faceRecognizer, 
            IntPtr faceFeature1, 
            IntPtr faceFeature2, 
            FaceRecognizerSF.DisType disType);
        
    }

}