//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      /// Windows
      /// </summary>
      Windows,
      /// <summary>
      /// Linux
      /// </summary>
      Linux, 
      /// <summary>
      /// Mac OSX
      /// </summary>
      MacOSX,
      /// <summary>
      /// iOS devices. iPhone, iPad, iPod Touch
      /// </summary>
      IOS, 
      /// <summary>
      /// Android devices
      /// </summary>
      Android,
      /// <summary>
      /// The windows phone
      /// </summary>
      WindowsPhone
   }

   /// <summary>
   /// The runtime environment
   /// </summary>
   public enum ClrType
   {
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
