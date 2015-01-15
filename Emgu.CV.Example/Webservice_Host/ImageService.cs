//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Webservice_Host
{
   public class ImageService : IImageService
   {
      public Image<Bgr, Byte> GrabFrame()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 300, new Bgr(255, 255, 255));

         String str = DateTime.Now.Ticks.ToString();
         img.Draw(str, new System.Drawing.Point(50, 150), FontFace.HersheyPlain, 1.0, new Bgr(0, 0, 0));
         return img;
      }
   }
}
