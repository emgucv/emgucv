//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   /// Use the Capture class as a FrameSource
   /// </summary>
   public class CaptureFrameSource : FrameSource
   {
      /// <summary>
      /// Create a Capture frame source
      /// </summary>
      /// <param name="capture">The capture object that will be converted to a FrameSource</param>
      public CaptureFrameSource(Capture capture)
      {
         _ptr = VideoStabInvoke.CaptureFrameSourceCreate(capture, ref FrameSourcePtr);
         CaptureSource = capture.CaptureSource;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this CaptureFrameSource
      /// </summary>
      protected override void DisposeObject()
      {
         VideoStabInvoke.CaptureFrameSourceRelease(ref _ptr);
         FrameSourcePtr = IntPtr.Zero;
         base.DisposeObject();
      }
   }
}
