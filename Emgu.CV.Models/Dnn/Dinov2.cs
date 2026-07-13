//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// DINOv2 (small) self-supervised image embedding model running on the dnn
    /// module. Images are embedded into a 384 dimensional space where visually
    /// similar images are close, enabling image retrieval, near-duplicate
    /// detection and clustering. Unlike CLIP/SigLIP the embedding is not
    /// aligned with text: it captures fine-grained visual similarity instead.
    ///
    /// The ONNX model (~85 MB, fp32) is the raw `optimum-cli export onnx
    /// --model facebook/dinov2-small` export.
    /// </summary>
    public class Dinov2 : DisposableObject
    {
        private const int ImageSize = 224;
        private const int ResizeShortestEdge = 256;

        private String _modelFolderName = Path.Combine("emgu", "dinov2_small_onnx");

        private Net _net = null;

        /// <summary>
        /// Create a DINOv2 embedding model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public Dinov2(String modelFolderName = null)
        {
            if (modelFolderName != null)
                _modelFolderName = modelFolderName;
        }

        /// <summary>
        /// Return true if the model is initialized.
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _net != null;
            }
        }

        /// <summary>
        /// Download and initialize the DINOv2 model.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (!Initialized)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/dinov2_small_onnx/model.onnx",
                    _modelFolderName,
                    "0A5C239A312F941761E9367BE409D43CD21F9E1862E6C230897A5C2B1140FD57");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                    _net = DnnInvoke.ReadNetFromONNX(manager.Files[0].LocalFile, EngineType.New);
#else
                    String modelPath = manager.Files[0].LocalFile;
                    _net = await Task.Run(() => DnnInvoke.ReadNetFromONNX(modelPath, EngineType.New));
#endif
                }
            }
        }

        /// <summary>
        /// DINOv2 image preprocessing: resize the shortest side to 256
        /// (bicubic), center crop 224x224, convert to RGB and normalize with
        /// the imagenet per-channel mean and std.
        /// </summary>
        private static Mat PreprocessImage(Mat image)
        {
            int w = image.Width, h = image.Height;
            double scale = ResizeShortestEdge / (double)Math.Min(w, h);
            int resizedW = Math.Max(ResizeShortestEdge, (int)Math.Round(w * scale));
            int resizedH = Math.Max(ResizeShortestEdge, (int)Math.Round(h * scale));

            using (Mat resized = new Mat())
            using (ScalarArray mean = new ScalarArray(new MCvScalar(0.485, 0.456, 0.406)))
            using (ScalarArray std = new ScalarArray(new MCvScalar(0.229, 0.224, 0.225)))
            {
                CvInvoke.Resize(image, resized, new Size(resizedW, resizedH), 0, 0, Inter.Cubic);
                Rectangle crop = new Rectangle((resizedW - ImageSize) / 2, (resizedH - ImageSize) / 2, ImageSize, ImageSize);
                using (Mat cropped = new Mat(resized, crop))
                using (Mat rgb = new Mat())
                using (Mat f32 = new Mat())
                {
                    CvInvoke.CvtColor(cropped, rgb, ColorConversion.Bgr2Rgb);
                    rgb.ConvertTo(f32, DepthType.Cv32F, 1.0 / 255.0);
                    CvInvoke.Subtract(f32, mean, f32, null, DepthType.Cv32F);
                    CvInvoke.Divide(f32, std, f32, 1.0, DepthType.Cv32F);
                    return DnnInvoke.BlobFromImage(f32);
                }
            }
        }

        /// <summary>
        /// Embed the given image into the visual similarity space.
        /// </summary>
        /// <param name="image">The image (BGR).</param>
        /// <returns>The L2-normalized embedding (384 values for the small model), the class token of the last hidden state.</returns>
        public float[] EmbedImage(IInputArray image)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            using (Mat pixelValues = PreprocessImage(mat))
            {
                _net.SetInput(pixelValues, "pixel_values");
                using (Mat lastHiddenState = _net.Forward())   //(1, tokens, dim)
                {
                    //The class token (position 0) is the image level descriptor.
                    int dim = lastHiddenState.SizeOfDimension[2];
                    float[] result = new float[dim];
                    Marshal.Copy(lastHiddenState.DataPointer, result, 0, dim);

                    double norm = 0;
                    for (int i = 0; i < dim; i++)
                        norm += (double)result[i] * result[i];
                    norm = Math.Sqrt(norm);
                    if (norm > 0)
                    {
                        for (int i = 0; i < dim; i++)
                            result[i] = (float)(result[i] / norm);
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// Compute the cosine similarity between two embeddings.
        /// </summary>
        /// <param name="a">The first embedding.</param>
        /// <param name="b">The second embedding.</param>
        /// <returns>The cosine similarity, in [-1, 1] for L2-normalized embeddings.</returns>
        public static double CosineSimilarity(float[] a, float[] b)
        {
            return Clip.CosineSimilarity(a, b);
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before use.
        /// </summary>
        public void Clear()
        {
            if (_net != null)
            {
                _net.Dispose();
                _net = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this DINOv2 model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
