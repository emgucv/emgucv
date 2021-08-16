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
    /// Defines a transformation that consists on a simple displacement
    /// </summary>
    public class MapShift : UnmanagedObject, IMap
    {
        private IntPtr _mapPtr;

        /// <summary>
        /// Create a transformation that consists on a simple displacement
        /// </summary>
        /// <param name="shift">A transformation.</param>
        public MapShift(MCvPoint2D64f shift)
        {
            _ptr = RegInvoke.cveMapShiftCreate(ref shift, ref _mapPtr);
        }

        /// <inheritdoc/> 
        public IntPtr MapPtr
        {
            get { return _mapPtr; }
        }

        /// <inheritdoc/> 
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                RegInvoke.cveMapShiftRelease(ref _ptr);
                _mapPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class RegInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMapShiftCreate(ref MCvPoint2D64f shift, ref IntPtr map);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapShiftRelease(ref IntPtr mapShift);
    }
}
