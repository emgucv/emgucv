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
   /// A one pass video stabilizer
   /// </summary>
   public class OnePassStabilizer : FrameSource
   {
      private IntPtr _stabilizerBase;

      private CaptureFrameSource _captureFrameSource;

      /// <summary>
      /// Create a one pass stabilizer
      /// </summary>
      /// <param name="capture">The capture object to be stabalized</param>
      public OnePassStabilizer(Capture capture)
      {
         _captureFrameSource = new CaptureFrameSource(capture);
         _ptr = VideoStabInvoke.OnePassStabilizerCreate(_captureFrameSource, ref _stabilizerBase, ref _frameSourcePtr);
      }

      /// <summary>
      /// Set the Motion Filter
      /// </summary>
      /// <param name="motionFilter">The motion filter</param>
      public void SetMotionFilter(GaussianMotionFilter motionFilter)
      {
         VideoStabInvoke.OnePassStabilizerSetMotionFilter(_ptr, motionFilter);
      }

      /*
      public void SetMotionEstimator(PyrLkRobustMotionEstimator estimator)
      {
         VideoStabInvoke.StabilizerBaseSetMotionEstimator(_stabilizerBase, estimator);
      }*/

      /// <summary>
      /// Release the unmanaged memory associated with the stabilizer
      /// </summary>
      protected override void DisposeObject()
      {
         VideoStabInvoke.OnePassStabilizerRelease(ref _ptr);
         _stabilizerBase = IntPtr.Zero;
         _captureFrameSource.Dispose();
         base.Dispose();
      }
   }
}
