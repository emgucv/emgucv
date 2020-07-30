//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Preferences;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Models;
using Emgu.Util;
using Color = Xamarin.Forms.Color;
using Environment = System.Environment;
using Point = System.Drawing.Point;

namespace Emgu.CV.XamarinForms
{
    public class LicensePlateRecognitionPage
#if __ANDROID__
        : AndroidCameraPage
#else
        : ButtonTextImagePage
#endif
    {
        private String _modelFolderName = "vehicle-license-plate-detection-barrier-0106";
        private Net _vehicleLicensePlateDetector = null;

        private Net _ocr;

        private async Task InitOCR()
        {
            if (_ocr == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://download.01.org/opencv/2020/openvinotoolkit/2020.4/open_model_zoo/models_bin/3/license-plate-recognition-barrier-0001/FP32/license-plate-recognition-barrier-0001.xml",
                    _modelFolderName);
                manager.AddFile(
                    "https://download.01.org/opencv/2020/openvinotoolkit/2020.4/open_model_zoo/models_bin/3/license-plate-recognition-barrier-0001/FP32/license-plate-recognition-barrier-0001.bin",
                    _modelFolderName);

                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
                await manager.Download();
                _ocr =
                    DnnInvoke.ReadNetFromModelOptimizer(manager.Files[0].LocalFile, manager.Files[1].LocalFile);

                /*
                if (Emgu.CV.Cuda.CudaInvoke.HasCuda)
                {
                    _ocr.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                    _ocr.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
                }*/
            }
        }

        private Net _vehicleAttrRecognizer = null;

