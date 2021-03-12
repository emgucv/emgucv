//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
    
    public class BEBLID : Feature2D
    {
        public BEBLID(float scaleFactor, int nBits)
        {
            _ptr = XFeatures2DInvoke.cveBEBLIDCreate(
                scaleFactor,
                nBits,
                ref _feature2D,
                ref _sharedPtr);
        }


        /// <summary>
        /// Release all the unmanaged resource associated with BEBLID
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XFeatures2DInvoke.cveBEBLIDRelease(ref _sharedPtr);
            }
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveBEBLIDCreate(
            float scaleFactor,
            int nBits,
            ref IntPtr beblid,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveBEBLIDRelease(ref IntPtr shared);
    }
}