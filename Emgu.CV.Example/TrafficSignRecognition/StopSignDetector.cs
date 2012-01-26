//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;

namespace TrafficSignRecognition
{
   public class StopSignDetector : DisposableObject
   {
      private Features2DTracker<float> _tracker;
      private SURFDetector _detector;
      private MemStorage _octagonStorage;
      private Contour<Point> _octagon;

      public StopSignDetector()
      {
         _detector = new SURFDetector(500, false);
         using (Image<Bgr, Byte> stopSignModel = new Image<Bgr, Byte>("stop-sign-model.png"))
         using (Image<Gray, Byte> redMask = GetRedPixelMask(stopSignModel))
         {
            _tracker = new Features2DTracker<float>(_detector.DetectFeatures(redMask, null));  
         }
         _octagonStorage = new MemStorage();
         _octagon = new Contour<Point>(_octagonStorage);
         _octagon.PushMulti(new Point[] { 
            new Point(1, 0),
            new Point(2, 0),
            new Point(3, 1),
            new Point(3, 2),
            new Point(2, 3),
            new Point(1, 3),
            new Point(0, 2),
            new Point(0, 1)},
            Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
      }

      /// <summary>
      /// Compute the red pixel mask for the given image. 
      /// A red pixel is a pixel where:  20 &lt; hue &lt; 160 AND satuation &gt; 10
      /// </summary>
      /// <param name="image">The color image to find red mask from</param>
      /// <returns>The red pixel mask</returns>
      private static Image<Gray, Byte> GetRedPixelMask(Image<Bgr, byte> image)
      {
         using (Image<Hsv, Byte> hsv = image.Convert<Hsv, Byte>())
         {
            Image<Gray, Byte>[] channels = hsv.Split();

            try
            {
               //channels[0] is the mask for hue less than 20 or larger than 160
               CvInvoke.cvInRangeS(channels[0], new MCvScalar(20), new MCvScalar(160), channels[0]);
               channels[0]._Not();

               //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
               channels[1]._ThresholdBinary(new Gray(10), new Gray(255.0));

               CvInvoke.cvAnd(channels[0], channels[1], channels[0], IntPtr.Zero);
            }
            finally
            {
               channels[1].Dispose();
               channels[2].Dispose();
            }
            return channels[0];
         }
      }

      private void FindStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, Contour<Point> contours)
      {
         for (; contours != null; contours = contours.HNext)
         {
            contours.ApproxPoly(contours.Perimeter * 0.02, 0, contours.Storage);
            if (contours.Area > 200)
            {
               double ratio = CvInvoke.cvMatchShapes(_octagon, contours, Emgu.CV.CvEnum.CONTOURS_MATCH_TYPE.CV_CONTOURS_MATCH_I3, 0);

               if (ratio > 0.1) //not a good match of contour shape
               {
                  Contour<Point> child = contours.VNext;
                  if (child != null)
                     FindStopSign(img, stopSignList, boxList, child);
                  continue;
               }

               Rectangle box = contours.BoundingRectangle;

               Image<Gray, Byte> candidate;
               using (Image<Bgr, Byte> tmp = img.Copy(box))
                  candidate = tmp.Convert<Gray, byte>();

               //set the value of pixels not in the contour region to zero
               using (Image<Gray, Byte> mask = new Image<Gray, byte>(box.Size))
               {
                  mask.Draw(contours, new Gray(255), new Gray(255), 0, -1, new Point(-box.X, -box.Y));

                  double mean = CvInvoke.cvAvg(candidate, mask).v0;
                  candidate._ThresholdBinary(new Gray(mean), new Gray(255.0));
                  candidate._Not();
                  mask._Not();
                  candidate.SetValue(0, mask);
               }

               ImageFeature<float>[] features = _detector.DetectFeatures(candidate, null);

               Features2DTracker<float>.MatchedImageFeature[] matchedFeatures = _tracker.MatchFeature(features, 2);

               int goodMatchCount = 0;
               foreach (Features2DTracker<float>.MatchedImageFeature ms in matchedFeatures)
                  if (ms.SimilarFeatures[0].Distance < 0.5) goodMatchCount++;

               if (goodMatchCount >= 10)
               {
                  boxList.Add(box);
                  stopSignList.Add(candidate);
               }
            }
         }
      }

      public void DetectStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList)
      {
         Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
         Image<Gray, Byte> smoothedRedMask = GetRedPixelMask(smoothImg);

         //Use Dilate followed by Erode to eliminate small gaps in some countour.
         smoothedRedMask._Dilate(1);
         smoothedRedMask._Erode(1);

         using (Image<Gray, Byte> canny = smoothedRedMask.Canny(new Gray(100), new Gray(50)))
         using (MemStorage stor = new MemStorage())
         {
            Contour<Point> contours = canny.FindContours(
               Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
               Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
               stor);
            FindStopSign(img, stopSignList, boxList, contours);
         }
      }

      protected override void DisposeObject()
      {
         _tracker.Dispose();
         _octagonStorage.Dispose();
      }
   }
}