        private async Task InitVehicleAttributesRecognizer()
        {
            if (_vehicleAttrRecognizer == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://download.01.org/opencv/2020/openvinotoolkit/2020.4/open_model_zoo/models_bin/3/vehicle-attributes-recognition-barrier-0042/FP32/vehicle-attributes-recognition-barrier-0042.xml",
                    _modelFolderName);
                manager.AddFile(
                    "https://download.01.org/opencv/2020/openvinotoolkit/2020.4/open_model_zoo/models_bin/3/vehicle-attributes-recognition-barrier-0042/FP32/vehicle-attributes-recognition-barrier-0042.bin",
                    _modelFolderName);


                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
                await manager.Download();
                _vehicleAttrRecognizer =
                    DnnInvoke.ReadNetFromModelOptimizer(manager.Files[0].LocalFile, manager.Files[1].LocalFile);

                /*
                if (Emgu.CV.Cuda.CudaInvoke.HasCuda)
                {
                    _vehicleAttrRecognizer.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                    _vehicleAttrRecognizer.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
                }*/
            }
        }


        private async Task InitLicensePlateDetector()
        {
            if (_vehicleLicensePlateDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://download.01.org/opencv/2020/openvinotoolkit/2020.4/open_model_zoo/models_bin/3/vehicle-license-plate-detection-barrier-0106/FP32/vehicle-license-plate-detection-barrier-0106.xml",
                    _modelFolderName);
                manager.AddFile(
                    "https://download.01.org/opencv/2020/openvinotoolkit/2020.4/open_model_zoo/models_bin/3/vehicle-license-plate-detection-barrier-0106/FP32/vehicle-license-plate-detection-barrier-0106.bin",
                    _modelFolderName);


                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
                await manager.Download();
                _vehicleLicensePlateDetector =
                    DnnInvoke.ReadNetFromModelOptimizer(manager.Files[0].LocalFile, manager.Files[1].LocalFile);

                /*
                if (Emgu.CV.Cuda.CudaInvoke.HasCuda)
                {
                    _vehicleLicensePlateDetector.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                    _vehicleLicensePlateDetector.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
                }*/
            }
        }

        private String[] _colorName = new String[] { "white", "gray", "yellow", "red", "green", "blue", "black" };
        private String[] _vehicleType = new String[] { "car", "van", "truck", "bus" };
        private String[] _plateText = new string[]
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "<Anhui>", "<Beijing>", "<Chongqing>", "<Fujian>",
            "<Gansu>", "<Guangdong>", "<Guangxi>", "<Guizhou>",
            "<Hainan>", "<Hebei>", "<Heilongjiang>", "<Henan>",
            "<HongKong>", "<Hubei>", "<Hunan>", "<InnerMongolia>",
            "<Jiangsu>", "<Jiangxi>", "<Jilin>", "<Liaoning>",
            "<Macau>", "<Ningxia>", "<Qinghai>", "<Shaanxi>",
            "<Shandong>", "<Shanghai>", "<Shanxi>", "<Sichuan>",
            "<Tianjin>", "<Tibet>", "<Xinjiang>", "<Yunnan>",
            "<Zhejiang>", "<police>",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
            "U", "V", "W", "X", "Y", "Z"
        };

        public struct LicensePlate
        {
            public Rectangle Region;
            public String Text;
        }

        public class Vehicle
        {
            public Rectangle Region;
            public String Color;
            public String Type;
            public LicensePlate? LicensePlate;

            public bool ContainsPlate(LicensePlate p, double plateOverlapRatio = 0.8)
            {
                if (Region.IsEmpty || p.Region.IsEmpty)
                    return false;
                double plateSize = p.Region.Width * p.Region.Height;
                Rectangle overlap = p.Region;
                overlap.Intersect(Region);
                double overlapSize = overlap.Width * overlap.Height;
                if (overlapSize / plateSize < plateOverlapRatio)
                    return false;
                return true;
            }
        }

        private void DetectAndRender(Mat image)
        {
            int imgDim = 300;
            int vehicleAttrSize = 72;
            MCvScalar meanVal = new MCvScalar();
            double scale = 1.0;
            float vehicleConfidenceThreshold = 0.5f;
            float licensePlateConfidenceThreshold = 0.5f;

            //MCvScalar meanVal = new MCvScalar(127.5, 127.5, 127.5);
            //double scale = 127.5;

            Size imageSize = image.Size;
            using (Mat inputBlob = DnnInvoke.BlobFromImage(
                image,
                scale,
                new Size(imgDim, imgDim),
                meanVal,
                false,
                false,
                DepthType.Cv32F))
                _vehicleLicensePlateDetector.SetInput(inputBlob, "Placeholder");

            List<Vehicle> vehicles = new List<Vehicle>();
            List<LicensePlate> plates = new List<LicensePlate>();
            using (Mat detection = _vehicleLicensePlateDetector.Forward("DetectionOutput_"))
            {
                float[,,,] values = detection.GetData(true) as float[,,,];
                for (int i = 0; i < values.GetLength(2); i++)
                {
                    float imageId = values[0, 0, i, 0];
                    float label = values[0, 0, i, 1]; //if label == 1, it is a vehicle; if label == 2, it is a license plate
                    float confident = values[0, 0, i, 2];
                    float xLeftBottom = values[0, 0, i, 3] * imageSize.Width;
                    float yLeftBottom = values[0, 0, i, 4] * imageSize.Height;
                    float xRightTop = values[0, 0, i, 5] * imageSize.Width;
                    float yRightTop = values[0, 0, i, 6] * imageSize.Height;
                    RectangleF objectRegion = new RectangleF(
                        xLeftBottom,
                        yLeftBottom,
                        xRightTop - xLeftBottom,
                        yRightTop - yLeftBottom);
                    Rectangle region = Rectangle.Round(objectRegion);

                    if (label == 1 && confident > vehicleConfidenceThreshold)
                    {
                        Vehicle v = new Vehicle();
                        v.Region = region;
                        using (Mat vehicle = new Mat(image, region))
                        {
                            using (Mat vehicleBlob = DnnInvoke.BlobFromImage(
                                vehicle,
                                scale,
                                new Size(vehicleAttrSize, vehicleAttrSize),
                                meanVal,
                                false,
                                false,
                                DepthType.Cv32F))
                            {
                                _vehicleAttrRecognizer.SetInput(vehicleBlob, "input");

                                using (VectorOfMat vm = new VectorOfMat(2))
                                {
                                    _vehicleAttrRecognizer.Forward(vm, new string[] { "color", "type" });
                                    using (Mat vehicleColorMat = vm[0])
                                    using (Mat vehicleTypeMat = vm[1])
                                    {
                                        float[] vehicleColorData = vehicleColorMat.GetData(false) as float[];
                                        float maxProbColor = vehicleColorData.Max();
                                        int maxIdxColor = Array.IndexOf(vehicleColorData, maxProbColor);
                                        v.Color = _colorName[maxIdxColor];
                                        float[] vehicleTypeData = vehicleTypeMat.GetData(false) as float[];
                                        float maxProbType = vehicleTypeData.Max();
                                        int maxIdxType = Array.IndexOf(vehicleTypeData, maxProbType);
                                        v.Type = _vehicleType[maxIdxType];
                                    }
                                }
                            }
                        }
                        vehicles.Add(v);
                    }

                    if (label == 2 && confident > licensePlateConfidenceThreshold)
                    {
                        LicensePlate p = new LicensePlate();
                        p.Region = region;

                        using (Mat plate = new Mat(image, region))
                        {
                            using (Mat inputBlob = DnnInvoke.BlobFromImage(
                                plate,
                                scale,
                                new Size(94, 24),
                                meanVal,
                                false,
                                false,
                                DepthType.Cv32F))
                            using (Mat seqInd = new Mat(
                                new Size(88, 1),
                                DepthType.Cv32F,
                                1))
                            {
                                _ocr.SetInput(inputBlob, "data");

                                if (seqInd.Depth == DepthType.Cv32F)
                                {
                                    float[] seqIndValues = new float[seqInd.Width * seqInd.Height];
                                    for (int j = 1; j < seqIndValues.Length; j++)
                                    {
                                        seqIndValues[j] = 1.0f;
                                    }
                                    seqIndValues[0] = 0.0f;
                                    seqInd.SetTo(seqIndValues);
                                }
                                _ocr.SetInput(seqInd, "seq_ind");

                                using (Mat output = _ocr.Forward("decode"))
                                {
                                    float[] plateValue = output.GetData(false) as float[];
                                    StringBuilder licensePlateStringBuilder = new StringBuilder();
                                    foreach (int j in plateValue)
                                    {
                                        if (j >= 0)
                                        {
                                            licensePlateStringBuilder.Append(_plateText[j]);
                                        }
                                    }

                                    p.Text = licensePlateStringBuilder.ToString();
                                }
                            }
                        }

                        plates.Add(p);
                    }

                }

                foreach (LicensePlate p in plates)
                {
                    foreach (Vehicle v in vehicles)
                    {
                        if (v.ContainsPlate(p))
                        {
                            v.LicensePlate = p;
                            break;
                        }
                    }
                }

                foreach (Vehicle v in vehicles)
                {
                    CvInvoke.Rectangle(image, v.Region, new MCvScalar(0, 0, 255), 2);
                    String label = String.Format("{0} {1} {2}",
                        v.Color, v.Type, v.LicensePlate == null ? String.Empty : v.LicensePlate.Value.Text);
                    CvInvoke.PutText(
                        image,
                        label,
                        new Point(v.Region.Location.X, v.Region.Location.Y + 20),
                        FontFace.HersheyComplex,
                        1.0,
                        new MCvScalar(0, 255, 0),
                        2);
                }
 
            }
        }

        private VideoCapture _capture = null;
        private Mat _mat = null;
        private String _defaultButtonText = "Perform License Plate Recognition";


        private String _StopCameraButtonText = "Stop Camera";
