//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
      /// <param name="radius">The radius</param>
      /// <param name="stdev">The standard deviation</param>
      public GaussianMotionFilter(int radius=15, float stdev=-1.0f)
      {
         _ptr = VideoStabInvoke.cveGaussianMotionFilterCreate(radius, stdev);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         VideoStabInvoke.cveGaussianMotionFilterRelease(ref _ptr);
      }
   }
}
