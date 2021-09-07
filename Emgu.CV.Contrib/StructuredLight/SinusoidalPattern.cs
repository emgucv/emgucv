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
using System.Drawing;

namespace Emgu.CV.StructuredLight
{
    /// <summary>
    /// This class generates sinusoidal patterns that can be used with FTP, PSP and FAPS.
    /// </summary>
    public class SinusoidalPattern : SharedPtrObject, IStructuredLightPattern
    {
        private IntPtr _structuredLightPatternPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Type of sinusoidal pattern profilometry methods.
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// Fourier transform profilometry
            /// </summary>
            FTP = 0,
            /// <summary>
            /// Phase-shifting profilometry
            /// </summary>
            PSP = 1,
            /// <summary>
            /// Fourier-assisted phase-shifting profilometry
            /// </summary>
            FAPS = 2
        }

        /// <summary>
        /// Create a new sinusoidal patterns
        /// </summary>
        /// <param name="width">Projector's width.</param>
        /// <param name="height">Projector's height.</param>
        /// <param name="nbrOfPeriods">Number of period along the patterns direction.</param>
        /// <param name="shiftValue">Phase shift between two consecutive patterns.</param>
        /// <param name="methodId">Allow to choose between FTP, PSP and FAPS.</param>
        /// <param name="nbrOfPixelsBetweenMarkers">Number of pixels between two consecutive markers on the same row.</param>
        /// <param name="horizontal">Horizontal</param>
        /// <param name="setMarkers">Allow to set markers on the patterns.</param>
        /// <param name="markersLocation">Vector used to store markers location on the patterns.</param>
        public SinusoidalPattern(
            int width = 800,
            int height = 600,
            int nbrOfPeriods = 20,
            float shiftValue = (float)(2 * Math.PI / 3),
            Method methodId = Method.FAPS,
            int nbrOfPixelsBetweenMarkers = 56,
            bool horizontal = false,
            bool setMarkers = false,
            VectorOfPointF markersLocation = null
            )
        {
            _ptr = StructuredLightInvoke.cveSinusoidalPatternCreate(
                width,
                height,
                nbrOfPeriods,
                shiftValue,
                methodId,
                nbrOfPixelsBetweenMarkers,
                horizontal,
                setMarkers,
                markersLocation,
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

        /// <summary>
        /// Compute a wrapped phase map from sinusoidal patterns.
        /// </summary>
        /// <param name="patternImages">Input data to compute the wrapped phase map.</param>
        /// <param name="wrappedPhaseMap">Wrapped phase map obtained through one of the three methods.</param>
        /// <param name="shadowMask">Mask used to discard shadow regions.</param>
        /// <param name="fundamental">Fundamental matrix used to compute epipolar lines and ease the matching step.</param>
        public void ComputePhaseMap(
            IInputArrayOfArrays patternImages,
            IOutputArray wrappedPhaseMap,
            IOutputArray shadowMask = null,
            IInputArray fundamental = null)
        {
            using (InputArray iaPatternImages = patternImages.GetInputArray())
            using (OutputArray oaWrappedPhaseMap = wrappedPhaseMap.GetOutputArray())
            using (OutputArray oaShadowMask = shadowMask == null ? OutputArray.GetEmpty() : shadowMask.GetOutputArray())
            using (InputArray iaFundamental = fundamental == null ? InputArray.GetEmpty() : fundamental.GetInputArray())
            {
                StructuredLightInvoke.cveSinusoidalPatternComputePhaseMap(
                    _ptr,
                    iaPatternImages,
                    oaWrappedPhaseMap,
                    oaShadowMask,
                    iaFundamental);
            }
        }

        /// <summary>
        /// Unwrap the wrapped phase map to remove phase ambiguities.
        /// </summary>
        /// <param name="wrappedPhaseMap">The wrapped phase map computed from the pattern.</param>
        /// <param name="unwrappedPhaseMap">The unwrapped phase map used to find correspondences between the two devices.</param>
        /// <param name="camSize">Resolution of the camera.</param>
        /// <param name="shadowMask">Mask used to discard shadow regions.</param>
        public void UnwrapPhaseMap(
            IInputArray wrappedPhaseMap,
            IOutputArray unwrappedPhaseMap,
            Size camSize,
            IInputArray shadowMask = null)
        {
            using (InputArray iaWrappedPhaseMap = wrappedPhaseMap.GetInputArray())
            using (OutputArray oaUnwrappedPhaseMap = unwrappedPhaseMap.GetOutputArray())
            using (InputArray iaShadowMask = shadowMask == null ? InputArray.GetEmpty() : shadowMask.GetInputArray())
            {
                StructuredLightInvoke.cveSinusoidalPatternUnwrapPhaseMap(
                    _ptr,
                    iaWrappedPhaseMap,
                    oaUnwrappedPhaseMap,
                    ref camSize,
                    iaShadowMask);
            }
        }

        /// <inheritdoc/>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                StructuredLightInvoke.cveSinusoidalPatternRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _structuredLightPatternPtr = IntPtr.Zero;
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
        internal static extern IntPtr cveSinusoidalPatternCreate(
            int width,
            int height,
            int nbrOfPeriods,
            float shiftValue,
            SinusoidalPattern.Method methodId,
            int nbrOfPixelsBetweenMarkers,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool horizontal,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool setMarkers,
            IntPtr markersLocation,
            ref IntPtr sharedPtr,
            ref IntPtr structuredLightPattern,
            ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSinusoidalPatternRelease(ref IntPtr pattern);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSinusoidalPatternComputePhaseMap(
            IntPtr pattern,
            IntPtr patternImages,
            IntPtr wrappedPhaseMap,
            IntPtr shadowMask,
            IntPtr fundamental);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSinusoidalPatternUnwrapPhaseMap(
            IntPtr pattern,
            IntPtr wrappedPhaseMap,
            IntPtr unwrappedPhaseMap,
            ref Size camSize,
            IntPtr shadowMask);
    }
}