//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
   /// <summary>
   /// A FrameSource that can be used by the Video Stabilizer
   /// </summary>
   public abstract class FrameSource : UnmanagedObject
   {
      private Capture.CaptureModuleType _captureSource;

      /// <summary>
      /// Get or Set the capture type
      /// </summary>
      public Capture.CaptureModuleType CaptureSource
      {
         get
         {
            return _captureSource;
         }
         set
         {
            _captureSource = value;
         }
      }

      /// <summary>
      /// The unmanaged pointer the the frameSource
      /// </summary>
      public IntPtr FrameSourcePtr;

      /// <summary>
      /// Retrieve the next frame from the FrameSoure
      /// </summary>
      /// <returns></returns>
      public Mat NextFrame()
      {
         Mat frame = new Mat();
         VideoStabInvoke.VideostabFrameSourceGetNextFrame(FrameSourcePtr, frame);
         return frame;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this FrameSource
      /// </summary>
      protected override void DisposeObject()
      {
         FrameSourcePtr = IntPtr.Zero;
      }
   }
}