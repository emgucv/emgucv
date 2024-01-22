//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
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
    /// A QR code detector
    /// </summary>
    public partial class QRCodeDetectorAruco : UnmanagedObject, IGraphicalCodeDetector
    {
        private IntPtr _graphicalCodeDetectorPtr;

        /// <summary>
        /// Pointer to the graphical code detector
        /// </summary>
        public IntPtr GraphicalCodeDetectorPtr
        {
            get { return _graphicalCodeDetectorPtr; }
        }

        /// <summary>
        /// Create a new QR code detector
        /// </summary>
        public QRCodeDetectorAruco()
        {
            _ptr = ObjdetectInvoke.cveQRCodeDetectorArucoCreate(ref _graphicalCodeDetectorPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this HOGDescriptor
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                ObjdetectInvoke.cveQRCodeDetectorArucoRelease(ref _ptr);
        }

    }

    public static partial class ObjdetectInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveQRCodeDetectorArucoCreate(ref IntPtr graphicalCodeDetector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeDetectorArucoRelease(ref IntPtr descriptor);

    }

}