//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// This is the proxy class for passing read-only input arrays into OpenCV functions.
    /// </summary>
    public partial class InputArray : UnmanagedObject
    {
        /// <summary>
        /// Input array type
        /// </summary>
        [Flags]
        public enum Type
        {
            /// <summary>
            /// kind shift
            /// </summary>
            KindShift = 16,
            /// <summary>
            /// Fixed type
            /// </summary>
            FixedType = 0x8000 << KindShift,
            /// <summary>
            /// Fixed size
            /// </summary>
            FixedSize = 0x4000 << KindShift,
            /// <summary>
            /// Kind mask
            /// </summary>
            KindMask = 31 << KindShift,

            /// <summary>
            /// None
            /// </summary>
            None = 0 << KindShift,
            /// <summary>
            /// Mat
            /// </summary>
            Mat = 1 << KindShift,
            /// <summary>
            /// Matx
            /// </summary>
            Matx = 2 << KindShift,
            /// <summary>
            /// StdVector
            /// </summary>
            StdVector = 3 << KindShift,
            /// <summary>
            /// StdVectorVector
            /// </summary>
            StdVectorVector = 4 << KindShift,
            /// <summary>
            /// StdVectorMat
            /// </summary>
            StdVectorMat = 5 << KindShift,
            /// <summary>
            /// Expr
            /// </summary>
            Expr = 6 << KindShift,
            /// <summary>
            /// Opengl buffer
            /// </summary>
            OpenglBuffer = 7 << KindShift,
            /// <summary>
            /// Cuda Host Mem
            /// </summary>
            CudaHostMem = 8 << KindShift,
            /// <summary>
            /// Cuda GpuMat
            /// </summary>
            CudaGpuMat = 9 << KindShift,
            /// <summary>
            /// UMat
            /// </summary>
            UMat = 10 << KindShift,
            /// <summary>
            /// StdVectorUMat
            /// </summary>
            StdVectorUMat = 11 << KindShift,
            /// <summary>
            /// StdBoolVector
            /// </summary>
            StdBoolVector = 12 << KindShift,
            /// <summary>
            /// StdVectorCudaGpuMat
            /// </summary>
            StdVectorCudaGpuMat = 13 << KindShift
        }

        internal InputArray()
        {
        }

        private object _parent;

        /// <summary>
        /// Create a Input array from an existing unmanaged inputArray pointer
        /// </summary>
        /// <param name="inputArrayPtr">The unmanaged pointer the the InputArray</param>
        /// <param name="parent">The parent object to keep reference to</param>
        public InputArray(IntPtr inputArrayPtr, object parent)
        {
            _ptr = inputArrayPtr;
            _parent = parent;
        }

        private static InputArray _empty = new InputArray();

        /// <summary>
        /// Get an empty input array
        /// </summary>
        /// <returns>An empty input array</returns>
        public static InputArray GetEmpty()
        {
            return _empty;
        }

        /// <summary>
        /// Get the Mat from the input array
        /// </summary>
        /// <param name="idx">The index, in case if this is an VectorOfMat</param>
        /// <returns>The Mat</returns>
        public Mat GetMat(int idx = -1)
        {
            Mat m = new Mat();
            CvInvoke.cveInputArrayGetMat(Ptr, idx, m);
            return m;
        }

        /// <summary>
        /// Get the UMat from the input array
        /// </summary>
        /// <param name="idx">The index, in case if this is an VectorOfUMat</param>
        /// <returns>The UMat</returns>
        public UMat GetUMat(int idx = -1)
        {
            UMat m = new UMat();
            CvInvoke.cveInputArrayGetUMat(Ptr, idx, m);
            return m;
        }

        /// <summary>
        /// Get the size of the input array
        /// </summary>
        /// <param name="idx">The optional index</param>
        /// <returns>The size of the input array</returns>
        public Size GetSize(int idx = -1)
        {
            Size s = new Size();
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveInputArrayGetSize(_ptr, ref s, idx);
            return s;
        }

        /// <summary>
        /// Return true if the input array is empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (_ptr == IntPtr.Zero)
                    return true;
                return CvInvoke.cveInputArrayIsEmpty(_ptr);
            }
        }

        /// <summary>
        /// Get the depth type
        /// </summary>
        /// <param name="idx">The optional index</param>
        /// <returns>The depth type</returns>
        public DepthType GetDepth(int idx = -1)
        {
            if (_ptr == IntPtr.Zero)
                return DepthType.Default;
            return CvInvoke.cveInputArrayGetDepth(_ptr, idx);
        }

        /// <summary>
        /// Get the number of dimensions
        /// </summary>
        /// <param name="i">The optional index</param>
        /// <returns>The dimensions</returns>
        public int GetDims(int i = -1)
        {
            return CvInvoke.cveInputArrayGetDims(_ptr, i);
        }

        /// <summary>
        /// Get the number of channels
        /// </summary>
        /// <param name="idx">The optional index</param>
        /// <returns>The number of channels</returns>
        public int GetChannels(int idx = -1)
        {
            if (_ptr == IntPtr.Zero)
                return 0;
            return CvInvoke.cveInputArrayGetChannels(_ptr, idx);
        }

        /// <summary>
        /// Copy this Input array to another.
        /// </summary>
        /// <param name="arr">The destination array.</param>
        /// <param name="mask">The optional mask.</param>
        public void CopyTo(IOutputArray arr, IInputArray mask = null)
        {
            using (OutputArray oaArr = arr.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                CvInvoke.cveInputArrayCopyTo(_ptr, oaArr, iaMask);
            }
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this InputArray
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveInputArrayRelease(ref _ptr);
        }
    }

    public partial class CvInvoke
    {
        /// <summary>
        /// Release the InputArray
        /// </summary>
        /// <param name="arr">Pointer to the input array</param>
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveInputArrayRelease(ref IntPtr arr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveInputArrayGetDims(IntPtr ia, int idx);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveInputArrayGetSize(IntPtr ia, ref System.Drawing.Size size, int idx);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern DepthType cveInputArrayGetDepth(IntPtr ia, int idx);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveInputArrayGetChannels(IntPtr ia, int idx);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(BoolMarshalType)]
        internal static extern bool cveInputArrayIsEmpty(IntPtr ia);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveInputArrayGetMat(IntPtr ia, int idx, IntPtr mat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveInputArrayGetUMat(IntPtr ia, int idx, IntPtr umat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveInputArrayCopyTo(IntPtr ia, IntPtr arr, IntPtr mask);
    }
}