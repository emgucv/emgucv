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
using System.Runtime.InteropServices;
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
    /// MaskRcnn model
    /// </summary>
    public class MaskRcnn : DisposableObject, IProcessAndRenderModel
    {
        private readonly String _modelFolderName = Path.Combine("emgu", "mask_rcnn_onnx");

        private string[] _labels = null;

        private MCvScalar[] _colors = null;

        private String[] _objectsOfInterest = null;

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

        /// <summary>
        /// Get or Set the list of object of interest.
        /// If null, all detected objects will be returned during the detection.
        /// Otherwise, only those objects with a label included in the list will be returned.
        /// </summary>
        public String[] ObjectsOfInterest
        {
            get { return _objectsOfInterest; }
            set { _objectsOfInterest = value; }
        }

        private Net _maskRcnnDetector = null;

        //The names of the output layers, in the order boxes, labels, scores, masks
        private String[] _outputNames = null;

        //The mean (in BGR order) that is subtracted from the input image
        private static readonly MCvScalar InputMean = new MCvScalar(102.9801, 115.9465, 122.7717);

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _maskRcnnDetector != null;
            }
        }

        /// <summary>
        /// Download and initialize the Mask Rcnn model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <param name="initOptions">A string, such as "OpenCV;CPU", in the format of "{backend};{option}"</param>
        /// <returns>Asyn task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null,
            Object initOptions = null)
#else
        public async Task Init(
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null,
            Object initOptions = null)
