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
   ///<summary> The interface that is used for WCF to provide a image capture service</summary>
#if !WINDOWS_PHONE_APP
   [XmlSerializerFormat]
   [ServiceContract]
#endif
   public interface ICapture
   {
      ///<summary> Capture a Bgr image frame </summary>
      ///<returns> A Bgr image frame</returns>
#if !WINDOWS_PHONE_APP
      [OperationContract]
#endif
      Mat QueryFrame();

      ///<summary> Capture a Bgr image frame that is half width and half heigh</summary>
      ///<returns> A Bgr image frame that is half width and half height</returns>
#if !WINDOWS_PHONE_APP
      [OperationContract]
#endif
      Mat QuerySmallFrame();
   }
}
