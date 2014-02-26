//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// The equavailent of cv::Mat, should only be used if you know what you are doing.
   /// In most case you should use the Matrix class instead
   /// </summary>
   public class UMat : MatDataAllocator, IInputArray, IOutputArray, IInputOutputArray
   {
      private IntPtr _oclMatAllocator;

      private IntPtr _inputArrayPtr;
      private IntPtr _outputArrayPtr;
      private IntPtr _inputOutputArrayPtr;

      private bool _needDispose;

      internal UMat(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
         //InitActionPtr();
         //UMatInvoke.cvUMatUseCustomAllocator(_ptr, AllocateCallback, DeallocateCallback, _allocateDataActionPtr, _freeDataActionPtr, ref _memoryAllocator, ref _oclMatAllocator);
      }

      /// <summary>
      /// Create an empty cv::UMat
      /// </summary>
      public UMat()
         : this(UMatInvoke.cvUMatCreate(), true)
      {
      }

      /// <summary>
      /// Create a umat of the specific type.
      /// </summary>
      /// <param name="rows">Number of rows in a 2D array.</param>
      /// <param name="cols">Number of columns in a 2D array.</param>
      /// <param name="type">Mat element type</param>
      /// <param name="channels">Number of channels</param>
      public UMat(int rows, int cols, CvEnum.DepthType type, int channels)
         : this()
      {
         Create(rows, cols, type, channels);
      }

      /// <summary>
      /// Get the Umat header for the specific roi of the parent
      /// </summary>
      /// <param name="parent">The parent Umat</param>
      /// <param name="roi">The region of interest</param>
      public UMat(UMat parent, Rectangle roi)
        : this(UMatInvoke.cvUMatCreateFromROI(parent.Ptr, ref roi), true)
      {
      }

      /// <summary>
      /// Allocates new array data if needed.
      /// </summary>
      /// <param name="rows">New number of rows.</param>
      /// <param name="cols">New number of columns.</param>
      /// <param name="type">New matrix element depth type.</param>
      /// <param name="channels">New matrix number of channels</param>
      public void Create(int rows, int cols, CvEnum.DepthType type, int channels)
      {
         UMatInvoke.cvUMatCreateData(_ptr, rows, cols, CvInvoke.MakeType(type, channels));
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return UMatInvoke.cvUMatGetSize(_ptr);
         }
      }

      /// <summary>
      /// Get the number of channels
      /// </summary>
      public int NumberOfChannels
      {
         get
         {
            return (int)UMatInvoke.cvUMatGetChannels(_ptr);
         }
      }

      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return UMatInvoke.cvUMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this mat to the other mat
      /// </summary>
      /// <param name="mask">Operation mask. Its non-zero elements indicate which matrix elements need to be copied.</param>
      /// <param name="m">The input array to copy to</param>
      public void CopyTo(IOutputArray m, IInputArray mask = null)
      {
         UMatInvoke.cvUMatCopyTo(this, m.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      /// <summary>
      /// Sets all or some of the array elements to the specified value.
      /// </summary>
      /// <param name="value">Assigned scalar converted to the actual array type.</param>
      /// <param name="mask">Operation mask of the same size as the umat.</param>
      public void SetTo(IInputArray value, IInputArray mask = null)
      {
         UMatInvoke.cvUMatSetTo(Ptr, value.InputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      /// <summary>
      /// Sets all or some of the array elements to the specified value.
      /// </summary>
      /// <param name="value">Assigned scalar value.</param>
      /// <param name="mask">Operation mask of the same size as the umat.</param>
      public void SetTo(MCvScalar value, IInputArray mask = null)
      {
         using (ScalarArray ia = new ScalarArray(value))
         {
            SetTo(ia, mask);
         }
      }

      /// <summary>
      /// Indicates if this cv::UMat is empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return UMatInvoke.cvUMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            UMatInvoke.cvUMatRelease(ref _ptr);

         if (_inputArrayPtr != IntPtr.Zero)
            CvInvoke.cveInputArrayRelease(ref _inputArrayPtr);

         if (_outputArrayPtr != IntPtr.Zero)
            CvInvoke.cveOutputArrayRelease(ref _outputArrayPtr);

         if (_inputOutputArrayPtr != IntPtr.Zero)
            CvInvoke.cveInputOutputArrayRelease(ref _inputOutputArrayPtr);

         if (_oclMatAllocator != IntPtr.Zero)
            MatDataAllocatorInvoke.cvMatAllocatorRelease(ref _oclMatAllocator);

         base.DisposeObject();

      }

      /// <summary>
      /// Pointer to the InputArray
      /// </summary>
      public IntPtr InputArrayPtr
      {
         get
         {
            if (_inputArrayPtr == IntPtr.Zero)
               _inputArrayPtr = UMatInvoke.cveInputArrayFromUMat(_ptr);
            return _inputArrayPtr;
         }
      }

      /// <summary>
      /// Pointer to the OutputArray
      /// </summary>
      public IntPtr OutputArrayPtr
      {
         get
         {
            if (_outputArrayPtr == IntPtr.Zero)
               _outputArrayPtr = UMatInvoke.cveOutputArrayFromUMat(_ptr);
            return _outputArrayPtr;
         }
      }

      /// <summary>
      /// Pointer to the InputOutputArray
      /// </summary>
      public IntPtr InputOutputArrayPtr
      {
         get
         {
            if (_inputOutputArrayPtr == IntPtr.Zero)
               _inputOutputArrayPtr = UMatInvoke.cveInputOutputArrayFromUMat(_ptr);
            return _inputOutputArrayPtr;
         }
      }

      /// <summary>
      /// Changes the shape and/or the number of channels of a 2D matrix without copying the data.
      /// </summary>
      /// <param name="cn">New number of channels. If the parameter is 0, the number of channels remains the same.</param>
      /// <param name="rows">New number of rows. If the parameter is 0, the number of rows remains the same.</param>
      /// <returns>A new mat header that has different shape</returns>
      public UMat Reshape(int cn, int rows = 0)
      {
         return new UMat(UMatInvoke.cvUMatReshape(Ptr, cn, rows), true);
      }
   }

   internal static class UMatInvoke
   {
      static UMatInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputArrayFromUMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveOutputArrayFromUMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputOutputArrayFromUMat(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvUMatCreate();
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cvUMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvUMatGetElementSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvUMatGetChannels(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvUMatIsEmpty(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatCreateData(IntPtr mat, int row, int cols, int type);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvUMatCreateFromROI(IntPtr mat, ref Rectangle roi);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatSetTo(IntPtr mat, IntPtr value, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvUMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator, IntPtr allocateDataActionPtr, IntPtr freeDataActionPtr, ref IntPtr matAllocator, ref IntPtr oclAllocator);


      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvUMatReshape(IntPtr mat, int cn, int rows);
   }
}

