//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LatentSvmDetectorExample
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
         using (Mat image = CvInvoke.Imread("cat.jpg", LoadImageType.AnyColor | LoadImageType.AnyDepth))
         using (LatentSvmDetector detector = new LatentSvmDetector(new string[] { "cat.xml" }))
         {
            Stopwatch watch = Stopwatch.StartNew();
            MCvObjectDetection[] regions = detector.Detect(image, 0.5f);
            watch.Stop();

            foreach (MCvObjectDetection region in regions)
            {
               CvInvoke.Rectangle(image, region.Rect, new MCvScalar(0, 0, 255));
            }

            ImageViewer.Show(image, String.Format("Object detected in {0} milliseconds", watch.ElapsedMilliseconds));
         }
      }
   }
}
