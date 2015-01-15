//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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

      private FrameSource _baseFrameSource;

      /// <summary>
      /// Create a one pass stabilizer
      /// </summary>
      /// <param name="baseFrameSource">The capture object to be stabalized</param>
      public OnePassStabilizer(FrameSource baseFrameSource)
      {
         _baseFrameSource = baseFrameSource;
         _ptr = VideoStabInvoke.OnePassStabilizerCreate(baseFrameSource.FrameSourcePtr, ref _stabilizerBase, ref FrameSourcePtr);
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
         base.Dispose();
      }
   }
}
