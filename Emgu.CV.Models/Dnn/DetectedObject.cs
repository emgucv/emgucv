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
    /// <summary>
    /// A detection result
    /// </summary>
    public class DetectedObject
    {
        /// <summary>
        /// The detected region
        /// </summary>
        public Rectangle Region;

        /// <summary>
        /// The confident
        /// </summary>
        public double Confident;

        /// <summary>
        /// The label
        /// </summary>
        public String Label;

        /// <summary>
        /// The class id
        /// </summary>
        public int ClassId;

        /// <summary>
        /// Draw the detected object on the image
        /// </summary>
        /// <param name="image">The image to draw on</param>
        public void Render(Mat image)
        {
            CvInvoke.Rectangle(image, this.Region, new MCvScalar(0, 0, 255), 2);
            CvInvoke.PutText(
                image,
                String.Format("{0}: {1}", this.Label, this.Confident),
                this.Region.Location,
                FontFace.HersheyDuplex,
                1.0,
                new MCvScalar(0, 0, 255),
                1);
        }
    }
}
