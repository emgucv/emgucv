//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
    public class Yolo : DisposableObject, IProcessAndRenderModel
    {
        private String _modelFolderName = "yolo_v3";

        private DetectionModel _yoloDetectionModel = null;

        private string[] _labels = null;

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
        public async Task Init(
            YoloVersion version = YoloVersion.YoloV3, 
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_yoloDetectionModel == null)
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
                    _yoloDetectionModel = new DetectionModel(manager.Files[0].LocalFile, manager.Files[1].LocalFile);
                    _yoloDetectionModel.SetInputSize(new Size(416, 416));
                    _yoloDetectionModel.SetInputScale(0.00392);
                    _yoloDetectionModel.SetInputMean(new MCvScalar());

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
        public DetectedObject[] Detect(IInputArray image, double confThreshold = 0.5, double nmsThreshold = 0.5)
        {
            if (_yoloDetectionModel == null)
            {
                throw new Exception("Please initialize the model first");
            }

            return _yoloDetectionModel.Detect(image, (float)confThreshold, (float)nmsThreshold, _labels);
        }

        /// <summary>
        /// Release the memory associated with this Yolo detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_yoloDetectionModel != null)
            {
                _yoloDetectionModel.Dispose();
                _yoloDetectionModel = null;
            }
        }

        public async Task Init(DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            await Init(YoloVersion.YoloV3, onDownloadProgressChanged);
        }

        public String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            var detectedObjects = Detect(imageIn);
            watch.Stop();

            if (imageOut != imageIn)
            {
                using (InputArray iaImageIn = imageIn.GetInputArray())
                {
                    iaImageIn.CopyTo(imageOut);
                }
            }

            foreach (var detected in detectedObjects)
                detected.Render(imageOut, new MCvScalar(0, 0, 255));
            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);
        }
    }
}
