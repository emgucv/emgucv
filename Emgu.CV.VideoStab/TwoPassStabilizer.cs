//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
   /// <summary>
   /// A two pass video stabilizer
   /// </summary>
   public class TwoPassStabilizer : FrameSource
   {
      private IntPtr _stabilizerBase;

      private CaptureFrameSource _captureFrameSource;

      /// <summary>
      /// Create a two pass video stabilizer.
      /// </summary>
      /// <param name="capture">The capture object to be stabilized. Should not be a camera stream.</param>
      public TwoPassStabilizer(Capture capture)
      {
         if (capture.CaptureSource == Capture.CaptureModuleType.Camera)
         {
            throw new ArgumentException("Two pass stabilizer cannot process camera stream");
         }

         _captureFrameSource = new CaptureFrameSource(capture);

         _ptr = VideoStabInvoke.TwoPassStabilizerCreate(_captureFrameSource, ref _stabilizerBase, ref _frameSourcePtr);
      }

      /// <summary>
      /// Release the unmanaged memory
      /// </summary>
      protected override void DisposeObject()
      {
         VideoStabInvoke.TwoPassStabilizerRelease(ref _ptr);
         _stabilizerBase = IntPtr.Zero;
         _captureFrameSource.Dispose();
         base.Dispose();
      }
   }
}
