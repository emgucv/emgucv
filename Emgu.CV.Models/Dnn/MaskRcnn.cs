//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
    public class MaskRcnn : DisposableObject
    {
        private String _modelFolderName = "mask_rcnn_inception_v2_coco_2018_01_28";

        private string[] _labels = null;

        private MCvScalar[] _colors = null;

        private String[] _objectsOfInterest = null;

        public String[] ObjectsOfInterest
        {
            get { return _objectsOfInterest; }
            set { _objectsOfInterest = value; }
        }

        private Net _maskRcnnDetector = null;

        /// <summary>
        /// Download and initialize the Mask Rcnn model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Asyn task</returns>
        public async Task Init(
            System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
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

                await manager.Download();

                if (manager.AllFilesDownloaded)
                {
                    _maskRcnnDetector = Emgu.CV.Dnn.DnnInvoke.ReadNetFromTensorflow(manager.Files[0].LocalFile, manager.Files[1].LocalFile);

                    _labels = File.ReadAllLines(manager.Files[2].LocalFile);

                    //prefer cuda backend if available
                    foreach (BackendTargetPair p in DnnInvoke.AvailableBackends)
                    {
                        if (p.Backend == Dnn.Backend.Cuda && p.Target == Target.Cuda)
                        {
                            _maskRcnnDetector.SetPreferableBackend(Dnn.Backend.Cuda);
                            _maskRcnnDetector.SetPreferableTarget(Target.Cuda);
                            break;
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

        public void DetectAndRender(Mat m, float matchScoreThreshold = 0.5f, float nmsThreshold = 0.4f)
        {
            MaskedObject[] objects = Detect(m, matchScoreThreshold, nmsThreshold);
            foreach (var obj in objects)
            {
                obj.Render(m, _colors[obj.ClassId]);
                obj.Dispose();
            }
        }


        public MaskedObject[] Detect(Mat m, float matchScoreThreshold = 0.5f, float nmsThreshold = 0.4f)
        {
            using (Mat blob = DnnInvoke.BlobFromImage(m))
            using (VectorOfMat tensors = new VectorOfMat())
            {
                _maskRcnnDetector.SetInput(blob, "image_tensor");
                _maskRcnnDetector.Forward(tensors, new string[] { "detection_out_final", "detection_masks" });

                using (Mat boxes = tensors[0])
                using (Mat masks = tensors[1])
                {
                    System.Drawing.Size imgSize = m.Size;
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
        /// Release the memory associated with this Mask Rcnn detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_maskRcnnDetector != null)
            {
                _maskRcnnDetector.Dispose();
                _maskRcnnDetector = null;
            }
            //throw new NotImplementedException();
        }
    }
}
