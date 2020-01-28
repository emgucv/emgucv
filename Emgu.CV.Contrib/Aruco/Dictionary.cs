//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        /// Generates a new customizable marker dictionary.
        /// </summary>
        /// <param name="nMarkers">number of markers in the dictionary</param>
        /// <param name="markerSize">number of bits per dimension of each markers</param>
        public Dictionary(int nMarkers, int markerSize)
        {
            //_predefined = false;
            _ptr = ArucoInvoke.cveArucoDictionaryCreate1(nMarkers, markerSize, ref _sharedPtr);
        }

        /// <summary>
        /// Generates a new customizable marker dictionary.
        /// </summary>
        /// <param name="nMarkers">number of markers in the dictionary</param>
        /// <param name="markerSize">number of bits per dimension of each markers</param>
        /// <param name="baseDictionary">Include the markers in this dictionary at the beginning (optional)</param>
        public Dictionary(int nMarkers, int markerSize, Dictionary baseDictionary)
        {
            //_predefined = false;
            _ptr = ArucoInvoke.cveArucoDictionaryCreate2(nMarkers, markerSize, baseDictionary._sharedPtr, ref _sharedPtr);
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
            DictArucoOriginal
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
        internal static extern IntPtr cveArucoDictionaryCreate1(int nMarkers, int markerSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveArucoDictionaryCreate2(int nMarkers, int markerSize, IntPtr baseDictionary, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDictionaryRelease(ref IntPtr dict, ref IntPtr sharedPtr);
    }
}