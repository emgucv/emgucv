//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public enum SolverType
    {
        /// <summary>
        /// Apply standard iterative refinement
        /// </summary>
        Standard = 0,
        /// <summary>
        /// Apply optimized iterative refinement based bilinear equation solutions
        /// </summary>
        Bilinear = 1
    }

    public partial class RLOFOpticalFlowParameter : UnmanagedObject
    {
        
        public RLOFOpticalFlowParameter()
        {
            _ptr = CvInvoke.cveRLOFOpticalFlowParameterCreate();
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveRLOFOpticalFlowParameterRelease(ref _ptr);
            }
        }

    }

    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRLOFOpticalFlowParameterCreate();
		
		[DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRLOFOpticalFlowParameterRelease(ref IntPtr p);
    }
}
