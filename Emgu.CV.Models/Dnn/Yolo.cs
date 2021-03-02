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
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Yolo model
    /// </summary>
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
            /// <summary>
            /// Yolo v3
            /// </summary>
            YoloV3,
            /// <summary>
            /// Yolo v3 spp
            /// </summary>
            YoloV3Spp,
            /// <summary>
            /// Yolo v3 tiny
            /// </summary>
            YoloV3Tiny
        }

        /// <summary>
        /// Download and initialize the yolo model
        /// </summary>
        /// <param name="version">The model version</param>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Asyn task</returns>
        public async Task Init(YoloVersion version = YoloVersion.YoloV3, System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_yoloDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                if (version == YoloVersion.YoloV3Spp)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3-spp.weights",
                        _modelFolderName,
                        "87A1E8C85C763316F34E428F2295E1DB9ED4ABCEC59DD9544F8052F50DE327B4");
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3-spp.cfg",
                        _modelFolderName,
                        "7A4EC2D7427340FB12059F2B0EF76D6FCFCAC132CC287CBBF0BE5E3ABAA856FD");
                }
                else if (version == YoloVersion.YoloV3)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3.weights",
                        _modelFolderName,
                        "523E4E69E1D015393A1B0A441CEF1D9C7659E3EB2D7E15F793F060A21B32F297");
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3.cfg",
                        _modelFolderName,
                        "22489EA38575DFA36C67A90048E8759576416A79D32DC11E15D2217777B9A953");
                }
                else if (version == YoloVersion.YoloV3Tiny)
                {
                    manager.AddFile(
                        "https://pjreddie.com/media/files/yolov3-tiny.weights",
                        _modelFolderName,
                        "DCCEA06F59B781EC1234DDF8D1E94B9519A97F4245748A7D4DB75D5B7080A42C");
                    manager.AddFile(
                        "https://github.com/pjreddie/darknet/raw/master/cfg/yolov3-tiny.cfg",
                        _modelFolderName,
                        "84EB7A675EF87C906019FF5A6E0EFFE275D175ADB75100DCB47F0727917DC2C7");
                }

                manager.AddFile("https://github.com/pjreddie/darknet/raw/master/data/coco.names",
                    _modelFolderName,
                    "634A1132EB33F8091D60F2C346ABABE8B905AE08387037AED883953B7329AF84");

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                await manager.Download();

                if (manager.AllFilesDownloaded)
                {
                    _yoloDetector =
                        DnnInvoke.ReadNetFromDarknet(manager.Files[1].LocalFile, manager.Files[0].LocalFile);
                    _labels = File.ReadAllLines(manager.Files[2].LocalFile);
                }
            }
        }


        /// <summary>
        /// Detect objects using Yolo model
        /// </summary>
        /// <param name="image">The input image</param>
        /// <param name="confThreshold">The confident threshold. Only detection with confident larger than this will be returned.</param>
        /// <param name="nmsThreshold">If positive, will perform non-maximum suppression using the threshold value. If less than or equals to 0, will not perform Non-maximum suppression.</param>
        /// <returns>The detected objects</returns>
        public DetectedObject[] Detect(Mat image, double confThreshold = 0.5, double nmsThreshold = 0.4)
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

                    DetectedObject[] results = detectedObjects.ToArray();
                    if (nmsThreshold <= 0)
                    {
                        return results;
                    }

                    Rectangle[] regions = Array.ConvertAll(results, input => input.Region);
                    float[] scores = Array.ConvertAll(results, input => (float) input.Confident);
                    int[] keepIdx = DnnInvoke.NMSBoxes(regions, scores, (float)confThreshold, (float) nmsThreshold);
                    List<DetectedObject> nmsResults = new List<DetectedObject>();
                    for (int i = 0; i < keepIdx.Length; i++)
                    {
                        nmsResults.Add(results[keepIdx[i]]);
                    }
                    return nmsResults.ToArray();
                }
                else
                {
                    throw new Exception(String.Format("Unknown output layer type: {0}", outLayerType));
                }

            }
        }
    }
}
