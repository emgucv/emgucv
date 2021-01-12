//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Models;

namespace Emgu.CV.Models
{
    public class VehicleLicensePlateDetector
    {
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

        private String _modelFolderName = "vehicle-license-plate-detection-barrier-0106-openvino-2021.2";
        private Net _vehicleLicensePlateDetector = null;
        private Net _vehicleAttrRecognizer = null;
        private Net _ocr = null;
        
        private async Task InitOCR(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_ocr == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://storage.openvinotoolkit.org/repositories/open_model_zoo/2021.2/models_bin/3/license-plate-recognition-barrier-0001/FP32/license-plate-recognition-barrier-0001.xml",
                    _modelFolderName);
                manager.AddFile(
                    "https://storage.openvinotoolkit.org/repositories/open_model_zoo/2021.2/models_bin/3/license-plate-recognition-barrier-0001/FP32/license-plate-recognition-barrier-0001.bin",
                    _modelFolderName);

                if (onDownloadProgressChanged != null)
                {
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                }

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

        private async Task InitVehicleAttributesRecognizer(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_vehicleAttrRecognizer == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://storage.openvinotoolkit.org/repositories/open_model_zoo/2021.2/models_bin/3/vehicle-attributes-recognition-barrier-0042/FP32/vehicle-attributes-recognition-barrier-0042.xml",
                    _modelFolderName);
                manager.AddFile(
                    "https://storage.openvinotoolkit.org/repositories/open_model_zoo/2021.2/models_bin/3/vehicle-attributes-recognition-barrier-0042/FP32/vehicle-attributes-recognition-barrier-0042.bin",
                    _modelFolderName);


                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
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

        private async Task InitLicensePlateDetector(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_vehicleLicensePlateDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://storage.openvinotoolkit.org/repositories/open_model_zoo/2021.2/models_bin/3/vehicle-license-plate-detection-barrier-0106/FP32/vehicle-license-plate-detection-barrier-0106.xml",
                    _modelFolderName);
                manager.AddFile(
                    "https://storage.openvinotoolkit.org/repositories/open_model_zoo/2021.2/models_bin/3/vehicle-license-plate-detection-barrier-0106/FP32/vehicle-license-plate-detection-barrier-0106.bin",
                    _modelFolderName);

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
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

        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            await InitLicensePlateDetector(onDownloadProgressChanged);
            await InitVehicleAttributesRecognizer(onDownloadProgressChanged);
            await InitOCR(onDownloadProgressChanged);
        }

        public Vehicle[] Detect(Mat image)
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
                    {   //this is a vehicle
                        Vehicle v = new Vehicle();
                        v.Region = region;

                        #region find out the type and color of the vehicle
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
                        #endregion
                        vehicles.Add(v);
                    }

                    if (label == 2 && confident > licensePlateConfidenceThreshold)
                    {   //this is a license plate
                        LicensePlate p = new LicensePlate();
                        p.Region = region;

                        #region OCR on license plate
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
                                new Size(1, 88),
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
                        #endregion
                        
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
            }
            return vehicles.ToArray();
        }

        public void Render(Mat image, Vehicle[] vehicles)
        {
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
}
