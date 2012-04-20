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
   public class OnePassStabilizer : FrameSource
   {
      private IntPtr _stabilizerBase;
      private IntPtr _frameSource;

      private CaptureFrameSource _captureFrameSource;

      public OnePassStabilizer(Capture capture)
      {
         _captureFrameSource = new CaptureFrameSource(capture);
         _ptr = VideoStabInvoke.OnePassStabilizerCreate(_captureFrameSource, ref _stabilizerBase, ref _frameSource);
      }

      public void SetMotionFilter(GaussianMotionFilter motionFilter)
      {
         VideoStabInvoke.OnePassStabilizerSetMotionFilter(_ptr, motionFilter);
      }

      public void SetMotionEstimator(PyrLkRobustMotionEstimator estimator)
      {
         VideoStabInvoke.StabilizerBaseSetMotionEstimator(_stabilizerBase, estimator);
      }

      protected override IntPtr GetFrameSourcePointer()
      {
         return _frameSource;
      }

      protected override void DisposeObject()
      {
         VideoStabInvoke.OnePassStabilizerRelease(ref _ptr);
         _stabilizerBase = IntPtr.Zero;
         _frameSource = IntPtr.Zero;
         _captureFrameSource.Dispose();
         base.Dispose();
      }
   }
}
