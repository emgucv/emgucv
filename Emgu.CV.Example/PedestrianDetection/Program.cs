//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
using Emgu.CV.GPU;

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

         using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
         {
            long processingTime;
            Rectangle[] results = FindPedestrian.Find(image, out processingTime);
            foreach (Rectangle rect in results)
            {
               image.Draw(rect, new Bgr(Color.Red), 1);
            }
            ImageViewer.Show(
               image,
               String.Format("Pedestrain detection using {0} in {1} milliseconds.",
                  GpuInvoke.HasCuda ? "GPU" : "CPU",
                  processingTime));
         }
      }
   }
}
