//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;

namespace Emgu.CV
{
    /// <summary>
    /// The base class for algorithms that align images of the same scene with different exposures
    /// </summary>
    public abstract class AlignExposures : UnmanagedObject
    {
        /// <summary>
        /// The pointer to the native AlignExposures object
        /// </summary>
        protected IntPtr _alignExposuresPtr;

        /// <summary>
        /// Aligns images.
        /// </summary>
        /// <param name="src">vector of input images</param>
        /// <param name="dst">vector of aligned images</param>
        /// <param name="times">vector of exposure time values for each image</param>
        /// <param name="response">256x1 matrix with inverse camera response function for each pixel value, it should have the same number of channels as images.</param>
        public void Process(IInputArrayOfArrays src, VectorOfMat dst, IInputArray times, IInputArray response)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaTimes = times.GetInputArray())
            using (InputArray iaResponse = times.GetInputArray())
            {
                CvInvoke.cveAlignExposuresProcess(_alignExposuresPtr, iaSrc, dst, iaTimes, iaResponse);
            }
        }

        /// <summary>
        /// Reset the pointer that points to the CalibrateCRF object.
        /// </summary>
        protected override void DisposeObject()
        {
            _alignExposuresPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This algorithm converts images to median threshold bitmaps (1 for pixels brighter than median luminance and 0 otherwise) and than aligns the resulting bitmaps using bit operations.
    /// </summary>
    public class AlignMTB : AlignExposures
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create an AlignMTB object
        /// </summary>
        /// <param name="maxBits">logarithm to the base 2 of maximal shift in each dimension. Values of 5 and 6 are usually good enough (31 and 63 pixels shift respectively).</param>
        /// <param name="excludeRange">range for exclusion bitmap that is constructed to suppress noise around the median value.</param>
        /// <param name="cut">if true cuts images, otherwise fills the new regions with zeros.</param>
        public AlignMTB(int maxBits = 6, int excludeRange = 4, bool cut = true)
        {
            _ptr = CvInvoke.cveAlignMTBCreate(maxBits, excludeRange, cut, ref _alignExposuresPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this AlignMTB object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveAlignMTBRelease(ref _ptr, ref _sharedPtr);
            }
            base.DisposeObject();
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAlignExposuresProcess(IntPtr alignExposures, IntPtr src, IntPtr dst, IntPtr times, IntPtr response);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveAlignMTBCreate(
            int maxBits,
            int excludeRange,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool cut,
            ref IntPtr alignExposures,
            ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAlignMTBRelease(ref IntPtr alignExposures, ref IntPtr sharedPtr);

    }
}
