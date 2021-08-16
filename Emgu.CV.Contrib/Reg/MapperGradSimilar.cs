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
    /// Calculates a similarity transformation between two images (scale, rotation, and shift)
    /// </summary>
    public class MapperGradSimilar: UnmanagedObject, IMapper
    {
        private IntPtr _mapperPtr;

        /// <summary>
        /// Create a mapper that calculates a similarity transformation between two images (scale, rotation, and shift)
        /// </summary>
        public MapperGradSimilar()
        {
            _ptr = RegInvoke.cveMapperGradSimilarCreate(ref _mapperPtr);
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
                RegInvoke.cveMapperGradSimilarRelease(ref _ptr);
                _mapperPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class RegInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMapperGradSimilarCreate(ref IntPtr mapper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapperGradSimilarRelease(ref IntPtr mapper);
    }
}
