using System;
using System.Collections.Generic;
using System.Text;
using Emgu.RPC.Serial;
using Emgu.RPC.Speech;
using Emgu.CV;
using System.ServiceModel;

namespace Emgu.RPC
{
    public class RpcClient: IDisposable
    {
        private Uri _baseUri;
        private System.ServiceModel.Channels.Binding _binding;
                private bool _disposed;
        private ISpeechService _speech;
        private CaptureClient _captureClient;
        private SerialClient _serialClient;

        public RpcClient(Uri baseUri, System.ServiceModel.Channels.Binding binding)
        {
            _baseUri = baseUri;
            _binding = binding;
        }

        public ISpeechService Speech
        {
            get
            {
                return _speech;
            }
        }

        public CaptureClient Capture
        {
            get
            {
                return _captureClient;
            }
        }

        public SerialClient Serial
        {
            get
            {
                return _serialClient;
            }
        }

        public void AddSpeechClient(String uri)
        {
            if (_speech == null)
            {
                _speech = new ChannelFactory<ISpeechService>(
                _binding,
                new EndpointAddress(_baseUri.ToString() + uri)
                ).CreateChannel();
            }
        }

        public void AddCaptureClient(String uri)
        {
            if (_captureClient == null)
            {
                _captureClient = new CaptureClient(_binding, _baseUri.ToString() + uri);
                //_captureClient.Update = true;
            }
        }

        public void AddSerialClient(String uri)
        {
            if (_serialClient == null)
            {
                _serialClient = new SerialClient(_binding, _baseUri.ToString() + uri);
            }
        }

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
                if (_serialClient != null) _serialClient.Dispose();
                if (_captureClient != null) _captureClient.Dispose();
                
            }
        }

        ~RpcClient()
        {
            Dispose(false);
        }
    }
}
