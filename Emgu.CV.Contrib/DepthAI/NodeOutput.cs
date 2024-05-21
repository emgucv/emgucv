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
    /// <summary>
    /// Represents the output of a node in a pipeline. This class is derived from the <see cref="Emgu.Util.UnmanagedObject"/> class.
    /// </summary>
    public class NodeOutput : UnmanagedObject
    {
        private bool _needDispose;

        internal NodeOutput(IntPtr ptr, bool needDispose=false)
		{
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Links this NodeOutput to a NodeInput.
        /// </summary>
        /// <param name="input">The NodeInput to link this NodeOutput to.</param>
        public void Link(NodeInput input)
        {
            DaiInvoke.daiNodeOutputLink(_ptr, input);
        }

        /// <summary>
        /// Gets the name of the NodeOutput.
        /// </summary>
        /// <value>
        /// The name of the NodeOutput.
        /// </value>
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

        /// <summary>
        /// Disposes the unmanaged resources used by the NodeOutput and optionally disposes the managed resources.
        /// </summary>
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