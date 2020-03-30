//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Blender for Image Stitching
    /// </summary>
    public abstract class Blender : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native Blender object.
        /// </summary>
        protected IntPtr _blenderPtr;

        /// <summary>
        /// Pointer to the native Blender object.
        /// </summary>
        public IntPtr BlenderPtr
        {
            get { return _blenderPtr; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_blenderPtr != IntPtr.Zero)
                _blenderPtr = IntPtr.Zero;
        }
    }


    /// <summary>
    /// Simple blender which mixes images at its borders.
    /// </summary>
    public class FeatherBlender : Blender
    {
        /// <summary>
        /// Create a simple blender which mixes images at its borders
        /// </summary>
        /// <param name="sharpness">Sharpness</param>
        public FeatherBlender(float sharpness = 0.02f)
        {
            _ptr = StitchingInvoke.cveFeatherBlenderCreate(sharpness, ref _blenderPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this blender
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveFeatherBlenderRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Blender which uses multi-band blending algorithm
    /// </summary>
    public class MultiBandBlender : Blender
    {
        /// <summary>
        /// Create a multiBandBlender
        /// </summary>
        /// <param name="tryGpu">If true, will try to use GPU</param>
        /// <param name="numBands">Number of bands</param>
        /// <param name="weightType">The weight type</param>
        public MultiBandBlender(bool tryGpu = true, int numBands = 5, CvEnum.DepthType weightType = CvEnum.DepthType.Cv32F)
        {
            _ptr = StitchingInvoke.cveMultiBandBlenderCreate(tryGpu ? 1 : 0, numBands, weightType, ref _blenderPtr);
        }

        /// <summary>
        /// Release all unmanaged resources associated with this blender
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveMultiBandBlenderRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    public static partial class StitchingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFeatherBlenderCreate(float sharpness, ref IntPtr blender);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFeatherBlenderRelease(ref IntPtr blender);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMultiBandBlenderCreate(int tryGpu, int numBands, CvEnum.DepthType weightType, ref IntPtr blender);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMultiBandBlenderRelease(ref IntPtr blender);
    }
}
