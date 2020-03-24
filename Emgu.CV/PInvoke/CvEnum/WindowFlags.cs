//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The named window type
    /// </summary>
    [Flags]
    public enum WindowFlags
    {
        /// <summary>
        /// The user can resize the window (no constraint) / also use to switch a fullscreen window to a normal size
        /// </summary>
        Normal = 0x00000000,
        /// <summary>
        /// The user cannot resize the window, the size is constrainted by the image displayed
        /// </summary>
        AutoSize = 0x00000001,
        /// <summary>
        /// Window with opengl support
        /// </summary>
        Opengl = 0x00001000,
        /// <summary>
        /// Change the window to fullscreen
        /// </summary>
        Fullscreen = 1,
        /// <summary>
        /// The image expends as much as it can (no ratio constraint)
        /// </summary>
        FreeRatio = 0x00000100,
        /// <summary>
        /// the ratio of the image is respected
        /// </summary>
        KeepRatio = 0x00000000,
        /// <summary>
        /// status bar and tool bar
        /// </summary>
        GuiExpanded = 0x00000000,
        /// <summary>
        /// Old fashion way
        /// </summary>
        GuiNormal = 0x00000010,
    }
}
