//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
    public static partial class DnnInvoke
    {
       static DnnInvoke()
       {
          CvInvoke.CheckLibraryLoaded();
          InitModule();
       }

      /// <summary>
      /// Initialize dnn module and built-in layers.
      /// </summary>
      /// <remarks>This function automatically called on most of OpenCV builds</remarks>
      [DllImport(CvInvoke.ExternLibrary, EntryPoint = "cveDnnInitModule", CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void InitModule();
   }
}

#endif