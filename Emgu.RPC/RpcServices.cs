//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Emgu.RPC.Speech;
using Emgu.CV;
using Emgu.RPC.Serial;

namespace Emgu.RPC
{
   public class RpcServices
   {
      private List<ServiceHost> _hosts;
      private System.ServiceModel.Channels.Binding _binding;
      private Uri _baseUri;
      private ServiceHost _speechHost;
      private ServiceHost _serialHost;
      private ServiceHost _cameraHost;

      public void AddSpeechHost(String uri)
      {
         _speechHost = new ServiceHost(typeof(RPC.Speech.SpeechService));
         _speechHost.AddServiceEndpoint(typeof(ISpeechService), _binding, _baseUri.ToString() + uri);
         _hosts.Add(_speechHost);
      }

      public void AddCameraHost(String uri)
      {
         _cameraHost = new ServiceHost(typeof(Capture));
         _cameraHost.AddServiceEndpoint(typeof(IDuplexCapture), _binding, _baseUri.ToString() + uri);
         _hosts.Add(_cameraHost);
      }

      public void AddSerialHost(String uri)
      {
         _serialHost = new ServiceHost(typeof(SerialService));
         _serialHost.AddServiceEndpoint(typeof(ISerialService), _binding, _baseUri.ToString() + uri);
         _hosts.Add(_serialHost);
      }

      public RpcServices(Uri baseUri, System.ServiceModel.Channels.Binding binding)
      {
         _baseUri = baseUri;
         _binding = binding;
         _hosts = new List<ServiceHost>();
      }

      /// <summary>
      /// configurate all the services hosts and open it
      /// </summary>
      public void Open()
      {
         foreach (ServiceHost h in _hosts)
         {
            ServiceBehaviorAttribute serviceBehavior = h.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            serviceBehavior.IncludeExceptionDetailInFaults = true;

            /*
            #region defines the ServiceMetadataBehavior for this host
            ServiceMetadataBehavior metadataBehavior = h.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                h.Description.Behaviors.Add(metadataBehavior);
            }
            metadataBehavior.HttpGetEnabled = true;
            #endregion
            */

            h.Open();
         }
      }

      public void Close()
      {
         foreach (ServiceHost h in _hosts)
            if (h.State != CommunicationState.Closed)
               h.Close();
      }
   }
}
