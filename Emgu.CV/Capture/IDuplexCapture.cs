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
    ///The interface to request a duplex image capture
    ///</summary>
    [ServiceContract(CallbackContract = typeof(IDuplexCaptureCallback))]
    public interface IDuplexCapture
    {
        /// <summary>
        /// Request a frame from server
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void DuplexQueryFrame();

        /// <summary>
        /// Request a frame from server which is half width and half height
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void DuplexQuerySmallFrame();
    };
#endif
}
