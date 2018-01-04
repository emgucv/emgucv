//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.RPC.Serial
{
    public class SerialServiceCallback : ISerialServiceCallback
    {
        private String _data;

        public String Data
        {
            get { return _data; }
        }

        public virtual void ReceiveData(String dataReceived)
        {
            _data = dataReceived;
            OnDataReceived(this, new EventArgs());
        }

        public event EventHandler OnDataReceived;

    };
}
