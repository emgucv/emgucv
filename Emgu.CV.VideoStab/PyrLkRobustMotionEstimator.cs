//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.VideoStab
{
   public class PyrLkRobustMotionEstimator : UnmanagedObject
   {
      public PyrLkRobustMotionEstimator(MotionModel motionModel)
      {
         VideoStabInvoke.PyrLkRobustMotionEstimatorCreate(motionModel);
      }

      public void SetDetector(IKeyPointDetector detector)
      {
         VideoStabInvoke.PyrLkRobustMotionEstimatorSetDetector(_ptr, detector.FeatureDetectorPtr);
      }

      protected override void DisposeObject()
      {
         VideoStabInvoke.PyrLkRobustMotionEstimatorRelease(ref _ptr);
      }
   }
}
