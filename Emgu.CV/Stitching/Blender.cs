//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
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

        /// <summary>
        /// Prepares the blender for blending.
        /// </summary>
        /// <param name="corners">Source images top-left corners</param>
        /// <param name="sizes">Source image sizes</param>
        public void Prepare(Point[] corners, Size[] sizes)
        {
            using (VectorOfPoint vpCorners = new VectorOfPoint(corners))
            using (VectorOfSize vsSizes = new VectorOfSize(sizes))
            {
                StitchingInvoke.cveBlenderPrepare(_blenderPtr, vpCorners, vsSizes);
            }
        }

        /// <summary>
        /// Prepares the blender for blending.
        /// </summary>
        /// <param name="dstRoi">Destination roi</param>
        public void Prepare(Rectangle dstRoi)
        {
            StitchingInvoke.cveBlenderPrepare2(_blenderPtr, ref dstRoi);
        }

        /// <summary>
        /// Processes the image.
        /// </summary>
        /// <param name="img">Source image</param>
        /// <param name="mask">Source image mask</param>
        /// <param name="tl">Source image top-left corners</param>
        public void Feed(IInputArray img, IInputArray mask, Point tl)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (InputArray iaMask = mask.GetInputArray())
            {
                StitchingInvoke.cveBlenderFeed(_blenderPtr, iaImg, iaMask, ref tl);
            }
        }

        /// <summary>
        /// Blends and returns the final pano.
        /// </summary>
        /// <param name="dst">Final pano</param>
        /// <param name="dstMask">Final pano mask</param>
        public void Blend(IInputOutputArray dst, IInputOutputArray dstMask)
        {
            using (InputOutputArray ioaDst = dst.GetInputOutputArray())
            using (InputOutputArray ioaDstMask = dstMask.GetInputOutputArray())
            {
                StitchingInvoke.cveBlenderBlend(_blenderPtr, ioaDst, ioaDstMask);
            }
        }
    }


    public static partial class StitchingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBlenderPrepare(IntPtr blender, IntPtr corners, IntPtr sizes);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBlenderPrepare2(IntPtr blender, ref Rectangle dstRoi);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBlenderFeed(IntPtr blender, IntPtr img, IntPtr mask, ref Point tl);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBlenderBlend(IntPtr blender, IntPtr dst, IntPtr dstMask);
    }
}
