//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
    /// PaddleOCR (PP-OCRv4) text detection and recognition using the dnn
    /// module, an alternative OCR engine to Tesseract. The pipeline uses the
    /// DBNet text detector to find text regions, rectifies each region with a
    /// perspective warp and reads it with the SVTR-based CTC recognition
    /// network. The recognition model supports Chinese and Latin characters.
    ///
    /// The recognition network has dynamic input shapes and requires the new
    /// dnn engine; it is not supported by the classic engine.
    /// </summary>
    public class PaddleOCR : DisposableObject, IProcessAndRenderModel
    {
        /// <summary>
        /// A single text region recognized by PaddleOCR.
        /// </summary>
        public class OcrResult
        {
            /// <summary>
            /// The four corners of the text region, in the original image coordinates.
            /// </summary>
            public PointF[] Region;

            /// <summary>
            /// The recognized text.
            /// </summary>
            public String Text;

            /// <summary>
            /// The mean confidence of the recognized characters.
            /// </summary>
            public double Confidence;
        }

        private String _modelFolderName = Path.Combine("emgu", "paddleocr");

        private Net _detNet = null;
        private Net _recNet = null;
        private String[] _dictionary = null;
        private FontFace _fontFace = null;

        /// <summary>
        /// The threshold applied to the detector probability map. Defaults to 0.3.
        /// </summary>
        public double BinaryThreshold { get; set; } = 0.3;

        /// <summary>
        /// Detected regions with a mean probability score below this value are discarded. Defaults to 0.6.
        /// </summary>
        public double BoxScoreThreshold { get; set; } = 0.6;

        /// <summary>
        /// The ratio used to expand the detected (shrunk) text regions. Defaults to 1.6.
        /// </summary>
        public double UnclipRatio { get; set; } = 1.6;

        /// <summary>
        /// The maximum side length of the image fed to the detector; larger images are scaled down. Defaults to 960.
        /// </summary>
        public int MaxSideLength { get; set; } = 960;

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
        /// Create a new PaddleOCR model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be saved to.</param>
        public PaddleOCR(String modelFolderName = null)
        {
            if (modelFolderName != null)
                _modelFolderName = modelFolderName;
        }

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _detNet != null && _recNet != null && _dictionary != null;
            }
        }

        /// <summary>
        /// Download and initialize the PaddleOCR detection and recognition models.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">Initialization options. None supported at the moment, any value passed will be ignored.</param>
        /// <returns>Async task</returns>
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
            if (!Initialized)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/paddleocr/ch_PP-OCRv4_det_infer.onnx",
                    _modelFolderName,
                    "D2A7720D45A54257208B1E13E36A8479894CB74155A5EFE29462512D42F49DA9");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/paddleocr/ch_PP-OCRv4_rec_infer.onnx",
                    _modelFolderName,
                    "48FC40F24F6D2A207A2B1091D3437EB3CC3EB6B676DC3EF9C37384005483683B");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/paddleocr/ppocr_keys_v1.txt",
                    _modelFolderName,
                    "28B2362AD4AB2DC38769AA72FEB535E3A9DDB3FD2A7585A05920E6393B1DC7F7");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    //The recognition model has dynamic input shapes and requires
                    //the new dnn engine.
                    _detNet = DnnInvoke.ReadNetFromONNX(manager.Files[0].LocalFile, EngineType.New);
                    _recNet = DnnInvoke.ReadNetFromONNX(manager.Files[1].LocalFile, EngineType.New);
                    _dictionary = File.ReadAllLines(manager.Files[2].LocalFile);

                    //Used to render the recognized text, which may contain CJK
                    //characters. Fall back to the default embedded font if the
                    //native library was built without WITH_UNIFONT.
                    _fontFace = new FontFace();
                    _fontFace.Set("uni");
                }
            }
        }

        /// <summary>
        /// Order the four corners of a rotated rectangle as top-left, top-right,
        /// bottom-right, bottom-left.
        /// </summary>
        private static PointF[] OrderClockwise(PointF[] points)
        {
            PointF tl = points[0], tr = points[0], br = points[0], bl = points[0];
            double minSum = double.MaxValue, maxSum = double.MinValue, minDiff = double.MaxValue, maxDiff = double.MinValue;
            foreach (PointF p in points)
            {
                double sum = p.X + p.Y;
                double diff = p.Y - p.X;
                if (sum < minSum) { minSum = sum; tl = p; }
                if (sum > maxSum) { maxSum = sum; br = p; }
                if (diff < minDiff) { minDiff = diff; tr = p; }
                if (diff > maxDiff) { maxDiff = diff; bl = p; }
            }
            return new PointF[] { tl, tr, br, bl };
        }

        private static double Distance(PointF a, PointF b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Run the DBNet text detector, returning the four corners of each
        /// detected text region in the original image coordinates.
        /// </summary>
        private List<PointF[]> DetectRegions(Mat image)
        {
            int srcW = image.Width, srcH = image.Height;
            double scale = Math.Min(1.0, MaxSideLength / (double)Math.Max(srcW, srcH));
            //DBNet requires the input dimensions to be multiples of 32
            int detW = Math.Max(32, (int)Math.Round(srcW * scale / 32) * 32);
            int detH = Math.Max(32, (int)Math.Round(srcH * scale / 32) * 32);

            List<PointF[]> regions = new List<PointF[]>();

            using (Mat resized = new Mat())
            using (Mat f32 = new Mat())
            using (ScalarArray mean = new ScalarArray(new MCvScalar(0.485, 0.456, 0.406)))
            using (ScalarArray std = new ScalarArray(new MCvScalar(0.229, 0.224, 0.225)))
            {
                CvInvoke.Resize(image, resized, new Size(detW, detH));
                resized.ConvertTo(f32, DepthType.Cv32F, 1.0 / 255.0);
                CvInvoke.Subtract(f32, mean, f32, null, DepthType.Cv32F);
                CvInvoke.Divide(f32, std, f32, 1.0, DepthType.Cv32F);

                using (Mat blob = DnnInvoke.BlobFromImage(f32))
                {
                    _detNet.SetInput(blob);
                    using (Mat probOut = _detNet.Forward())    //(1, 1, detH, detW)
                    using (Mat prob2d = new Mat(new int[] { detH, detW }, DepthType.Cv32F, probOut.DataPointer))
                    using (Mat mask32f = new Mat())
                    using (Mat mask8u = new Mat())
                    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                    {
                        CvInvoke.Threshold(prob2d, mask32f, BinaryThreshold, 255, ThresholdType.Binary);
                        mask32f.ConvertTo(mask8u, DepthType.Cv8U);

                        CvInvoke.FindContours(mask8u, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                        double scaleBackX = srcW / (double)detW;
                        double scaleBackY = srcH / (double)detH;

                        for (int i = 0; i < contours.Size; i++)
                        {
                            using (VectorOfPoint contour = contours[i])
                            {
                                double area = CvInvoke.ContourArea(contour);
                                double perimeter = CvInvoke.ArcLength(contour, true);
                                if (area < 9 || perimeter <= 0)
                                    continue;

                                //Mean probability score of the region
                                Rectangle bound = CvInvoke.BoundingRectangle(contour);
                                MCvScalar score;
                                using (Mat probRoi = new Mat(prob2d, bound))
                                using (Mat maskRoi = new Mat(mask8u, bound))
                                    score = CvInvoke.Mean(probRoi, maskRoi);
                                if (score.V0 < BoxScoreThreshold)
                                    continue;

                                RotatedRect box = CvInvoke.MinAreaRect(contour);
                                if (Math.Min(box.Size.Width, box.Size.Height) < 3)
                                    continue;

                                //Unclip: DBNet is trained on shrunk text regions;
                                //expand the box by the polygon offset distance
                                //area * ratio / perimeter.
                                float delta = (float)(area * UnclipRatio / perimeter);
                                box.Size.Width += 2 * delta;
                                box.Size.Height += 2 * delta;

                                PointF[] vertices = OrderClockwise(box.GetVertices());
                                for (int j = 0; j < vertices.Length; j++)
                                {
                                    vertices[j].X = (float)(Math.Max(0, Math.Min(detW, vertices[j].X)) * scaleBackX);
                                    vertices[j].Y = (float)(Math.Max(0, Math.Min(detH, vertices[j].Y)) * scaleBackY);
                                }
                                regions.Add(vertices);
                            }
                        }
                    }
                }
            }

            //Sort into reading order: top to bottom, then left to right.
            regions.Sort((a, b) =>
            {
                float ya = (a[0].Y + a[2].Y) / 2, yb = (b[0].Y + b[2].Y) / 2;
                float ha = Math.Abs(a[2].Y - a[0].Y), hb = Math.Abs(b[2].Y - b[0].Y);
                if (Math.Abs(ya - yb) > Math.Min(ha, hb) / 2)
                    return ya.CompareTo(yb);
                return ((a[0].X + a[2].X) / 2).CompareTo((b[0].X + b[2].X) / 2);
            });

            return regions;
        }

        /// <summary>
        /// Rectify a text region with a perspective warp and read it with the
        /// recognition network using CTC greedy decoding.
        /// </summary>
        private OcrResult RecognizeRegion(Mat image, PointF[] region)
        {
            int boxW = Math.Max(1, (int)Math.Round((Distance(region[0], region[1]) + Distance(region[3], region[2])) / 2));
            int boxH = Math.Max(1, (int)Math.Round((Distance(region[0], region[3]) + Distance(region[1], region[2])) / 2));

            using (Mat warped = new Mat())
            {
                PointF[] dst = new PointF[]
                {
                    new PointF(0, 0),
                    new PointF(boxW, 0),
                    new PointF(boxW, boxH),
                    new PointF(0, boxH)
                };
                using (Mat transform = CvInvoke.GetPerspectiveTransform(region, dst))
                    CvInvoke.WarpPerspective(image, warped, transform, new Size(boxW, boxH));

                //Vertical text: rotate to horizontal like PaddleOCR does.
                if (boxH >= boxW * 1.5)
                {
                    CvInvoke.Rotate(warped, warped, RotateFlags.Rotate90CounterClockwise);
                    int tmp = boxW; boxW = boxH; boxH = tmp;
                }

                //The recognition network takes a (1, 3, 48, W) input, W is dynamic.
                int recH = 48;
                int recW = Math.Max(16, Math.Min(640, (int)Math.Round(boxW * recH / (double)boxH)));

                using (Mat blob = DnnInvoke.BlobFromImage(
                    warped,
                    1.0 / 127.5,
                    new Size(recW, recH),
                    new MCvScalar(127.5, 127.5, 127.5),
                    false,
                    false))
                {
                    _recNet.SetInput(blob);
                    using (Mat logits = _recNet.Forward())   //(1, T, C), softmax probabilities
                    {
                        int[] shape = logits.SizeOfDimension;
                        int timeSteps = shape[1];
                        int classes = shape[2];

                        //CTC greedy decode: argmax per time step, drop blanks
                        //(index 0) and collapse consecutive repeats.
                        //Index i in [1, dictionary.Length] maps to dictionary[i - 1];
                        //the last index is the space character.
                        StringBuilderText textAndScore = new StringBuilderText();
                        int previousIndex = 0;
                        for (int t = 0; t < timeSteps; t++)
                        {
                            using (Mat row = new Mat(
                                new int[] { 1, classes },
                                DepthType.Cv32F,
                                new IntPtr(logits.DataPointer.ToInt64() + (long)t * classes * sizeof(float))))
                            {
                                double minVal = 0, maxVal = 0;
                                Point minLoc = new Point(), maxLoc = new Point();
                                CvInvoke.MinMaxLoc(row, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                                int index = maxLoc.X;

                                if (index != 0 && index != previousIndex)
                                {
                                    if (index - 1 < _dictionary.Length)
                                        textAndScore.Append(_dictionary[index - 1], maxVal);
                                    else
                                        textAndScore.Append(" ", maxVal);
                                }
                                previousIndex = index;
                            }
                        }

                        return new OcrResult
                        {
                            Region = region,
                            Text = textAndScore.Text,
                            Confidence = textAndScore.MeanScore
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Helper that accumulates decoded characters and their scores.
        /// </summary>
        private class StringBuilderText
        {
            private readonly System.Text.StringBuilder _builder = new System.Text.StringBuilder();
            private double _scoreSum = 0;
            private int _count = 0;

            public void Append(String s, double score)
            {
                _builder.Append(s);
                _scoreSum += score;
                _count++;
            }

            public String Text
            {
                get { return _builder.ToString(); }
            }

            public double MeanScore
            {
                get { return _count == 0 ? 0 : _scoreSum / _count; }
            }
        }

        /// <summary>
        /// Detect and recognize all the text regions in the image.
        /// </summary>
        /// <param name="image">The input image (BGR).</param>
        /// <returns>The recognized text regions, in reading order (top to bottom, left to right).</returns>
        public OcrResult[] Recognize(IInputArray image)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            {
                List<OcrResult> results = new List<OcrResult>();
                foreach (PointF[] region in DetectRegions(mat))
                {
                    OcrResult result = RecognizeRegion(mat, region);
                    if (!String.IsNullOrEmpty(result.Text))
                        results.Add(result);
                }
                return results.ToArray();
            }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_detNet != null)
            {
                _detNet.Dispose();
                _detNet = null;
            }

            if (_recNet != null)
            {
                _recNet.Dispose();
                _recNet = null;
            }

            if (_fontFace != null)
            {
                _fontFace.Dispose();
                _fontFace = null;
            }

            _dictionary = null;
        }

        /// <summary>
        /// Release the memory associated with this PaddleOCR model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        private MCvScalar _renderColor = new MCvScalar(0, 0, 255);

        /// <summary>
        /// Get or Set the color used in rendering.
        /// </summary>
        public MCvScalar RenderColor
        {
            get { return _renderColor; }
            set { _renderColor = value; }
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">
        /// The output image, can be the same as <paramref name="imageIn"/>, in which case we will render directly into the input image.
        /// </param>
        /// <returns>The messages that we want to display.</returns>
        public String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            OcrResult[] results = Recognize(imageIn);
            watch.Stop();

            foreach (OcrResult result in results)
            {
                Point[] corners = Array.ConvertAll(result.Region, Point.Round);
                using (VectorOfPoint vp = new VectorOfPoint(corners))
                    CvInvoke.Polylines(imageOut, vp, true, RenderColor, 2);
                CvInvoke.PutText(
                    imageOut,
                    result.Text,
                    Point.Round(result.Region[3]),
                    RenderColor,
                    _fontFace,
                    20);
            }

            return String.Format("Recognized {0} text region(s) in {1} milliseconds.", results.Length, watch.ElapsedMilliseconds);
        }
    }
}
