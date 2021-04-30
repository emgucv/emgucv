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
    /// DNN Scene text detector
    /// </summary>
    public class SceneTextDetector : DisposableObject, IProcessAndRenderModel
    {
        /// <summary>
        /// Create a new SceneTextDetector
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be saved to.</param>
        public SceneTextDetector(String modelFolderName = "scene_text_detector")
        {
            _modelFolderName = modelFolderName;
        }

        private String _modelFolderName;

        private TextDetectionModel_DB _textDetector = null;

        private TextRecognitionModel _ocr = null;

        private FreetypeNotoSansCJK _freetype = null;

        /// <summary>
        /// Download and initialize the vehicle detector, the license plate detector and OCR.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">Initialization options. None supported at the moment, any value passed will be ignored.</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null,
            Object initOptions = null)
#else
        public async Task Init(
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null,
            Object initOptions = null)
#endif
        {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE  || UNITY_WEBGL
            yield return InitTextDetector(onDownloadProgressChanged);
            yield return InitTextRecognizer(onDownloadProgressChanged);
            yield return InitFreetype(onDownloadProgressChanged);
#else
            await InitTextDetector(onDownloadProgressChanged);
            await InitTextRecognizer(onDownloadProgressChanged);
            await InitFreetype(onDownloadProgressChanged);
#endif
        }

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        private IEnumerator InitFreetype(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        private async Task InitFreetype(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_freetype == null)
            {
                _freetype = new FreetypeNotoSansCJK(_modelFolderName);
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return _freetype.Init(onDownloadProgressChanged);
#else
                await _freetype.Init(onDownloadProgressChanged);
#endif
            }
        }

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        private IEnumerator InitTextDetector(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        private async Task InitTextDetector(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_textDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/emgucv/models/raw/master/scene_text/DB_TD500_resnet50.onnx",
                    _modelFolderName,
                    "7B83A5E7AFBBD9D70313C902D188FF328656510DBF57D66A711E07DFDB81DF20");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _textDetector = new TextDetectionModel_DB(manager.Files[0].LocalFile);
                    _textDetector.BinaryThreshold = 0.3f;
                    _textDetector.PolygonThreshold = 0.5f;
                    _textDetector.MaxCandidates = 200;
                    _textDetector.UnclipRatio = 2.0;
                    _textDetector.SetInputScale(1.0 / 255.0);
                    _textDetector.SetInputSize(new Size(736, 736));
                    _textDetector.SetInputMean(new MCvScalar(122.67891434, 116.66876762, 104.00698793));

                    /*
                    if (Emgu.CV.Cuda.CudaInvoke.HasCuda)
                    {
                        _vehicleAttrRecognizer.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                        _vehicleAttrRecognizer.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
                    }*/
                }
            }
        }

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        private IEnumerator InitTextRecognizer(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        private async Task InitTextRecognizer(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_ocr == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/emgucv/models/raw/master/scene_text/crnn_cs_CN.onnx",
                    _modelFolderName,
                    "C760BF82D684B87DFABB288E6C0F92D41A8CD6C1780661CA2C3CD10C2065A9BA");

                manager.AddFile(
                    "https://github.com/emgucv/models/raw/master/scene_text/alphabet_3944.txt",
                    _modelFolderName,
                    "8027C9832D86764FECCD9BDD8974829C86994617E5787F178ED97DB2BDA1481A");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _ocr = new TextRecognitionModel(manager.Files[0].LocalFile);
                    _ocr.DecodeType = "CTC-greedy";
                    String[] vocab = File.ReadAllLines(manager.Files[1].LocalFile);
                    _ocr.Vocabulary = vocab;
                    _ocr.SetInputScale(1.0 / 127.5);
                    _ocr.SetInputMean(new MCvScalar(127.5, 127.5, 127.5));
                    _ocr.SetInputSize(new Size(100, 32));


                    /*
                    if (Emgu.CV.Cuda.CudaInvoke.HasCuda)
                    {
                        _vehicleAttrRecognizer.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                        _vehicleAttrRecognizer.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
                    }*/
                }
            }
        }

        private static void FourPointsTransform(IInputArray frame, PointF[] vertices, Size outputSize, IOutputArray result)
        {
            PointF[] targetVertices = {
                new Point(0, outputSize.Height - 1),
                new Point(0, 0),
                new Point(outputSize.Width - 1, 0),
                new Point(outputSize.Width - 1, outputSize.Height - 1)
            };
            Mat rotationMatrix = CvInvoke.GetPerspectiveTransform(vertices, targetVertices);

            CvInvoke.WarpPerspective(frame, result, rotationMatrix, outputSize);
        }

        /// <summary>
        /// Detect scene text from the given image
        /// </summary>
        /// <param name="image">The image</param>
        /// <returns>The detected scene text.</returns>
        public DetectedObject[] Detect(IInputArray image)
        {
            using (VectorOfVectorOfPoint vvp = new VectorOfVectorOfPoint())
            using (VectorOfFloat confidents = new VectorOfFloat())
            {
                _textDetector.Detect(image, vvp, confidents);

                Point[][] detectionResults = vvp.ToArrayOfArray();
                float[] confidentResult = confidents.ToArray();
                List<DetectedObject> results = new List<DetectedObject>();
                for (int i = 0; i < detectionResults.Length; i++)
                {
                    DetectedObject st = new DetectedObject();
                    PointF[] detectedPointF =
                        Array.ConvertAll(detectionResults[i], p => new PointF((float)p.X, (float)p.Y));
                    st.Region = CvInvoke.BoundingRectangle(detectionResults[i]);
                    st.Confident = confidentResult[i];

                    using (Mat textSubMat = new Mat())
                    {
                        FourPointsTransform(image, detectedPointF, new Size(100, 32), textSubMat);
                        String text = _ocr.Recognize(textSubMat);
                        st.Label = text;
                    }

                    results.Add(st);
                }

                return results.ToArray();
            }
        }

        /// <summary>
        /// Draw the vehicles to the image.
        /// </summary>
        /// <param name="image">The image to be drawn to.</param>
        /// <param name="sceneTexts">The scene texts.</param>
        public void Render(IInputOutputArray image, DetectedObject[] sceneTexts)
        {
            foreach (var detected in sceneTexts)
                detected.Render(image, new MCvScalar(0, 0, 255), _freetype);
            /*
            foreach (SceneText st in sceneTexts)
            {
                CvInvoke.Polylines(image, st.Region, true, new MCvScalar(0, 0, 255));
            }*/
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
            var sceneTexts = Detect(imageIn);
            watch.Stop();
            if (imageOut != imageIn)
            {
                using (InputArray iaImageIn = imageIn.GetInputArray())
                {
                    iaImageIn.CopyTo(imageOut);
                }
            }
            Render(imageOut, sceneTexts);
            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_textDetector != null)
            {
                _textDetector.Dispose();
                _textDetector = null;
            }

            if (_ocr != null)
            {
                _ocr.Dispose();
                _ocr = null;
            }

            if (_freetype != null)
            {
                _freetype.Dispose();
                _freetype = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this scene text detector.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
