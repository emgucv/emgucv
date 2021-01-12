//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;
using Emgu.CV;

namespace Webservice_Client
{
   public partial class Form1 : Form
   {
      private bool _started;
      private Webservice_Host.IImageService _imageGrabber;
      private NetTcpBinding _binding;

      public Form1()
      {
         InitializeComponent();

         _binding = new NetTcpBinding();

         _binding.MaxReceivedMessageSize = 500000;

         _started = false;
      }

      public bool Started
      {
         get { return _started; }
         set
         {
            _started = value;
            serviceUrlBox.Enabled = !_started;

            if (!_started)
            {  
               //stop grabing frames
               Application.Idle -= ProcessImage;
            }
            else
            {
               //start to grab frames
               _imageGrabber = new ChannelFactory<Webservice_Host.IImageService>(
                   _binding,
                   new EndpointAddress(serviceUrlBox.Text)
                   ).CreateChannel();

               Application.Idle += ProcessImage;
            }
         }
      }

      private void ProcessImage(object sender, EventArgs e)
      {
         imageBox1.Image = _imageGrabber.GrabFrame();
      }

      private void button1_Click(object sender, EventArgs e)
      {
         Started = !Started;
         if (Started == true)
            button1.Text = "Stop";
         else
            button1.Text = "Start";
      }

      private void ReleaseResource()
      {
         Started = false;
      }
   }
}
