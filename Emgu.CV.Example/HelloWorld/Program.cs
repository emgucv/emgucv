//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace HelloWorld
{
   class Program
   {
      static void Main(string[] args)
      {
         String win1 = "Test Window"; //The name of the window
         CvInvoke.NamedWindow(win1); //Create the window using the specific name

         Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0)); //Create an image of 400x200 of Blue color

         img.Draw("Hello, world", new System.Drawing.Point(10, 80), FontFace.HersheyComplex, 1.0, new Bgr(0, 255, 0)); //Draw "Hello, world." on the image using the specific font

         CvInvoke.Imshow(win1, img); //Show the image
         CvInvoke.WaitKey(0);  //Wait for the key pressing event
         CvInvoke.DestroyWindow(win1); //Destroy the window
      }
   }
}
