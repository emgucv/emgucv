//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;

namespace Emgu.CV.Util
{
   /// <summary>
   /// An Unmanaged Object is a disposable object with a Ptr property pointing to the unmanaged object
   /// </summary>
   public abstract class SharedPtrObject : UnmanagedObject
    {
      /// <summary>
      /// A pointer to the shared pointer to the unmanaged object
      /// </summary>
      protected IntPtr _sharedPtr;

      /// <summary>
      /// Pointer to the shared pointer to the unmanaged object
      /// </summary>
      public IntPtr SharedPtr
      {
         get
         {
            return _sharedPtr;
         }
      }
   }
}
