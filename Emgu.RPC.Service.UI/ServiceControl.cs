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
using Emgu.RPC;
using System.ServiceModel;
using System.Net;

namespace Emgu.Remote
{
   public partial class ServiceControl : Form
   {
      public ServiceControl()
      {
         InitializeComponent();

         String hostName = Dns.GetHostName();
         IPAddress[] address = Dns.GetHostEntry(hostName).AddressList;
         toolStripStatusLabel1.Text = String.Format("Host: {0}; IP: {1}", hostName, String.Join(";", Array.ConvertAll<IPAddress, string>(address, System.Convert.ToString)));

         #region configure binding
         Uri uri = new Uri("net.tcp://localhost");
         NetTcpBinding binding = new NetTcpBinding();
         //BasicHttpBinding httpbinding = new BasicHttpBinding();
         //binding.TransferMode = TransferMode.Streamed;
         binding.MaxReceivedMessageSize = 500000000;
         //binding.PortSharingEnabled = true;

         #endregion

         RpcServices s = new RpcServices(uri, binding);
         s.AddSpeechHost(":8082/Speech");
         s.AddCameraHost(":8084/Capture");
         s.AddSerialHost(":8083/Serial");
         s.Open();

      }
   }
}