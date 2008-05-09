using System;
using System.Collections.Generic;
using System.Text;
#if NET_2_0
#else
using System.ServiceModel;
#endif
using System.Threading;

namespace Emgu.CV
{
#if NET_2_0
    /*
        Since mono-olive do not support creation of DuplexChannelFactory, this class is disabled
    */
#else

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
            : base()
        {
            _captureFactory = new DuplexChannelFactory<IDuplexCapture>(
                 typeof(DuplexCaptureCallback),
                 binding);

            _capture = _captureFactory.CreateChannel(
                new InstanceContext(this),
                new EndpointAddress(url));

            this.onFrameReceived += new EventHandler(
                delegate {
                    if (!_disposed)
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CaptureImage), _capture); 
                }); //signal that data has been received

            ThreadPool.QueueUserWorkItem(new WaitCallback(CaptureImage), _capture); 
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
#endif
}
