//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
   /// <summary>
   /// Gaussian motion filter
   /// </summary>
   public class GaussianMotionFilter : UnmanagedObject
   {
      /// <summary>
      /// Create a Gaussian motion filter
      /// </summary>
      /// <param name="radius">The radius, use 15 for default.</param>
      /// <param name="stdev">The standard deviation, use -1.0f for default</param>
      public GaussianMotionFilter(int radius, float stdev)
      {
         _ptr = VideoStabInvoke.GaussianMotionFilterCreate(radius, stdev);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         VideoStabInvoke.GaussianMotionFilterRelease(ref _ptr);
      }
   }
}
