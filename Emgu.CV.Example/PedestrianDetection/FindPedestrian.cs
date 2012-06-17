//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using Emgu.CV.GPU;

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
      public static Rectangle[] Find(Image<Bgr, Byte> image, out long processingTime)
      {
         Stopwatch watch;
         Rectangle[] regions;

         //check if there is a compatible GPU to run pedestrian detection
         if (GpuInvoke.HasCuda)
         {  //this is the GPU version
            using (GpuHOGDescriptor des = new GpuHOGDescriptor())
            {
               des.SetSVMDetector(GpuHOGDescriptor.GetDefaultPeopleDetector());

               watch = Stopwatch.StartNew();
               using (GpuImage<Bgr, Byte> gpuImg = new GpuImage<Bgr, byte>(image))
               using (GpuImage<Bgra, Byte> gpuBgra = gpuImg.Convert<Bgra, Byte>())
               {
                  regions = des.DetectMultiScale(gpuBgra);
               }
            }
         }
         else
         {  //this is the CPU version
            using (HOGDescriptor des = new HOGDescriptor())
            {
               des.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

               watch = Stopwatch.StartNew();
               regions = des.DetectMultiScale(image);
            }
         }
         watch.Stop();

         processingTime = watch.ElapsedMilliseconds;

         return regions;
      }
   }
}
