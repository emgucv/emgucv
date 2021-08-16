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
    /// Calculates a map using a gaussian pyramid
    /// </summary>
    public class MapperPyramid: UnmanagedObject, IMapper
    {
        private IntPtr _mapperPtr;

        /// <summary>
        /// Create a mapper using a gaussian pyramid
        /// </summary>
        /// <param name="baseMapper">The base mapper</param>
        public MapperPyramid(IMapper baseMapper)
        {
            _ptr = RegInvoke.cveMapperPyramidCreate(baseMapper.MapperPtr, ref _mapperPtr);
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
                RegInvoke.cveMapperPyramidRelease(ref _ptr);
                _mapperPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class RegInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMapperPyramidCreate(IntPtr baseMapper, ref IntPtr mapper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapperPyramidRelease(ref IntPtr mapper);
    }
}
