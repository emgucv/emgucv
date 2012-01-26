//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
         if (!IsPlaformCompatable()) return;

         String win1 = "Test Window"; //The name of the window
         CvInvoke.cvNamedWindow(win1); //Create the window using the specific name

         Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0)); //Create an image of 400x200 of Blue color
         MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0); //Create the font

         img.Draw("Hello, world", ref f, new System.Drawing.Point(10, 80), new Bgr(0, 255, 0)); //Draw "Hello, world." on the image using the specific font

         CvInvoke.cvShowImage(win1, img); //Show the image
         CvInvoke.cvWaitKey(0);  //Wait for the key pressing event
         CvInvoke.cvDestroyWindow(win1); //Destory the window
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
