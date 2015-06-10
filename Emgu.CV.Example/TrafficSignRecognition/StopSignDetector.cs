//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.XFeatures2D;

namespace TrafficSignRecognition
{
   public class StopSignDetector : DisposableObject
   {
      private VectorOfKeyPoint _modelKeypoints;
      private Mat _modelDescriptors;
      private BFMatcher _modelDescriptorMatcher;
      //private Features2DTracker<float> _tracker;
      private SURF _detector;

      private VectorOfPoint _octagon;

      public StopSignDetector(IInputArray stopSignModel)
      {
         _detector = new SURF(500);
         using (Mat redMask = new Mat())
         {
            GetRedPixelMask(stopSignModel, redMask);
            _modelKeypoints = new VectorOfKeyPoint();
            _modelDescriptors = new Mat();
            _detector.DetectAndCompute(redMask, null, _modelKeypoints, _modelDescriptors, false);
            if (_modelKeypoints.Size == 0)
               throw new Exception("No image feature has been found in the stop sign model");
         }

         _modelDescriptorMatcher = new BFMatcher(DistanceType.L2);
         _modelDescriptorMatcher.Add(_modelDescriptors);

         _octagon = new VectorOfPoint(
            new Point[]
            {
               new Point(1, 0),
               new Point(2, 0),
               new Point(3, 1),
               new Point(3, 2),
               new Point(2, 3),
               new Point(1, 3),
               new Point(0, 2),
               new Point(0, 1)
            });

      }

      /// <summary>
      /// Compute the red pixel mask for the given image. 
      /// A red pixel is a pixel where:  20 &lt; hue &lt; 160 AND saturation &gt; 10
      /// </summary>
      /// <param name="image">The color image to find red mask from</param>
      /// <param name="mask">The red pixel mask</param>
      private static void GetRedPixelMask(IInputArray image, IInputOutputArray mask)
      {
         bool useUMat;
         using (InputOutputArray ia = mask.GetInputOutputArray())
            useUMat = ia.IsUMat;

         using (IImage hsv = useUMat ? (IImage)new UMat() : (IImage)new Mat())
         using (IImage s = useUMat ? (IImage)new UMat() : (IImage)new Mat())
         {
            CvInvoke.CvtColor(image, hsv, ColorConversion.Bgr2Hsv);
            CvInvoke.ExtractChannel(hsv, mask, 0);
            CvInvoke.ExtractChannel(hsv, s, 1);

            //the mask for hue less than 20 or larger than 160
            using (ScalarArray lower = new ScalarArray(20))
            using (ScalarArray upper = new ScalarArray(160))
               CvInvoke.InRange(mask, lower, upper, mask);
            CvInvoke.BitwiseNot(mask, mask);

            //s is the mask for saturation of at least 10, this is mainly used to filter out white pixels
            CvInvoke.Threshold(s, s, 10, 255, ThresholdType.Binary);
            CvInvoke.BitwiseAnd(mask, s, mask, null);

         }
      }

