//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// This is a variation of
    /// "Stereo Processing by Semiglobal Matching and Mutual Information"
    /// by Heiko Hirschmuller.
    /// We match blocks rather than individual pixels, thus the algorithm is called
    /// SGBM (Semi-global block matching)
    /// </summary>
    public class StereoSGBM : SharedPtrObject, IStereoMatcher
    {
        /// <summary>
        /// The SGBM mode
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// This is the default mode, the algorithm is single-pass, which means that you consider only 5 directions instead of 8
            /// </summary>
            SGBM = 0,
            /// <summary>
            /// Run the full-scale two-pass dynamic programming algorithm. It will consume O(W*H*numDisparities) bytes, which is large for 640x480 stereo and huge for HD-size pictures.
            /// </summary>
            HH = 1
        }

        private IntPtr _stereoMatcherPtr;

        /// <summary>
        /// Create a stereo disparity solver using StereoSGBM algorithm (combination of H. Hirschmuller + K. Konolige approaches) 
        /// </summary>
        /// <param name="minDisparity">Minimum possible disparity value. Normally, it is zero but sometimes rectification algorithms can shift images, so this parameter needs to be adjusted accordingly.</param>
        /// <param name="numDisparities">Maximum disparity minus minimum disparity. The value is always greater than zero. In the current implementation, this parameter must be divisible by 16.</param>
        /// <param name="blockSize">Matched block size. It must be an odd number &gt;=1 . Normally, it should be somewhere in the 3..11 range. Use 0 for default. </param>
        /// <param name="p1">The first parameter controlling the disparity smoothness. It is the penalty on the disparity change by plus or minus 1 between neighbor pixels. Reasonably good value is 8*number_of_image_channels*SADWindowSize*SADWindowSize. Use 0 for default</param>
        /// <param name="p2">The second parameter controlling the disparity smoothness. It is the penalty on the disparity change by more than 1 between neighbor pixels. The algorithm requires <paramref name="p2"/> &gt; <paramref name="p1"/>. Reasonably good value is 32*number_of_image_channels*SADWindowSize*SADWindowSize. Use 0 for default</param>
        /// <param name="disp12MaxDiff">Maximum allowed difference (in integer pixel units) in the left-right disparity check. Set it to a non-positive value to disable the check.</param>
        /// <param name="preFilterCap">Truncation value for the prefiltered image pixels. The algorithm first computes x-derivative at each pixel and clips its value by [-preFilterCap, preFilterCap] interval. The result values are passed to the Birchfield-Tomasi pixel cost function.</param>
        /// <param name="uniquenessRatio">Margin in percentage by which the best (minimum) computed cost function value should “win” the second best value to consider the found match correct. Normally, a value within the 5-15 range is good enough.</param>
        /// <param name="speckleWindowSize">Maximum size of smooth disparity regions to consider their noise speckles and invalidate. Set it to 0 to disable speckle filtering. Otherwise, set it somewhere in the 50-200 range</param>
        /// <param name="speckleRange">Maximum disparity variation within each connected component. If you do speckle filtering, set the parameter to a positive value, it will be implicitly multiplied by 16. Normally, 1 or 2 is good enough.</param>
        /// <param name="mode">Set it to HH to run the full-scale two-pass dynamic programming algorithm. It will consume O(W*H*numDisparities) bytes, which is large for 640x480 stereo and huge for HD-size pictures. By default, it is set to false.</param>
        public StereoSGBM(int minDisparity, int numDisparities, int blockSize,
           int p1 = 0, int p2 = 0, int disp12MaxDiff = 0,
           int preFilterCap = 0, int uniquenessRatio = 0,
           int speckleWindowSize = 0, int speckleRange = 0,
           Mode mode = Mode.SGBM)
        {
            _ptr = CvInvoke.cveStereoSGBMCreate(minDisparity, numDisparities, blockSize, p1, p2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, mode, ref _stereoMatcherPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this stereo solver
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CvInvoke.cveStereoSGBMRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _stereoMatcherPtr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Pointer to the StereoMatcher 
        /// </summary>
        public IntPtr StereoMatcherPtr
        {
            get { return _stereoMatcherPtr; }
        }
    }

}