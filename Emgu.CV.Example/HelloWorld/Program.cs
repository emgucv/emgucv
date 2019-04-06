//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace HelloWorld
{
   class Program
   {
      static void Main(string[] args)
      {
          Viz3d viz = new Viz3d("show_simple_widgets");
          viz.SetBackgroundMeshLab();
          WCoordinateSystem coor = new WCoordinateSystem();
          viz.ShowWidget("coor", coor);
          WCube cube = new WCube(new MCvPoint3D64f(-.5, -.5, -.5), new MCvPoint3D64f(.5, .5, .5), true, new MCvScalar(255, 255, 255));
          viz.ShowWidget("cube", cube);
          WCube cube0 = new WCube(new MCvPoint3D64f(-1, -1, -1), new MCvPoint3D64f(-.5, -.5, -.5), false, new MCvScalar(123, 45, 200));
          viz.ShowWidget("cub0", cube0);
          viz.Spin();

            String win1 = "Test Window"; //The name of the window
         CvInvoke.NamedWindow(win1); //Create the window using the specific name

         Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
         img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

         //Draw "Hello, world." on the image using the specific font
         CvInvoke.PutText(
            img, 
            "Hello, world", 
            new System.Drawing.Point(10, 80), 
            FontFace.HersheyComplex, 
            1.0, 
            new Bgr(0, 255, 0).MCvScalar);
         

         CvInvoke.Imshow(win1, img); //Show the image
         CvInvoke.WaitKey(0);  //Wait for the key pressing event
         CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed
      }
   }
}