#endif
        {
            if (_maskRcnnDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                //Mask R-CNN R-50-FPN from the ONNX Model Zoo, converted from facebookresearch/maskrcnn-benchmark.
                manager.AddFile(
                    "https://huggingface.co/onnxmodelzoo/MaskRCNN-10/resolve/main/MaskRCNN-10.onnx",
                    _modelFolderName,
                    "A519D8102CB162E78CBF123615AA5A8F3BF9D0FA1DEC61A2FBBB42FA3F0E0757");
                //The 80 COCO classes, preceded by "__background" so that the model labels (1 based) index it directly.
                manager.AddFile(
                    "https://raw.githubusercontent.com/onnx/models/main/validated/vision/object_detection_segmentation/mask-rcnn/dependencies/coco_classes.txt",
                    _modelFolderName,
                    "B41621CA18B70742B4CDA496814D35BB7CE8E24C9FBE89AE9059A088E00D11ED");

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _maskRcnnDetector = DnnInvoke.ReadNetFromONNX(manager.Files[0].LocalFile, EngineType.OpenCV);
                    _outputNames = _maskRcnnDetector.UnconnectedOutLayersNames;

                    _labels = File.ReadAllLines(manager.Files[1].LocalFile);

                    if (initOptions != null && ((initOptions as String) != null))
                    {
                        String[] backendOptions = (initOptions as String).Split(';');
                        if (backendOptions.Length == 2)
                        {
                            String backendStr = backendOptions[0];
                            String targetStr = backendOptions[1];

                            Dnn.Backend backend;
                            Dnn.Target target;
                            if (Enum.TryParse(backendStr, true, out backend) &&
                                Enum.TryParse(targetStr, true, out target))
                            {
                                _maskRcnnDetector.SetPreferableBackend(backend);
                                _maskRcnnDetector.SetPreferableTarget(target);
                            }
                        }
                    }

                    if (_colors == null || _colors.Length != _labels.Length)
                    {
                        _colors = new MCvScalar[_labels.Length];
                        Random r = new Random(12345);
                        for (int i = 0; i < _colors.Length; i++)
                        {
                            _colors[i] = new MCvScalar(r.Next(256), r.Next(256), r.Next(256));
                        }
                    }
                }
            }
        }

        private MCvScalar _renderColor = new MCvScalar(0, 0, 255);

        /// <summary>
        /// Get or Set the color used in rendering the rectangle around the object.
        /// </summary>
        public MCvScalar RenderColorRectangle
        {
            get
            {
                return _renderColor;
            }
            set
            {
                _renderColor = value;
            }
        }

        /// <summary>
        /// Get the rendering colors for the mask. The size of the array equals to the size of labels.
        /// You can change the colors for rendering masks by updating this array.
        /// It will only contains value after the Init function is called.
        /// </summary>
        public MCvScalar[] RenderColorMask
        {
            get
            {
                return _colors;
            }
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">The output image, can be the same as imageIn, in which case we will render directly into the input image</param>
        /// <returns>The messages that we want to display.</returns>
        String IProcessAndRenderModel.ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            ProcessAndRender(imageIn, imageOut, 0.5f, 0.4f);
            watch.Stop();
            
            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">The output image, can be the same as imageIn, in which case we will render directly into the input image</param>
        /// <param name="matchScoreThreshold">A threshold used to filter boxes by score.</param>
        /// <param name="nmsThreshold">A threshold used in non maximum suppression.</param>
        /// <returns>The messages that we want to display.</returns>
        public void ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut, float matchScoreThreshold = 0.5f, float nmsThreshold = 0.4f)
        {
            MaskedObject[] objects = Detect(imageIn, matchScoreThreshold, nmsThreshold);

            foreach (var obj in objects)
            {
                obj.Render(imageOut, RenderColorRectangle, RenderColorMask[obj.ClassId]);
                obj.Dispose();
            }
        }

        /// <summary>
        /// Perform detection on the input image and return the results
        /// </summary>
        /// <param name="m">The input image</param>
        /// <param name="matchScoreThreshold">A threshold used to filter boxes by score.</param>
        /// <param name="nmsThreshold">A threshold used in non maximum suppression.</param>
        /// <returns>The detected objects</returns>
        public MaskedObject[] Detect(IInputArray m, float matchScoreThreshold = 0.5f, float nmsThreshold = 0.4f)
        {
            using (InputArray iaM = m.GetInputArray())
            using (Mat img = iaM.GetMat())
            using (Mat bgr = new Mat())
            using (Mat resized = new Mat())
            using (Mat resizedF = new Mat())
            {
                System.Drawing.Size imgSize = img.Size;
                if (img.NumberOfChannels == 4)
                    CvInvoke.CvtColor(img, bgr, ColorConversion.Bgra2Bgr);
                else if (img.NumberOfChannels == 1)
                    CvInvoke.CvtColor(img, bgr, ColorConversion.Gray2Bgr);
                else
                    img.CopyTo(bgr);

                //The short side is resized to 800 pixels
                double ratio = 800.0 / Math.Min(imgSize.Width, imgSize.Height);
                CvInvoke.Resize(bgr, resized, System.Drawing.Size.Empty, ratio, ratio, Inter.Linear);
                resized.ConvertTo(resizedF, DepthType.Cv32F);
                using (ScalarArray mean = new ScalarArray(InputMean))
                    CvInvoke.Subtract(resizedF, mean, resizedF);

                //Zero pad to a multiple of 32, and convert from HWC to CHW without a batch dimension.
                int h = resizedF.Rows;
                int w = resizedF.Cols;
                int paddedH = (h + 31) / 32 * 32;
                int paddedW = (w + 31) / 32 * 32;
                int planeSize = paddedH * paddedW;
                float[] blobData = new float[3 * planeSize];
                GCHandle blobHandle = GCHandle.Alloc(blobData, GCHandleType.Pinned);
                try
                {
                    using (Mat padded = new Mat(paddedH, paddedW, DepthType.Cv32F, 3))
                    {
                        padded.SetTo(new MCvScalar());
                        using (Mat roi = new Mat(padded, new Rectangle(0, 0, w, h)))
                            resizedF.CopyTo(roi);

                        IntPtr blobPtr = blobHandle.AddrOfPinnedObject();
                        for (int c = 0; c < 3; c++)
                        {
                            using (Mat plane = new Mat(
                                paddedH,
                                paddedW,
                                DepthType.Cv32F,
                                1,
                                IntPtr.Add(blobPtr, c * planeSize * sizeof(float)),
                                paddedW * sizeof(float)))
                                CvInvoke.ExtractChannel(padded, plane, c);
                        }

                        using (Mat blob = new Mat(new int[] { 3, paddedH, paddedW }, DepthType.Cv32F, blobPtr))
                        using (VectorOfMat tensors = new VectorOfMat())
                        {
                            _maskRcnnDetector.SetInput(blob);
                            _maskRcnnDetector.Forward(tensors, _outputNames);

                            //The outputs are boxes (n, 4), labels (n), scores (n) and masks (n, 1, mh, mw)
                            using (Mat boxesMat = tensors[0])
                            using (Mat labelsMat = tensors[1])
                            using (Mat scoresMat = tensors[2])
                            using (Mat masksMat = tensors[3])
                            {
                                return ParseDetections(
                                    boxesMat, labelsMat, scoresMat, masksMat,
                                    ratio, imgSize, matchScoreThreshold, nmsThreshold);
                            }
                        }
                    }
                }
                finally
                {
                    blobHandle.Free();
                }
            }
        }

        private static float[] ToFloatArray(Mat tensor)
        {
            using (Mat tensorF = new Mat())
            {
                tensor.ConvertTo(tensorF, DepthType.Cv32F);
                float[] data = new float[tensorF.Total.ToInt32()];
                if (data.Length > 0)
                    tensorF.CopyTo(data);
                return data;
            }
        }

        private MaskedObject[] ParseDetections(
            Mat boxesMat,
            Mat labelsMat,
            Mat scoresMat,
            Mat masksMat,
            double ratio,
            System.Drawing.Size imgSize,
            float matchScoreThreshold,
            float nmsThreshold)
        {
            float[] boxes = ToFloatArray(boxesMat);
            float[] labels = ToFloatArray(labelsMat);
            float[] scores = ToFloatArray(scoresMat);
            int numDetections = labels.Length;
            if (numDetections == 0)
                return new MaskedObject[0];

            int[] masksDim = masksMat.SizeOfDimension;
            int maskHeight = masksDim[masksDim.Length - 2];
            int maskWidth = masksDim[masksDim.Length - 1];
            float[] masks = ToFloatArray(masksMat);

            List<int> detectionIdx = new List<int>();
            List<int> classIds = new List<int>();
            List<Rectangle> regions = new List<Rectangle>();
            List<float> filteredScores = new List<float>();

            for (int i = 0; i < numDetections; i++)
            {
                //The labels are 1 based, 0 is the background
                int classId = (int)labels[i];
                if (classId <= 0 || classId >= _labels.Length)
                    continue;
                if (_objectsOfInterest != null && !_objectsOfInterest.Contains(_labels[classId]))
                    continue;

                //The boxes are x1, y1, x2, y2 in the resized image
                int x1 = (int)Math.Round(boxes[i * 4] / ratio);
                int y1 = (int)Math.Round(boxes[i * 4 + 1] / ratio);
                int x2 = (int)Math.Round(boxes[i * 4 + 2] / ratio);
                int y2 = (int)Math.Round(boxes[i * 4 + 3] / ratio);
                Rectangle rect = Rectangle.FromLTRB(x1, y1, x2, y2);
                rect.Intersect(new Rectangle(Point.Empty, imgSize));
                if (rect.Width <= 0 || rect.Height <= 0)
                    continue;

                detectionIdx.Add(i);
                classIds.Add(classId);
                regions.Add(rect);
                filteredScores.Add(scores[i]);
            }

            if (regions.Count == 0)
                return new MaskedObject[0];

            int[] validIdx = DnnInvoke.NMSBoxes(regions.ToArray(), filteredScores.ToArray(), matchScoreThreshold, nmsThreshold);
            List<MaskedObject> maskedObjects = new List<MaskedObject>();
            int maskSize = maskHeight * maskWidth;
            float[] maskData = new float[maskSize];
            for (int i = 0; i < validIdx.Length; i++)
            {
                int idx = validIdx[i];
                int classId = classIds[idx];
                Array.Copy(masks, detectionIdx[idx] * maskSize, maskData, 0, maskSize);
                using (Mat mask = new Mat(maskHeight, maskWidth, DepthType.Cv32F, 1))
                {
                    mask.SetTo(maskData);
                    maskedObjects.Add(new MaskedObject(classId, _labels[classId], filteredScores[idx], regions[idx], mask));
                }
            }

            return maskedObjects.ToArray();
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_maskRcnnDetector != null)
            {
                _maskRcnnDetector.Dispose();
                _maskRcnnDetector = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this Mask Rcnn detector.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
