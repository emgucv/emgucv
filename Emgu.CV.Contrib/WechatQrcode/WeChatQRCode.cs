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
using System.Drawing;

namespace Emgu.CV
{
    /// <summary>
    /// WeChat QRCode includes two CNN-based models: A object detection model and a super resolution model. Object detection model is applied to detect QRCode with the bounding box. super resolution model is applied to zoom in QRCode when it is small.
    /// </summary>
    public class WeChatQRCode : UnmanagedObject
    {
        /// <summary>
        /// Initialize the WeChatQRCode. It includes two models, which are packaged with caffe format. Therefore, there are prototxt and caffe models (In total, four paramenters).
        /// </summary>
        /// <param name="detectorPrototxtPath">Prototxt file path for the detector</param>
        /// <param name="detectorCaffeModelPath">Caffe model file path for the detector</param>
        /// <param name="superResolutionPrototxtPath">Prototxt file path for the super resolution model</param>
        /// <param name="superResolutionCaffeModelPath">Caffe file path for the super resolution model</param>
        public WeChatQRCode(
            String detectorPrototxtPath,
            String detectorCaffeModelPath,
            String superResolutionPrototxtPath,
            String superResolutionCaffeModelPath)
        {
            using (CvString csDetectorPrototxtPath = new CvString(detectorPrototxtPath))
            using (CvString csDetectorCaffeModelPath = new CvString(detectorCaffeModelPath))
            using (CvString csSuperResolutionPrototxtPath = new CvString(superResolutionPrototxtPath))
            using (CvString csSuperResolutionCaffeModelPath = new CvString(superResolutionCaffeModelPath))
                _ptr = WeChatQRCodeInvoke.cveWeChatQRCodeCreate(
                    csDetectorPrototxtPath,
                    csDetectorCaffeModelPath,
                    csSuperResolutionPrototxtPath,
                    csSuperResolutionCaffeModelPath
                    );
        }

        /// <summary>
        /// Both detects and decodes QR code.
        /// </summary>
        /// <param name="img">Supports grayscale or color (BGR) image</param>
        /// <param name="points">Optional output array of vertices of the found QR code quadrangle. Will be empty if not found.</param>
        /// <returns>The array of decoded string.</returns>
        public String[] DetectAndDecode(
            IInputArray img,
            IOutputArrayOfArrays points = null)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (OutputArray oaPoints = points == null? OutputArray.GetEmpty() : points.GetOutputArray())
            using (VectorOfCvString result = new VectorOfCvString())
            {
                WeChatQRCodeInvoke.cveWeChatQRCodeDetectAndDecode(
                    _ptr,
                    iaImg,
                    oaPoints,
                    result);
                return result.ToArray();
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                WeChatQRCodeInvoke.cveWeChatQRCodeRelease(ref _ptr);
            }
        }
    }


    /// <summary>
    /// Class that contains entry points for the WeChatQRCode module.
    /// </summary>
    internal static class WeChatQRCodeInvoke
    {
        static WeChatQRCodeInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWeChatQRCodeCreate(
            IntPtr detectorPrototxtPath,
            IntPtr detectorCaffeModelPath,
            IntPtr superResolutionPrototxtPath,
            IntPtr superResolutionCaffeModelPath);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWeChatQRCodeRelease(ref IntPtr detector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWeChatQRCodeDetectAndDecode(
            IntPtr detector,
            IntPtr img,
            IntPtr points,
            IntPtr results);

    }

}