//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
using System.Runtime.InteropServices.ComTypes;

namespace Emgu.CV
{
    /// <summary>
    /// Barcode detector
    /// </summary>
    public class BarcodeDetector : UnmanagedObject, IGraphicalCodeDetector
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
        /// Initialize the BarcodeDetector. Super resolution is disabled.
        /// </summary>
        public BarcodeDetector()
            : this(String.Empty)
        {
        }

        /// <summary>
        /// Initialize the BarcodeDetector with an ONNX Super Resolution model.
        /// </summary>
        /// <param name="superResolutionModelPath">Path to the ONNX super resolution model file. Leave empty to disable super resolution.</param>
        public BarcodeDetector(String superResolutionModelPath)
        {
            using (CvString csSrModelPath = new CvString(superResolutionModelPath))
                _ptr = ObjdetectInvoke.cveBarcodeDetectorCreate(
                    csSrModelPath,
                    ref _graphicalCodeDetectorPtr);
        }


        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                ObjdetectInvoke.cveBarcodeDetectorRelease(ref _ptr);
            }
        }

        /*

        /// <summary>
        /// Barcode type
        /// </summary>
        public enum BarcodeType
        {
            /// <summary>
            /// None
            /// </summary>
            None,
            /// <summary>
            /// EAN-8
            /// </summary>
            EAN_8,
            /// <summary>
            /// EAN-13
            /// </summary>
            EAN_13,
            /// <summary>
            /// UPC-A
            /// </summary>
            UPC_A,
            /// <summary>
            /// UPC-E
            /// </summary>
            UPC_E,
            /// <summary>
            /// UPC-EAN-EXTENSION
            /// </summary>
            UPC_EAN_Extension
        };
        */
        
    }


    /// <summary>
    /// This class contains functions to call into object detect module
    /// </summary>
    public static partial class ObjdetectInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBarcodeDetectorCreate(
            IntPtr superResolutionModelPath,
            ref IntPtr graphicalCodeDetector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBarcodeDetectorRelease(ref IntPtr detector);

    }

}