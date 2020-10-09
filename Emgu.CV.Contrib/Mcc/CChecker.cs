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
    public partial class CChecker : SharedPtrObject
    {
        /// <summary>
        /// The type of checker
        /// </summary>
        public enum TypeChart
        {
            /// <summary>
            /// Standard Macbeth Chart with 24 squares
            /// </summary>
            MCC24 = 0,
            /// <summary>
            /// DigitalSG with 140 squares
            /// </summary>
            SG140,
            /// <summary>
            /// DKK color chart with 12 squares and 6 rectangle
            /// </summary>
            Vinyl18,   

        };

        private bool _needDispose;

        internal CChecker(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        public CChecker()
        {
            _ptr = MccInvoke.cveCCheckerCreate(ref _sharedPtr);
            _needDispose = true;
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero && _needDispose)
            {
                MccInvoke.cveCCheckerRelease(ref _sharedPtr);
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
        internal static extern IntPtr cveCCheckerCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerRelease(ref IntPtr sharedPtr);

    }
}
