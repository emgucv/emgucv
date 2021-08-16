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
    /// Base class for modelling a Map between two images.
    /// </summary>
    public class Map : SharedPtrObject, IMap
    {
        internal Map(IntPtr ptr, IntPtr sharedPtr)
        {
            _ptr = ptr;
            _sharedPtr = sharedPtr;
        }

        /// <inheritdoc/> 
        public IntPtr MapPtr
        {
            get { return _ptr; }
        }

        /// <inheritdoc/> 
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                RegInvoke.cveMapRelease(ref _sharedPtr);
            }
        }
    }

    public static partial class RegInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMapRelease(ref IntPtr mapSharedPtr);
    }
}
