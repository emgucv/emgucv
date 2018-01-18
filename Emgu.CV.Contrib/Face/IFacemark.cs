//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
        public static bool SetFaceDetector(this Emgu.CV.Face.IFacemark facemark, FaceDetectNative faceDetect)
        {
            return cveFacemarkSetFaceDetector(facemark.FacemarkPtr, faceDetect);
        }

        [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
        public delegate int FaceDetectNative(IntPtr input, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveFacemarkSetFaceDetector(IntPtr facemark, FaceDetectNative detector);

        public static void LoadModel(this Emgu.CV.Face.IFacemark facemark, String model)
        {
            using (CvString cs = new CvString(model))
            {
                cveFacemarkLoadModel(facemark.FacemarkPtr, cs);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkLoadModel(IntPtr facemark, IntPtr model);

        public static bool GetFaces(this Emgu.CV.Face.IFacemark facemark, IInputArray image, IOutputArray faces)
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

        public static bool Fit(this Emgu.CV.Face.IFacemark facemark, IInputArray image, IInputArray faces, IInputOutputArray landmarks)
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
    }
}