//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace FeatureMatchingExample
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
         using(Mat modelImage = CvInvoke.Imread("box.png", ImreadModes.Grayscale))
         using (Mat observedImage = CvInvoke.Imread("box_in_scene.png", ImreadModes.Grayscale))
         {
            Mat result = DrawMatches.Draw(modelImage, observedImage, out matchTime);
            ImageViewer.Show(result, String.Format("Matched in {0} milliseconds", matchTime));
         }
      }
   }
}
