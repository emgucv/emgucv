//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
﻿using Emgu.CV.CvEnum;
﻿using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Cuda;

namespace PedestrianDetection
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

         using (Mat image = new Mat("pedestrian.png"))
         {
            
            long processingTime;
            Rectangle[] results;

            if (CudaInvoke.HasCuda)
            {
               using (GpuMat gpuMat = new GpuMat(image))
                  results = FindPedestrian.Find(gpuMat, out processingTime);
            }
            else
            {
               using (UMat uImage = image.GetUMat(AccessType.ReadWrite))
                  results = FindPedestrian.Find(uImage, out processingTime);
            }
            
            foreach (Rectangle rect in results)
            {
               CvInvoke.Rectangle(image, rect, new Bgr(Color.Red).MCvScalar);
            }
            ImageViewer.Show(
               image,
               String.Format("Pedestrian detection using {0} in {1} milliseconds.",
                  CudaInvoke.HasCuda ? "GPU" : 
                  CvInvoke.UseOpenCL ? "OpenCL":
                  "CPU",
                  processingTime));
         }
      }
   }
}
