//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
using Emgu.CV.Nonfree;

namespace TrafficSignRecognition
{
   public class StopSignDetector : DisposableObject
   {
      private VectorOfKeyPoint _modelKeypoints;
      private Mat _modelDescriptors;
      
      //private Features2DTracker<float> _tracker;
      private SURFDetector _detector;
      
      private VectorOfPoint _octagon;

      public StopSignDetector(Image<Bgr, Byte> stopSignModel)
      {
         _detector = new SURFDetector(500);
         using (Image<Gray, Byte> redMask = GetRedPixelMask(stopSignModel))
         {
            _modelKeypoints = new VectorOfKeyPoint();
            _modelDescriptors = new Mat();
            _detector.DetectAndCompute(redMask, null, _modelKeypoints, _modelDescriptors, false);
            if (_modelKeypoints.Size == 0)
               throw new Exception("No image feature has been found in the stop sign model");
         }
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
               using (ScalarArray lower = new ScalarArray(20))
               using (ScalarArray upper = new ScalarArray(160))
                  CvInvoke.InRange(channels[0], lower, upper, channels[0]);
               channels[0]._Not();

               //channels[1] is the mask for satuation of at least 10, this is mainly used to filter out white pixels
               channels[1]._ThresholdBinary(new Gray(10), new Gray(255.0));

               CvInvoke.BitwiseAnd(channels[0], channels[1], channels[0], null);
            }
            finally
            {
               channels[1].Dispose();
               channels[2].Dispose();
            }
            return channels[0];
         }
      }

      private void FindStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList, VectorOfVectorOfPoint contours, int[,] hierachy, int idx1)
      {
         for (VectorOfPoint c = contours[idx1]; hierachy[idx1,0] >= 0;  c = contours[hierachy[idx1,0]], idx1 = hierachy[idx1,0])
         {
            using (VectorOfPoint approx = new VectorOfPoint())
            {
               CvInvoke.ApproxPolyDP(c, approx, CvInvoke.ArcLength(c, true)*0.02, true);
               if (CvInvoke.ContourArea(c) > 200)
               {
                  double ratio = CvInvoke.MatchShapes(_octagon, c, Emgu.CV.CvEnum.ContoursMatchType.I3, 0);

                  if (ratio > 0.1) //not a good match of contour shape
                  {
                     if (hierachy[idx1,0] >= 0)
                        FindStopSign(img, stopSignList, boxList, contours, hierachy, hierachy[idx1,0]);
                     continue;
                  }

                  Rectangle box = CvInvoke.BoundingRectangle(c);

                  Image<Gray, Byte> candidate;
                  using (Image<Bgr, Byte> tmp = img.Copy(box))
                     candidate = tmp.Convert<Gray, byte>();

                  //set the value of pixels not in the contour region to zero
                  using (Image<Gray, Byte> mask = new Image<Gray, byte>(box.Size))
                  {
                     mask.Draw(contours, idx1,  new Gray(255), -1, LineType.EightConnected, null, int.MaxValue, new Point(-box.X, -box.Y));

                     double mean = CvInvoke.Mean(candidate, mask).V0;
                     candidate._ThresholdBinary(new Gray(mean), new Gray(255.0));
                     candidate._Not();
                     mask._Not();
                     candidate.SetValue(0, mask);
                  }

                  int minMatchCount = 10;
                  double uniquenessThreshold = 0.8;
                  VectorOfKeyPoint _observeredKeypoint = new VectorOfKeyPoint();
                  Mat _observeredDescriptor = new Mat();
                  _detector.DetectAndCompute(candidate, null, _observeredKeypoint, _observeredDescriptor, false);
                  
                  if ( _observeredKeypoint.Size >= minMatchCount)
                  {
                     BruteForceMatcher matcher = new BruteForceMatcher(DistanceType.L2);
                     matcher.Add(_modelDescriptors);
                     int k = 2;
                     Matrix<int> indices = new Matrix<int>(_observeredDescriptor.Size.Height, k);
                     Matrix<byte> mask;
                     using (Matrix<float> dist = new Matrix<float>(_observeredDescriptor.Size.Height, k))
                     {
                        matcher.KnnMatch(_observeredDescriptor, indices, dist, k, null);
                        mask = new Matrix<byte>(dist.Rows, 1);
                        mask.SetValue(255);
                        Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
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

      public void DetectStopSign(Image<Bgr, byte> img, List<Image<Gray, Byte>> stopSignList, List<Rectangle> boxList)
      {
         Image<Bgr, Byte> smoothImg = img.SmoothGaussian(5, 5, 1.5, 1.5);
         Image<Gray, Byte> smoothedRedMask = GetRedPixelMask(smoothImg);

         //Use Dilate followed by Erode to eliminate small gaps in some countour.
         smoothedRedMask._Dilate(1);
         smoothedRedMask._Erode(1);

         using (Image<Gray, Byte> canny = smoothedRedMask.Canny(100, 50))
         using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
         {
            int[,] hierachy = CvInvoke.FindContourTree(canny, contours, ChainApproxMethod.ChainApproxSimple);
            
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
         if (_octagon != null)
         {
            _octagon.Dispose();
            _octagon = null;
         }
      }
   }
}
