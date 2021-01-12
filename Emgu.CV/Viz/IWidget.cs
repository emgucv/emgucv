//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
#if !( UNITY_IOS || UNITY_ANDROID )

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV
{

    /// <summary>
    /// Interface for all widgets
    /// </summary>
    public interface IWidget
    {
        /// <summary>
        /// Get the pointer to the widget object
        /// </summary>
        IntPtr GetWidget { get; }
    }

    /// <summary>
    /// Interface for all widget3D
    /// </summary>
    public interface IWidget3D : IWidget
    {
        /// <summary>
        /// Get the pointer to the widget3D object
        /// </summary>
        IntPtr GetWidget3D { get; }
    }

    /// <summary>
    /// Interface for all widget2D
    /// </summary>
    public interface IWidget2D : IWidget
    {
        /// <summary>
        /// Get the pointer to the widget2D object
        /// </summary>
        IntPtr GetWidget2D { get; }
    }

}
#endif