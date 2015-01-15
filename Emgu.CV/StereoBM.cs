//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Class for computing stereo correspondence using the block matching algorithm, introduced and contributed to OpenCV by K. Konolige.
   /// </summary>
   public class StereoBM : UnmanagedObject, IStereoMatcher
   {

      /// <summary>
      /// Create a stereoBM object
      /// </summary>
      /// <param name="blockSize">the linear size of the blocks compared by the algorithm. The size should be odd (as the block is centered at the current pixel). Larger block size implies smoother, though less accurate disparity map. Smaller block size gives more detailed disparity map, but there is higher chance for algorithm to find a wrong correspondence.</param>
      /// <param name="numberOfDisparities">the disparity search range. For each pixel algorithm will find the best disparity from 0 (default minimum disparity) to <paramref name="numberOfDisparities"/>. The search range can then be shifted by changing the minimum disparity.</param>
      public StereoBM(int numberOfDisparities = 0, int blockSize = 21)
      {
         _ptr = StereoMatcherExtensions.CvStereoBMCreate(numberOfDisparities, blockSize);
      }

      /// <summary>
      /// Release the stereo state and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         if(_ptr != IntPtr.Zero)
            StereoMatcherExtensions.CvStereoMatcherRelease(ref _ptr);
      }

      /// <summary>
      /// Pointer to the stereo matcher
      /// </summary>
      public IntPtr StereoMatcherPtr
      {
         get { return _ptr; }
      }
   }
}
