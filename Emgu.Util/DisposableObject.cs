using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.Util
{
   /// <summary>
   /// An abstract class that wrap around a disposable object
   /// </summary>
   public abstract class DisposableObject : IDisposable
   {
      ///<summary> Track whether Dispose has been called. </summary>
      private bool _disposed;

      /// <summary>
      /// The dispose function that implements IDisposable interface
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      ///<summary> 
      /// Release the all the memory associate with this object
      ///</summary>
      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            // Free other state (managed objects).
         }
         // Free your own state (unmanaged objects).
         if (!_disposed)
         {
            _disposed = true;
            DisposeObject();
         }
      }

      /// <summary>
      /// Function use to release _ptr object
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
