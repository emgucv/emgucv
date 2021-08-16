//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Reg
{
    /// <summary>
    /// Gradient mapper for a translation
    /// </summary>
    public class MapperGradShift : UnmanagedObject, IMapper
    {
        private IntPtr _mapperPtr;

        /// <summary>
        /// Create gradient mapper for a translation
        /// </summary>
        public MapperGradShift()
        {
            _ptr = RegInvoke.cveMapperGradShiftCreate(ref _mapperPtr);
        }

        /// <inheritdoc/> 
        public IntPtr MapperPtr
        {
            get { return _mapperPtr; }
        }

        /// <inheritdoc/> 
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                RegInvoke.cveMapperGradShiftRelease(ref _ptr);
                _mapperPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class RegInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMapperGradShiftCreate(ref IntPtr mapper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapperGradShiftRelease(ref IntPtr mapper);
    }
}
