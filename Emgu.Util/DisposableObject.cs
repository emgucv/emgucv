//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.Util
{
   /// <summary>
   /// An abstract class that wrap around a disposable object
   /// </summary>
   public abstract class DisposableObject : IDisposable
   {
      /// <summary> Track whether Dispose has been called. </summary>
      private bool _disposed;

      /// <summary>
      /// The dispose function that implements IDisposable interface
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary> 
      /// Dispose(bool disposing) executes in two distinct scenarios.
      /// If disposing equals true, the method has been called directly
      /// or indirectly by a user's code. Managed and unmanaged resources
      /// can be disposed.
      /// If disposing equals false, the method has been called by the
      /// runtime from inside the finalizer and you should not reference
      /// other objects. Only unmanaged resources can be disposed.
      /// </summary>
      /// <param name="disposing"> If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed. </param>
      private void Dispose(bool disposing)
      {
         // Check to see if Dispose has already been called.
         if (!_disposed)
         {
            _disposed = true;

            // If disposing equals true, release all managed resources as well
            if (disposing)
            {
               ReleaseManagedResources();
            }

            //release unmanaged resource.
            DisposeObject();
         }
      }

      /// <summary>
      /// Release the managed resources. This function will be called during the disposal of the current object.
      /// override ride this function if you need to call the Dispose() function on any managed IDisposable object created by the current object
      /// </summary>
      protected virtual void ReleaseManagedResources()
      {
      }

      /// <summary>
      /// Release the unmanaged resources
      /// </summary>
      protected abstract void DisposeObject();

      /// <summary>
      /// Destructor
      /// </summary>
      ~DisposableObject()
      {
         Dispose(false);
      }
   }
}
