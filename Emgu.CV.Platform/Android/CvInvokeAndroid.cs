//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV
{
    /// <summary>
    /// CvInvoke for Android
    /// </summary>
    public static class CvInvokeAndroid
    {
        /// <summary>
        /// Return true if the class is loaded.
        /// </summary>
        public static bool Init()
        {
            return CvInvoke.Init();
        }
    }
}
