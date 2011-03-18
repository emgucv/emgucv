//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Emgu.CV.UI;

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
         if (!IsPlaformCompatable()) return;
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Run();
      }

      static void Run()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png");

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

         foreach (Rectangle pedestrain in regions)
         {
            image.Draw(pedestrain, new Bgr(Color.Red), 1);
         }

         ImageViewer.Show(
            image,
            String.Format("Pedestrain detection using {0} in {1} milliseconds.", 
               GpuInvoke.HasCuda ? "GPU" : "CPU", 
               watch.ElapsedMilliseconds));
      }

      /// <summary>
      /// Check if both the managed and unmanaged code are compiled for the same architecture
      /// </summary>
      /// <returns>Returns true if both the managed and unmanaged code are compiled for the same architecture</returns>
      static bool IsPlaformCompatable()
      {
         int clrBitness = Marshal.SizeOf(typeof(IntPtr)) * 8;
         if (clrBitness != CvInvoke.UnmanagedCodeBitness)
         {
            MessageBox.Show(String.Format("Platform mismatched: CLR is {0} bit, C++ code is {1} bit."
               + " Please consider recompiling the executable with the same platform target as C++ code.",
               clrBitness, CvInvoke.UnmanagedCodeBitness));
            return false;
         }
         return true;
      }
   }
}
