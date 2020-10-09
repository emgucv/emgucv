//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Mcc
{
    public partial class CCheckerDraw : SharedPtrObject
    {

        public CCheckerDraw(CChecker cchecker, MCvScalar color, int thickness)
        {
            _ptr = MccInvoke.cveCCheckerDrawCreate(cchecker, ref color, thickness, ref _sharedPtr);
        }

        public void Draw(IInputOutputArray img)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
            {
                MccInvoke.cveCCheckerDrawDraw(_ptr, ioaImg);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                MccInvoke.cveCCheckerDrawRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }


    /// <summary>
    /// Class that contains entry points for the Mcc module.
    /// </summary>
    public static partial class MccInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCCheckerDrawCreate(
            IntPtr pChecker,
            ref MCvScalar color,
            int thickness,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerDrawDraw(IntPtr ccheckerDraw, IntPtr img);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerDrawRelease(ref IntPtr sharedPtr);

    }
}
