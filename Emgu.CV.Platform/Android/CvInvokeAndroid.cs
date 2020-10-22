//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
      public static bool CheckLibraryLoaded ()
      {
         return CvInvoke.CheckLibraryLoaded ();
      }
   }
}
