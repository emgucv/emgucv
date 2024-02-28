//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Aruco
{
    /// <summary>
    /// Dictionary/Set of markers. It contains the inner codification.
    /// </summary>
    public class Dictionary : UnmanagedObject
    {
        //private bool _predefined;
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a Dictionary using predefined values
        /// </summary>
        /// <param name="name">The name of the predefined dictionary</param>
        public Dictionary(PredefinedDictionaryName name)
        {
            //_predefined = true;
            _ptr = ArucoInvoke.cveArucoGetPredefinedDictionary(name, ref _sharedPtr);
        }

        /// <summary>
        /// Generates a new marker dictionary.
        /// </summary>
        public Dictionary()
        {
            //_predefined = false;
            _ptr = ArucoInvoke.cveArucoDictionaryCreate(ref _sharedPtr);
        }

        /// <summary>
        /// The name of the predefined dictionary
        /// </summary>
        public enum PredefinedDictionaryName
        {
            /// <summary>
            /// Dict4X4_50
            /// </summary>
            Dict4X4_50 = 0,

            /// <summary>
            /// Dict4X4_100
            /// </summary>
            Dict4X4_100,

            /// <summary>
            /// Dict4X4_250
            /// </summary>
            Dict4X4_250,

            /// <summary>
            /// Dict4X4_1000
            /// </summary>
            Dict4X4_1000,

            /// <summary>
            /// Dict5X5_50
            /// </summary>
            Dict5X5_50,

            /// <summary>
            /// Dict5X5_100
            /// </summary>
            Dict5X5_100,

            /// <summary>
            /// Dict5X5_250
            /// </summary>
            Dict5X5_250,

            /// <summary>
            /// Dict5X5_1000
            /// </summary>
            Dict5X5_1000,

            /// <summary>
            /// Dict6X6_50
            /// </summary>
            Dict6X6_50,

            /// <summary>
            /// Dict6X6_100
            /// </summary>
            Dict6X6_100,

            /// <summary>
            /// Dict6X6_250
            /// </summary>
            Dict6X6_250,

            /// <summary>
            /// Dict6X6_1000
            /// </summary>
            Dict6X6_1000,

            /// <summary>
            /// Dict7X7_50
            /// </summary>
            Dict7X7_50,

            /// <summary>
            /// Dict7X7_100
            /// </summary>
            Dict7X7_100,

            /// <summary>
            /// Dict7X7_250
            /// </summary>
            Dict7X7_250,

            /// <summary>
            /// Dict7X7_1000
            /// </summary>
            Dict7X7_1000,

            /// <summary>
            /// standard ArUco Library Markers. 1024 markers, 5x5 bits, 0 minimum distance
            /// </summary>
            DictArucoOriginal,

            /// <summary>
            /// 4x4 bits, minimum hamming distance between any two codes = 5, 30 codes
            /// </summary>
            DictAprilTag16h5,

            /// <summary>
            /// 5x5 bits, minimum hamming distance between any two codes = 9, 35 codes
            /// </summary>
            DictAprilTag25h9,

            /// <summary>
            /// 6x6 bits, minimum hamming distance between any two codes = 10, 2320 codes
            /// </summary>
            DictAprilTag36h10,

            /// <summary>
            /// 6x6 bits, minimum hamming distance between any two codes = 11, 587 codes
            /// </summary>
            DictAprilTag36h11,
        }

        /// <summary>
        /// Generate a canonical marker image.
        /// </summary>
        /// <param name="id">Identifier of the marker that will be returned. It has to be a valid id in the specified dictionary.</param>
        /// <param name="sizePixels">Size of the image in pixels</param>
        /// <param name="img">A marker image in its canonical form (i.e. ready to be printed)</param>
        /// <param name="borderBits">Width of the marker border.</param>
        public void GenerateImageMarker(int id, int sizePixels, IOutputArray img, int borderBits = 1)
        {
            using (OutputArray oaImg = img.GetOutputArray())
                ArucoInvoke.cveArucoDictionaryGenerateImageMarker(_ptr, id, sizePixels, oaImg, borderBits);
        }

        /// <summary>
        /// Release the unmanaged resource
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ArucoInvoke.cveArucoDictionaryRelease(ref _ptr, ref _sharedPtr);
        }
    }

    public static partial class ArucoInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveArucoGetPredefinedDictionary(Dictionary.PredefinedDictionaryName name, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveArucoDictionaryCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDictionaryRelease(ref IntPtr dict, ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDictionaryGenerateImageMarker(IntPtr dict, int id, int sizePixels, IntPtr img, int borderBits);
    }
}