//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.IO;


namespace Emgu.CV
{
    /// <summary>
    /// The equivalent of cv::Moments
    /// </summary>
    public partial class Moments : UnmanagedObject
    {
        /// <summary>
        /// Create an empty Moment object
        /// </summary>
        public Moments()
        {
            _ptr = CvInvoke.cveMomentsCreate();
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveMomentsRelease(ref _ptr);
        }

        /*
        /// <summary>
        /// Calculates seven Hu invariants
        /// </summary>
        /// <param name="hu">The output Hu moments. You can pass a Mat</param>
        public void GetHuMoments(IOutputArray hu)
        {
            using (OutputArray oaHu = hu.GetOutputArray())
                CvInvoke.cveHuMoments(_ptr, oaHu);
        }

        /// <summary>
        /// Calculates seven Hu invariants
        /// </summary>
        public double[] HuMoments
        {
            get
            {
                double[] hu = new double[7];
                GCHandle handle = GCHandle.Alloc(hu, GCHandleType.Pinned);
                CvInvoke.cveHuMoments2(_ptr, handle.AddrOfPinnedObject());
                handle.Free();
                return hu;
            }
        }*/

        /// <summary>
        /// The Gravity Center of this Moment
        /// </summary>
        public MCvPoint2D64f GravityCenter
        {
            get
            {
                return new MCvPoint2D64f(M10 / M00, M01 / M00);
            }
        }
    }

    public static partial class CvInvoke
    {
        

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveMomentsCreate();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMomentsRelease(ref IntPtr moments);

        /*
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHuMoments(IntPtr moments, IntPtr huMoments);
        
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHuMoments2(IntPtr moments, IntPtr hu);
		*/
    }
}

