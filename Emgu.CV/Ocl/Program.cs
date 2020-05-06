//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Util;

namespace Emgu.CV.Ocl
{
    /// <summary>
    /// This class contains ocl Program information
    /// </summary>
    public partial class Program : UnmanagedObject
    {
        /// <summary>
        /// Create a empty OclProgram object
        /// </summary>
        public Program()
           : this(OclInvoke.oclProgramCreate())
        {
        }

        internal Program(IntPtr ptr)
        {
            _ptr = ptr;
        }

        
        /// <summary>
        /// Get the program binary
        /// </summary>
        public byte[] Binary
        {
            get
            {
                if (_ptr == IntPtr.Zero)
                    return null;

                using (VectorOfByte vb = new VectorOfByte())
                {
                    OclInvoke.oclProgramGetBinary(_ptr, vb);
                    return vb.ToArray();
                }
            }
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this OclProgram
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                OclInvoke.oclProgramRelease(ref _ptr);
            }
        }

    }

    /// <summary>
    /// Class that contains ocl functions
    /// </summary>
    public static partial class OclInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr oclProgramCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void oclProgramRelease(ref IntPtr oclProgram);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void oclProgramGetBinary(IntPtr program, IntPtr binary);
    }
}
