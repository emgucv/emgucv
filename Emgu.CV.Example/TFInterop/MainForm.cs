//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using System.Linq;
using System.Runtime.InteropServices;
using Tensorflow;

namespace TFInterop
{
    public partial class MainForm : Form
    {
        private MaskRcnnInceptionV2Coco _inceptionGraph;
        private String _startCameraText = "Start Camera";
        private String _stopCameraText = "Stop Camera";
        public MainForm()
        {
            InitializeComponent();

            TfInvoke.CheckLibraryLoaded();
            messageLabel.Text = String.Empty;
            cameraButton.Text = _startCameraText;

            //DisableUI();

            SessionOptions so = new SessionOptions();
            if (TfInvoke.IsGoogleCudaEnabled)
            {
                Tensorflow.ConfigProto config = new Tensorflow.ConfigProto();
                config.GpuOptions = new Tensorflow.GPUOptions();
                config.GpuOptions.AllowGrowth = true;
                so.SetConfig(config.ToProtobuf());
            }
            _inceptionGraph = new MaskRcnnInceptionV2Coco(null, so );

            _inceptionGraph.OnDownloadProgressChanged += OnDownloadProgressChangedEventHandler;

            //_inceptionGraph.Init();
        }

        /*
        public void onDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            EnableUI();

            // Image file from
            // https://github.com/opencv/opencv_extra/blob/master/testdata/dnn/dog416.png
            Recognize("dog416.png");

        }*/

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

        private bool _coldSession = true;

        private Mat _renderMat;

        private Tensor _imageTensor;

        public void Recognize(Mat m)
        {
            int[] dim = new int[] {1, m.Height, m.Width, 3};
            if (_imageTensor == null)
            {
                _imageTensor = new Tensor(Emgu.TF.DataType.Uint8, dim);
            }
            else
            {
                if (!(_imageTensor.Type == Emgu.TF.DataType.Uint8 && Enumerable.SequenceEqual(dim, _imageTensor.Dim)))
                {
                    _imageTensor.Dispose();
                    _imageTensor = new Tensor(Emgu.TF.DataType.Uint8, dim);
                }
            }

            Emgu.TF.TensorConvert.ReadTensorFromMatBgr(m, _imageTensor);

            MaskRcnnInceptionV2Coco.RecognitionResult[] results;
            if (_coldSession)
            {
                //First run of the recognition graph, here we will compile the graph and initialize the session
                //This is expected to take much longer time than consecutive runs.
                results = _inceptionGraph.Recognize(_imageTensor);
                _coldSession = false;
            }

            //Here we are trying to time the execution of the graph after it is loaded
            Stopwatch sw = Stopwatch.StartNew();
            results = _inceptionGraph.Recognize(_imageTensor);
            sw.Stop();
            int goodResultCount = 0;
            foreach (var r in results)
            {
                if (r.Probability > 0.5)
                {
                    float x1 = r.Region[0] * m.Height;
                    float y1 = r.Region[1] * m.Width;
                    float x2 = r.Region[2] * m.Height;
                    float y2 = r.Region[3] * m.Width;
                    RectangleF rectf = new RectangleF(y1, x1, y2 - y1, x2 - x1);
                    Rectangle rect = Rectangle.Round(rectf);

                    rect.Intersect(new Rectangle(Point.Empty, m.Size)); //only keep the region that is inside the image
                    if (rect.IsEmpty)
                        continue;

                    //draw the rectangle around the region
                    CvInvoke.Rectangle(m, rect, new Emgu.CV.Structure.MCvScalar(0, 0, 255), 2);

                    #region draw the mask
                    float[,] mask = r.Mask;
                    GCHandle handle = GCHandle.Alloc(mask, GCHandleType.Pinned);
                    using (Mat mk = new Mat(new Size(mask.GetLength(1), mask.GetLength(0)), Emgu.CV.CvEnum.DepthType.Cv32F, 1, handle.AddrOfPinnedObject(), mask.GetLength(1) * sizeof(float)))
                    using (Mat subRegion = new Mat(m, rect))
                    using (Mat maskLarge = new Mat())
                    using (Mat maskLargeInv = new Mat())
                    using (Mat largeColor = new Mat(subRegion.Size, Emgu.CV.CvEnum.DepthType.Cv8U, 3))
                    {
                        CvInvoke.Resize(mk, maskLarge, subRegion.Size);

                        //give the mask at least 30% transparency
                        using (ScalarArray sa = new ScalarArray(0.7))
                            CvInvoke.Min(sa, maskLarge, maskLarge);

                        //Create the inverse mask for the original image
                        using (ScalarArray sa = new ScalarArray(1.0))
                            CvInvoke.Subtract(sa, maskLarge, maskLargeInv);

                        //The mask color
                        largeColor.SetTo(new Emgu.CV.Structure.MCvScalar(255, 0, 0));

                        CvInvoke.BlendLinear(largeColor, subRegion, maskLarge, maskLargeInv, subRegion);
                        
                    }
                    handle.Free();
                    #endregion

                    //draw the label
                    CvInvoke.PutText(m, r.Label, Point.Round(rect.Location), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1.0, new Emgu.CV.Structure.MCvScalar(0, 255, 0), 1);

                    goodResultCount++;
                }
            }
            
            String resStr = String.Format("{0} objects detected in {1} milliseconds.", goodResultCount, sw.ElapsedMilliseconds);

            if (_renderMat == null)
                _renderMat = new Mat();
            m.CopyTo(_renderMat);
            //Bitmap bmp = _renderMat.ToBitmap();

            if (InvokeRequired)
            {
                this.Invoke( (MethodInvoker)  (() =>
                   {
                       messageLabel.Text = resStr;
                       pictureBox.Image = _renderMat;
                   }));
            } else
            {
                messageLabel.Text = resStr;
                pictureBox.Image = _renderMat;
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
            await _inceptionGraph.Init();
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
            } else
            {
                _capture.Stop();
                _capture.Dispose();
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
