//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.ServiceModel;

namespace Emgu.RPC.Serial
{
    ///<summary> The interface that is used for WCF to prvide a serial port service</summary>
    [XmlSerializerFormat]
    [ServiceContract(CallbackContract = typeof(ISerialServiceCallback))]
    public interface ISerialService
    {
        ///<summary> Write the specific data to the serial port </summary>
        [OperationContract(IsOneWay = true)]
        void Write(String data);

        [OperationContract(IsOneWay = true)]
        void Read();
    }
}
