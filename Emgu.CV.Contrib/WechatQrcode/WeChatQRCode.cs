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
            IOutputArrayOfArrays points)
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
        /// Both detects and decodes QR code.
        /// </summary>
        /// <param name="img">Supports grayscale or color (BGR) image</param>
        /// <returns>The detected QRCode.</returns>
        public QRCode[] DetectAndDecode(IInputArray img)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (VectorOfMat pointsVec = new VectorOfMat())
            using (VectorOfCvString result = new VectorOfCvString())
            {
                String[] codes = DetectAndDecode(img, pointsVec);
                if (codes.Length == 0)
                {
                    return new QRCode[0];
                }
                Point[][] points = WeChatQRCode.VectorOfMatToPoints(pointsVec);

                QRCode[] results = new QRCode[codes.Length];
                for (int i = 0;i < codes.Length;i++)
                {
                    QRCode c = new QRCode();
                    c.Code = codes[i];
                    c.Region = points[i];
                    results[i] = c;
                }
                return results;
            }
        }

        /// <summary>
        /// The detected QR code
        /// </summary>
        public class QRCode
        {
            /// <summary>
            /// The string that this QR code represents
            /// </summary>
            public String Code { get; set; }

            /// <summary>
            /// The region of the QR code.
            /// </summary>
            public Point[] Region { get; set; }
        }

        /// <summary>
        /// Can be used to convert the second parameter of DetectAndDecode function from VectorOfMat to points
        /// </summary>
        /// <param name="vm">The VectorOfMat that is passed to the second parameter of DetectAndDecode</param>
        /// <returns>The detected points</returns>
        private static Point[][] VectorOfMatToPoints(VectorOfMat vm)
        {
            Point[][] points = new Point[vm.Size][];
            for (int i = 0; i < points.Length; i++)
            {
                using (Mat p = vm[i])
                {
                    points[i] = MatToPoints(p);
                }
            }

            return points;
        }

        private static Point[] MatToPoints(Mat m)
        {
            PointF[] points = new PointF[m.Width * m.Height / 2];
            GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
            Emgu.CV.Util.CvToolbox.Memcpy(handle.AddrOfPinnedObject(), m.DataPointer, points.Length * Marshal.SizeOf<PointF>());
            handle.Free();
            return Array.ConvertAll(points, Point.Round);
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