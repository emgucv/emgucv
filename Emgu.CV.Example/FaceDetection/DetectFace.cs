//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cuda;

namespace FaceDetection
{
   public static class DetectFace
   {
      public static void Detect(Image<Bgr, Byte> image, String faceFileName, String eyeFileName, List<Rectangle> faces, List<Rectangle> eyes, out long detectionTime)
      {
         Stopwatch watch;

         if (CudaInvoke.HasCuda)
         {
            using (CudaCascadeClassifier face = new CudaCascadeClassifier(faceFileName))
            using (CudaCascadeClassifier eye = new CudaCascadeClassifier(eyeFileName))
            {
               watch = Stopwatch.StartNew();
               using (CudaImage<Bgr, Byte> gpuImage = new CudaImage<Bgr, byte>(image))
               using (CudaImage<Gray, Byte> gpuGray = gpuImage.Convert<Gray, Byte>())
               {
                  Rectangle[] faceRegion = face.DetectMultiScale(gpuGray, 1.1, 10, Size.Empty);
                  faces.AddRange(faceRegion);
                  foreach (Rectangle f in faceRegion)
                  {
                     using (CudaImage<Gray, Byte> faceImg = gpuGray.GetSubRect(f))
                     {
                        //For some reason a clone is required.
                        //Might be a bug of CudaCascadeClassifier in opencv
                        using (CudaImage<Gray, Byte> clone = faceImg.Clone(null))
                        {
                           Rectangle[] eyeRegion = eye.DetectMultiScale(clone, 1.1, 10, Size.Empty);

                           foreach (Rectangle e in eyeRegion)
                           {
                              Rectangle eyeRect = e;
                              eyeRect.Offset(f.X, f.Y);
                              eyes.Add(eyeRect);
                           }
                        }
                     }
                  }
               }
               watch.Stop();
            }
         }
         else
         {
            bool tryUseOpenCL = true;
            bool usingOpenCL = false;
            if (CvInvoke.HaveOpenCL)
            {
               CvInvoke.UseOpenCL = tryUseOpenCL;
               usingOpenCL = true;
            }

            //Read the HaarCascade objects
            using (CascadeClassifier face = new CascadeClassifier(faceFileName))
            using (CascadeClassifier eye = new CascadeClassifier(eyeFileName))
            {
               watch = Stopwatch.StartNew();
               using (UMat uimg = image.Mat.ToUMat(Emgu.CV.CvEnum.AccessType.Read)) //Convert it to Grayscale
               using (UMat ugray = new UMat())
               {
                  CvInvoke.CvtColor(uimg, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                  //normalizes brightness and increases contrast of the image
                  CvInvoke.EqualizeHist(ugray, ugray);

                  //Detect the faces  from the gray scale image and store the locations as rectangle
                  //The first dimensional is the channel
                  //The second dimension is the index of the rectangle in the specific channel
                  Rectangle[] facesDetected = face.DetectMultiScale(
                     ugray,
                     1.1,
                     10,
                     new Size(20, 20));
                     
                  faces.AddRange(facesDetected);

                  foreach (Rectangle f in facesDetected)
                  {
                     //Get the region of interest on the faces
                     using (UMat faceRegion = new UMat(ugray, f))
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
               watch.Stop();
            }
         }
         detectionTime = watch.ElapsedMilliseconds;
      }
   }
}
