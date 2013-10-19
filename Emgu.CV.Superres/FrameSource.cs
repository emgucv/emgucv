//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.Superres
{
   /// <summary>
   /// Create a video frame source
   /// </summary>
   public class FrameSource : UnmanagedObject
   {
      private Util.Mat _frame = new Util.Mat();
      private Image<Bgr, Byte> _image;

      protected IntPtr _frameSourcePtr;

      /// <summary>
      /// Create video frame source from video file
      /// </summary>
      /// <param name="fileName">The name of the file</param>
      /// <param name="tryUseGpu">If true, it will try to create video frame source using gpu</param>
      public FrameSource(String fileName, bool tryUseGpu)
      {
         if (tryUseGpu)
         {
            try
            {
               _ptr = SuperresInvoke.cvSuperresCreateFrameSourceVideo(fileName, true);
            }
            catch
            {
               _ptr = SuperresInvoke.cvSuperresCreateFrameSourceVideo(fileName, false);
            }
         }
         else
         {
            _ptr = SuperresInvoke.cvSuperresCreateFrameSourceVideo(fileName, false);
         }

         _frameSourcePtr = _ptr;
      }

      ///<summary> Create a framesource using the specific camera</summary>
      ///<param name="camIndex"> The index of the camera to create capture from, starting from 0</param>
      public FrameSource(int camIndex)
      {
         _ptr = SuperresInvoke.cvSuperresCreateFrameSourceCamera(camIndex);
         _frameSourcePtr = _ptr;
      }

      internal FrameSource()
      {
      }

      /// <summary>
      /// Get the next frame
      /// </summary>
      /// <returns>The next frame, this image should not be release by the user. I will be reused over frames. If there are no more frames available, null will be returned</returns>
      public Image<Bgr, Byte> NextFrame()
      {
         SuperresInvoke.cvSuperresFrameSourceNextFrame(_frameSourcePtr, _frame);

         if (_frame.IsEmpty)
            return null;

         if (_image == null || _image.Ptr == IntPtr.Zero)
            _image = new Image<Bgr, byte>(_frame.Size);

         _frame.CopyTo(_image);

         return _image;
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this framesource
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            SuperresInvoke.cvSuperresFrameSourceRelease(ref _ptr);

         ReleaseCache();
      }

      /// <summary>
      /// Release the cached images
      /// </summary>
      protected void ReleaseCache()
      {
         if (_frameSourcePtr != IntPtr.Zero)
         {
            _frame.Dispose();
            if (_image != null)
            {
               _image.Dispose();
               _image = null;
            }
            _frameSourcePtr = IntPtr.Zero;
         }
      }
   }

   internal static partial class SuperresInvoke
   {
      static SuperresInvoke()
      {
         //Dummy code to make sure the static constructor of GpuInvoke has been called
         bool hasCuda = GpuInvoke.HasCuda;
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvSuperresCreateFrameSourceVideo(
         [MarshalAs(CvInvoke.StringMarshalType)]
           String fileName,
         [MarshalAs(CvInvoke.BoolMarshalType)]
           bool useGpu);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvSuperresCreateFrameSourceCamera(int deviceId);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperresFrameSourceRelease(ref IntPtr frameSource);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperresFrameSourceNextFrame(IntPtr frameSource, IntPtr frame);
   }

}
