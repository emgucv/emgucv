//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Cuda;

namespace FaceDetection
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Run();
      }

      static void Run()
      {
         Mat image = new Mat("lena.jpg", LoadImageType.Color); //Read the files as an 8-bit Bgr image  
         long detectionTime;
         List<Rectangle> faces = new List<Rectangle>();
         List<Rectangle> eyes = new List<Rectangle>();

         //The cuda cascade classifier doesn't seem to be able to load "haarcascade_frontalface_default.xml" file in this release
         //disabling CUDA module for now
         bool tryUseCuda = false;
         bool tryUseOpenCL = true;

         DetectFace.Detect(
           image, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml", 
           faces, eyes,
           tryUseCuda,
           tryUseOpenCL,
           out detectionTime);

         foreach (Rectangle face in faces)
            CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
         foreach (Rectangle eye in eyes)
            CvInvoke.Rectangle(image, eye, new Bgr(Color.Blue).MCvScalar, 2);

         //display the image 
         ImageViewer.Show(image, String.Format(
            "Completed face and eye detection using {0} in {1} milliseconds", 
            (tryUseCuda && CudaInvoke.HasCuda) ? "GPU"
            : (tryUseOpenCL && CvInvoke.HaveOpenCLCompatibleGpuDevice) ? "OpenCL" 
            : "CPU",
            detectionTime));
      }

   }
}