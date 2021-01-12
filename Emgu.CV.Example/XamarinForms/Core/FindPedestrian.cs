//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
        /// <returns>The region where pedestrians are detected</returns>
        public static Rectangle[] Find(IInputArray image, HOGDescriptor hog, CudaHOG hogCuda = null)
        {
            //Stopwatch watch;
            Rectangle[] regions;

            using (InputArray iaImage = image.GetInputArray())
            {
                //if the input array is a GpuMat
                //check if there is a compatible Cuda device to run pedestrian detection
                if (iaImage.Kind == InputArray.Type.CudaGpuMat && hogCuda != null)
                {
                    //this is the Cuda version

                    

                    //watch = Stopwatch.StartNew();
                    using (GpuMat cudaBgra = new GpuMat())
                    using (VectorOfRect vr = new VectorOfRect())
                    {
                        CudaInvoke.CvtColor(image, cudaBgra, ColorConversion.Bgr2Bgra);
                        hogCuda.DetectMultiScale(cudaBgra, vr);
                        regions = vr.ToArray();
                    }

                }
                else
                {
                    //this is the CPU/OpenCL version

                    
                    //watch = Stopwatch.StartNew();

                    MCvObjectDetection[] results = hog.DetectMultiScale(image);
                    regions = new Rectangle[results.Length];
                    for (int i = 0; i < results.Length; i++)
                        regions[i] = results[i].Rect;
                    //watch.Stop();

                }

                //processingTime = watch.ElapsedMilliseconds;

                return regions;
            }
        }
    }
}
