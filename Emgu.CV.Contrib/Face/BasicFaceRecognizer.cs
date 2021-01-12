//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Face
{
    /// <summary>
    /// Basic Face Recognizer
    /// </summary>
    public abstract class BasicFaceRecognizer : FaceRecognizer
    {
        /// <summary>
        /// The native pointer to the BasicFaceRecognizer object 
        /// </summary>
        protected IntPtr _basicFaceRecognizerPtr;

        /// <summary>
        /// Release the unmanaged memory associated with this BasicFaceRecognizer
        /// </summary>
        protected override void DisposeObject()
        {
            _basicFaceRecognizerPtr = IntPtr.Zero;
            base.DisposeObject();
        }
    }
}
