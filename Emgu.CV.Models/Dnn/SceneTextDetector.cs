//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class SceneTextDetector
    {
        public class SceneText
        {
            public Point[] Region;
            public float Confident;
        }

        private String _modelFolderName = "scene_text_detector";
        private TextDetectionModel_DB _textDetector = null;

        private TextRecognitionModel _ocr = null;

        /// <summary>
        /// Download and initialize the vehicle detector, the license plate detector and OCR.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            await InitTextDetector(onDownloadProgressChanged);
        }

        private async Task InitTextDetector(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_textDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/emgucv/models/raw/master/scene_text/DB_TD500_resnet50.onnx",
                    _modelFolderName,
                    "1b4dd21a6baa5e3523156776970895bd3db6960a");
                
                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                await manager.Download();

                _textDetector = new TextDetectionModel_DB(manager.Files[0].LocalFile);
                _textDetector.BinaryThreshold = 0.3f;
                _textDetector.PolygonThreshold = 0.5f;
                _textDetector.MaxCandidates = 200;
                _textDetector.UnclipRatio = 2.0;
                _textDetector.SetInputScale(1.0/255.0);
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

        
        /*
        /// <summary>
        /// Download and initialize the vehicle detector, the license plate detector and OCR.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            await InitLicensePlateDetector(onDownloadProgressChanged);
            await InitVehicleAttributesRecognizer(onDownloadProgressChanged);
            await InitOCR(onDownloadProgressChanged);
        }
        */

        /// <summary>
        /// Detect scene text from the given image
        /// </summary>
        /// <param name="image">The image</param>
        /// <returns>The detected scene text.</returns>
        public SceneText[] Detect(Mat image)
        {
            using (VectorOfVectorOfPoint vvp = new VectorOfVectorOfPoint())
            using (VectorOfFloat confidents = new VectorOfFloat())
            {
                _textDetector.Detect(image, vvp, confidents);

                Point[][] detectionResults = vvp.ToArrayOfArray();
                float[] confidentResult = confidents.ToArray();
                List<SceneText> results = new List<SceneText>();
                for (int i = 0; i < detectionResults.Length; i++)
                {
                    SceneText st = new SceneText();
                    st.Region = detectionResults[i];
                    st.Confident = confidents[i];
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
        public void Render(Mat image, SceneText[] sceneTexts)
        {
            foreach (SceneText st in sceneTexts)
            {
                CvInvoke.Polylines(image, st.Region, true, new MCvScalar(0, 0, 255));
                /*
                String label = String.Format("{0} {1} {2}",
                    v.Color, v.Type, v.LicensePlate == null ? String.Empty : v.LicensePlate.Value.Text);
                CvInvoke.PutText(
                    image,
                    label,
                    new Point(v.Region.Location.X, v.Region.Location.Y + 20),
                    FontFace.HersheyComplex,
                    1.0,
                    new MCvScalar(0, 255, 0),
                    2);*/
            }
        }
    }
}
