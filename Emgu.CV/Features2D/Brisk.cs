//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
    /// <summary>
    /// BRISK: Binary Robust Invariant Scalable Keypoints
    /// </summary>
    public class Brisk : Feature2D
    {

        /// <summary>
        /// Create a BRISK keypoint detector and descriptor extractor.
        /// </summary>
        /// <param name="thresh">Feature parameters.</param>
        /// <param name="octaves">The number of octave layers.</param>
        /// <param name="patternScale">Pattern scale</param>
        public Brisk(int thresh = 30, int octaves = 3, float patternScale = 1.0f)
        {
            _ptr = Features2DInvoke.cveBriskCreate(thresh, octaves, patternScale, ref _feature2D, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                Features2DInvoke.cveBriskRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class Features2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBriskCreate(int thresh, int octaves, float patternScale,
           ref IntPtr feature2D, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBriskRelease(ref IntPtr sharedPtr);
    }
}
