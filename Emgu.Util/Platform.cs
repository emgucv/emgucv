//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using Emgu.Util.TypeEnum;

namespace Emgu.Util
{
    /// <summary>
    /// Provide information for the platform which is using. 
    /// </summary>
    public static class Platform
    {
        private static readonly OS _os;
        private static readonly ClrType _runtime;

        static Platform()
        {
#if UNITY_IPHONE
         _os = OS.IOS;
         _runtime = ClrType.Mono;
#elif UNITY_ANDROID
         _os = OS.Android;
         _runtime = ClrType.Mono;
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _os = OS.Windows;
                if (RuntimeInformation.FrameworkDescription.StartsWith(".NET Native",
                        StringComparison.OrdinalIgnoreCase) ||
                    RuntimeInformation.FrameworkDescription.StartsWith(".NET Core", StringComparison.OrdinalIgnoreCase))
                    _runtime = ClrType.NetFxCore;
                else
                {
                    _runtime = ClrType.DotNet;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _os = OS.Linux;
                _runtime = ClrType.Mono;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                _os = OS.MacOS;
                _runtime = ClrType.Mono;
            }
            else if (Emgu.Util.Toolbox.FindAssembly("Mono.Android.dll") != null)
            {
                _os = OS.Android;
                _runtime = ClrType.Mono;
            }
            else
            {
                _os = OS.Unknown;
                _runtime = ClrType.Unknown;

            }
#endif
        }


        /// <summary>
        /// Get the type of the current operating system
        /// </summary>
        public static OS OperationSystem
        {
            get { return _os; }
        }

        /// <summary>
        /// Get the type of the current runtime environment
        /// </summary>
        public static ClrType ClrType
        {
            get { return _runtime; }
        }

    }
}

