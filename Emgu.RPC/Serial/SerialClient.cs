//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Threading;

namespace Emgu.RPC.Serial
{
    public class SerialClient : SerialServiceCallback, IDisposable
    {
        ISerialService _serialService;
        DuplexChannelFactory<ISerialService> _serialFactory;

        private bool _disposed;

        public SerialClient(System.ServiceModel.Channels.Binding binding, String url)
        {
            _serialFactory = new DuplexChannelFactory<ISerialService>(
                 typeof(SerialServiceCallback),
                 binding);

            _serialService = _serialFactory.CreateChannel(
                new InstanceContext(this),
                new EndpointAddress(url));

            OnDataReceived += 
               delegate
               {
                  if (!_disposed)
                     ThreadPool.QueueUserWorkItem(ReadData, _serialService);
               }; //signal that data has been received
            ThreadPool.QueueUserWorkItem(ReadData, _serialService); 
        }

        private static void ReadData(Object serialService)
        {
            ((ISerialService)serialService).Read();
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
                _serialFactory.Close();
            }
        }

        ~SerialClient()
        {
            Dispose(false);
        }
    }
}
