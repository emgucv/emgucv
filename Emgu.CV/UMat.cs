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
   public class UMat : UnmanagedObject, IInputArray, IOutputArray, IInputOutputArray
   {
      private IntPtr _inputArrayPtr;
      private IntPtr _outputArrayPtr;
      private IntPtr _inputOutputArrayPtr;

      private bool _needDispose;

      internal UMat(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Create an empty cv::UMat
      /// </summary>
      public UMat()
         : this(cvUMatCreate(), true)
      {
      }

      public UMat(int rows, int cols, Mat.Depth type, int channels)
         : this(cvUMatCreateWithType(rows, cols, Mat.MakeType(type, channels)), true)
      {
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return cvUMatGetSize(_ptr);
         }
      }

      public int NumberOfChannels
      {
         get
         {
            return (int)cvUMatGetChannels(_ptr);
         }
      }

      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return cvUMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this mat to the other mat
      /// </summary>
      /// <param name="m">The input array to copy to</param>
      public void CopyTo(IOutputArray m, IInputArray mask)
      {
         cvUMatCopyTo(this, m.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      /// <summary>
      /// Indicates if this cv::Mat is empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return cvUMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            cvUMatRelease(ref _ptr);

         if (_inputArrayPtr != IntPtr.Zero)
            CvInvoke.cvInputArrayRelease(ref _inputArrayPtr);

         if (_outputArrayPtr != IntPtr.Zero)
            CvInvoke.cvOutputArrayRelease(ref _outputArrayPtr);

         if (_inputOutputArrayPtr != IntPtr.Zero)
            CvInvoke.cvInputOutputArrayRelease(ref _inputOutputArrayPtr);
      }

      public IntPtr InputArrayPtr
      {
         get
         {
            if (_inputArrayPtr == IntPtr.Zero)
               _inputArrayPtr = cvInputArrayFromUMat(_ptr);
            return _inputArrayPtr;
         }
      }

      public IntPtr OutputArrayPtr
      {
         get
         {
            if (_outputArrayPtr == IntPtr.Zero)
               _outputArrayPtr = cvOutputArrayFromUMat(_ptr);
            return _outputArrayPtr;
         }
      }

      public IntPtr InputOutputArrayPtr
      {
         get
         {
            if (_inputOutputArrayPtr == IntPtr.Zero)
               _inputOutputArrayPtr = cvInputOutputArrayFromUMat(_ptr);
            return _inputOutputArrayPtr;
         }
      }

      #region PInvoke

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvInputArrayFromUMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvOutputArrayFromUMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvInputOutputArrayFromUMat(IntPtr mat);

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
      internal extern static IntPtr cvUMatCreateWithType(int row, int cols, int type);

      #endregion
   }
}

