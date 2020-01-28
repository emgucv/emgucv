//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || UNITY_WSA || NETSTANDARD || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
#define HAVE_SERVICE_MODEL
#endif


using System;
#if HAVE_SERVICE_MODEL
using System.ServiceModel;
#endif
using Emgu.CV.Structure;

namespace Emgu.CV
{
    /// <summary> The interface that is used for WCF to provide a image capture service</summary>
#if HAVE_SERVICE_MODEL
   [XmlSerializerFormat]
   [ServiceContract]
#endif
    public interface ICapture
   {
        /// <summary> Capture a Bgr image frame </summary>
        /// <returns> A Bgr image frame</returns>
#if HAVE_SERVICE_MODEL
      [OperationContract]
#endif
        Mat QueryFrame();

        /// <summary> Capture a Bgr image frame that is half width and half height</summary>
        /// <returns> A Bgr image frame that is half width and half height</returns>
#if HAVE_SERVICE_MODEL
      [OperationContract]
#endif
        Mat QuerySmallFrame();
   }
}
