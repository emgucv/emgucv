//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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

        private Net _yoloNet = null;
        private YoloVersion _versionUsed = YoloVersion.YoloDefault;

        private string[] _labels = null;

        /// <summary>
        /// The Yolo model version
        /// </summary>
        public enum YoloVersion
        {
            /// <summary>
            /// Represents the nano version of the YOLO12 model. 2.6M parameters, 6.5G FLOPS, 40.6% AP_val
            /// </summary>
            Yolo12N,

            /// <summary>
            /// Represents the small version of the YOLO12 model. 9.3M parameters, 21.4G FLOPS, 48.0% AP_val
            /// </summary>
            Yolo12S,

            /// <summary>
            /// Represents the medium version of the YOLO12 model. 20.2M parameters, 67.5G FLOPS, 52.5% AP_val
            /// </summary>
            Yolo12M,

            /// <summary>
            /// Represents the large version of the YOLO12 model. 26.4M parameters, 88.9G FLOPS, 53.7% AP_val
            /// </summary>
            Yolo12L,

            /// <summary>
            /// Represents the extra-large version of the YOLO12 model. 59.1M parameters, 199.0G FLOPS, 55.2% AP_val
            /// </summary>
            Yolo12X,

            /// <summary>
            /// Represents the nano version of the YOLO11 model. 2.6M parameters, 6.5G FLOPS, 39.5% AP_val
            /// </summary>
            Yolo11N,

            /// <summary>
            /// Represents the small version of the YOLO11 model. 9.4M parameters, 21.5G FLOPS, 47.0% AP_val
            /// </summary>
            Yolo11S,

            /// <summary>
            /// Represents the medium version of the YOLO11 model. 20.1M parameters, 68.0G FLOPS, 51.5% AP_val
            /// </summary>
            Yolo11M,

            /// <summary>
            /// Represents the large version of the YOLO11 model. 25.3M parameters, 86.9G FLOPS, 53.4% AP_val
            /// </summary>
            Yolo11L,

            /// <summary>
            /// Represents the extra-large version of the YOLO11 model. 56.9M parameters, 194.9G FLOPS, 54.7% AP_val
            /// </summary>
            Yolo11X,

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
            /// The default YoloVersion
            /// </summary>
            YoloDefault = Yolo12N
        }

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _yoloNet != null && _labels != null;
            }
        }

        /// <summary>
        /// Return true if the given version is one of the YOLOv10 "no_post_process" exports,
        /// which declare 0 ONNX graph outputs and must be fetched by internal tensor name.
        /// </summary>
        private static bool IsV10(YoloVersion version)
        {
            return version == YoloVersion.YoloV10N || version == YoloVersion.YoloV10S ||
                   version == YoloVersion.YoloV10M || version == YoloVersion.YoloV10B ||
                   version == YoloVersion.YoloV10L || version == YoloVersion.YoloV10X;
        }

        /// <summary>
        /// Download and initialize the yolo model
        /// </summary>
        /// <param name="version">The model version</param>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            YoloVersion version = YoloVersion.YoloDefault,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(
            YoloVersion version = YoloVersion.YoloDefault,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_yoloNet == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                switch (version)
                {
                    case YoloVersion.Yolo12N:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo12/yolo12n.onnx",
                            _modelFolderName,
                            "FA0141234202F103B0A5CFD12CDB2DD28919837CD3A7A50D2FFBDF4FE093D389");
                        break;
                    case YoloVersion.Yolo12S:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo12/yolo12s.onnx",
                            _modelFolderName,
                            "F99511551746F42CDD0AE0D9113CF439704C89C35B7054F732185E0343B25281");
                        break;
                    case YoloVersion.Yolo12M:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo12/yolo12m.onnx",
                            _modelFolderName,
                            "8B340BEC82FA9624F733AEC2EEB016B5E9D65A905E615C5D304125FF6F728615");
                        break;
                    case YoloVersion.Yolo12L:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo12/yolo12l.onnx",
                            _modelFolderName,
                            "A4D0828C6CE1E58155020E8C4B2A0C7808E9F0CB96677E1EB62E431B57CE2C98");
                        break;
                    case YoloVersion.Yolo12X:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo12/yolo12x.onnx",
                            _modelFolderName,
                            "4A667CE6FB2CAA177CEDC51E5183527AF438A58B75FA5AD54BAD4452E1C8920F");
                        break;
                    case YoloVersion.Yolo11N:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo11/yolo11n.onnx",
                            _modelFolderName,
                            "DEAAC1046874862D88934C558747D064E8A869F029365A33C6B9FB87BAF041BC");
                        break;
                    case YoloVersion.Yolo11S:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo11/yolo11s.onnx",
                            _modelFolderName,
                            "C8AB6D22E0BF3AF9CCBE034866CED8FE84CA009CB22FF4E76DA8CD5DA6144DC7");
                        break;
                    case YoloVersion.Yolo11M:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo11/yolo11m.onnx",
                            _modelFolderName,
                            "3890CAF9D0FF24D8C5FF736779F41C3CC40787DC65153B7A5A3B0C19AEA4BE43");
                        break;
                    case YoloVersion.Yolo11L:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo11/yolo11l.onnx",
                            _modelFolderName,
                            "3D7105E7286CFD873F3E176B4505CC15297A9FA8EF8A91A2C9E1391710A96C18");
                        break;
                    case YoloVersion.Yolo11X:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolo11/yolo11x.onnx",
                            _modelFolderName,
                            "DC01218DB2FC4E6FB5F81B53FED127F0A276E37B0AE4DD6F28FA165341CAF007");
                        break;
                    case YoloVersion.YoloV8N:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolov8/yolov8n.onnx",
                            _modelFolderName,
                            "F2F050909F184C2F12D78C4BAFAD57ABE7089C9F917511E23FE3D963117B6A40");
                        break;
                    case YoloVersion.YoloV10N:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolov10/yolov10n_no_post_process.onnx",
                            _modelFolderName,
                            "B426F3C7C9795C2819F68B149ABEE83447FCBC9DF81E2020C5786A132DAF2D66");
                        break;
                    case YoloVersion.YoloV10S:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolov10/yolov10s_no_post_process.onnx",
                            _modelFolderName,
                            "FE28B06648B1A80B18857D0EE5F0EBADF970406B20EE4CE4C43767C830D0AB3B");
                        break;
                    case YoloVersion.YoloV10M:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolov10/yolov10m_no_post_process.onnx",
                            _modelFolderName,
                            "BB91F00BA3C9866794DC60F43CEE23EDC8C43D3E160E60B98BF3A50E22741CF9");
                        break;
                    case YoloVersion.YoloV10L:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolov10/yolov10l_no_post_process.onnx",
                            _modelFolderName,
                            "44F000C37EA7761EA860B24366CC883862D8C93FE6D00391548894E32961DC84");
                        break;
                    case YoloVersion.YoloV10B:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolov10/yolov10b_no_post_process.onnx",
                            _modelFolderName,
                            "74309F2B7B33DA4B38B80C77087CE6A42A89811C00EA409F32AF0F75914BAB03");
                        break;
                    case YoloVersion.YoloV10X:
                        manager.AddFile(
                            "https://emgu-public.s3.amazonaws.com/yolov10/yolov10x_no_post_process.onnx",
                            _modelFolderName,
                            "2A70E726043BFBAD95DEEDA3026CE541A1143737133E15C296D278F0F41809E2");
                        break;
                }

                manager.AddFile(
                    "https://github.com/pjreddie/darknet/raw/master/data/coco.names",
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
                        //YOLO is a single-pass detector (no KV-cache), so it does not
                        //hit the KV-cache limitation of OpenCV's ORT engine and can use
                        //the ONNX Runtime CUDA execution provider when available. This
                        //does not apply to the V10 "no_post_process" exports: those graphs
                        //declare zero ONNX outputs (the result is fetched by internal
                        //tensor name instead, see Detect below), which OpenCV's ORT engine
                        //requires and rejects with an assertion failure during Net loading.
                        bool useOrtCuda = CvInvoke.HaveOnnxRuntime && Emgu.CV.Cuda.CudaInvoke.HasCuda && !IsV10(version);
                        _yoloNet = DnnInvoke.ReadNetFromONNX(
                            manager.Files[0].LocalFile,
                            useOrtCuda ? EngineType.Ort : EngineType.Auto);
                        if (useOrtCuda)
                            _yoloNet.SetPreferableTarget(Target.Cuda);
                        _labels = File.ReadAllLines(manager.Files[1].LocalFile);
                        _versionUsed = version;
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(String.Format("Failed to initialize model: {0}", ex.Message));
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

            Size inputSize = new Size(640, 640);
            using (InputArray iaImage = image.GetInputArray())
            using (Mat blob = DnnInvoke.BlobFromImage(image, 1.0 / 255.0, inputSize, swapRB: true))
            {
                _yoloNet.SetInput(blob);
                // V10 models exported without NMS have 0 declared ONNX outputs; retrieve by
                // internal tensor name. V8 and YOLO11 declare output0 in the ONNX graph.
                bool isV10 = IsV10(_versionUsed);
                string outputTensorName = isV10 ? "/model.23/Concat_5_output_0" : "output0";
                using (Mat output = _yoloNet.Forward(outputTensorName))
                {
                    Size imageSize = iaImage.GetSize();
                    int[] dims = output.SizeOfDimension;
                    using (Mat reshaped = output.Reshape(1, new int[] { dims[1], dims[2] }))
                    using (Mat transposed = reshaped.T())
                    {
                        // Network produces output blob with a shape NxC where N is a number of
                        // detected objects and C is a number of classes + 4 where the first 4
                        // numbers are [center_x, center_y, width, height] (V8) or
                        // [left, top, right, bottom] (V10).
                        List<Rectangle> bboxes = new List<Rectangle>();
                        List<float> scores = new List<float>();
                        List<int> classes = new List<int>();
                        int rows = transposed.Rows;
                        int cols = transposed.Cols;
                        for (int i = 0; i < rows; i++)
                        {
                            using (Mat row = new Mat(transposed, new Range(i, i + 1), new Range(4, cols)))
                            {
                                double min = 0, max = 0;
                                Point minLoc = new Point(), maxLoc = new Point();
                                CvInvoke.MinMaxLoc(row, ref min, ref max, ref minLoc, ref maxLoc);

                                if (max > confThreshold)
                                {
                                    classes.Add(maxLoc.X);
                                    scores.Add((float)max);

                                    using (Mat rectData = new Mat(transposed, new Range(i, i + 1), new Range(0, 4)))
                                    {
                                        float[] data = rectData.GetData(multiDimensional: false) as float[];

                                        Rectangle rect;
                                        if (isV10)
                                        {
                                            // V10: output coords are [x1, y1, x2, y2]
                                            int left   = (int)(data[0] / inputSize.Width  * imageSize.Width);
                                            int top    = (int)(data[1] / inputSize.Height * imageSize.Height);
                                            int right  = (int)(data[2] / inputSize.Width  * imageSize.Width);
                                            int bottom = (int)(data[3] / inputSize.Height * imageSize.Height);
                                            rect = new Rectangle(left, top, right - left, bottom - top);
                                        }
                                        else
                                        {
                                            // V8 and YOLO11: output coords are [cx, cy, w, h]
                                            int centerX = (int)(data[0] / inputSize.Width * imageSize.Width);
                                            int centerY = (int)(data[1] / inputSize.Height * imageSize.Height);
                                            int width   = (int)(data[2] / inputSize.Width * imageSize.Width);
                                            int height  = (int)(data[3] / inputSize.Height * imageSize.Height);
                                            rect = new Rectangle(centerX - width / 2, centerY - height / 2, width, height);
                                        }
                                        bboxes.Add(rect);
                                    }
                                }
                            }
                        }

                        if (bboxes.Count == 0)
                            return new DetectedObject[0];

                        int[] indexes = DnnInvoke.NMSBoxes(
                            bboxes.ToArray(),
                            scores.ToArray(),
                            (float)confThreshold,
                            (float)nmsThreshold);
                        List<DetectedObject> results = new List<DetectedObject>();
                        for (int i = 0; i < indexes.Length; i++)
                        {
                            int idx = indexes[i];
                            DetectedObject o = new DetectedObject();
                            o.Region = bboxes[idx];
                            o.Confident = scores[idx];
                            o.ClassId = classes[idx];
                            o.Label = _labels[classes[idx]];
                            results.Add(o);
                        }
                        return results.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
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
        /// <param name="initOptions">A string specifying the model version: "YoloV10N", "YoloV10S", "YoloV10M", "YoloV10B", "YoloV10L", "YoloV10X", or "YoloV8N". Defaults to "YoloV10N".</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#endif
        {
            YoloVersion v = YoloVersion.YoloDefault;
            if (initOptions != null && ((initOptions as String) != null))
            {
                String versionStr = initOptions as String;
                if (!Enum.TryParse(versionStr, true, out v))
                    v = YoloVersion.YoloDefault;
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
