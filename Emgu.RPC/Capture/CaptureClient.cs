//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.ServiceModel;
using System.Threading;
using Emgu.CV;

namespace Emgu.RPC
{
   /// <summary>
   /// A Camera capture client that use DuplexCaptureCallback to request image from the server
   /// </summary>
   public class CaptureClient : DuplexCaptureCallback, IDisposable
   {
      private IDuplexCapture _capture;
      private DuplexChannelFactory<IDuplexCapture> _captureFactory;
      private volatile bool _disposed;

      /// <summary>
      /// Create a capture client with the specific binding and url
      /// </summary>
      /// <param name="binding">The binding for this client</param>
      /// <param name="url">The url of the server</param>
      public CaptureClient(System.ServiceModel.Channels.Binding binding, string url)
      {
         _captureFactory = new DuplexChannelFactory<IDuplexCapture>(
              typeof(DuplexCaptureCallback),
              binding);

         _capture = _captureFactory.CreateChannel(
             new InstanceContext(this),
             new EndpointAddress(url));

         onFrameReceived +=
            delegate
            {
               if (!_disposed)
                  ThreadPool.QueueUserWorkItem(CaptureImage, _capture);
            }; //signal that data has been received

         ThreadPool.QueueUserWorkItem(CaptureImage, _capture);
      }

      private static void CaptureImage(Object capture)
      {
         ((IDuplexCapture)capture).DuplexQueryFrame();
      }

      /// <summary>
      /// Dispose function
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      ///<summary> 
      /// Release the capture and all the memory associate with it
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
            _captureFactory.Close();
         }
      }

      /// <summary>
      /// Destructor
      /// </summary>
      ~CaptureClient()
      {
         Dispose(false);
      }

   }
}