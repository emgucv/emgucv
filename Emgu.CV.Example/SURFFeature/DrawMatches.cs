//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Cuda;
using Emgu.CV.Nonfree;

namespace SURFFeatureExample
{
   public static class DrawMatches
   {
      public static void FindMatch(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, out Matrix<int> indices, out Matrix<byte> mask, out HomographyMatrix homography)
      {
         int k = 2;
         double uniquenessThreshold = 0.8;
         double hessianThresh = 300;
         
         Stopwatch watch;
         homography = null;

         if (CudaInvoke.HasCuda)
         {
            CudaSURFDetector surfCuda = new CudaSURFDetector((float) hessianThresh);
            using (CudaImage<Gray, Byte> gpuModelImage = new CudaImage<Gray, byte>(modelImage))
            //extract features from the object image
            using (GpuMat<float> gpuModelKeyPoints = surfCuda.DetectKeyPointsRaw(gpuModelImage, null))
            using (GpuMat<float> gpuModelDescriptors = surfCuda.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints))
            using (CudaBruteForceMatcher<float> matcher = new CudaBruteForceMatcher<float>(DistanceType.L2))
            {
               modelKeyPoints = new VectorOfKeyPoint();
               surfCuda.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
               watch = Stopwatch.StartNew();

               // extract features from the observed image
               using (CudaImage<Gray, Byte> gpuObservedImage = new CudaImage<Gray, byte>(observedImage))
               using (GpuMat<float> gpuObservedKeyPoints = surfCuda.DetectKeyPointsRaw(gpuObservedImage, null))
               using (GpuMat<float> gpuObservedDescriptors = surfCuda.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints))
               using (GpuMat<int> gpuMatchIndices = new GpuMat<int>(gpuObservedDescriptors.Size.Height, k, 1, true))
               using (GpuMat<float> gpuMatchDist = new GpuMat<float>(gpuObservedDescriptors.Size.Height, k, 1, true))
               using (GpuMat<Byte> gpuMask = new GpuMat<byte>(gpuMatchIndices.Size.Height, 1, 1))
               using (Stream stream = new Stream())
               {
                  matcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuMatchIndices, gpuMatchDist, k, null, stream);
                  indices = new Matrix<int>(gpuMatchIndices.Size);
                  mask = new Matrix<byte>(gpuMask.Size);

                  //gpu implementation of voteForUniquess
                  using (GpuMat<float> col0 = gpuMatchDist.Col(0))
                  using (GpuMat<float> col1 = gpuMatchDist.Col(1))
                  {
                     CudaInvoke.Multiply(col1, new MCvScalar(uniquenessThreshold), col1, stream);
                     CudaInvoke.Compare(col0, col1, gpuMask, CmpType.LessEqual, stream);
                  }

                  observedKeyPoints = new VectorOfKeyPoint();
                  surfCuda.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

                  //wait for the stream to complete its tasks
                  //We can perform some other CPU intesive stuffs here while we are waiting for the stream to complete.
                  stream.WaitForCompletion();

                  gpuMask.Download(mask);
                  gpuMatchIndices.Download(indices);

                  if (CudaInvoke.CountNonZero(gpuMask) >= 4)
                  {
                     int nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                     if (nonZeroCount >= 4)
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                  }

                  watch.Stop();
               }
            }
         }
         else
         {
            using (UMat uModelImage = modelImage.Mat.ToUMat(AccessType.Read))
            using (UMat uObservedImage = observedImage.Mat.ToUMat(AccessType.Read))
            {
               SURFDetector surfCPU = new SURFDetector(hessianThresh);
               //extract features from the object image
               modelKeyPoints = new VectorOfKeyPoint();
               Mat modelDescriptors = new Mat();
               surfCPU.DetectAndCompute(uModelImage, null, modelKeyPoints, modelDescriptors, false);

               watch = Stopwatch.StartNew();

               // extract features from the observed image
               observedKeyPoints = new VectorOfKeyPoint();
               Mat observedDescriptors = new Mat();
               surfCPU.DetectAndCompute(uObservedImage, null, observedKeyPoints, observedDescriptors, false);
               BruteForceMatcher matcher = new BruteForceMatcher(DistanceType.L2);
               matcher.Add(modelDescriptors);

               indices = new Matrix<int>(observedDescriptors.Size.Height, k);
               using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Size.Height, k))
               {
                  matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
                  mask = new Matrix<byte>(dist.Rows, 1);
                  mask.SetValue(255);
                  Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
               }

               int nonZeroCount = CvInvoke.CountNonZero(mask);
               if (nonZeroCount >= 4)
               {
                  nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                  if (nonZeroCount >= 4)
                     homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
               }
               
               watch.Stop();
            }
         }
         matchTime = watch.ElapsedMilliseconds;
      }

      /// <summary>
      /// Draw the model image and observed image, the matched features and homography projection.
      /// </summary>
      /// <param name="modelImage">The model image</param>
      /// <param name="observedImage">The observed image</param>
      /// <param name="matchTime">The output total time for computing the homography matrix.</param>
      /// <returns>The model image and observed image, the matched features and homography projection.</returns>
      public static Image<Bgr, Byte> Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime)
      {
         HomographyMatrix homography;
         VectorOfKeyPoint modelKeyPoints;
         VectorOfKeyPoint observedKeyPoints;
         Matrix<int> indices;
         Matrix<byte> mask;

         FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, out indices, out mask, out homography);

         //Draw the matched keypoints
         Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
            indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask);

         #region draw the projected region on the image
         if (homography != null)
         {  //draw a rectangle along the projected model
            Rectangle rect = modelImage.ROI;
            PointF[] pts = new PointF[] { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};
            homography.ProjectPoints(pts);

            result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);
         }
         #endregion

         return result;
      }
   }
}
