//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

namespace Emgu.CV.Platform.Maui
{
    /// <summary>
    /// PInvoke for MAUI
    /// </summary>
    public partial class MauiInvoke
    {
        private static readonly bool _libraryLoaded;

        /// <summary>
        /// Check to make sure all the unmanaged libraries are loaded
        /// </summary>
        /// <returns>True if library loaded</returns>
        public static bool Init()
        {
            return _libraryLoaded;
        }

        static MauiInvoke()
        {
            _libraryLoaded = Emgu.CV.CvInvokeAndroid.Init();
        }
    }
}