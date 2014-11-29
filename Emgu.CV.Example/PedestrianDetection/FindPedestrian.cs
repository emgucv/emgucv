//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
#if !(IOS || NETFX_CORE)
using Emgu.CV.Cuda;
#endif

namespace PedestrianDetection
{
   public static class FindPedestrian
   {
      /// <summary>
      /// Find the pedestrian in the image
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="processingTime">The pedestrian detection time in milliseconds</param>
      /// <returns>The region where pedestrians are detected</returns>
      public static Rectangle[] Find(Mat image, bool tryUseCuda, bool tryUseOpenCL, out long processingTime)
      {
         Stopwatch watch;
         Rectangle[] regions;

#if !(IOS || NETFX_CORE)
         //check if there is a compatible Cuda device to run pedestrian detection
         if (tryUseCuda && CudaInvoke.HasCuda)
         {  //this is the Cuda version
            using (CudaHOGDescriptor des = new CudaHOGDescriptor())
            {
               des.SetSVMDetector(CudaHOGDescriptor.GetDefaultPeopleDetector());

               watch = Stopwatch.StartNew();
               using (GpuMat cudaBgr = new GpuMat(image))
               using (GpuMat cudaBgra = new GpuMat() )
               {
                  CudaInvoke.CvtColor(cudaBgr, cudaBgra, ColorConversion.Bgr2Bgra);
                  regions = des.DetectMultiScale(cudaBgra);
               }
            }
         }
         else
#endif
         {  
            //Many opencl functions require opencl compatible gpu devices. 
            //As of opencv 3.0-alpha, opencv will crash if opencl is enable and only opencv compatible cpu device is presented
            //So we need to call CvInvoke.HaveOpenCLCompatibleGpuDevice instead of CvInvoke.HaveOpenCL (which also returns true on a system that only have cpu opencl devices).
            CvInvoke.UseOpenCL = tryUseOpenCL && CvInvoke.HaveOpenCLCompatibleGpuDevice;

            //this is the CPU/OpenCL version
            using (HOGDescriptor des = new HOGDescriptor())
            {
               des.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
               
               //load the image to umat so it will automatically use opencl is available
               UMat umat = image.ToUMat(AccessType.Read);

               watch = Stopwatch.StartNew();
               
               MCvObjectDetection[] results = des.DetectMultiScale(umat);
               regions = new Rectangle[results.Length];
               for (int i = 0; i < results.Length; i++)
                  regions[i] = results[i].Rect;
               watch.Stop();
            }
         }
        
         processingTime = watch.ElapsedMilliseconds;

         return regions;
      }
   }
}
