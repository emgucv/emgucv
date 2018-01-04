//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.RPC.Serial;
using Emgu.RPC.Speech;
using Emgu.CV;
using Emgu.Util;
using System.ServiceModel;

namespace Emgu.RPC
{
   public class RpcClient : DisposableObject
   {
      private Uri _baseUri;
      private System.ServiceModel.Channels.Binding _binding;
      private ISpeechService _speech;
      private CaptureClient _captureClient;
      private SerialClient _serialClient;

      public RpcClient(Uri baseUri, System.ServiceModel.Channels.Binding binding)
      {
         _baseUri = baseUri;
         _binding = binding;
      }

      public ISpeechService Speech
      {
         get
         {
            return _speech;
         }
      }

      public CaptureClient Capture
      {
         get
         {
            return _captureClient;
         }
      }

      public SerialClient Serial
      {
         get
         {
            return _serialClient;
         }
      }

      public void AddSpeechClient(String uri)
      {
         if (_speech == null)
         {
            _speech = new ChannelFactory<ISpeechService>(
            _binding,
            new EndpointAddress(_baseUri.ToString() + uri)
            ).CreateChannel();
         }
      }

      public void AddCaptureClient(String uri)
      {
         if (_captureClient == null)
         {
            _captureClient = new CaptureClient(_binding, _baseUri.ToString() + uri);
         }
      }

      public void AddSerialClient(String uri)
      {
         if (_serialClient == null)
         {
            _serialClient = new SerialClient(_binding, _baseUri.ToString() + uri);
         }
      }

      protected override void DisposeObject()
      {
         if (_serialClient != null) _serialClient.Dispose();
         if (_captureClient != null) _captureClient.Dispose();
      }
   }
}
