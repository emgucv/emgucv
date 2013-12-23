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
   public class Mat : UnmanagedObject, IInputArray, IOutputArray, IInputOutputArray
   {
      private IntPtr _inputArrayPtr;
      private IntPtr _outputArrayPtr;
      private IntPtr _inputOutputArrayPtr;

      public enum Depth
      {
         Default = -1,
         Cv8U = 0,
         Cv8S = 1,
         Cv16U = 2,
         Cv16S = 3,
         Cv32S = 4,
         Cv32F = 5,
         Cv64F = 6
      }



      public static Depth GetDepth(Type t)
      {
         if (t == typeof(byte))
         {
            return Depth.Cv8U;
         } else if (t == typeof(SByte))
         {
            return Depth.Cv8S;
         } else if (t == typeof(UInt16))
         {
            return Depth.Cv16U;
         } else if (t == typeof(Int16))
         {
            return Depth.Cv16S;
         } else if (t == typeof(Int32))
         {
            return Depth.Cv32S;
         } else if (t == typeof(float))
         {
            return Depth.Cv32F;
         } else if (t == typeof(double))
         {
            return Depth.Cv64F;
         } else
         {
            throw new ArgumentException(String.Format("Unable to convert type {0} to depth type", t.ToString()));
         }
      }

      public static int MakeType(Depth type, int channels)
      {
         int shift = 3;
         return (((int)type) & ( (1 << shift) - 1)) + (((channels) - 1) << shift);
      }

      private bool _needDispose;

      internal Mat(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Create an empty cv::Mat
      /// </summary>
      public Mat()
         : this(cvMatCreate(), true)
      {
      }

      public Mat(int rows, int cols, Depth type, int channels)
         : this(cvMatCreateWithType(rows, cols, MakeType( type, channels)), true)
      {
      }

      public Mat(int rows, int cols, Depth type, int channels, IntPtr data, int step)
         : this(cvMatCreateWithData(rows, cols, MakeType(type, channels), data, new IntPtr(step)), true)
      {
      }

      public Mat(String fileName, CvEnum.LOAD_IMAGE_TYPE loadType)
         : this(cvMatCreateFromFile(fileName, loadType), true)
      {  
      }

      public UMat GetUMat(CvEnum.AccessType access)
      {
         return new UMat(cvMatGetUMat(Ptr, access), true);
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return cvMatGetSize(_ptr);
         }
      }

      public IntPtr DataPointer
      {
         get
         {
            return cvMatGetDataPointer(_ptr);
         }
      }

      public int Step
      {
         get
         {
            return (int) cvMatGetStep(_ptr);
         }
      }

      public int NumberOfChannels
      {
         get
         {
            return (int)cvMatGetChannels(_ptr);
         }
      }

      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return cvMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this cv::Mat to a CvArray
      /// </summary>
      /// <param name="m">The input array to copy to</param>
      public void CopyTo(IOutputArray m, IInputArray mask)
      {
         cvMatCopyTo(this, m.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      /// <summary>
      /// Indicates if this cv::Mat is empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return cvMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            cvMatRelease(ref _ptr);

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
               _inputArrayPtr = cvInputArrayFromMat(_ptr);
            return _inputArrayPtr; 
         }
      }

      public IntPtr OutputArrayPtr
      {
         get
         {
            if (_outputArrayPtr == IntPtr.Zero)
               _outputArrayPtr = cvOutputArrayFromMat(_ptr);
            return _outputArrayPtr;
         }
      }

      public IntPtr InputOutputArrayPtr
      {
         get 
         {
            if (_inputOutputArrayPtr == IntPtr.Zero)
               _inputOutputArrayPtr = cvInputOutputArrayFromMat(_ptr);
            return _inputOutputArrayPtr;
         }
      }

      #region PInvoke

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvInputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvOutputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvInputOutputArrayFromMat(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreate();
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cvMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatCopyTo(IntPtr mat, IntPtr m, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvMatGetElementSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cvMatGetChannels(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetDataPointer(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetStep(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvMatIsEmpty(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreateWithType(int row, int cols, int type);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreateWithData(int rows, int cols, int type, IntPtr data, IntPtr step);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreateFromFile(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String fileName,
         CvEnum.LOAD_IMAGE_TYPE flag
         );

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetUMat(IntPtr mat, CvEnum.AccessType access);
      #endregion
   }
}

