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
    /// This class contains the information about the detected checkers,i.e, their type, the corners of the chart, the color profile, the cost, centers chart, etc.
    /// </summary>
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

        /// <summary>
        /// Create a new CChecker
        /// </summary>
        public CChecker()
        {
            _ptr = MccInvoke.cveCCheckerCreate(ref _sharedPtr);
            _needDispose = true;
        }

        /// <summary>
        /// Get or Set the points that forms a box for the CChecker.
        /// </summary>
        public PointF[] Box
        {
            get
            {
                using (VectorOfPointF vp = new VectorOfPointF())
                {
                    MccInvoke.cveCCheckerGetBox(_ptr, vp);
                    return vp.ToArray();
                }
            }
            set
            {
                using (VectorOfPointF vp = new VectorOfPointF(value))
                {
                    MccInvoke.cveCCheckerSetBox(_ptr, vp);
                }
            }
        }

        /// <summary>
        /// Get or Set the center of the CChecker
        /// </summary>
        public PointF Center
        {
            get
            {
                PointF p = new PointF();
                MccInvoke.cveCCheckerGetCenter(_ptr, ref p);
                return p;
            }
            set
            {
                MccInvoke.cveCCheckerSetCenter(_ptr, ref value);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero && _needDispose)
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
        static MccInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCCheckerCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerGetBox(IntPtr checker, IntPtr box);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerSetBox(IntPtr checker, IntPtr box);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerGetCenter(IntPtr checker, ref PointF center);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerSetCenter(IntPtr checker, ref PointF center);

    }
}
