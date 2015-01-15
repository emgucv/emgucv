//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.Superres
{
   /// <summary>
   /// Create a video frame source
   /// </summary>
   public class FrameSource : UnmanagedObject
   {
      /// <summary>
      /// The pointer to the frame source
      /// </summary>
      protected IntPtr _frameSourcePtr;

      /// <summary>
      /// Create video frame source from video file
      /// </summary>
      /// <param name="fileName">The name of the file</param>
      /// <param name="tryUseGpu">If true, it will try to create video frame source using gpu</param>
      public FrameSource(String fileName, bool tryUseGpu)
      {
         using(CvString s = new CvString(fileName))
         if (tryUseGpu)
         {
            try
            {
               _ptr = SuperresInvoke.cvSuperresCreateFrameSourceVideo(s, true);
            }
            catch
            {
               _ptr = SuperresInvoke.cvSuperresCreateFrameSourceVideo(s, false);
            }
         }
         else
         {
            _ptr = SuperresInvoke.cvSuperresCreateFrameSourceVideo(s, false);
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
      public void NextFrame(IOutputArray frame)
      {
         using (OutputArray oaFrame = frame.GetOutputArray())
            SuperresInvoke.cvSuperresFrameSourceNextFrame(_frameSourcePtr, oaFrame);

      }

      /// <summary>
      /// Release all the unmanaged memory associated with this framesource
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            SuperresInvoke.cvSuperresFrameSourceRelease(ref _ptr);
      }
   }

   internal static partial class SuperresInvoke
   {
      static SuperresInvoke()
      {
         //Dummy code to make sure the static constructor of GpuInvoke has been called
         bool hasCuda = CudaInvoke.HasCuda;
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvSuperresCreateFrameSourceVideo(
         IntPtr fileName,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useGpu);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvSuperresCreateFrameSourceCamera(int deviceId);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperresFrameSourceRelease(ref IntPtr frameSource);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperresFrameSourceNextFrame(IntPtr frameSource, IntPtr frame);
   }

}
