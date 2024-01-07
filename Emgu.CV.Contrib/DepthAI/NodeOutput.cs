//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Dai
{
    public class NodeOutput : UnmanagedObject
    {
        private bool _needDispose;

        internal NodeOutput(IntPtr ptr, bool needDispose=false)
		{
            _ptr = ptr;
            _needDispose = needDispose;
        }

        public void Link(NodeInput input)
        {
            DaiInvoke.daiNodeOutputLink(_ptr, input);
        }

        public String Name
        {
            get
            {
                using (CvString csName = new CvString())
                {
                    DaiInvoke.daiNodeOutputGetName(_ptr, csName);
                    return csName.ToString();
                }
            }
        }

        protected override void DisposeObject()
        {
            
        }
    }

    /// <summary>
    /// Entry points for the DepthAI module.
    /// </summary>
    public static partial class DaiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiNodeOutputLink(IntPtr nodeOutput, IntPtr nodeInput);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiNodeOutputGetName(IntPtr nodeOutput, IntPtr name);
    }

}