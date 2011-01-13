using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.GPU;
using System.Drawing;
using System.Diagnostics;

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
         Run();
      }

      static void Run()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png");

         Stopwatch watch = Stopwatch.StartNew();
         Rectangle[] regions;
         if (GpuInvoke.HasCuda)
         {
            using (GpuHOGDescriptor des = new GpuHOGDescriptor())
            using (GpuImage<Bgr, Byte> gpuImg = new GpuImage<Bgr,byte>(image))
            using (GpuImage<Bgra, Byte> gpuBgra = gpuImg.Convert<Bgra, Byte>())
            {
               des.SetSVMDetector(GpuHOGDescriptor.GetDefaultPeopleDetector());
               regions = des.DetectMultiScale(gpuBgra);
            }
         }
         else
         {
            using (HOGDescriptor des = new HOGDescriptor())
            {
               des.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
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
   }
}
