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

    public partial class FaceDetectorYN : SharedPtrObject
    {

        public FaceDetectorYN(
            String model,
            String config,
            Size inputSize,
            float scoreThreshold,
            float nmsThreshold,
            int topK,
            Emgu.CV.Dnn.Backend backendId,
            Emgu.CV.Dnn.Target targetId)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = CvInvoke.cveFaceDetectorYNCreate(
                    csModel,
                    csConfig,
                    ref inputSize,
                    scoreThreshold,
                    nmsThreshold,
                    topK,
                    backendId, 
                    targetId,
                    ref _sharedPtr
                );
            }
        }

        public int Detect(IInputArray image, IOutputArray faces)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaFaces = faces.GetOutputArray())
                return CvInvoke.cveFaceDetectorYNDetect(_ptr, iaImage, oaFaces);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this FaceDetectorYN
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_sharedPtr))
            {
                CvInvoke.cveFaceDetectorYNRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFaceDetectorYNCreate(
            IntPtr model,
            IntPtr config,
            ref Size inputSize,
            float scoreThreshold,
            float nmsThreshold,
            int topK,
            Emgu.CV.Dnn.Backend backendId,
            Emgu.CV.Dnn.Target targetId,
            ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFaceDetectorYNRelease(ref IntPtr faceDetector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveFaceDetectorYNDetect(IntPtr faceDetector, IntPtr image, IntPtr faces);
        
    }

}