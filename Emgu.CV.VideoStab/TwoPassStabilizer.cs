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
   public class TwoPassStabilizer : FrameSource
   {
      private IntPtr _stabilizerBase;
      private IntPtr _frameSource;

      private CaptureFrameSource _captureFrameSource;

      public TwoPassStabilizer(Capture capture)
      {
         _captureFrameSource = new CaptureFrameSource(capture);
         _ptr = VideoStabInvoke.TwoPassStabilizerCreate(_captureFrameSource, ref _stabilizerBase, ref _frameSource);
      }

      protected override IntPtr GetFrameSourcePointer()
      {
         return _frameSource;
      }

      protected override void DisposeObject()
      {
         VideoStabInvoke.TwoPassStabilizerRelease(ref _ptr);
         _stabilizerBase = IntPtr.Zero;
         _frameSource = IntPtr.Zero;
         _captureFrameSource.Dispose();
         base.Dispose();
      }
   }
}
