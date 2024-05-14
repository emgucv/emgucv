//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// Represents an input node in the DepthAI module.
    /// </summary>
    public class NodeInput : UnmanagedObject
    {
        private bool _needDispose;

        internal NodeInput(IntPtr ptr, bool needDispose=false)
		{
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Gets the name of the input node in the DepthAI module.
        /// </summary>
        /// <value>
        /// The name of the input node.
        /// </value>
        public String Name
        {
            get
            {
                using (CvString csName = new CvString())
                {
                    DaiInvoke.daiNodeInputGetName(_ptr, csName);
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
        internal static extern void daiNodeInputGetName(IntPtr nodeInput, IntPtr name);
    }
}