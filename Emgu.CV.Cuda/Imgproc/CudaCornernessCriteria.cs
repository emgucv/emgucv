//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Base CornernessCriteria class
    /// </summary>
    public abstract class CudaCornernessCriteria : SharedPtrObject
    {
        /// <summary>
        /// Release all the unmanaged memory associated with this gpu filter
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaCornernessCriteriaRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Apply the cuda filter
        /// </summary>
        /// <param name="image">The source CudaImage where the filter will be applied to</param>
        /// <param name="dst">The destination CudaImage</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Apply(IInputArray image, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                CudaInvoke.cudaCornernessCriteriaCompute(SharedPtr, iaImage, oaDst, stream);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaCornernessCriteriaCompute(IntPtr detector, IntPtr src, IntPtr dst, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaCornernessCriteriaRelease(ref IntPtr detector);
    }

}
