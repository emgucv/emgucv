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
    /// The INode interface represents a node in the Emgu.CV.Dai namespace.
    /// </summary>
    /// <remarks>
    /// This interface is implemented by various classes in the Emgu.CV.Dai namespace, including MonoCamera, ColorCamera, StereoDepth, and NeuralNetwork. 
    /// Each implementing class represents a different type of node, and provides its own implementation of the NodePtr property.
    /// </remarks>
    public interface INode
    {
        /// <summary>
        /// Gets the pointer to the node in the unmanaged memory.
        /// </summary>
        /// <value>
        /// The pointer to the node.
        /// </value>
        IntPtr NodePtr
        {
            get;
        }
    }
}