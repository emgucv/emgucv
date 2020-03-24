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
       WND_PROP_FULLSCREEN   = 0, //!< fullscreen property    (can be WINDOW_NORMAL or WINDOW_FULLSCREEN).
       WND_PROP_AUTOSIZE     = 1, //!< autosize property      (can be WINDOW_NORMAL or WINDOW_AUTOSIZE).
       WND_PROP_ASPECT_RATIO = 2, //!< window's aspect ration (can be set to WINDOW_FREERATIO or WINDOW_KEEPRATIO).
       WND_PROP_OPENGL       = 3, //!< opengl support.
       WND_PROP_VISIBLE      = 4, //!< checks whether the window exists and is visible
       WND_PROP_TOPMOST      = 5  //!< property to toggle normal window being topmost or not
    }
}
