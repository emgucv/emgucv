//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
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

         using (Image<Bgr, byte> image = new Image<Bgr, byte>("pedestrian.png"))
         {
            long processingTime;
            Rectangle[] results = FindPedestrian.Find(image.Mat, out processingTime);
            foreach (Rectangle rect in results)
            {
               CvInvoke.Rectangle(image, rect, new MCvScalar(0, 0, 255, 255));
            }
            ImageViewer.Show(
               image,
               String.Format("Pedestrain detection using {0} in {1} milliseconds.",
                  CudaInvoke.HasCuda ? "GPU" : 
                  (CvInvoke.HaveOpenCL ? "OpenCL":
                  "CPU"),
                  processingTime));
         }
      }
   }
}
