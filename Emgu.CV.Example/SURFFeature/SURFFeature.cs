//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.Cuda;

namespace SURFFeatureExample
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
         long matchTime;
         using(Mat modelImage = CvInvoke.Imread("box.png", LoadImageType.Grayscale))
         using (Mat observedImage = CvInvoke.Imread("box_in_scene.png", LoadImageType.Grayscale))
         {
            Mat result = DrawMatches.Draw(modelImage, observedImage, out matchTime);
            ImageViewer.Show(result, String.Format("Matched using {0} in {1} milliseconds", CudaInvoke.HasCuda ? "GPU" : "CPU", matchTime));
         }
      }
   }
}
