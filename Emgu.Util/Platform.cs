//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util.TypeEnum;

namespace Emgu.Util
{
   /// <summary>
   /// Provide information for the platform which is using. 
   /// </summary>
   public static class Platform
   {
      private static readonly OS _os;
      private static readonly Runtime _runtime;

      static Platform()
      {
         int p = (int)Environment.OSVersion.Platform;
         _os = ((p == 4) || (p == 128)) ? OS.Linux : OS.Windows;

         _runtime = (Type.GetType("System.MonoType", false) != null) ? Runtime.Mono : Runtime.DotNet;
      }

      /// <summary>
      /// Get the type of the current operating system
      /// </summary>
      public static OS OperationSystem
      {
         get
         {
            return _os;
         }
      }

      /// <summary>
      /// Get the type of the current runtime environment
      /// </summary>
      public static Runtime Runtime
      {
         get
         {
            return _runtime;
         }
      }
   }
}
