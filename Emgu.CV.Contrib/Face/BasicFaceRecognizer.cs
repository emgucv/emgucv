//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
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
    }
}
