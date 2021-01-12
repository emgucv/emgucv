//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Ocl
{
    /// <summary>
    /// This class contains ocl context information
    /// </summary>
    public partial class Context : UnmanagedObject
    {
        private static Context _defaultContext = new Context(OclInvoke.oclContextGetDefault(), false);

        private bool _needDispose;

        /// <summary>
        /// Create a empty OclContext object
        /// </summary>
        public Context()
           : this(OclInvoke.oclContextCreate(), true)
        {
        }

        /// <summary>
        /// Get the default OclContext. Do not dispose this context.
        /// </summary>
        public static Context Default
        {
            get
            {
                return _defaultContext;
            }
        }

        internal Context(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this OclContext
        /// </summary>
        protected override void DisposeObject()
        {
            if (_needDispose)
            {
                if (_ptr != IntPtr.Zero)
                {
                    OclInvoke.oclContextRelease(ref _ptr);
                }
            }
        }

        /// <summary>
        /// Compile the program 
        /// </summary>
        /// <param name="prog">The program source</param>
        /// <param name="buildOpt">The build option</param>
        /// <param name="errMsg">Error message</param>
        /// <returns>The compiled program</returns>
        public Program GetProgram(ProgramSource prog, String buildOpt, CvString errMsg)
        {
            using (CvString csBuildOpt = new CvString(buildOpt))
            {
                return new Program(OclInvoke.oclContextGetProg(_ptr, prog, csBuildOpt, errMsg));
            }
        }

    }

    /// <summary>
    /// Class that contains ocl functions
    /// </summary>
    public static partial class OclInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr oclContextCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr oclContextGetDefault();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void oclContextRelease(ref IntPtr oclContext);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr oclContextGetProg(
            IntPtr context,
            IntPtr prog,
            IntPtr buildopt,
            IntPtr errmsg);

    }
}
