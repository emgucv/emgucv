//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.ServiceModel;

namespace Emgu.RPC
{
   ///<summary> 
   ///The interface that is used for WCF to prvide a serial port service
   ///</summary>
   [XmlSerializerFormat]
   [ServiceContract]
   public interface ISpeechService
   {
      /// <summary>
      /// Speak the specific sentence on the server
      /// </summary>
      /// <param name="sentences">The sentense to be speak</param>
      [OperationContract(IsOneWay = true)]
      void Speak(String sentences);
   }
}
