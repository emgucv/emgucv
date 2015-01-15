//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
#if !WINDOWS_PHONE_APP
using System.ServiceModel;
#endif

namespace Emgu.CV
{
   ///<summary>
   ///The interface to request a duplex image capture
   ///</summary>
#if !WINDOWS_PHONE_APP
   [ServiceContract(CallbackContract = typeof(IDuplexCaptureCallback))]
#endif
   public interface IDuplexCapture
   {
      /// <summary>
      /// Request a frame from server
      /// </summary>
#if !WINDOWS_PHONE_APP
      [OperationContract(IsOneWay = true)]
#endif
      void DuplexQueryFrame();

      /// <summary>
      /// Request a frame from server which is half width and half height
      /// </summary>
#if !WINDOWS_PHONE_APP
      [OperationContract(IsOneWay = true)]
#endif
      void DuplexQuerySmallFrame();
   }
}
