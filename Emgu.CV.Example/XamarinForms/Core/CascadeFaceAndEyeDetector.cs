//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV.Models
{
    public class CascadeFaceAndEyeDetector : IProcessAndRenderModel
    {
        private CascadeClassifier _faceCascadeClassifier = null;
        private CascadeClassifier _eyeCascadeClassifier = null;

        public static void Detect(
           IInputArray image, CascadeClassifier face, CascadeClassifier eye,
           List<Rectangle> faces, List<Rectangle> eyes)
        {
            using (Mat gray = new Mat())
            {
                CvInvoke.CvtColor(image, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                //normalizes brightness and increases contrast of the image
                CvInvoke.EqualizeHist(gray, gray);

                //Detect the faces from the gray scale image and store the locations as rectangle
                //The first dimensional is the channel
                //The second dimension is the index of the rectangle in the specific channel                     
                Rectangle[] facesDetected = face.DetectMultiScale(
                   gray,
                   1.1,
                   10,
                   new Size(20, 20));

                faces.AddRange(facesDetected);

                foreach (Rectangle f in facesDetected)
                {
                    //Get the region of interest on the faces
                    using (Mat faceRegion = new Mat(gray, f))
                    {
                        Rectangle[] eyesDetected = eye.DetectMultiScale(
                           faceRegion,
                           1.1,
                           10,
                           new Size(20, 20));

                        foreach (Rectangle e in eyesDetected)
                        {
                            Rectangle eyeRect = e;
                            eyeRect.Offset(f.X, f.Y);
                            eyes.Add(eyeRect);
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Download and initialize the face and eye cascade classifier detection model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Asyn task</returns>
        public async Task Init(DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_faceCascadeClassifier == null && _eyeCascadeClassifier == null)
            {
                FileDownloadManager downloadManager = new FileDownloadManager();
                String url = "https://github.com/opencv/opencv/raw/4.2.0/data/haarcascades/";
                downloadManager.AddFile(url + "/haarcascade_frontalface_default.xml", "haarcascade");
                downloadManager.AddFile(url + "/haarcascade_eye.xml", "haarcascade");

                downloadManager.OnDownloadProgressChanged += onDownloadProgressChanged;

                await downloadManager.Download();
                String faceFile = downloadManager.Files[0].LocalFile;
                String eyeFile = downloadManager.Files[1].LocalFile;
                _faceCascadeClassifier = new CascadeClassifier(faceFile);
                _eyeCascadeClassifier = new CascadeClassifier(eyeFile);
            }
        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();

            Stopwatch watch = Stopwatch.StartNew();
            Detect(imageIn, _faceCascadeClassifier, _eyeCascadeClassifier, faces, eyes);
            watch.Stop();

            if (imageOut != imageIn)
            {
                using (InputArray iaImageIn = imageIn.GetInputArray())
                {
                    iaImageIn.CopyTo(imageOut);
                }
            }

            //Draw the faces in red
            foreach (Rectangle rect in faces)
                CvInvoke.Rectangle(imageOut, rect, new MCvScalar(0, 0, 255), 2);

            //Draw the eyes in blue
            foreach (Rectangle rect in eyes)
                CvInvoke.Rectangle(imageOut, rect, new MCvScalar(255, 0, 0), 2);

            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);

        }
    }
}
