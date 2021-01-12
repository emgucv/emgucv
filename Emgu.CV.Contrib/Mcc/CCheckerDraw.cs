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

namespace Emgu.CV.Mcc
{
    /// <summary>
    /// This class contains the functions for drawing a detected chart. 
    /// </summary>
    public partial class CCheckerDraw : SharedPtrObject
    {
        /// <summary>
        /// Create a new CCheckerDraw object.
        /// </summary>
        /// <param name="cchecker">The checker which will be drawn by this object.</param>
        /// <param name="color">The color by with which the squares of the checker will be drawn</param>
        /// <param name="thickness">The thickness with which the squares will be drawn</param>

        public CCheckerDraw(CChecker cchecker, MCvScalar color, int thickness = 2)
        {
            _ptr = MccInvoke.cveCCheckerDrawCreate(cchecker, ref color, thickness, ref _sharedPtr);
        }

        /// <summary>
        /// Draws the checker to the given image.
        /// </summary>
        /// <param name="img">mage in color space BGR</param>
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
            if (_sharedPtr != IntPtr.Zero)
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
