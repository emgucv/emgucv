//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Freetype;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
        /// <param name="color">The color to draw the rectangle around the detected object</param>
        /// <param name="freetype2">Optional freetype2 object, if provided, it will be used to draw the label. If null, will use CvInvoke.PutText instead.</param>
        public virtual void Render(IInputOutputArray image, MCvScalar color, Freetype2 freetype2 = null)
        {
            CvInvoke.Rectangle(image, this.Region, color, 2);
            String label = String.Format("{0}: {1}", this.Label == null ? this.ClassId.ToString() : this.Label,
                this.Confident);
            if (freetype2 == null)
                CvInvoke.PutText(
                    image,
                    label,
                    this.Region.Location,
                    FontFace.HersheyDuplex,
                    1.0,
                    color,
                    1);
            else
            {
                freetype2.PutText(
                    image,
                    label,
                    this.Region.Location,
                    16,
                    color,
                    1,
                    LineType.EightConnected,
                    false
                    );
            }
        }

        /// <summary>
        /// Get a rectangle using the 4 fraction number of network output and size of the image
        /// </summary>
        /// <param name="left">A [0-1.0] value that indicates the left side of the rectangle. </param>
        /// <param name="top">A [0-1.0] value that indicates the top side of the rectangle. </param>
        /// <param name="right">A [0-1.0] value that indicates the right side of the rectangle. </param>
        /// <param name="bottom">A [0-1.0] value that indicates the bottom side of the rectangle. </param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <returns>A rectangle based on the image coordinate.</returns>
        public static Rectangle GetRectangle(float left, float top, float right, float bottom, int width, int height)
        {
            RectangleF rectF = new RectangleF(left * width, top * height, (right - left) * width, (bottom - top) * height);
            return Rectangle.Round(rectF);
        }

    }

    /// <summary>
    /// Extension functions for Emgu.CV.Dnn.DetectionModel
    /// </summary>
    public static class DetectionModelExtension
    {
        /// <summary>
        /// Given the input frame, create input blob, run net and return result detections.
        /// </summary>
        /// <param name="model">The Dnn DetectionModel</param>
        /// <param name="frame">The input image.</param>
        /// <param name="confThreshold">A threshold used to filter boxes by confidences.</param>
        /// <param name="nmsThreshold">A threshold used in non maximum suppression. The default value 0 means we will not perform non-maximum supression.</param>
        /// <param name="labels">Optional labels mapping, if provided, it will use classId as lookup index to get the Label. If null, the Label field of the DetectedObject will be null.</param>
        /// <returns>The array of detected objects</returns>
        public static DetectedObject[] Detect(
            this Emgu.CV.Dnn.DetectionModel model,
            IInputArray frame,
            float confThreshold = 0.5f,
            float nmsThreshold = 0.5f,
            String[] labels = null)
        {
            using (VectorOfInt classIds = new VectorOfInt())
            using (VectorOfFloat confidents = new VectorOfFloat())
            using (VectorOfRect regions = new VectorOfRect())
            {
                model.Detect(
                    frame,
                    classIds,
                    confidents,
                    regions,
                    (float)confThreshold,
                    (float)nmsThreshold);
                var classIdArr = classIds.ToArray();
                var confidentArr = confidents.ToArray();
                var regionArr = regions.ToArray();
                List<DetectedObject> results = new List<DetectedObject>();
                for (int i = 0; i < classIdArr.Length; i++)
                {
                    DetectedObject o = new DetectedObject();
                    o.ClassId = classIdArr[i];
                    o.Confident = confidentArr[i];
                    o.Region = regionArr[i];
                    if (labels != null)
                        o.Label = labels[o.ClassId];
                    results.Add(o);
                }

                return results.ToArray();
            }
        }
    }
}
