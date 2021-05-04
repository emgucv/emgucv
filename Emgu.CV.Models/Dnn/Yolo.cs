//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
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
        private String _modelFolderName = "yolo";

        private DetectionModel _yoloDetectionModel = null;

        private string[] _labels = null;

        /// <summary>
        /// The Yolo model version
        /// </summary>
        public enum YoloVersion
        {   
            /// <summary>
            /// Yolo V4
            /// </summary>
            YoloV4,
            /// <summary>
            /// Yolo V4 tiny
            /// </summary>
            YoloV4Tiny,

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
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            YoloVersion version = YoloVersion.YoloV3,
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(
            YoloVersion version = YoloVersion.YoloV3, 
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_yoloDetectionModel == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                bool version3 = false;
                bool version4 = false;
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
                    version3 = true;
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
                    version3 = true;
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
                    version3 = true;
                } else if (version == YoloVersion.YoloV4)
                {
                    manager.AddFile(
                        "https://github.com/aj-ames/YOLOv4-OpenCV-CUDA-DNN/raw/dd58dba457fff98e483e0f67113e0be1f17f2120/models/yolov4.weights",
                        _modelFolderName,
                        "8463FDE6EE7130A947A73104CE73C6FA88618A9D9ECD4A65D0B38F07E17EC4E4");
                    manager.AddFile(
                        "https://github.com/aj-ames/YOLOv4-OpenCV-CUDA-DNN/raw/dd58dba457fff98e483e0f67113e0be1f17f2120/models/yolov4.cfg",
                        _modelFolderName,
                        "A15524EC710005ADD4EB672140CF15CBFE46DEA0561F1AEA90CB1140B466073E");
                    version4 = true;
                }
                else if (version == YoloVersion.YoloV4Tiny)
                {
                    manager.AddFile(
                        "https://github.com/aj-ames/YOLOv4-OpenCV-CUDA-DNN/raw/dd58dba457fff98e483e0f67113e0be1f17f2120/models/yolov4-tiny.weights",
                        _modelFolderName,
                        "CF9FBFD0F6D4869B35762F56100F50ED05268084078805F0E7989EFE5BB8CA87");
                    manager.AddFile(
                        "https://github.com/aj-ames/YOLOv4-OpenCV-CUDA-DNN/raw/dd58dba457fff98e483e0f67113e0be1f17f2120/models/yolov4-tiny.cfg",
                        _modelFolderName,
                        "6CBF5ECE15235F66112E0BEDEBB324F37199B31AEE385B7E18F0BBFB536B258E");
                    version4 = true;
                }

                if (version3)
                    manager.AddFile("https://github.com/pjreddie/darknet/raw/master/data/coco.names",
                        _modelFolderName,
                        "634A1132EB33F8091D60F2C346ABABE8B905AE08387037AED883953B7329AF84");
                if (version4)
                    manager.AddFile("https://github.com/aj-ames/YOLOv4-OpenCV-CUDA-DNN/raw/dd58dba457fff98e483e0f67113e0be1f17f2120/models/coco.names",
                        _modelFolderName,
                        "634A1132EB33F8091D60F2C346ABABE8B905AE08387037AED883953B7329AF84");

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

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
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_yoloDetectionModel != null)
            {
                _yoloDetectionModel.Dispose();
                _yoloDetectionModel = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this Yolo detector.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }


        /// <summary>
        /// Download and initialize the yolo model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">A string, can be either "YoloV3", "YoloV3Spp", "YoloV3Tiny", specify the yolo model to use. Deafult to use "YoloV3". </param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#endif
        {
            YoloVersion v = YoloVersion.YoloV4;
            if (initOptions != null && ((initOptions as String) != null))
            {
                String versionStr = initOptions as String;
                if (versionStr.Equals("YoloV3Spp"))
                    v = YoloVersion.YoloV3Spp;
                else if (versionStr.Equals("YoloV3Tiny"))
                    v = YoloVersion.YoloV3Tiny;
                else if (versionStr.Equals("YoloV3"))
                    v = YoloVersion.YoloV3;
                else if (versionStr.Equals("YoloV4Tiny"))
                    v = YoloVersion.YoloV4Tiny;
            }
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
            yield return Init(v, onDownloadProgressChanged);
#else
            await Init(v, onDownloadProgressChanged);
#endif
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">The output image, can be the same as imageIn, in which case we will render directly into the input image</param>
        /// <returns>The messages that we want to display.</returns>

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
