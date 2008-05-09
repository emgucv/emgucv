using System;
using System.Collections.Generic;
using System.Text;
#if NET_2_0
#else
using System.ServiceModel;
#endif

namespace Emgu.CV
{
    ///<summary> The interface that is used for WCF to provide a image capture service</summary>
#if NET_2_0
#else
    [XmlSerializerFormat]
    [ServiceContract]
#endif
    public interface ICapture
    {
        ///<summary> Capture a Bgr image frame </summary>
        ///<returns> A Bgr image frame</returns>
#if NET_2_0
#else
        [OperationContract]
#endif
        Image<Bgr, Byte> QueryFrame();

        ///<summary> Capture a Bgr image frame that is half width and half heigh</summary>
        ///<returns> A Bgr image frame that is half width and half height</returns>
#if NET_2_0
#else
        [OperationContract]
#endif
        Image<Bgr, Byte> QuerySmallFrame();
    };
}
