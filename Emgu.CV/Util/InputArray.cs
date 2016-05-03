//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
#if !(__IOS__ || UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || NETFX_CORE)
using Emgu.CV.Cuda;
#endif
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// This is the proxy class for passing read-only input arrays into OpenCV functions.
   /// </summary>
   public partial class InputArray : UnmanagedObject
   {
      [Flags]
      public enum Type
      {
         KindShift = 16,
         FixedType = 0x8000 << KindShift,
         FixedSize = 0x4000 << KindShift,
         KindMask = 31 << KindShift,

         None = 0 << KindShift,
         Mat = 1 << KindShift,
         Matx = 2 << KindShift,
         StdVector = 3 << KindShift,
         StdVectorVector = 4 << KindShift,
         StdVectorMat = 5 << KindShift,
         Expr = 6 << KindShift,
         OpenglBuffer = 7 << KindShift,
         CudaHostMem = 8 << KindShift,
         CudaGpuMat = 9 << KindShift,
         UMat = 10 << KindShift,
         StdVectorUMat = 11 << KindShift,
         StdBoolVector = 12 << KindShift,
         StdVectorCudaGpuMat = 13 << KindShift
      }

      internal InputArray()
      {
      }

      /// <summary>
      /// Create a Input array from an existing unmanaged inputArray pointer
      /// </summary>
      /// <param name="inputArrayPtr">The unmanaged pointer the the InputArray</param>
      public InputArray(IntPtr inputArrayPtr)
      {
         _ptr = inputArrayPtr;
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

#if !(__IOS__ || UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || NETFX_CORE)


      public Cuda.GpuMat GetGpuMat()
      {
         Cuda.GpuMat m = new Cuda.GpuMat();
         CvInvoke.cveInputArrayGetGpuMat(Ptr, m);
         return m;
      }
#endif

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
      /// <returns>True if the input array is empty</returns>
      public bool IsEmpty()
      {
         if (_ptr == IntPtr.Zero)
            return true;
         return CvInvoke.cveInputArrayIsEmpty(_ptr);
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
      internal static extern void cveInputArrayGetGpuMat(IntPtr ia, IntPtr gpumat);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveInputArrayCopyTo(IntPtr ia, IntPtr arr, IntPtr mask);
   }
}