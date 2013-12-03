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

namespace Emgu.CV.Util
{
   /// <summary>
   /// The equavailent of cv::Mat, should only be used if you know what you are doing.
   /// In most case you should use the Matrix class instead
   /// </summary>
   public class Mat : UnmanagedObject
   {
      public enum Depth
      {
         Cv8U = 0,
         Cv8S = 1,
         Cv16U = 2,
         Cv16S = 3,
         Cv32S = 4,
         Cv32F = 5,
         Cv64F = 6
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
         : this(CvInvoke.cvMatCreate(), true)
      {
      }

      public Mat(int rows, int cols, Depth type, int channels)
         : this(CvInvoke.cvMatCreateWithType(rows, cols, MakeType( type, channels)), true)
      {
      }

      public Mat(int rows, int cols, Depth type, int channels, IntPtr data, int step)
         : this(CvInvoke.cvMatCreateWithData(rows, cols, MakeType(type, channels), data, new IntPtr(step)), true)
      {
      }

      /// <summary>
      /// The size of this matrix
      /// </summary>
      public Size Size
      {
         get
         {
            return CvInvoke.cvMatGetSize(_ptr);
         }
      }

      public IntPtr DataPointer
      {
         get
         {
            return CvInvoke.cvMatGetDataPointer(_ptr);
         }
      }

      public int Step
      {
         get
         {
            return (int) CvInvoke.cvMatGetStep(_ptr);
         }
      }

      public int NumberOfChannels
      {
         get
         {
            return (int)CvInvoke.cvMatGetChannels(_ptr);
         }
      }

   

      /// <summary>
      /// The size of the elements in this matrix
      /// </summary>
      public int ElementSize
      {
         get
         {
            return CvInvoke.cvMatGetElementSize(_ptr);
         }
      }

      /// <summary>
      /// Copy the data in this cv::Mat to a CvArray
      /// </summary>
      /// <param name="cvArray">The CvArray to copy to</param>
      public void CopyTo(IntPtr cvArray)
      {
         CvInvoke.cvMatCopyToCvArr(_ptr, cvArray);
      }

      /// <summary>
      /// Make this to represent the cvArray without data copy
      /// </summary>
      /// <param name="cvArray">The cvArray to make header from</param>
      public void From(IntPtr cvArray)
      {
         CvInvoke.cvMatFromCvArr(_ptr, cvArray);
      }

      /// <summary>
      /// Indicates if this cv::Mat is empty
      /// </summary>
      public bool IsEmpty
      {
         get
         {
            return CvInvoke.cvMatIsEmpty(_ptr);
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            CvInvoke.cvMatRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvMatCreate();
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatRelease(ref IntPtr mat);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static Size cvMatGetSize(IntPtr mat);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatCopyToCvArr(IntPtr mat, IntPtr cvArray);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvMatFromCvArr(IntPtr mat, IntPtr cvArray);

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
   }
}
