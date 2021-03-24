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
    public class WeChatQRCode : UnmanagedObject
    {
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

        public String[] DetectAndDecode(
            IInputArray img,
            IOutputArrayOfArrays points)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
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
        internal extern static IntPtr cveWeChatQRCodeCreate(
            IntPtr detectorPrototxtPath,
            IntPtr detectorCaffeModelPath,
            IntPtr superResolutionPrototxtPath,
            IntPtr superResolutionCaffeModelPath);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveWeChatQRCodeRelease(ref IntPtr detector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveWeChatQRCodeDetectAndDecode(
            IntPtr detector,
            IntPtr img,
            IntPtr points,
            IntPtr results);

    }

}