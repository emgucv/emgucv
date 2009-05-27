using System;
using System.ServiceModel;

namespace Emgu.RPC
{
    ///<summary> The interface that is used for WCF to prvide a serial port service callback</summary>
    [XmlSerializerFormat]
    [ServiceContract]
    public interface ISerialServiceCallback
    {
        ///<summary> notify the cleint that data is received from the serial port </summary>
        [OperationContract(IsOneWay = true)]
        void ReceiveData(String dataReceived);
    }
}
