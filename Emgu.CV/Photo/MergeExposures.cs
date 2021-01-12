//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// The base class algorithms that can merge exposure sequence to a single image.
    /// </summary>
    public abstract class MergeExposures : UnmanagedObject
    {
        /// <summary>
        /// The pointer to the unmanaged MergeExposure object
        /// </summary>
        protected IntPtr _mergeExposuresPtr;

        /// <summary>
        /// Merges images.
        /// </summary>
        /// <param name="src">Vector of input images</param>
        /// <param name="dst">Result image</param>
        /// <param name="times">Vector of exposure time values for each image</param>
        /// <param name="response">256x1 matrix with inverse camera response function for each pixel value, it should have the same number of channels as images.</param>
        public void Process(IInputArray src, IOutputArray dst, IInputArray times, IInputArray response)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaTimes = times.GetInputArray())
            using (InputArray iaResponse = response.GetInputArray())
            {
                CvInvoke.cveMergeExposuresProcess(_mergeExposuresPtr, iaSrc, oaDst, iaTimes, iaResponse);
            }
        }

        /// <summary>
        /// Reset the native pointer to the MergeExposure object
        /// </summary>
        protected override void DisposeObject()
        {
            _mergeExposuresPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// The resulting HDR image is calculated as weighted average of the exposures considering exposure values and camera response.
    /// </summary>
    public class MergeDebevec : MergeExposures
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates MergeDebevec object.
        /// </summary>
        public MergeDebevec()
        {
            _ptr = CvInvoke.cveMergeDebevecCreate(ref _mergeExposuresPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the MergeDebevec object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveMergeDebevecRelease(ref _ptr, ref _sharedPtr);
            }

            base.DisposeObject();
        }
    }


    /// <summary>
    /// Pixels are weighted using contrast, saturation and well-exposedness measures, than images are combined using laplacian pyramids.
    /// The resulting image weight is constructed as weighted average of contrast, saturation and well-exposedness measures.
    /// The resulting image doesn't require tonemapping and can be converted to 8-bit image by multiplying by 255, but it's recommended to apply gamma correction and/or linear tonemapping.
    /// </summary>
    public class MergeMertens : MergeExposures
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates MergeMertens object.
        /// </summary>
        /// <param name="contrastWeight">contrast measure weight.</param>
        /// <param name="saturationWeight">saturation measure weight</param>
        /// <param name="exposureWeight">well-exposedness measure weight</param>
        public MergeMertens(float contrastWeight = 1.0f, float saturationWeight = 1.0f, float exposureWeight = 0.0f)
        {
            _ptr = CvInvoke.cveMergeMertensCreate(contrastWeight, saturationWeight, exposureWeight, ref _mergeExposuresPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Merges images.
        /// </summary>
        /// <param name="src">Vector of input images</param>
        /// <param name="dst">Result image</param>
        public void Process(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaTimes = InputArray.GetEmpty())
            using (InputArray iaResponse = InputArray.GetEmpty())
            {
                CvInvoke.cveMergeExposuresProcess(_mergeExposuresPtr, iaSrc, oaDst, iaTimes, iaResponse);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this MergeMertens object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveMergeMertensRelease(ref _ptr, ref _sharedPtr);
            }

            base.DisposeObject();
        }
    }

    /// <summary>
    /// The resulting HDR image is calculated as weighted average of the exposures considering exposure values and camera response
    /// </summary>
    public class MergeRobertson : MergeExposures
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates MergeRobertson object.
        /// </summary>
        public MergeRobertson()
        {
            _ptr = CvInvoke.cveMergeRobertsonCreate(ref _mergeExposuresPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this MergeRobertson object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveMergeRobertsonRelease(ref _ptr, ref _sharedPtr);
            }

            base.DisposeObject();
        }
    }


    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMergeExposuresProcess(
           IntPtr mergeExposures,
           IntPtr src, IntPtr dst,
           IntPtr times, IntPtr response);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMergeDebevecCreate(ref IntPtr merge, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMergeDebevecRelease(ref IntPtr merge, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMergeMertensCreate(float contrastWeight, float saturationWeight, float exposureWeight, ref IntPtr merge, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMergeMertensRelease(ref IntPtr merge, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMergeRobertsonCreate(ref IntPtr merge, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMergeRobertsonRelease(ref IntPtr merge, ref IntPtr sharedPtr);
    }
}
