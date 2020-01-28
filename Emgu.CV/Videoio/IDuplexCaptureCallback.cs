//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || UNITY_WSA || NETSTANDARD || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
using System.ServiceModel;
#endif
using Emgu.CV.Structure;

namespace Emgu.CV
{
    ///<summary>
    ///The interface for DuplexCaptureCallback
    ///</summary>
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || UNITY_WSA || NETSTANDARD || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
   [ServiceContract]
#endif
    public interface IDuplexCaptureCallback
   {
        ///<summary>
        ///Function to call when an image is received
        ///</summary>
        ///<param name="img">The image received</param>
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || UNITY_WSA || NETSTANDARD || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
      [OperationContract(IsOneWay = true)]
#endif
        void ReceiveFrame(Mat img);
   }
}
