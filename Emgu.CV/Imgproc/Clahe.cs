//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Contrast Limited Adaptive Histogram Equalization
    /// </summary>
    public class Clahe : SharedPtrObject
    {
        /// <summary>
        /// Create a Contrast Limited Adaptive Histogram Equalization object.
        /// </summary>
        /// <param name="clipLimit">Threshold for contrast limiting. Use 40.0 for default.</param>
        /// <param name="tileGridSize">Size of grid for histogram equalization. Input image will be divided into equally sized rectangular tiles. This parameter defines the number of tiles in row and column. Use (8, 8) for default.</param>
        public Clahe(double clipLimit = 40.0, Size tileGridSize = default)
        {
            if (tileGridSize.IsEmpty)
                tileGridSize = new Size(8, 8);
            _ptr = CvInvoke.cveCLAHECreate(clipLimit, ref tileGridSize, ref _sharedPtr);
        }

        /// <summary>
        /// Equalizes the histogram of a grayscale image using Contrast Limited Adaptive Histogram Equalization.
        /// </summary>
        /// <param name="src">Source image of type CV_8UC1 or CV_16UC1.</param>
        /// <param name="dst">Destination image.</param>
        public void Apply(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                CvInvoke.cveCLAHEApply(_ptr, iaSrc, oaDst);
        }

        /// <summary>
        /// Gets or sets the threshold for contrast limiting.
        /// </summary>
        public double ClipLimit
        {
            get => CvInvoke.cveCLAHEGetClipLimit(_ptr);
            set => CvInvoke.cveCLAHESetClipLimit(_ptr, value);
        }

        /// <summary>
        /// Gets or sets the size of the grid for histogram equalization.
        /// </summary>
        public Size TilesGridSize
        {
            get
            {
                Size s = new Size();
                CvInvoke.cveCLAHEGetTilesGridSize(_ptr, ref s);
                return s;
            }
            set => CvInvoke.cveCLAHESetTilesGridSize(_ptr, ref value);
        }

        /// <summary>
        /// Gets or sets the bit shift for histogram bins.
        /// </summary>
        public int BitShift
        {
            get => CvInvoke.cveCLAHEGetBitShift(_ptr);
            set => CvInvoke.cveCLAHESetBitShift(_ptr, value);
        }

        /// <summary>
        /// Clears any caches built up from processing the previous set of images.
        /// </summary>
        public void CollectGarbage()
        {
            CvInvoke.cveCLAHECollectGarbage(_ptr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this object.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CvInvoke.cveCLAHERelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern IntPtr cveCLAHECreate(double clipLimit, ref Size tileGridSize, ref IntPtr sharedPtr);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern void cveCLAHEApply(IntPtr clahe, IntPtr src, IntPtr dst);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern void cveCLAHERelease(ref IntPtr sharedPtr);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern void cveCLAHESetClipLimit(IntPtr clahe, double clipLimit);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern double cveCLAHEGetClipLimit(IntPtr clahe);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern void cveCLAHESetTilesGridSize(IntPtr clahe, ref Size tileGridSize);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern void cveCLAHEGetTilesGridSize(IntPtr clahe, ref Size size);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern void cveCLAHESetBitShift(IntPtr clahe, int shift);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern int cveCLAHEGetBitShift(IntPtr clahe);

        [DllImport(ExternLibrary, CallingConvention = CvCallingConvention)]
        internal static extern void cveCLAHECollectGarbage(IntPtr clahe);
    }
}
