//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Tracking
{

    /// <summary>
    /// Median Flow tracker implementation.
    /// The tracker is suitable for very smooth and predictable movements when object is visible throughout
    /// the whole sequence.It's quite and accurate for this type of problems (in particular, it was shown
    /// by authors to outperform MIL). During the implementation period the code at
    /// http://www.aonsquared.co.uk/node/5, the courtesy of the author Arthur Amarra, was used for the
    /// reference purpose.
    /// </summary>
    public class TrackerMedianFlow : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>Create a median flow tracker</summary>
        /// <param name="pointsInGrid">Points in grid, use 10 for default.</param>
        /// <param name="winSize">Win size, use (3, 3) for default</param>
        /// <param name="maxLevel">Max level, use 5 for default.</param>
        /// <param name="termCriteria">Termination criteria, use count = 20 and eps = 0.3 for default</param>
        /// <param name="winSizeNCC">win size NCC, use (30, 30) for default</param>
        /// <param name="maxMedianLengthOfDisplacementDifference">Max median length of displacement difference</param>
        public TrackerMedianFlow(
            int pointsInGrid, 
            Size winSize, 
            int maxLevel, 
            MCvTermCriteria termCriteria, 
            Size winSizeNCC, 
            double maxMedianLengthOfDisplacementDifference = 10)
        {
            ContribInvoke.cveTrackerMedianFlowCreate(pointsInGrid, ref winSize, maxLevel, ref termCriteria, ref winSizeNCC, maxMedianLengthOfDisplacementDifference, ref _trackerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerMedianFlowRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

}
