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
    public class FaceDetector
    {
        private String _modelFolderName = "dnn_samples_face_detector_20170830";

        private Net _faceDetector = null;

        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_faceDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/opencv/opencv_3rdparty/raw/dnn_samples_face_detector_20170830/res10_300x300_ssd_iter_140000.caffemodel",
                    _modelFolderName);

                manager.AddFile(
                    "https://raw.githubusercontent.com/opencv/opencv/4.0.1/samples/dnn/face_detector/deploy.prototxt",
                    _modelFolderName);

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                await manager.Download();
                _faceDetector = DnnInvoke.ReadNetFromCaffe(manager.Files[1].LocalFile, manager.Files[0].LocalFile);

                if (Emgu.CV.Cuda.CudaInvoke.HasCuda)
                {
                    _faceDetector.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                    _faceDetector.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
                }
            }
        }

        public void Detect(Mat image, List<Rectangle> fullFaceRegions, List<Rectangle> partialFaceRegions)
        {
            int imgDim = 300;
            MCvScalar meanVal = new MCvScalar(104, 177, 123);
            Size imageSize = image.Size;
            using (Mat inputBlob = DnnInvoke.BlobFromImage(
                image,
                1.0,
                new Size(imgDim, imgDim),
                meanVal,
                false,
                false))
                _faceDetector.SetInput(inputBlob, "data");
            using (Mat detection = _faceDetector.Forward("detection_out"))
            {
                float confidenceThreshold = 0.5f;

                //List<Rectangle> fullFaceRegions = new List<Rectangle>();
                //List<Rectangle> partialFaceRegions = new List<Rectangle>();
                Rectangle imageRegion = new Rectangle(Point.Empty, image.Size);

                float[,,,] values = detection.GetData(true) as float[,,,];
                for (int i = 0; i < values.GetLength(2); i++)
                {
                    float confident = values[0, 0, i, 2];

                    if (confident > confidenceThreshold)
                    {
                        float xLeftBottom = values[0, 0, i, 3] * imageSize.Width;
                        float yLeftBottom = values[0, 0, i, 4] * imageSize.Height;
                        float xRightTop = values[0, 0, i, 5] * imageSize.Width;
                        float yRightTop = values[0, 0, i, 6] * imageSize.Height;
                        RectangleF objectRegion = new RectangleF(
                            xLeftBottom,
                            yLeftBottom,
                            xRightTop - xLeftBottom,
                            yRightTop - yLeftBottom);
                        Rectangle faceRegion = Rectangle.Round(objectRegion);

                        if (imageRegion.Contains(faceRegion))
                            fullFaceRegions.Add(faceRegion);
                        else
                        {
                            partialFaceRegions.Add(faceRegion);
                        }
                    }
                }
            }
        }
    }
}
