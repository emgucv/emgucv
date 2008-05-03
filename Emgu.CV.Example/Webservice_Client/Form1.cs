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
        private String _messages;
        private DateTime _lastCheckTime;
        private volatile int _framesSinceLastCheckTime;

        public Form1()
        {
            InitializeComponent();

            //_binding = new BasicHttpBinding();

            _binding = new NetTcpBinding();

            _binding.MaxReceivedMessageSize = 500000;
            /*
            _binding.ReceiveTimeout = new TimeSpan(1, 0, 0);
            _binding.SendTimeout = new TimeSpan(1, 0, 0);
            _binding.Security.Mode = SecurityMode.None;
            _binding.TransferMode = TransferMode.Streamed;
            */
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
                    if (_frameGrabber != null)
                        _frameGrabber.Join();
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
                                using (Image<Bgr, Byte> img = _imageGrabber.GrabFrame())
                                {
                                    pictureBox1.Image = img.ToBitmap(pictureBox1.Width, pictureBox1.Height);
                                }

                                TimeSpan ts = DateTime.Now.Subtract(_lastCheckTime);
                                if ( ts.TotalSeconds > 1.0 )
                                {
                                    _lastCheckTime = DateTime.Now;
                                    _messages = String.Format("{0} FPS", _framesSinceLastCheckTime);
                                    this.Invoke((Emgu.Utils.Action)UpdateMessage);
                                    _framesSinceLastCheckTime = 0;
                                }
                                _framesSinceLastCheckTime++;

                            }
                        });
                    _lastCheckTime = DateTime.Now;
                    _frameGrabber.Start();
                }
            }
        }

        private void UpdateMessage()
        {
            StatusTextBox.Text = _messages;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Started = !Started;
            if (Started == true)
                button1.Text = "Stop";
            else
                button1.Text = "Start";
        }

        private void ReleaseManaged()
        {
            Started = false;
        }

        private void ReleaseUnmanaged()
        {
            Started = false;
        }
    }
}
