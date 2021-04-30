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
    /// MaskRcnn model
    /// </summary>
    public class MaskRcnn : DisposableObject, IProcessAndRenderModel
    {
        private String _modelFolderName = "mask_rcnn_inception_v2_coco_2018_01_28";

        private string[] _labels = null;

        private MCvScalar[] _colors = null;

        private String[] _objectsOfInterest = null;

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
        //private Model _maskRcnnModel = null;

        /// <summary>
        /// Download and initialize the Mask Rcnn model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <param name="initOptions">A string, such as "OpenCV;CPU", in the format of "{backend};{option}"</param>
        /// <returns>Asyn task</returns>
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
            if (_maskRcnnDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/emgucv/models/raw/master/mask_rcnn_inception_v2_coco_2018_01_28/frozen_inference_graph.pb",
                    _modelFolderName,
                    "AC9B51CDE227B24D20030042E6C1E29AF75668F509E51AA84ED686787CCCC309");
                manager.AddFile(
                    "https://github.com/opencv/opencv_extra/raw/4.1.0/testdata/dnn/mask_rcnn_inception_v2_coco_2018_01_28.pbtxt",
                    _modelFolderName,
                    "090923144D81B2E442093F965BE27ECCDA11CE5F781EAF1C7EC76932CE99641E");
                manager.AddFile(
                    "https://github.com/emgucv/models/raw/master/mask_rcnn_inception_v2_coco_2018_01_28/coco-labels-paper.txt",
                    _modelFolderName,
                    "8925173E1B0AABFAEFDA27DE2BB908233BB8FB6E7582323D72988E4BE15A5F0B");

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _maskRcnnDetector = Emgu.CV.Dnn.DnnInvoke.ReadNetFromTensorflow(manager.Files[0].LocalFile, manager.Files[1].LocalFile);
                    /*
                    _maskRcnnModel = new Model(manager.Files[0].LocalFile, manager.Files[1].LocalFile);
                    _maskRcnnModel.SetInputSize(new Size(-1, -1));
                    _maskRcnnModel.SetInputCrop(false);
                    _maskRcnnModel.SetInputMean(new MCvScalar());
                    _maskRcnnModel.SetInputSwapRB(false);
                    */

                    _labels = File.ReadAllLines(manager.Files[2].LocalFile);

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
                    

                    _colors = new MCvScalar[_labels.Length];
                    Random r = new Random(12345);
                    for (int i = 0; i < _colors.Length; i++)
                    {
                        _colors[i] = new MCvScalar(r.Next(256), r.Next(256), r.Next(256));
                    }
                }
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
            if (imageOut != imageIn)
            {
                using (InputArray iaImageIn = imageIn.GetInputArray())
                {
                    iaImageIn.CopyTo(imageOut);
                }
            }
            foreach (var obj in objects)
            {
                obj.Render(imageOut, new MCvScalar(0, 0, 255), _colors[obj.ClassId]);
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
            using (Mat blob = DnnInvoke.BlobFromImage(m))
            using (VectorOfMat tensors = new VectorOfMat())
            //using(VectorOfMat outputTensors = new VectorOfMat())
            {
                _maskRcnnDetector.SetInput(blob, "image_tensor");
                _maskRcnnDetector.Forward(tensors, new string[] { "detection_out_final", "detection_masks" });
                //_maskRcnnModel.Predict(m, outputTensors);

                using (Mat boxes = tensors[0])
                using (Mat masks = tensors[1])
                {
                    System.Drawing.Size imgSize = iaM.GetSize();
                    float[,,,] boxesData = boxes.GetData(true) as float[,,,];
                    int numDetections = boxesData.GetLength(2);

                    List<int> classIds = new List<int>();
                    List<Rectangle> regions = new List<Rectangle>();
                    List<float> scores = new List<float>();

                    for (int i = 0; i < numDetections; i++)
                    {
                        int classId = (int)boxesData[0, 0, i, 1];

                        if (_objectsOfInterest == null || _objectsOfInterest.Contains(_labels[classId]))
                        {
                            float score = boxesData[0, 0, i, 2];
                            Rectangle rect = DetectedObject.GetRectangle(
                                boxesData[0, 0, i, 3],
                                boxesData[0, 0, i, 4],
                                boxesData[0, 0, i, 5],
                                boxesData[0, 0, i, 6],
                                imgSize.Width,
                                imgSize.Height);
                            rect.Intersect(new Rectangle(Point.Empty, imgSize));

                            regions.Add(rect);
                            scores.Add(score);
                            classIds.Add(classId);
                        }

                    }
                    int[] validIdx = DnnInvoke.NMSBoxes(regions.ToArray(), scores.ToArray(), matchScoreThreshold, nmsThreshold);
                    List<MaskedObject> maskedObjects = new List<MaskedObject>();

                    for (int i = 0; i < validIdx.Length; i++)
                    {
                        int idx = validIdx[i];
                        int classId = classIds[idx];
                        Rectangle rect = regions[idx];
                        float score = scores[idx];

                        int[] masksDim = masks.SizeOfDimension;
                        using (Mat mask = new Mat(
                            masksDim[2],
                            masksDim[3],
                            DepthType.Cv32F,
                            1,
                            masks.GetDataPointer(i, classId),
                            masksDim[3] * masks.ElementSize))
                        {

                            MaskedObject mo = new MaskedObject(classId, _labels[classId], score, rect, mask);
                            maskedObjects.Add(mo);

                        }
                    }

                    return maskedObjects.ToArray();

                }
            }
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
