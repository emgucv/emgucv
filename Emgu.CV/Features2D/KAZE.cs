//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
    /// <summary>
    /// Wrapped KAZE detector
    /// </summary>
    public class KAZE : Feature2D
    {
        /// <summary>
        /// The diffusivity
        /// </summary>
        public enum Diffusivity
        {
            /// <summary>
            /// PM G1
            /// </summary>
            PmG1 = 0,
            /// <summary>
            /// PM G2
            /// </summary>
            PmG2 = 1,
            /// <summary>
            /// Weickert
            /// </summary>
            Weickert = 2,
            /// <summary>
            /// Charbonnier
            /// </summary>
            Charbonnier = 3
        }

        /// <summary>
        /// Create KAZE using the specific values
        /// </summary>
        /// <param name="extended">Set to enable extraction of extended (128-byte) descriptor.</param>
        /// <param name="upright">Set to enable use of upright descriptors (non rotation-invariant).</param>
        /// <param name="threshold">Detector response threshold to accept point</param>
        /// <param name="octaves">Maximum octave evolution of the image</param>
        /// <param name="sublevels">Default number of sublevels per scale level</param>
        /// <param name="diffusivity">Diffusivity type.</param>
        public KAZE(
            bool extended = false, 
            bool upright = false, 
            float threshold = 0.001f, 
            int octaves = 4, 
            int sublevels = 4, 
            Diffusivity diffusivity = Diffusivity.PmG2)
        {
            _ptr = Features2DInvoke.cveKAZEDetectorCreate(
                extended, upright, threshold, octaves, sublevels, diffusivity,
                ref _feature2D, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                Features2DInvoke.cveKAZEDetectorRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class Features2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveKAZEDetectorCreate(
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool extended,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool upright,
           float threshold,
           int octaves, 
           int sublevels, 
           KAZE.Diffusivity diffusivity,
           ref IntPtr feature2D, 
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveKAZEDetectorRelease(ref IntPtr sharedPtr);
    }
}
