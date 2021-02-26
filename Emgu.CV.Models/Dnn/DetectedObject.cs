//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Emgu.CV.Models
{
    public class DetectedObject
    {
        public Rectangle Region;
        public double Confident;
        public String Label;
        public int ClassId;

        public static void Render(Mat image, DetectedObject[] detectedObjects)
        {
            for (int i = 0; i < detectedObjects.Length; i++)
            {
                CvInvoke.Rectangle(image, detectedObjects[i].Region, new MCvScalar(0, 0, 255), 2);
                CvInvoke.PutText(
                    image,
                    String.Format("{0}: {1}", detectedObjects[i].Label, detectedObjects[i].Confident),
                    detectedObjects[i].Region.Location,
                    FontFace.HersheyDuplex,
                    1.0,
                    new MCvScalar(0, 0, 255),
                    1);
            }
        }
    }
}
