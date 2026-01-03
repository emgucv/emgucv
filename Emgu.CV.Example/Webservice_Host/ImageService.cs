//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
        public Mat GrabFrame()
        {
            //Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 300, new Bgr(255, 255, 255));
            Mat img = new Mat(300, 300, DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(255, 255, 255));

            String str = DateTime.Now.Ticks.ToString();
            CvInvoke.PutText(img, str, new System.Drawing.Point(50, 150), FontFace.HersheyPlain, 1.0, new MCvScalar(0,0,0) );
            //img.Draw(str, new System.Drawing.Point(50, 150), FontFace.HersheyPlain, 1.0, new Bgr(0, 0, 0));
            return img;
        }
    }
}
