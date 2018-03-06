//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.IO.Ports;
using System.Threading;
using Emgu.Util;

namespace Emgu.RPC.Serial
{
    /// <summary>
    /// The service for a single serial port
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SerialService : DisposableObject, ISerialService
    {
        private static SerialPort _port;

        public SerialService()
        {
            _port = new SerialPort("COM1");
            _port.BaudRate = 38400;
            _port.Open();   
        }

        public static String[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }
        
        public void Write(String data)
        {
            _port.Write(data);
        }

        public void Read()
        {
            ISerialServiceCallback callback = OperationContext.Current.GetCallbackChannel<ISerialServiceCallback>();
            Thread t = new Thread(delegate()
            {
                String data = string.Empty;
                lock (this) // only one thread at a time is allowed to read the serial port
                {
                    while (data.Equals(string.Empty))
                    {
                        try
                        {
                            data = _port.ReadExisting();
                        }
                        catch 
                        { };
                    }
                }
                callback.ReceiveData(data);
            });
            t.Start();
        }

        protected override void DisposeObject()
        {
            _port.Close();
        }
    }
}