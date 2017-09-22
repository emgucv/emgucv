//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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
    public abstract class AlignExposures : UnmanagedObject
    {
        /// <summary>
        /// The pointer to the native AlignExposures object
        /// </summary>
        protected IntPtr _alignExposuresPtr;

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

    public class AlignMTB : AlignExposures
    {
        public AlignMTB(int maxBits, int excludeRange, bool cut)
        {
            _ptr = CvInvoke.cveAlignMTBCreate(maxBits, excludeRange, cut, ref _alignExposuresPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this AlignMTB object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveAlignMTBRelease(ref _ptr);
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
            ref IntPtr alignExposures);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAlignMTBRelease(ref IntPtr alignExposures);

    }
}
