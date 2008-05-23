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
        private volatile bool _started;
        private Webservice_Host.IImageService _imageGrabber;
        private NetTcpBinding _binding;
        private Thread _frameGrabber;

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

                if (_started == false)
                {
                    if (_frameGrabber != null && _frameGrabber.IsAlive)
                        _frameGrabber.Abort();
                }
                else //_started = true
                {
                    _imageGrabber = new ChannelFactory<Webservice_Host.IImageService>(
                        _binding,
                        new EndpointAddress(serviceUrlBox.Text)
                        ).CreateChannel();
                    _frameGrabber = new Thread(
                        delegate()
                        {
                            while (_started)
                            {
                                imageBox1.Image = _imageGrabber.GrabFrame();
                            }
                        });
                    _frameGrabber.Start();
                }
            }
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
