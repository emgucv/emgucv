//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.RPC;
using Emgu.RPC.Speech;
using Emgu.RPC.Serial;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Net;

namespace Client
{
   public partial class ClientControl : Form
   {
      private RpcClient _client;

      public ClientControl()
      {
         InitializeComponent();
         _client = null;

         /*
         IPHostEntry ipEntry = Dns.GetHostByName(Dns.GetHostName());
         IPAddress[] addr = ipEntry.AddressList;
         */
      }

      private void button1_Click(object sender, EventArgs e)
      {
         if (_client == null)
         {
            Uri baseUri = new Uri(serviceUrl.Text);
            NetTcpBinding binding = new NetTcpBinding();
            //BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 500000;

            _client = new RpcClient(baseUri, binding);
            _client.AddSpeechClient(":8082/Speech");
            _client.AddCaptureClient(":8084/Capture");
            _client.AddSerialClient(":8083/Serial");

            _client.Capture.onFrameReceived +=
               delegate
               {
                  imageBox1.Image = _client.Capture.CapturedImage;
               };

            _client.Serial.OnDataReceived +=
               delegate
               {
                  SerialBox.Text += _client.Serial.Data;
               };

            button1.Text = "Stop";

         }
         else
         {
            _client.Dispose();
            _client = null;
            button1.Text = "Start";
         }
      }

      private void OnTextChange(object sender, EventArgs e)
      {
         String s = textBox1.Text;

         if (s.Length > 0)
         {
            String last = s.Substring(s.Length - 1, 1);
            if (last.Equals("\n"))
            {
               textBox1.Text = "";
               _client.Speech.Speak(s);
            }
         }
      }

      private void CleanUp()
      {
      }
   }
}