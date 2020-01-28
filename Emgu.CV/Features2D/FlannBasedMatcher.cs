//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Flann;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
    /// <summary>
    /// This matcher trains flann::Index_ on a train descriptor collection and calls its nearest search methods to find the best matches. So, this matcher may be faster when matching a large train collection than the brute force matcher. 
    /// </summary>
    public class FlannBasedMatcher : DescriptorMatcher
    {
        /// <summary>
        /// Create a Flann based matcher.
        /// </summary>
        /// <param name="indexParams">The type of index parameters</param>
        /// <param name="search">The search parameters</param>
        public FlannBasedMatcher(IIndexParams indexParams, SearchParams search)
        {
            _ptr = Features2DInvoke.cveFlannBasedMatcherCreate(indexParams.IndexParamPtr, search.Ptr, ref _descriptorMatcherPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Flann based matcher.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                Features2DInvoke.cveFlannBasedMatcherRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    public static partial class Features2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFlannBasedMatcherCreate(
           IntPtr ip, IntPtr sp, ref IntPtr dmPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFlannBasedMatcherRelease(ref IntPtr matcher);
    }
}