#if __ANDROID__
        private bool _isBusy = false;
#endif
        public LicensePlateRecognitionPage()
            : base()
        {
#if __ANDROID__
            HasCameraOption = true;
#endif

            var button = this.GetButton();
            button.Text = _defaultButtonText;
            button.Clicked += OnButtonClicked;

            var label = this.GetLabel();
            label.Text =
                "This demo is based on the security barrier camera demo in the OpenVino model zoo. The models is trained with BIT-vehicle dataset. License plate is trained based on Chinese license plate that has white character on blue background. You will need to re-train your own model if you intend to use this in other countries.";
        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            _capture.Retrieve(_mat);
            Stopwatch watch = Stopwatch.StartNew();
            DetectAndRender(_mat);
            watch.Stop();
            SetImage(_mat);
            this.DisplayImage.BackgroundColor = Color.Black;
            this.DisplayImage.IsEnabled = true;
            SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
        }

        private async void OnButtonClicked(Object sender, EventArgs args)
        {
            var button = GetButton();

            if (button.Text.Equals(_StopCameraButtonText))
            {
#if __ANDROID__
                StopCapture();
                //AndroidImageView.Visibility = ViewStates.Invisible;
#else
                _capture.Stop();
                _capture.Dispose();
                _capture = null;
#endif
                button.Text = _defaultButtonText;

                return;
            }

            Mat[] images = await LoadImages(new string[] { "cars_license_plate.png" });

            if (images == null || (images.Length > 0 && images[0] == null))
                return;

            SetMessage("Please wait...");
            SetImage(null);
            await InitLicensePlateDetector();
            await InitVehicleAttributesRecognizer();
            await InitOCR();

            if (images.Length == 0)
            {
#if __ANDROID__
                button.Text = _StopCameraButtonText;
                StartCapture(async delegate (Object sender, Mat m)
                {
                    //Skip the frame if busy, 
                    //Otherwise too many frames arriving and will eventually saturated the memory.
                    if (!_isBusy)
                    {
                        _isBusy = true;
                        try
                        {
                            Stopwatch watch = Stopwatch.StartNew();
                            await Task.Run(() => { DetectAndRender(m); });
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
                button.Text = _StopCameraButtonText;
#endif
            }
            else
            {
                Stopwatch watch = Stopwatch.StartNew();

                DetectAndRender(images[0]);
                watch.Stop();

                SetImage(images[0]);
                //String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";

                SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
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
