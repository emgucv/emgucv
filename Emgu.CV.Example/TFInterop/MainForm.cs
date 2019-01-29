//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.TF;
using Emgu.TF.Models;
using Emgu.CV;
using System.Diagnostics;

namespace TFInterop
{
    public partial class MainForm : Form
    {
        private Inception inceptionGraph;

        public MainForm()
        {
            InitializeComponent();

            TfInvoke.CheckLibraryLoaded();
            messageLabel.Text = String.Empty;
            cameraButton.Text = "Start Camera";

            DisableUI();

            //Use the following code for the full inception model
            inceptionGraph = new Inception();
            inceptionGraph.OnDownloadProgressChanged += OnDownloadProgressChangedEventHandler;
            inceptionGraph.OnDownloadCompleted += onDownloadCompleted;

            inceptionGraph.Init();
        }

        public void onDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            EnableUI();
            Recognize("space_shuttle.jpg");
        }

        public void DisableUI()
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    openFileButton.Enabled = false;
                    cameraButton.Enabled = false;
                }));
            }
            else
            {
                openFileButton.Enabled = false;
                cameraButton.Enabled = false;
            }
        }

        public void EnableUI()
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    openFileButton.Enabled = true;
                    cameraButton.Enabled = true;
                }));
            }
            else
            {
                openFileButton.Enabled = true;
                cameraButton.Enabled = true;
            }

        }

        public void OnDownloadProgressChangedEventHandler(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            String msg = String.Format("Downloading models, please wait... {0} of {1} bytes ({2}%) downloaded.", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    messageLabel.Text = msg;
             
                }));
            }
            else
            {
                messageLabel.Text = msg;
             
            }
        }

        public void Recognize(Mat m)
        {
            Tensor imageTensor = Emgu.TF.TensorConvert.ReadTensorFromMatBgr(m, 224, 224, 128.0f, 1.0f / 128.0f);

            //Uncomment the following code to use a retrained model to recognize followers, downloaded from the internet
            //Inception inceptionGraph = new Inception(null, new string[] {"optimized_graph.pb", "output_labels.txt"}, "https://github.com/emgucv/models/raw/master/inception_flower_retrain/", "Mul", "final_result");
            //Tensor imageTensor = ImageIO.ReadTensorFromMatBgr(fileName, 299, 299, 128.0f, 1.0f / 128.0f);

            //Uncomment the following code to use a retrained model to recognize followers, if you deployed the models with the application
            //For ".pb" and ".txt" bundled with the application, set the url to null
            //Inception inceptionGraph = new Inception(null, new string[] {"optimized_graph.pb", "output_labels.txt"}, null, "Mul", "final_result");
            //Tensor imageTensor = ImageIO.ReadTensorFromMatBgr(fileName, 299, 299, 128.0f, 1.0f / 128.0f);

            Stopwatch sw = Stopwatch.StartNew();
            float[] probability = inceptionGraph.Recognize(imageTensor);
            sw.Stop();

            String resStr = String.Empty;
            if (probability != null)
            {
                String[] labels = inceptionGraph.Labels;
                float maxVal = 0;
                int maxIdx = 0;
                for (int i = 0; i < probability.Length; i++)
                {
                    if (probability[i] > maxVal)
                    {
                        maxVal = probability[i];
                        maxIdx = i;
                    }
                }
                resStr = String.Format("Object is {0} with {1}% probability. Recognized in {2} milliseconds.", labels[maxIdx], maxVal * 100, sw.ElapsedMilliseconds);
            }

            if (InvokeRequired)
            {
                this.Invoke( (MethodInvoker)  (() =>
                   {
                       messageLabel.Text = resStr;
                       pictureBox.Image = m.Bitmap;
                   }));
            } else
            {
                messageLabel.Text = resStr;
                pictureBox.Image = m.Bitmap;
            }
        }


        public void Recognize(String fileName)
        {
            fileNameTextBox.Text = fileName;
            
            
            using (Mat m = CvInvoke.Imread(fileName))
            {
                Recognize(m);
                
            }
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                String fileName = ofd.FileName;
                Recognize(fileName);
            }
        }

        private VideoCapture _capture = null;

        private void cameraButton_Click(object sender, EventArgs e)
        {
            if (cameraButton.Text.Equals("Start Camera"))
            {
                if (_capture == null)
                {
                    _capture = new VideoCapture(0);
                    if (!_capture.IsOpened)
                    {
                        _capture = null;
                        MessageBox.Show("Failed to open camera");
                        return;
                    }      
                    _capture.ImageGrabbed += _capture_ImageGrabbed;
                    _capture.Start();
                }
                cameraButton.Text = "Stop Camera";   
            } else
            {
                _capture.Stop();
                _capture.Dispose();
                _capture = null;
                cameraButton.Text = "Start Camera";
            }
        }

        private Mat _captureFrame = new Mat();

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            _capture.Retrieve(_captureFrame);
            Recognize(_captureFrame);
        }
    }
}
