//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ServiceModel;

namespace Webservice_Host
{
   [ServiceContract]
   [XmlSerializerFormat]
   public interface IImageService
   {
      [OperationContract(IsOneWay = false)]
      Image<Bgr, Byte> GrabFrame();
   }
}
