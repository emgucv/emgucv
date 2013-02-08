//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
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
         using (Image<Bgr, Byte> image = new Image<Bgr, byte>("cat.jpg"))
         using (LatentSvmDetector detector = new LatentSvmDetector("cat.xml"))
         {
            Stopwatch watch = Stopwatch.StartNew();
            MCvObjectDetection[] regions = detector.Detect(image, 0.5f);
            watch.Stop();

            foreach (MCvObjectDetection region in regions)
            {
               if (region.score > -0.5)
                  image.Draw(region.Rect, new Bgr(Color.Red), 1);
            }

            ImageViewer.Show(image, String.Format("Object detected in {0} milliseconds", watch.ElapsedMilliseconds));
         }
      }
   }
}
