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
    /// Defines a transformation that consists on a projective transformation
    /// </summary>
    public class MapProjec : UnmanagedObject, IMap
    {
        private IntPtr _mapPtr;

        /// <summary>
        /// Create a transformation that consists on a projective transformation
        /// </summary>
        /// <param name="projTr"></param>
        public MapProjec(IInputArray projTr)
        {
            using (InputArray iaProjTr = projTr.GetInputArray())
                _ptr = RegInvoke.cveMapProjecCreate(iaProjTr, ref _mapPtr);
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
                RegInvoke.cveMapProjecRelease(ref _ptr);
                _mapPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class RegInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMapProjecCreate(IntPtr projTr, ref IntPtr map);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapProjecRelease(ref IntPtr mapShift);
    }
}
