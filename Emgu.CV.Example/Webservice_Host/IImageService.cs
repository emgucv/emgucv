using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using System.ServiceModel;

namespace Webservice_Host
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IImageService
    {
        [OperationContract(IsOneWay=false) ]
        Image<Bgr, Byte> GrabFrame();
    }
}
