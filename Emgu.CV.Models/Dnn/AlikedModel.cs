//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Features;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// ALIKED deep learning feature extractor with optional LightGlue matcher.
    /// Automatically downloads the required ONNX models on first use.
    /// </summary>
    public class AlikedModel : DisposableObject
    {
        private readonly String _modelFolderName = Path.Combine("emgu", "aliked");

        private ALIKED _aliked = null;
        private LightGlueMatcher _lightGlue = null;

        private String _alikedModelFile = null;
        private String _lightGlueModelFile = null;

        private readonly bool _withLightGlue;

        /// <summary>
        /// Create an AlikedModel instance.
        /// </summary>
        /// <param name="withLightGlue">
        /// When true (the default), also downloads and initialises the LightGlue matcher so
        /// that <see cref="Match"/> can be used to find correspondences between two images.
        /// When false, only the ALIKED extractor is downloaded.
        /// </param>
        public AlikedModel(bool withLightGlue = true)
        {
            _withLightGlue = withLightGlue;
        }

        /// <summary>
        /// Return true if the model has been initialised.
        /// </summary>
        public bool Initialized => _alikedModelFile != null && (!_withLightGlue || _lightGlueModelFile != null);

        /// <summary>
        /// Download and initialise the ALIKED (and optionally LightGlue) models.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback invoked during file download.</param>
        /// <param name="initOptions">Reserved; pass null.</param>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#endif
        {
            if (!Initialized)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://raw.githubusercontent.com/YangGuanyuhan/lightglue_opencv_project/main/model/aliked-n16rot-top1k-640.onnx",
                    _modelFolderName);

                if (_withLightGlue)
                {
                    manager.AddFile(
                        "https://raw.githubusercontent.com/YangGuanyuhan/lightglue_opencv_project/main/model/aliked_lightglue.onnx",
                        _modelFolderName);
                }

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _alikedModelFile = manager.Files[0].LocalFile;
                    if (_withLightGlue)
                        _lightGlueModelFile = manager.Files[1].LocalFile;
                }
            }
        }

        private ALIKED GetAliked()
        {
            if (_aliked == null)
                _aliked = new ALIKED(_alikedModelFile);
            return _aliked;
        }

        private LightGlueMatcher GetLightGlue()
        {
            if (_lightGlue == null)
                _lightGlue = new LightGlueMatcher(_lightGlueModelFile);
            return _lightGlue;
        }

        /// <summary>
        /// Detect keypoints and compute descriptors for an image.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="keypoints">The detected keypoints.</param>
        /// <param name="descriptors">The computed descriptors (one row per keypoint).</param>
        public void DetectAndDescribe(IInputArray image, out MKeyPoint[] keypoints, out Mat descriptors)
        {
            descriptors = new Mat();
            using (VectorOfKeyPoint vkp = new VectorOfKeyPoint())
            {
                GetAliked().DetectAndCompute(image, null, vkp, descriptors, false);
                keypoints = vkp.ToArray();
            }
        }

        /// <summary>
        /// Detect keypoints and compute descriptors, then use LightGlue to find matches
        /// between the query image and the train image.
        /// </summary>
        /// <param name="queryImage">The first (query) image.</param>
        /// <param name="trainImage">The second (train) image.</param>
        /// <returns>The matched keypoint pairs, one <see cref="MDMatch"/> per accepted match.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the model was constructed with <c>withLightGlue=false</c>.
        /// </exception>
        public MDMatch[] Match(IInputArray queryImage, IInputArray trainImage)
        {
            if (!_withLightGlue)
                throw new InvalidOperationException("LightGlue was not enabled. Construct AlikedModel with withLightGlue=true.");

            MKeyPoint[] queryKpts, trainKpts;
            Mat queryDesc, trainDesc;
            DetectAndDescribe(queryImage, out queryKpts, out queryDesc);
            DetectAndDescribe(trainImage, out trainKpts, out trainDesc);

            Size querySize, trainSize;
            using (InputArray iaQ = queryImage.GetInputArray())
                querySize = iaQ.GetSize();
            using (InputArray iaT = trainImage.GetInputArray())
                trainSize = iaT.GetSize();

            using (Mat qKptsMat = KeyPointsToMat(queryKpts))
            using (Mat tKptsMat = KeyPointsToMat(trainKpts))
            {
                GetLightGlue().SetPairInfo(qKptsMat, tKptsMat, querySize, trainSize);
            }

            using (VectorOfVectorOfDMatch vvMatches = new VectorOfVectorOfDMatch())
            {
                GetLightGlue().KnnMatch(queryDesc, trainDesc, vvMatches, 1);
                List<MDMatch> result = new List<MDMatch>();
                MDMatch[][] all = vvMatches.ToArrayOfArray();
                foreach (MDMatch[] group in all)
                    if (group.Length > 0)
                        result.Add(group[0]);
                return result.ToArray();
            }
        }

        private static Mat KeyPointsToMat(MKeyPoint[] keypoints)
        {
            Mat m = new Mat(keypoints.Length, 2, CvEnum.DepthType.Cv32F, 1);
            float[] data = new float[keypoints.Length * 2];
            for (int i = 0; i < keypoints.Length; i++)
            {
                data[i * 2] = keypoints[i].Point.X;
                data[i * 2 + 1] = keypoints[i].Point.Y;
            }
            m.SetTo(data);
            return m;
        }

        /// <inheritdoc />
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// Release the ALIKED and LightGlue models.
        /// </summary>
        public void Clear()
        {
            if (_aliked != null)
            {
                _aliked.Dispose();
                _aliked = null;
            }

            if (_lightGlue != null)
            {
                _lightGlue.Dispose();
                _lightGlue = null;
            }
        }
    }
}
