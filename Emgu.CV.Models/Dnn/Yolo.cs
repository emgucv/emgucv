//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
using Range = Emgu.CV.Structure.Range;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Yolo model
    /// </summary>
    public class Yolo : DisposableObject, IProcessAndRenderModel
    {
        /// <summary>
        /// The rendering method
        /// </summary>
        public RenderType RenderMethod
        {
            get
            {
                return RenderType.Update;
            }
        }

        private readonly String _modelFolderName = Path.Combine("emgu", "yolo");

        private DetectionModel _yoloDetectionModel = null;
        private Net _yoloNet = null;
        private YoloVersion _versionUsed = YoloVersion.YoloDefault;

        private string[] _labels = null;

        /// <summary>
        /// The Yolo model version
        /// </summary>
        public enum YoloVersion
        {
            /// <summary>
            /// Represents the nano version of the Yolo V10 model. 2.3M parameters, 6.7G FLOPS, 38.5% AP_val 
            /// </summary>
            YoloV10N,

            /// <summary>
            /// Represents the small version of the Yolo V10 model. 7.2M parameters, 21.6G FLOPS, 46.3% AP_val 
            /// </summary>
            YoloV10S,

            /// <summary>
            /// Represents the medium version of the Yolo V10 model. 15.4M parameters, 59.1G FLOPS, 51.1% AP_val 
            /// </summary>
            YoloV10M,

            /// <summary>
            /// Represents the big version of the Yolo V10 model. 19.1M parameters, 92.0G FLOPS, 52.5% AP_val 
            /// </summary>
            YoloV10B,

            /// <summary>
            /// Represents the large version of the Yolo V10 model. 24.4M parameters, 120.3G FLOPS, 53.2% AP_val 
            /// </summary>
            YoloV10L,

            /// <summary>
            /// Represents the V10X (extra large) version of the Yolo model. 29.5M parameters, 160.4G FLOPS, 54.4% AP_val 
            /// </summary>
            YoloV10X,

            /// <summary>
            /// Represents the V8N version of the Yolo model. 3.2M parameters, 8.7 FLOPS, 37.3% AP_val
            /// </summary>
            YoloV8N,
            
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
            YoloV3Tiny, 
            
            /// <summary>
            /// The default YoloVersion
            /// </summary>
            YoloDefault = YoloV10N
        }

        private static int GetMajorVersion(YoloVersion version)
        {
            switch (version)
            {
                case YoloVersion.YoloV3:
                case YoloVersion.YoloV3Spp:
                case YoloVersion.YoloV3Tiny:
                    return 3;
                case YoloVersion.YoloV4:
                case YoloVersion.YoloV4Tiny:
                    return 4;
                case YoloVersion.YoloV8N:
                    return 8;
                case YoloVersion.YoloV10N:
                case YoloVersion.YoloV10S:
                case YoloVersion.YoloV10M:
                case YoloVersion.YoloV10L:
                case YoloVersion.YoloV10B:
                case YoloVersion.YoloV10X:
                    return 10;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                if (_yoloDetectionModel == null && _yoloNet == null)
                    return false;
                if (_labels == null)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Download and initialize the yolo model
        /// </summary>
        /// <param name="version">The model version</param>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Asyn task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            YoloVersion version = YoloVersion.YoloV4,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(
            YoloVersion version = YoloVersion.YoloV10N,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if ((_yoloDetectionModel == null) && (_yoloNet == null))
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
                } else if (version == YoloVersion.YoloV4)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov4/yolov4.weights",
                        _modelFolderName,
                        "8463FDE6EE7130A947A73104CE73C6FA88618A9D9ECD4A65D0B38F07E17EC4E4");
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov4/yolov4.cfg",
                        _modelFolderName,
                        "A15524EC710005ADD4EB672140CF15CBFE46DEA0561F1AEA90CB1140B466073E");
                }
                else if (version == YoloVersion.YoloV4Tiny)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov4/yolov4-tiny.weights",
                        _modelFolderName,
                        "CF9FBFD0F6D4869B35762F56100F50ED05268084078805F0E7989EFE5BB8CA87");
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov4/yolov4-tiny.cfg",
                        _modelFolderName,
                        "6CBF5ECE15235F66112E0BEDEBB324F37199B31AEE385B7E18F0BBFB536B258E");
                }
                else if (version == YoloVersion.YoloV8N)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov8/yolov8n.onnx",
                        _modelFolderName,
                        "F2F050909F184C2F12D78C4BAFAD57ABE7089C9F917511E23FE3D963117B6A40");
                } else if (version == YoloVersion.YoloV10N)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov10/yolov10n_no_post_process.onnx",
                        _modelFolderName,
                        "B426F3C7C9795C2819F68B149ABEE83447FCBC9DF81E2020C5786A132DAF2D66");
                }
                else if (version == YoloVersion.YoloV10S)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov10/yolov10s_no_post_process.onnx",
                        _modelFolderName,
                        "FE28B06648B1A80B18857D0EE5F0EBADF970406B20EE4CE4C43767C830D0AB3B");
                }
                else if (version == YoloVersion.YoloV10M)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov10/yolov10m_no_post_process.onnx",
                        _modelFolderName,
                        "BB91F00BA3C9866794DC60F43CEE23EDC8C43D3E160E60B98BF3A50E22741CF9");
                }
                else if (version == YoloVersion.YoloV10L)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov10/yolov10l_no_post_process.onnx",
                        _modelFolderName,
                        "44F000C37EA7761EA860B24366CC883862D8C93FE6D00391548894E32961DC84");
                }
                else if (version == YoloVersion.YoloV10B)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov10/yolov10b_no_post_process.onnx",
                        _modelFolderName,
                        "74309F2B7B33DA4B38B80C77087CE6A42A89811C00EA409F32AF0F75914BAB03");
                }
                else if (version == YoloVersion.YoloV10X)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/yolov10/yolov10x_no_post_process.onnx",
                        _modelFolderName,
                        "2A70E726043BFBAD95DEEDA3026CE541A1143737133E15C296D278F0F41809E2");
                }

                manager.AddFile("https://github.com/pjreddie/darknet/raw/master/data/coco.names",
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
                    try
                    {
                        int majorVersion = GetMajorVersion(version);
                        if (majorVersion == 3 || majorVersion == 4)
                        {
                            _yoloDetectionModel =
                                new DetectionModel(manager.Files[0].LocalFile, manager.Files[1].LocalFile);
                            _yoloDetectionModel.SetInputSize(new Size(416, 416));
                            _yoloDetectionModel.SetInputScale(0.00392);
                            _yoloDetectionModel.SetInputMean(new MCvScalar());

                            _labels = File.ReadAllLines(manager.Files[2].LocalFile);
                        }
                        else if (majorVersion == 8 || majorVersion == 10)
                        {
                            _yoloNet = DnnInvoke.ReadNetFromONNX(manager.Files[0].LocalFile);

                            _labels = File.ReadAllLines(manager.Files[1].LocalFile);
                        }

                        _versionUsed = version;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine( String.Format("Failed to initialize model: {0}", ex.Message));
                    }
                }
                else
                {
                    Trace.WriteLine("Failed to download model.");
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
            if (!Initialized)
            {
                throw new Exception("Please initialize the model first");
            }

            if (_yoloDetectionModel != null)
            {
                return _yoloDetectionModel.Detect(image, (float) confThreshold, (float) nmsThreshold, _labels);
            }
            else
            {
                Size inputSize = new Size(640, 640);
                using (InputArray iaImage = image.GetInputArray())
                using (Mat blob = DnnInvoke.BlobFromImage(image, 1.0 / 255.0, inputSize, swapRB: true))
                {
                    _yoloNet.SetInput(blob);
                    using (Mat output = _yoloNet.Forward())
                    {
                        Size imageSize = iaImage.GetSize();
                        int[] dims = output.SizeOfDimension;
                        using (Mat reshaped = output.Reshape(1, new int[] { dims[1], dims[2] }))
                        using (Mat transposed = reshaped.T())
                        {
                            // Network produces output blob with a shape NxC where N is a number of
                            // detected objects and C is a number of classes + 4 where the first 4
                            // numbers are [center_x, center_y, width, height]

                            List<Rectangle> bboxes = new List<Rectangle>();
                            List<float> scores = new List<float>();
                            List<int> classes = new List<int>();
                            int rows = transposed.Rows;
                            int cols = transposed.Cols;
                            for (int i = 0; i < rows; i++)
                            {
                                using (Mat row = new Mat(transposed, new Range(i, i + 1), new Range(4, cols)))
                                {
                                    double min=0, max=0;
                                    Point minLoc=new Point(), maxLoc = new Point();
                                    CvInvoke.MinMaxLoc(row, ref min, ref max, ref minLoc,ref maxLoc );

                                    if (max > confThreshold)
                                    {
                                        classes.Add(maxLoc.X);
                                        scores.Add((float)max);

                                        using (Mat rectData = new Mat(transposed, new Range(i, i + 1), new Range(0, 4)))
                                        {
                                            float[] data = rectData.GetData(jagged: false) as float[];

                                            if (GetMajorVersion(_versionUsed) == 8)
                                            {
                                                int centerX = (int) (data[0] / inputSize.Width * imageSize.Width);
                                                int centerY = (int) (data[1] / inputSize.Height * imageSize.Height);
                                                int width = (int) (data[2] / inputSize.Width * imageSize.Width);
                                                int height = (int) (data[3] / inputSize.Height * imageSize.Height);
                                                int left = centerX - width / 2;
                                                int top = centerY - height / 2;
                                                Rectangle rect = new Rectangle(left, top, width, height);

                                                bboxes.Add(rect);
                                            }
                                            else if (GetMajorVersion(_versionUsed) == 10)
                                            {
                                                int left = (int)(data[0] / inputSize.Width * imageSize.Width);
                                                int top = (int)(data[1] / inputSize.Height * imageSize.Height);
                                                int right = (int)(data[2] / inputSize.Width * imageSize.Width);
                                                int bottom = (int)(data[3] / inputSize.Height * imageSize.Height);
                                                Rectangle rect = new Rectangle(left, top, right - left, bottom - top);

                                                bboxes.Add(rect);
                                            }
                                        }
                                    }
                                }
                            }

                            if (bboxes.Count == 0)
                            {
                                return new DetectedObject[0];
                            }
                            
                            int[] indexes = DnnInvoke.NMSBoxes(
                                bboxes.ToArray(), 
                                scores.ToArray(), 
                                (float)confThreshold, 
                                (float)nmsThreshold);
                            List<DetectedObject> results = new List<DetectedObject>();
                            for (int i = 0; i < indexes.Length; i++)
                            {
                                DetectedObject o = new DetectedObject();
                                o.Region = bboxes[i];
                                o.Confident = scores[i];
                                o.ClassId = classes[i];
                                o.Label = _labels[classes[i]];
                                results.Add(o);
                            }
                            return results.ToArray();
                        }
                    }
                }
            }
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

            if (_yoloNet != null)
            {
                _yoloNet.Dispose();
                _yoloNet = null;
            }

            _labels = null;
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
        /// <param name="initOptions">A string, can be either "YoloV10N","YoloV10X","YoloV8N", "YoloV4", "YoloV4Tiny", "YoloV3", "YoloV3Spp", "YoloV3Tiny", specify the yolo model to use. Deafult to use "YoloV4". </param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
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
                else if (versionStr.Equals("YoloV4"))
                    v = YoloVersion.YoloV4;
                else if (versionStr.Equals("YoloV8N"))
                    v = YoloVersion.YoloV8N;
                else if (versionStr.Equals("YoloV10N"))
                    v = YoloVersion.YoloV10N;
                else if (versionStr.Equals("YoloV10X"))
                    v = YoloVersion.YoloV10X;
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
        /// <param name="imageOut">
        /// The output image, can be the same as <paramref name="imageIn"/>, in which case we will render directly into the input image.
        /// Note that if no object is detected, <paramref name="imageOut"/> will remain unchanged.
        /// If objects are detected, we will draw the label and (rectangle) regions on top of the existing pixels of <paramref name="imageOut"/>.
        /// If the <paramref name="imageOut"/> is not the same object as <paramref name="imageIn"/>, it is a good idea to copy the pixels over from the input image before passing it to this function.
        /// </param>
        /// <returns>The messages that we want to display.</returns>
        public String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            var detectedObjects = Detect(imageIn);
            watch.Stop();

            foreach (var detected in detectedObjects)
                detected.Render(imageOut, new MCvScalar(0, 0, 255));
            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);
        }
    }
}
