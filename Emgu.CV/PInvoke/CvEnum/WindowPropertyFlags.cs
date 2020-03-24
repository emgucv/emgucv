//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Flags for SetWindowProperty / GetWindowProperty
    /// </summary>
    [Flags]
    public enum WindowPropertyFlags
    {
        /// <summary>
        /// fullscreen property    (can be WINDOW_NORMAL or WINDOW_FULLSCREEN).
        /// </summary>
        FullScreen = 0,
        /// <summary>
        /// autosize property      (can be WINDOW_NORMAL or WINDOW_AUTOSIZE).
        /// </summary>
        AutoSize = 1,
        /// <summary>
        /// window's aspect ration (can be set to WINDOW_FREERATIO or WINDOW_KEEPRATIO).
        /// </summary>
        AspectRatio = 2,
        /// <summary>
        /// opengl support.
        /// </summary>
        Opengl = 3,
        /// <summary>
        /// checks whether the window exists and is visible
        /// </summary>
        Visible = 4,
        /// <summary>
        /// property to toggle normal window being topmost or not
        /// </summary>
        TopMost = 5 
    }
}
