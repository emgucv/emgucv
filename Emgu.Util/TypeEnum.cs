//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;

namespace Emgu.Util.TypeEnum
{
    /// <summary>
    /// Type of operating system
    /// </summary>
    public enum OS
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// Windows
        /// </summary>
        Windows,
        /// <summary>
        /// Linux
        /// </summary>
        Linux,
        /// <summary>
        /// Mac OS
        /// </summary>
        MacOS,
        /// <summary>
        /// iOS devices. iPhone, iPad, iPod Touch
        /// </summary>
        IOS,
        /// <summary>
        /// Android devices
        /// </summary>
        Android
    }

    /// <summary>
    /// The runtime environment
    /// </summary>
    public enum ClrType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// .Net runtime
        /// </summary>
        DotNet,
        /// <summary>
        /// Windows Store app runtime
        /// </summary>
        NetFxCore,
        /// <summary>
        /// Mono runtime
        /// </summary>
        Mono
    }

    /// <summary>
    /// The type of Programming languages
    /// </summary>
    public enum ProgrammingLanguage
    {
        /// <summary>
        /// C#
        /// </summary>
        CSharp,
        /// <summary>
        /// C++
        /// </summary>
        CPlusPlus
    }
}
