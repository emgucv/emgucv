//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || NETFX_CORE || NETSTANDARD1_4 || UNITY_IOS )

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.Superres
{
   /// <summary>
   /// Create a video frame source
   /// </summary>
   public class FrameSource : UnmanagedObject
   {
       private IntPtr _sharedPtr;

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
               _ptr = SuperresInvoke.cveSuperresCreateFrameSourceVideo(s, true, ref _sharedPtr);
            }
            catch
            {
               _ptr = SuperresInvoke.cveSuperresCreateFrameSourceVideo(s, false, ref _sharedPtr);
            }
         }
         else
         {
            _ptr = SuperresInvoke.cveSuperresCreateFrameSourceVideo(s, false, ref _sharedPtr);
         }

         _frameSourcePtr = _ptr;
      }

      ///<summary> Create a framesource using the specific camera</summary>
      ///<param name="camIndex"> The index of the camera to create capture from, starting from 0</param>
      public FrameSource(int camIndex)
      {
         _ptr = SuperresInvoke.cveSuperresCreateFrameSourceCamera(camIndex, ref _sharedPtr);
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
            SuperresInvoke.cveSuperresFrameSourceNextFrame(_frameSourcePtr, oaFrame);

      }

      /// <summary>
      /// Release all the unmanaged memory associated with this framesource
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            SuperresInvoke.cveSuperresFrameSourceRelease(ref _ptr, ref _sharedPtr);
      }
   }

   internal static partial class SuperresInvoke
   {
      static SuperresInvoke()
      {
          CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSuperresCreateFrameSourceVideo(
         IntPtr fileName,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useGpu, 
         ref IntPtr sharedPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSuperresCreateFrameSourceCamera(int deviceId, ref IntPtr sharedPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperresFrameSourceRelease(ref IntPtr frameSource, ref IntPtr sharedPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperresFrameSourceNextFrame(IntPtr frameSource, IntPtr frame);
   }

}
#endif