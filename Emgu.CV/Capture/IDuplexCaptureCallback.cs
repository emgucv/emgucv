using System;
using System.Collections.Generic;
using System.Text;
#if NET_2_0
#else
using System.ServiceModel;
#endif

namespace Emgu.CV
{
#if NET_2_0
#else
    ///<summary>
    ///The interface for DuplexCaptureCallback
    ///</summary>
    [ServiceContract]
    public interface IDuplexCaptureCallback
    {
        ///<summary>
        ///Function to call when an image is received
        ///</summary>
        ///<param name="img">The image received</param>
        [OperationContract(IsOneWay = true)]
        void ReceiveFrame(Image<Bgr, Byte> img);
    };
#endif
}
