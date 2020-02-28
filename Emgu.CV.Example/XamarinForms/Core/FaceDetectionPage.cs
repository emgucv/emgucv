//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Models;
using Emgu.Util;
using FaceDetection;

namespace Emgu.CV.XamarinForms
{
    public class FaceDetectionPage : ButtonTextImagePage
    {
        public FaceDetectionPage()
           : base()
        {

            var button = this.GetButton();
            button.Text = "Perform Face Detection";
            button.Clicked += OnButtonClicked;

            OnImagesLoaded += async (sender, image) =>
            {
                if (image == null || image[0] == null)
                    return;
                SetMessage("Please wait...");
                //SetImage(image[0]);
                SetImage(null);

                FileDownloadManager downloadManager = new FileDownloadManager();
                String url = "https://github.com/opencv/opencv/raw/4.2.0/data/haarcascades/";
                downloadManager.AddFile(url + "/haarcascade_frontalface_default.xml", "haarcascade");
                downloadManager.AddFile(url + "/haarcascade_eye.xml", "haarcascade");

                downloadManager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;

                await downloadManager.Download();

                String faceFile = downloadManager.Files[0].LocalFile;
                String eyeFile = downloadManager.Files[1].LocalFile;
                long time;
                List<Rectangle> faces = new List<Rectangle>();
                List<Rectangle> eyes = new List<Rectangle>();

                using (UMat img = image[0].GetUMat(AccessType.ReadWrite))
                    DetectFace.Detect(img, faceFile, eyeFile, faces, eyes, out time);

                //Draw the faces in red
                foreach (Rectangle rect in faces)
                    CvInvoke.Rectangle(image[0], rect, new MCvScalar(0, 0, 255), 2);

                //Draw the eyes in blue
                foreach (Rectangle rect in eyes)
                    CvInvoke.Rectangle(image[0], rect, new MCvScalar(255, 0, 0), 2);

                String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
                SetMessage(String.Format("Detected with {1} in {0} milliseconds.", time, computeDevice));

                SetImage(image[0]);

            };
        }

        private void DownloadManager_OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive <= 0)
                SetMessage(String.Format("{0} bytes downloaded.", e.BytesReceived));
            else
                SetMessage(String.Format("{0} of {1} bytes downloaded ({2}%)", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage));

        }

        private void OnButtonClicked(Object sender, EventArgs args)
        {
            LoadImages(new string[] { "lena.jpg" });
        }

    }
}
