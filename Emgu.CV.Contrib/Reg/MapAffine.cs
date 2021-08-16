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
    /// Defines an affine transformation
    /// </summary>
    public class MapAffine : UnmanagedObject, IMap
    {
        private IntPtr _mapPtr;

        /// <summary>
        /// Constructor providing explicit values
        /// </summary>
        /// <param name="linTr">Linear part of the affine transformation</param>
        /// <param name="shift">Displacement part of the affine transformation</param>
        public MapAffine(IInputArray linTr, IInputArray shift)
        {
            using (InputArray iaLinTr = linTr.GetInputArray())
            using (InputArray iaShift = shift.GetInputArray())
            {
                _ptr = RegInvoke.cveMapAffineCreate(iaLinTr, iaShift, ref _mapPtr);
            }
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
                RegInvoke.cveMapAffineRelease(ref _ptr);
                _mapPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class RegInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMapAffineCreate(IntPtr lineTr, IntPtr shift, ref IntPtr map);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapAffineRelease(ref IntPtr mapAffine);
    }
}
