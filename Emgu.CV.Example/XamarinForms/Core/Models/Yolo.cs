//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    public class Yolo
    {
        private String _modelFolderName = "yolo_v3";

        private Net _yoloDetector = null;

        private string[] _labels = null;

        private Mat _inputBlob = new Mat();

        /// <summary>
        /// The Yolo model version
        /// </summary>
        public enum YoloVersion
        {
            YoloV3,
            YoloV3Spp,
            YoloV3Tiny
        }

        public async Task Init(YoloVersion version = YoloVersion.YoloV3, System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_yoloDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                if (version == YoloVersion.YoloV3Spp)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3-spp.weights",
                        _modelFolderName);
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3-spp.cfg",
                        _modelFolderName);
                }
                else if (version == YoloVersion.YoloV3)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3.weights",
                        _modelFolderName);
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3.cfg",
                        _modelFolderName);
                }
                else if (version == YoloVersion.YoloV3Tiny)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3-tiny.weights",
                        _modelFolderName);
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3-tiny.cfg",
                        _modelFolderName);
                }

                manager.AddFile("https://github.com/pjreddie/darknet/raw/master/data/coco.names",
                    _modelFolderName);

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                await manager.Download();
                _yoloDetector = DnnInvoke.ReadNetFromDarknet(manager.Files[1].LocalFile, manager.Files[0].LocalFile);
                _labels = File.ReadAllLines(manager.Files[2].LocalFile);
            }
        }

        public void Render(Mat image, DetectedObject[] detectedObjects)
        {
            for (int i = 0; i < detectedObjects.Length; i++)
            {
                CvInvoke.Rectangle(image, detectedObjects[i].Region, new MCvScalar(0, 0, 255), 2);
                CvInvoke.PutText(
                    image,
                    String.Format("{0}: {1}", detectedObjects[i].Label, detectedObjects[i].Confident),
                    detectedObjects[i].Region.Location,
                    FontFace.HersheyDuplex,
                    1.0,
                    new MCvScalar(0, 0, 255),
                    1);
            }
        }

        public DetectedObject[] Detect(Mat image, double confThreshold = 0.5)
        {
            MCvScalar meanVal = new MCvScalar();

            Size imageSize = image.Size;

            DnnInvoke.BlobFromImage(
                image,
                _inputBlob,
                1.0,
                new Size(416, 416),
                meanVal,
                true,
                false,
                DepthType.Cv8U);
            _yoloDetector.SetInput(_inputBlob, "", 0.00392);
            int[] outLayers = _yoloDetector.UnconnectedOutLayers;
            String outLayerType = _yoloDetector.GetLayer(outLayers[0]).Type;
            String[] outLayerNames = _yoloDetector.UnconnectedOutLayersNames;

            using (VectorOfMat outs = new VectorOfMat())
            {
                List<DetectedObject> detectedObjects = new List<DetectedObject>();
                _yoloDetector.Forward(outs, outLayerNames);

                if (outLayerType.Equals("Region"))
                {
                    int size = outs.Size;

                    for (int i = 0; i < size; i++)
                    {
                        // Network produces output blob with a shape NxC where N is a number of
                        // detected objects and C is a number of classes + 4 where the first 4
                        // numbers are [center_x, center_y, width, height]
                        using (Mat m = outs[i])
                        {
                            int rows = m.Rows;
                            int cols = m.Cols;
                            float[,] data = m.GetData(true) as float[,];
                            for (int j = 0; j < rows; j++)
                            {
                                using (Mat subM = new Mat(m, new Emgu.CV.Structure.Range(j, j + 1), new Emgu.CV.Structure.Range(5, cols)))
                                {
                                    double minVal = 0, maxVal = 0;
                                    Point minLoc = new Point();
                                    Point maxLoc = new Point();
                                    CvInvoke.MinMaxLoc(subM, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                                    if (maxVal > confThreshold)
                                    {

                                        int centerX = (int)(data[j, 0] * imageSize.Width);
                                        int centerY = (int)(data[j, 1] * imageSize.Height);
                                        int width = (int)(data[j, 2] * imageSize.Width);
                                        int height = (int)(data[j, 3] * imageSize.Height);
                                        int left = centerX - width / 2;
                                        int top = centerY - height / 2;
                                        Rectangle rect = new Rectangle(left, top, width, height);

                                        DetectedObject obj = new DetectedObject();
                                        obj.ClassId = maxLoc.X;
                                        obj.Confident = maxVal;
                                        obj.Region = rect;
                                        obj.Label = _labels[obj.ClassId];
                                        detectedObjects.Add(obj);
                                    }
                                }
                            }
                        }
                    }

                    return detectedObjects.ToArray();
                }
                else
                {
                    throw new Exception(String.Format("Unknown output layer type: {0}", outLayerType));
                }

            }
        }
    }
}
