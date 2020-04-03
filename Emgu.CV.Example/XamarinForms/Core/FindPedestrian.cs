//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using Emgu.CV.Util;
using Emgu.CV.Cuda;

namespace PedestrianDetection
{
   public static class FindPedestrian
   {
      /// <summary>
      /// Find the pedestrian in the image
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="processingTime">The processing time in milliseconds</param>
      /// <returns>The region where pedestrians are detected</returns>
      public static Rectangle[] Find(IInputArray image)
      {
         //Stopwatch watch;
         Rectangle[] regions;

         using (InputArray iaImage = image.GetInputArray())
         {

            //if the input array is a GpuMat
            //check if there is a compatible Cuda device to run pedestrian detection
            if (iaImage.Kind == InputArray.Type.CudaGpuMat)
            {
               //this is the Cuda version
               using (CudaHOG des = new CudaHOG(new Size(64, 128), new Size(16, 16), new Size(8, 8), new Size(8, 8)))
               {
                  des.SetSVMDetector(des.GetDefaultPeopleDetector());

                  //watch = Stopwatch.StartNew();
                  using (GpuMat cudaBgra = new GpuMat())
                  using (VectorOfRect vr = new VectorOfRect())
                  {
                     CudaInvoke.CvtColor(image, cudaBgra, ColorConversion.Bgr2Bgra);
                     des.DetectMultiScale(cudaBgra, vr);
                     regions = vr.ToArray();
                  }
               }
            }
            else
            {
               //this is the CPU/OpenCL version
               using (HOGDescriptor des = new HOGDescriptor())
               {
                  des.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
                  //watch = Stopwatch.StartNew();

                  MCvObjectDetection[] results = des.DetectMultiScale(image);
                  regions = new Rectangle[results.Length];
                  for (int i = 0; i < results.Length; i++)
                     regions[i] = results[i].Rect;
                  //watch.Stop();
               }
            }

            //processingTime = watch.ElapsedMilliseconds;

            return regions;
         }
      }
   }
}
