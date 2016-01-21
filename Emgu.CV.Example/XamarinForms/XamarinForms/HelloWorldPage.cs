//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XamarinForms
{
    public class HelloWorldPage : ButtonTextImagePage
    {
       public HelloWorldPage()
          : base()
       {
          this.GetButton().Opacity = 0;
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

         SetImage(img);
      }

    }
}
