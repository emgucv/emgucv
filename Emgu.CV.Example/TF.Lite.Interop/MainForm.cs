using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.TF.Lite;
using Emgu.TF.Lite.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace CVInterop.Lite.Net
{
    public partial class MainForm : Form
    {
        private CocoSsdMobilenetV3 _mobileNet;
        private String _startCameraText = "Start Camera";
        private String _stopCameraText = "Stop Camera";

        public MainForm()
        {
            InitializeComponent();

            TfLiteInvoke.Init();
            messageLabel.Text = String.Empty;
            cameraButton.Text = _startCameraText;

            _mobileNet = new CocoSsdMobilenetV3();
            _mobileNet.OnDownloadProgressChanged += OnDownloadProgressChangedEventHandler;
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

        private bool _coldSession = true;

        private Mat _renderMat;

        //private Tensor _imageTensor;

        public void Recognize(Mat m)
        {
            int[] dim = new int[] { 1, m.Height, m.Width, 3 };

            Tensor inputTensor = _mobileNet.Interpreter.Inputs[0];

            int height = inputTensor.Dims[1];
            int width = inputTensor.Dims[2];
            using (Mat inputTensorMat = new Mat(new Size(width, height), DepthType.Cv8U, 3, inputTensor.DataPointer, 3 * width))
            {
                CvInvoke.Resize(m, inputTensorMat, inputTensorMat.Size);
            }

            if (_coldSession)
            {
                //First run of the recognition graph, here we will compile the graph and initialize the session
                //This is expected to take much longer time than consecutive runs.
                _mobileNet.Interpreter.Invoke();
                _coldSession = false;
            }

            //Here we are trying to time the execution of the graph after it is loaded
            Stopwatch sw = Stopwatch.StartNew();
            _mobileNet.Interpreter.Invoke();
            var results = _mobileNet.GetResults(0.5f);
            sw.Stop();
            int goodResultCount = 0;
            foreach (var r in results)
            {

                float x1 = r.Rectangle[0] * m.Width;
                float y1 = r.Rectangle[1] * m.Height;
                float x2 = r.Rectangle[2] * m.Width;
                float y2 = r.Rectangle[3] * m.Height;

                RectangleF rectf = new RectangleF(x1, y1, x2 - x1, y2 - y1);
                Rectangle rect = Rectangle.Round(rectf);

                rect.Intersect(new Rectangle(Point.Empty, m.Size)); //only keep the region that is inside the image
                if (rect.IsEmpty)
                    continue;

                //draw the rectangle around the region
                CvInvoke.Rectangle(m, rect, new Emgu.CV.Structure.MCvScalar(0, 0, 255), 2);

                //draw the label
                CvInvoke.PutText(m, r.Label, Point.Round(rect.Location), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1.0, new Emgu.CV.Structure.MCvScalar(0, 255, 0), 1);

                goodResultCount++;

            }

            String resStr = String.Format("{0} objects detected in {1} milliseconds.", goodResultCount, sw.ElapsedMilliseconds);

            if (_renderMat == null)
                _renderMat = new Mat();
            m.CopyTo(_renderMat);
            //Bitmap bmp = _renderMat.ToBitmap();

            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    messageLabel.Text = resStr;
                    imageBox.Image = _renderMat;
                }));
            }
            else
            {
                messageLabel.Text = resStr;
                imageBox.Image = _renderMat;
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

        public async Task Init()
        {
            //IDelegate d1 = TfLiteInvoke.DefaultGpuDelegate;
            //IDelegate d2 = TfLiteInvoke.DefaultGpuDelegateV2;
            //await _mobileNet.Init(null, null, "CocoSsdMobilenetV3", TfLiteInvoke.DefaultGpuDelegateV2);

            await _mobileNet.Init();
        }

        private async void openFileButton_Click(object sender, EventArgs e)
        {
            await Init();
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

        private async void cameraButton_Click(object sender, EventArgs e)
        {
            if (cameraButton.Text.Equals(_startCameraText))
            {
                await Init();
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
                cameraButton.Text = _stopCameraText;
            }
            else
            {
                if (_capture != null)
                {
                    _capture.Stop();
                    _capture.Dispose();
                }

                _capture = null;
                cameraButton.Text = _startCameraText;
            }
        }

        private Mat _captureFrame = new Mat();

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            _capture.Retrieve(_captureFrame);
            Recognize(_captureFrame);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cameraButton.Text.Equals(_stopCameraText))
            {
                //stop the camera if we are in a recording session.
                cameraButton_Click(this, null);
            }
        }

    }
}
