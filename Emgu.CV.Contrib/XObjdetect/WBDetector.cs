//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.XObjdetect
{
    /// <summary>
    /// WaldBoost detector.
    /// </summary>
    public class WBDetector : UnmanagedObject
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create instance of WBDetector.
        /// </summary>
        public WBDetector()
        {
            _ptr = XObjdetectInvoke.cveWBDetectorCreate(ref _sharedPtr);
        }

        /// <summary>
        /// Read detector from FileNode.
        /// </summary>
        /// <param name="node">FileNode for input</param>
        public void Read(FileNode node)
        {
            XObjdetectInvoke.cveWBDetectorRead(_ptr, node);
        }

        /// <summary>
        /// Write detector to FileStorage.
        /// </summary>
        /// <param name="fs">FileStorage for output</param>
        public void Write(FileStorage fs)
        {
            XObjdetectInvoke.cveWBDetectorWrite(_ptr, fs);
        }

        /// <summary>
        /// Train WaldBoost detector.
        /// </summary>
        /// <param name="posSamples">Path to directory with cropped positive samples</param>
        /// <param name="negImgs">Path to directory with negative (background) images</param>
        public void Train(String posSamples, String negImgs)
        {
            using (CvString csPosSamples = new CvString(posSamples))
            using (CvString csNegImgs = new CvString(negImgs))
            {
                XObjdetectInvoke.cveWBDetectorTrain(_ptr, csPosSamples, csNegImgs);
            }
        }

        /// <summary>
        /// Detect objects on image using WaldBoost detector.
        /// </summary>
        /// <param name="image">Input image for detection</param>
        /// <param name="bboxes">Bounding boxes coordinates output vector</param>
        /// <param name="confidences">Confidence values for bounding boxes output vector</param>
        public void Detect(Mat image, VectorOfRect bboxes, VectorOfDouble confidences)
        {
            XObjdetectInvoke.cveWBDetectorDetect(_ptr, image, bboxes, confidences);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this WBDetector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                XObjdetectInvoke.cveWBDetectorRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }

    
    /// <summary>
    /// Class that contains entry points for the XObjdetect module.
    /// </summary>
    public static partial class XObjdetectInvoke
    {
        static XObjdetectInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWBDetectorCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWBDetectorRead(IntPtr detector, IntPtr node);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWBDetectorWrite(IntPtr detector, IntPtr fs);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWBDetectorTrain(IntPtr detector, IntPtr posSamples, IntPtr negImgs);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWBDetectorDetect(IntPtr detector, IntPtr img, IntPtr bboxes, IntPtr confidences);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWBDetectorRelease(ref IntPtr detector, ref IntPtr sharedPtr);

    }
}
