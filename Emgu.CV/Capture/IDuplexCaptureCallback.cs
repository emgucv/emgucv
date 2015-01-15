//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
#if !WINDOWS_PHONE_APP
using System.ServiceModel;
#endif
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary>
   ///The interface for DuplexCaptureCallback
   ///</summary>
#if !WINDOWS_PHONE_APP
   [ServiceContract]
#endif
   public interface IDuplexCaptureCallback
   {
      ///<summary>
      ///Function to call when an image is received
      ///</summary>
      ///<param name="img">The image received</param>
#if !WINDOWS_PHONE_APP
      [OperationContract(IsOneWay = true)]
#endif
      void ReceiveFrame(Mat img);
   }
}
