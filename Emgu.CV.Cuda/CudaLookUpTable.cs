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

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Gpu look up table
    /// </summary>
    public class CudaLookUpTable : SharedPtrObject
    {
        /// <summary>
        /// Create the look up table
        /// </summary>
        /// <param name="lookUpTable">It should be either 1 or 3 channel matrix of 1x256</param>
        public CudaLookUpTable(IInputArray lookUpTable)
        {
            using (InputArray iaLookupTable = lookUpTable.GetInputArray())
                _ptr = CudaInvoke.cudaLookUpTableCreate(iaLookupTable, ref _sharedPtr);
        }

        /// <summary>
        /// Transform the image using the lookup table
        /// </summary>
        /// <param name="image">The image to be transformed</param>
        /// <param name="dst">The transformation result</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Transform(IInputArray image, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                CudaInvoke.cudaLookUpTableTransform(_ptr, iaImage, oaDst, stream);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this look up table
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaLookUpTableRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaLookUpTableCreate(IntPtr lut, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaLookUpTableTransform(IntPtr lut, IntPtr image, IntPtr dst, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaLookUpTableRelease(ref IntPtr lut);
    }
}
