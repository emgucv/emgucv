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
using System.Threading.Tasks;
using System.Net;

namespace Emgu.CV.Models
{
    public class PedestrianDetector : IProcessAndRenderModel
    {
        private CudaHOG _hogCuda;
        private HOGDescriptor _hog;

        /// <summary>
        /// Find the pedestrian in the image
        /// </summary>
        /// <param name="image">The image</param>
        /// <returns>The region where pedestrians are detected</returns>
        public static Rectangle[] Find(IInputArray image, HOGDescriptor hog, CudaHOG hogCuda = null)
        {
            Rectangle[] regions;

            using (InputArray iaImage = image.GetInputArray())
            {
                //if the input array is a GpuMat
                //check if there is a compatible Cuda device to run pedestrian detection
                if (iaImage.Kind == InputArray.Type.CudaGpuMat && hogCuda != null)
                {
                    //this is the Cuda version
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
                    MCvObjectDetection[] results = hog.DetectMultiScale(image);
                    regions = new Rectangle[results.Length];
                    for (int i = 0; i < results.Length; i++)
                        regions[i] = results[i].Rect;
                }
                
                return regions;
            }
        }

        public async Task Init(DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            _hog = new HOGDescriptor();
            _hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

            if (CudaInvoke.HasCuda)
            {
                _hogCuda = new CudaHOG(
                    new Size(64, 128),
                    new Size(16, 16),
                    new Size(8, 8),
                    new Size(8, 8));
                _hogCuda.SetSVMDetector(_hogCuda.GetDefaultPeopleDetector());
            }

        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            Rectangle[] pedestrians = Find(imageIn, _hog, _hogCuda);
            watch.Stop();

            if (imageOut != imageIn)
            {
                using (InputArray iaImageIn = imageIn.GetInputArray())
                {
                    iaImageIn.CopyTo(imageOut);
                }
            }

            foreach (Rectangle rect in pedestrians)
            {
                CvInvoke.Rectangle(imageOut, rect, new MCvScalar(0, 0, 255), 2);
            }

            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);
        }
    }
}
