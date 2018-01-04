//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || NETSTANDARD1_4 || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
using System.ServiceModel;
#endif

namespace Emgu.CV
{
    ///<summary>
    ///The interface to request a duplex image capture
    ///</summary>
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || NETSTANDARD1_4 || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
   [ServiceContract(CallbackContract = typeof(IDuplexCaptureCallback))]
#endif
    public interface IDuplexCapture
   {
        /// <summary>
        /// Request a frame from server
        /// </summary>
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || NETSTANDARD1_4 || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
      [OperationContract(IsOneWay = true)]
#endif
        void DuplexQueryFrame();

        /// <summary>
        /// Request a frame from server which is half width and half height
        /// </summary>
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || NETSTANDARD1_4 || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
      [OperationContract(IsOneWay = true)]
#endif
        void DuplexQuerySmallFrame();
   }
}