      private void FindStopSign(Mat img, List<Mat> stopSignList, List<Rectangle> boxList, VectorOfVectorOfPoint contours, int[,] hierachy, int idx)
      {
         for (; idx >= 0; idx = hierachy[idx, 0])
         {
            using (VectorOfPoint c = contours[idx])
            using (VectorOfPoint approx = new VectorOfPoint())
            {
               CvInvoke.ApproxPolyDP(c, approx, CvInvoke.ArcLength(c, true) * 0.02, true);
               double area = CvInvoke.ContourArea(approx);
               if (area > 200)
               {
                  double ratio = CvInvoke.MatchShapes(_octagon, approx, Emgu.CV.CvEnum.ContoursMatchType.I3);

                  if (ratio > 0.1) //not a good match of contour shape
                  {
                     //check children
                     if (hierachy[idx, 2] >= 0)
                        FindStopSign(img, stopSignList, boxList, contours, hierachy, hierachy[idx, 2]);
                     continue;
                  }

                  Rectangle box = CvInvoke.BoundingRectangle(c);

                  Mat candidate = new Mat();
                  using (Mat tmp = new Mat(img, box))
                     CvInvoke.CvtColor(tmp, candidate, ColorConversion.Bgr2Gray);

                  //set the value of pixels not in the contour region to zero
                  using (Mat mask = new Mat(candidate.Size.Height, candidate.Width, DepthType.Cv8U, 1))
                  {
                     mask.SetTo(new MCvScalar(0));
                     CvInvoke.DrawContours(mask, contours, idx, new MCvScalar(255), -1, LineType.EightConnected, null, int.MaxValue, new Point(-box.X, -box.Y));

                     double mean = CvInvoke.Mean(candidate, mask).V0;
                     CvInvoke.Threshold(candidate, candidate, mean, 255, ThresholdType.Binary);
                     CvInvoke.BitwiseNot(candidate, candidate);
                     CvInvoke.BitwiseNot(mask, mask);

                     candidate.SetTo(new MCvScalar(0), mask);
                  }

                  int minMatchCount = 8;
                  double uniquenessThreshold = 0.8;
                  VectorOfKeyPoint _observeredKeypoint = new VectorOfKeyPoint();
                  Mat _observeredDescriptor = new Mat();
                  _detector.DetectAndCompute(candidate, null, _observeredKeypoint, _observeredDescriptor, false);

                  if (_observeredKeypoint.Size >= minMatchCount)
                  {
                     int k = 2;

                     Mat mask;

                     using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                     {
                        _modelDescriptorMatcher.KnnMatch(_observeredDescriptor, matches, k, null);
                        mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                        mask.SetTo(new MCvScalar(255));
                        Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);
                     }

                     int nonZeroCount = CvInvoke.CountNonZero(mask);
                     if (nonZeroCount >= minMatchCount)
                     {
                        boxList.Add(box);
                        stopSignList.Add(candidate);
                     }
                  }
               }
            }
         }
      }

      public void DetectStopSign(Mat img, List<Mat> stopSignList, List<Rectangle> boxList)
      {
         Mat smoothImg = new Mat();
         CvInvoke.GaussianBlur(img, smoothImg, new Size(5, 5), 1.5, 1.5);
         //Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);

         Mat smoothedRedMask = new Mat();
         GetRedPixelMask(smoothImg, smoothedRedMask);

         //Use Dilate followed by Erode to eliminate small gaps in some contour.
         CvInvoke.Dilate(smoothedRedMask, smoothedRedMask, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
         CvInvoke.Erode(smoothedRedMask, smoothedRedMask, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);

         using (Mat canny = new Mat())
         using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
         {
            CvInvoke.Canny(smoothedRedMask, canny, 100, 50);
            int[,] hierachy = CvInvoke.FindContourTree(canny, contours, ChainApproxMethod.ChainApproxSimple);

            //Image<Gray, Byte> tmp = new Image<Gray, byte>(canny.Size);
            //CvInvoke.DrawContours(tmp, contours, -1, new MCvScalar(255, 255, 255));
            //Emgu.CV.UI.ImageViewer.Show(tmp);

            if (hierachy.GetLength(0) > 0)
               FindStopSign(img, stopSignList, boxList, contours, hierachy, 0);
         }

      }

      protected override void DisposeObject()
      {
         if (_modelKeypoints != null)
         {
            _modelKeypoints.Dispose();
            _modelKeypoints = null;
         }
         if (_modelDescriptors != null)
         {
            _modelDescriptors.Dispose();
            _modelDescriptors = null;
         }
         if (_modelDescriptorMatcher != null)
         {
            _modelDescriptorMatcher.Dispose();
            _modelDescriptorMatcher = null;
         }
         if (_octagon != null)
         {
            _octagon.Dispose();
            _octagon = null;
         }
      }
   }
}
