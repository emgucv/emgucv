//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Quality;

namespace Emgu.CV.StructuredLight
{
    /// <summary>
    /// Class implementing the Gray-code pattern, based on 
    /// Kyriakos Herakleous and Charalambos Poullis. 3DUNDERWORLD-SLS: An Open-Source Structured-Light Scanning System for Rapid Geometry Acquisition. arXiv preprint arXiv:1406.6595, 2014.
    /// </summary>
    public class GrayCodePattern : SharedPtrObject, IStructuredLightPattern
    {
        private IntPtr _structuredLightPatternPtr;
        private IntPtr _algorithmPtr;


        /// <summary>
        /// Create a new GrayCodePattern
        /// </summary>
        /// <param name="width">The width of the projector.</param>
        /// <param name="height">The height of the projector.</param>
        public GrayCodePattern(
            int width = 1024,
            int height = 768)
        {
            _ptr = StructuredLightInvoke.cveGrayCodePatternCreate(
                width,
                height,
                ref _sharedPtr,
                ref _structuredLightPatternPtr, 
                ref _algorithmPtr);
        }

        /// <inheritdoc/>        
        public IntPtr StructuredLightPatternPtr
        {
            get
            {
                return _structuredLightPatternPtr;
            }
        }

        /// <inheritdoc/>
        public IntPtr AlgorithmPtr
        {
            get {return _algorithmPtr; }
        }

        /// <inheritdoc/>
        protected override void DisposeObject()
        {
            if (_sharedPtr != null)
            {
                StructuredLightInvoke.cveGrayCodePatternRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }


    /// <summary>
    /// Provide interfaces to the Open CV StructuredLight functions
    /// </summary>
    public static partial class StructuredLightInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGrayCodePatternCreate(
            int width,
            int height,
            ref IntPtr sharedPtr,
            ref IntPtr structuredLightPattern,
            ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGrayCodePatternRelease(ref IntPtr pattern);
    }
}