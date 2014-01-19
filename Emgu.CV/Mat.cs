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
   public class Mat : MatDataAllocator, IInputArray, IOutputArray, IInputOutputArray
   {

      internal IntPtr _inputArrayPtr;
      internal IntPtr _outputArrayPtr;
      internal IntPtr _inputOutputArrayPtr;

      public enum DepthType
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

      public static DepthType GetDepthType(Type t)
      {
         if (t == typeof(byte))
         {
            return DepthType.Cv8U;
         } else if (t == typeof(SByte))
         {
            return DepthType.Cv8S;
         } else if (t == typeof(UInt16))
         {
            return DepthType.Cv16U;
         } else if (t == typeof(Int16))
         {
            return DepthType.Cv16S;
         } else if (t == typeof(Int32))
         {
            return DepthType.Cv32S;
         } else if (t == typeof(float))
         {
            return DepthType.Cv32F;
         } else if (t == typeof(double))
         {
            return DepthType.Cv64F;
         } else
         {
            throw new ArgumentException(String.Format("Unable to convert type {0} to depth type", t.ToString()));
         }
      }

      public static int MakeType(DepthType type, int channels)
      {
         int shift = 3;
         return (((int)type) & ( (1 << shift) - 1)) + (((channels) - 1) << shift);
      }

      internal bool _needDispose;

      internal Mat(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
         _memoryAllocator = MatInvoke.cvMatUseCustomAllocator(_ptr, AllocateCallback, DeallocateCallback);
      }

      /// <summary>
      /// Create an empty cv::Mat
      /// </summary>
      public Mat()
         : this(MatInvoke.cvMatCreate(), true)
      {  
      }

      public Mat(int rows, int cols, DepthType type, int channels)
         : this()
      {
         Create(rows, cols, type, channels);
      }

      public Mat(int rows, int cols, DepthType type, int channels, IntPtr data, int step)
         : this(MatInvoke.cvMatCreateWithData(rows, cols, MakeType(type, channels), data, new IntPtr(step)), true)
      {
      }

      public Mat(String fileName, CvEnum.LOAD_IMAGE_TYPE loadType)
         : this(MatInvoke.cvMatCreateFromFile(fileName, loadType), true)
      {
      }

      public UMat GetUMat(CvEnum.AccessType access)
      {
         return new UMat(MatInvoke.cvMatGetUMat(Ptr, access), true);
      }

      public void Create(int rows, int cols, DepthType type, int channels)
      {
         MatInvoke.cvMatCreateData(_ptr, rows, cols, MakeType(type, channels));
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return MatInvoke.cvMatGetSize(_ptr);
         }
      }

      public IntPtr DataPointer
      {
         get
         {
            return MatInvoke.cvMatGetDataPointer(_ptr);
         }
      }

      public int Step
      {
         get
         {
            return (int)MatInvoke.cvMatGetStep(_ptr);
         }
      }

      public int NumberOfChannels
      {
         get
         {
            return (int)MatInvoke.cvMatGetChannels(_ptr);
         }
      }

      public DepthType Depth
      {
         get
         {
            return (DepthType)MatInvoke.cvMatGetDepth(_ptr);
         }
      }

      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return MatInvoke.cvMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this cv::Mat to a CvArray
      /// </summary>
      /// <param name="m">The input array to copy to</param>
      public void CopyTo(IOutputArray m, IInputArray mask = null)
      {
         MatInvoke.cvMatCopyTo(Ptr, m.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      /// <summary>
      /// Indicates if this cv::Mat is empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return MatInvoke.cvMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            MatInvoke.cvMatRelease(ref _ptr);

         if (_inputArrayPtr != IntPtr.Zero)
            CvInvoke.cveInputArrayRelease(ref _inputArrayPtr);

         if (_outputArrayPtr != IntPtr.Zero)
            CvInvoke.cveOutputArrayRelease(ref _outputArrayPtr);

         if (_inputOutputArrayPtr != IntPtr.Zero)
            CvInvoke.cveInputOutputArrayRelease(ref _inputOutputArrayPtr);

         base.DisposeObject();

      }

      public IntPtr InputArrayPtr
      {
         get 
         { 
            if (_inputArrayPtr == IntPtr.Zero)
               _inputArrayPtr = MatInvoke.cveInputArrayFromMat(_ptr);
            return _inputArrayPtr; 
         }
      }

      public IntPtr OutputArrayPtr
      {
         get
         {
            if (_outputArrayPtr == IntPtr.Zero)
               _outputArrayPtr = MatInvoke.cveOutputArrayFromMat(_ptr);
            return _outputArrayPtr;
         }
      }

      public IntPtr InputOutputArrayPtr
      {
         get 
         {
            if (_inputOutputArrayPtr == IntPtr.Zero)
               _inputOutputArrayPtr = MatInvoke.cveInputOutputArrayFromMat(_ptr);
            return _inputOutputArrayPtr;
         }
      }


   }

   internal static class MatInvoke
   {
      static MatInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveOutputArrayFromMat(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputOutputArrayFromMat(IntPtr mat);

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
      internal extern static int cvMatGetDepth(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetDataPointer(IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatGetStep(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvMatIsEmpty(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatCreateData(IntPtr mat, int row, int cols, int type);

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

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatUseCustomAllocator(IntPtr mat, MatDataAllocatorInvoke.MatAllocateCallback allocator, MatDataAllocatorInvoke.MatDeallocateCallback deallocator);

   }
}

