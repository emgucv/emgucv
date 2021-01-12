//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Xamarin.Forms;
using Emgu.CV.Dnn;
using Emgu.CV.Util;
using Emgu.Models;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace Emgu.CV.XamarinForms
{
    public class MaskRcnnPage
#if __ANDROID__
        : AndroidCameraPage
#else
        : ButtonTextImagePage
#endif
    {
        //private String _modelFolderName = "dnn_data";
        //private String _path = null;
        private Net _maskRcnnDetector = null;
        private string[] _labels = null;
        private MCvScalar[] _colors = null;
        private String[] _objectsOfInterest = null;
        private String _defaultImage = "dog416.png";

        public String[] ObjectsOfInterest
        {
            get { return _objectsOfInterest; }
            set { _objectsOfInterest = value; }
        }

        public String DefaultImage
        {
            get { return _defaultImage; }
            set { _defaultImage = value; }
        }


        /// <summary>
        /// Initiate the DNN model. If needed, it will download the model from internet.
        /// </summary>
        private async Task InitDetector()
        {
            if (_maskRcnnDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();
                manager.AddFile("https://github.com/emgucv/models/raw/master/mask_rcnn_inception_v2_coco_2018_01_28/frozen_inference_graph.pb",
                    "mask_rcnn_inception_v2_coco_2018_01_28");
                manager.AddFile("https://github.com/emgucv/models/raw/master/mask_rcnn_inception_v2_coco_2018_01_28/coco-labels-paper.txt",
                    "mask_rcnn_inception_v2_coco_2018_01_28");
                manager.AddFile("https://github.com/opencv/opencv_extra/raw/4.1.0/testdata/dnn/mask_rcnn_inception_v2_coco_2018_01_28.pbtxt",
                    "mask_rcnn_inception_v2_coco_2018_01_28");

                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;

                await manager.Download();
                _maskRcnnDetector = Emgu.CV.Dnn.DnnInvoke.ReadNetFromTensorflow(manager.Files[0].LocalFile, manager.Files[2].LocalFile);

                //prefer cuda backend if available
                
                foreach (BackendTargetPair p in DnnInvoke.AvailableBackends)
                {
                    if (p.Backend == Dnn.Backend.Cuda && p.Target == Target.Cuda)
                    {
                        _maskRcnnDetector.SetPreferableBackend(Dnn.Backend.Cuda);
                        _maskRcnnDetector.SetPreferableTarget(Target.Cuda);
                        break;
                    }
                }

                //_maskRcnnDetector.SetPreferableBackend(Dnn.Backend.OpenCV);
                //_maskRcnnDetector.SetPreferableTarget(Dnn.Target.Cpu);

                _labels = File.ReadAllLines(manager.Files[1].LocalFile);
                _colors = new MCvScalar[_labels.Length];
                Random r = new Random(12345);
                for (int i = 0; i < _colors.Length; i++)
                {
                    _colors[i] = new MCvScalar(r.Next(256), r.Next(256), r.Next(256));
                }
            }
        }

        /*
        public async Task ProcessImageAsync(Mat m)
        {
            await Task.Run(() => { ProcessImage(m); });
        }*/

        public void ProcessImage(Mat m, float matchScoreThreshold = 0.5f, float nmsThreshold = 0.4f)
        {
            using (Mat blob = DnnInvoke.BlobFromImage(m))
            using (VectorOfMat tensors = new VectorOfMat())
            {
                _maskRcnnDetector.SetInput(blob, "image_tensor");
                _maskRcnnDetector.Forward(tensors, new string[] { "detection_out_final", "detection_masks" });

                using (Mat boxes = tensors[0])
                using (Mat masks = tensors[1])
                {
                    System.Drawing.Size imgSize = m.Size;
                    float[,,,] boxesData = boxes.GetData(true) as float[,,,];
                    int numDetections = boxesData.GetLength(2);

                    List<int> classIds = new List<int>();
                    List<Rectangle> regions = new List<Rectangle>();
                    List<float> scores = new List<float>();

                    for (int i = 0; i < numDetections; i++)
                    {
                        int classId = (int)boxesData[0, 0, i, 1];

                        if (_objectsOfInterest == null || _objectsOfInterest.Contains(_labels[classId]))
                        {
                            float score = boxesData[0, 0, i, 2];
                            float left = boxesData[0, 0, i, 3] * imgSize.Width;
                            float top = boxesData[0, 0, i, 4] * imgSize.Height;
                            float right = boxesData[0, 0, i, 5] * imgSize.Width;
                            float bottom = boxesData[0, 0, i, 6] * imgSize.Height;

                            RectangleF rectF = new RectangleF(left, top, right - left, bottom - top);
                            Rectangle rect = Rectangle.Round(rectF);
                            rect.Intersect(new Rectangle(Point.Empty, imgSize));

                            regions.Add(rect);
                            scores.Add(score);
                            classIds.Add(classId);
                        }

                    }
                    int[] validIdx = DnnInvoke.NMSBoxes(regions.ToArray(), scores.ToArray(), matchScoreThreshold, nmsThreshold);


                    for (int i = 0; i < validIdx.Length; i++)
                    {
                        int idx = validIdx[i];
                        int classId = classIds[idx];
                        Rectangle rect = regions[idx];
                        float score = scores[idx];

                        MCvScalar color = _colors[classId];
                        CvInvoke.Rectangle(m, rect, new MCvScalar(0, 0, 0, 0), 1);
                        CvInvoke.PutText(m, String.Format("{0} ({1})", _labels[classId], score), rect.Location, FontFace.HersheyComplex, 1.0,
                            new MCvScalar(0, 0, 255), 2);

                        int[] masksDim = masks.SizeOfDimension;
                        using (Mat mask = new Mat(
                            masksDim[2],
                            masksDim[3],
                            DepthType.Cv32F,
                            1,
                            masks.GetDataPointer(i, classId),
                            masksDim[3] * masks.ElementSize))
                        using (Mat maskLarge = new Mat())
                        using (Mat maskLargeInv = new Mat())
                        using (Mat subRegion = new Mat(m, rect))
                        using (Mat largeColor = new Mat(subRegion.Size, Emgu.CV.CvEnum.DepthType.Cv8U,
                            3))
                        {
                            CvInvoke.Resize(mask, maskLarge, rect.Size);

                            //give the mask at least 30% transparency
                            using (ScalarArray sa = new ScalarArray(0.7))
                                CvInvoke.Min(sa, maskLarge, maskLarge);

                            //Create the inverse mask for the original image
                            using (ScalarArray sa = new ScalarArray(1.0))
                                CvInvoke.Subtract(sa, maskLarge, maskLargeInv);

                            //The mask color
                            largeColor.SetTo(color);
                            if (subRegion.NumberOfChannels == 4)
                            {
                                using (Mat bgrSubRegion = new Mat())
                                {
                                    CvInvoke.CvtColor(subRegion, bgrSubRegion,
                                        ColorConversion.Bgra2Bgr);
                                    CvInvoke.BlendLinear(largeColor, bgrSubRegion, maskLarge,
                                        maskLargeInv, bgrSubRegion);
                                    CvInvoke.CvtColor(bgrSubRegion, subRegion,
                                        ColorConversion.Bgr2Bgra);
                                }

                            }
                            else
                                CvInvoke.BlendLinear(largeColor, subRegion, maskLarge, maskLargeInv,
                                    subRegion);
                        }
                    }
                }
            }
        }

        private VideoCapture _capture = null;
        private Mat _mat = null;
        private String _defaultButtonText = "Perform Mask-rcnn Detection";

#if __ANDROID__
        private String _StopCameraButtonText = "Stop Camera";
        private bool _isBusy = false;
#endif

        public MaskRcnnPage()
         : base()
        {
#if __ANDROID__
            HasCameraOption = true;
#endif

            var button = this.GetButton();
            button.Text = _defaultButtonText;

            button.Clicked += OnButtonClicked;

            BackendTargetPair[] availableBackends = Emgu.CV.Dnn.DnnInvoke.AvailableBackends;

            StringBuilder availableBackendsStr = new StringBuilder("Available backends: " + System.Environment.NewLine);
            foreach (BackendTargetPair p in availableBackends)
            {
                availableBackendsStr.Append(
                    String.Format(
                        "Backend: {0}, Target: {1}{2}",
                        p.Backend, p.Target,
                        System.Environment.NewLine));
            }
            SetMessage(availableBackendsStr.ToString());

        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            _capture.Retrieve(_mat);
            Stopwatch watch = Stopwatch.StartNew();
            ProcessImage(_mat);
            watch.Stop();
            SetImage(_mat);
            //this.DisplayImage.BackgroundColor = Color.Black;
            this.DisplayImage.IsEnabled = true;
            SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
        }

        private async void OnButtonClicked(Object sender, EventArgs args)
        {
#if __ANDROID__
            var button = GetButton();
            if (button.Text.Equals(_StopCameraButtonText))
            {
                StopCapture();
                button.Text = _defaultButtonText;
                //AndroidImageView.Visibility = ViewStates.Invisible;
                return;
            }
#endif
            Mat[] images = await LoadImages(new string[] { _defaultImage });

            if (images == null || (images.Length > 0 && images[0] == null))
                return;

            if (images.Length == 0)
            {
                //Use Camera
                await InitDetector();

#if __ANDROID__
                button.Text = _StopCameraButtonText;
                //AndroidImageView.Visibility = ViewStates.Visible;
                StartCapture(async delegate (Object captureSender, Mat m)
                {
                    //Skip the frame if busy, 
                    //Otherwise too many frames arriving and will eventually saturated the memory.
                    if (!_isBusy)
                    {
                        _isBusy = true;
                        try
                        {
                            Stopwatch watch = Stopwatch.StartNew();
                            await Task.Run(() => { ProcessImage(m); });
                            watch.Stop();
                            SetImage(m);
                            SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
                        }
                        finally
                        {
                            _isBusy = false;
                        }
                    }
                });
#else
                    //Handle video
                    if (_capture == null)
                    {
                        _capture = new VideoCapture();
                        _capture.ImageGrabbed += _capture_ImageGrabbed;
                    }
                    _capture.Start();
#endif
            }
            else
            {
                SetMessage("Please wait...");
                SetImage(null);

                await InitDetector();
                Stopwatch watch = Stopwatch.StartNew();
                await Task.Run(() => ProcessImage(images[0]));
                watch.Stop();
                String msg = String.Format("Mask RCNN inception completed in {0} milliseconds",
                    watch.ElapsedMilliseconds);
                SetImage(images[0]);
                SetMessage(msg);
            }

        }

        private void DownloadManager_OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive <= 0)
                SetMessage(String.Format("{0} bytes downloaded.", e.BytesReceived));
            else
                SetMessage(String.Format("{0} of {1} bytes downloaded ({2}%)", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage));
        }

    }
}
