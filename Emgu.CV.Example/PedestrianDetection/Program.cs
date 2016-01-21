//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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

         using (Mat image = new Mat("pedestrian.png", LoadImageType.Color))
         {
            bool tryUseCuda = true;
            long processingTime;
            Rectangle[] results = FindPedestrian.Find(image, tryUseCuda, out processingTime);
            foreach (Rectangle rect in results)
            {
               CvInvoke.Rectangle(image, rect, new Bgr(Color.Red).MCvScalar);
            }
            ImageViewer.Show(
               image,
               String.Format("Pedestrian detection using {0} in {1} milliseconds.",
                  (tryUseCuda && CudaInvoke.HasCuda) ? "GPU" : 
                  CvInvoke.UseOpenCL ? "OpenCL":
                  "CPU",
                  processingTime));
         }
      }
   }
}
