//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.ServiceModel;
using Emgu.CV.Structure;

namespace Emgu.CV
{
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
   }
}
