//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.GPU;

namespace SURFFeatureExample
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         if (!IsPlaformCompatable()) return;
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Run();
      }

      static void Run()
      {
         Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png");
         Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");
         Stopwatch watch;
         HomographyMatrix homography = null;

         SURFDetector surfParam = new SURFDetector(500, false);

         if (GpuInvoke.HasCuda)
         {
            GpuSURFDetector surf = new GpuSURFDetector(surfParam, 0.01f);
            using (GpuImage<Gray, Byte> gpuModelImage = new GpuImage<Gray, byte>(modelImage))
            //extract features from the object image
            using (GpuMat<float> gpuModelKeyPoints = surf.DetectKeyPointsRaw(gpuModelImage, null))
            using (GpuMat<float> gpuModelDescriptors = surf.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints, true))
            using (VectorOfKeyPoint modelKeyPoints = new VectorOfKeyPoint())
            using (GpuBruteForceMatcher matcher = new GpuBruteForceMatcher(GpuBruteForceMatcher.DistanceType.L2))
            {
               surf.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
               watch = Stopwatch.StartNew();

               // extract features from the observed image
               using (GpuImage<Gray, Byte> gpuObservedImage = new GpuImage<Gray,byte>(observedImage))
               using (GpuMat<float> gpuObservedKeyPoints = surf.DetectKeyPointsRaw(gpuObservedImage, null))
               using (GpuMat<float> gpuObservedDescriptors = surf.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints, true))
               using (GpuMat<int> gpuMatchIndices = new GpuMat<int>(gpuObservedDescriptors.Size.Height, 2, 1))
               using (GpuMat<float> gpuMatchDist = new GpuMat<float>(gpuMatchIndices.Size, 1))
               using (VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint())
               {
                  surf.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

                  matcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, gpuMatchIndices, gpuMatchDist, 2, null);

                  Matrix<int> indices = new Matrix<int>(gpuMatchIndices.Size);
                  Matrix<float> dist = new Matrix<float>(indices.Size);
                  gpuMatchIndices.Download(indices);
                  gpuMatchDist.Download(dist);

                  using (Matrix<byte> mask = new Matrix<byte>(dist.Rows, 1))
                  {
                     mask.SetValue(255);

                     Features2DTracker.VoteForUniqueness(dist, 0.8, mask);

                     int nonZeroCount = CvInvoke.cvCountNonZero(mask);
                     if (nonZeroCount >= 4)
                     {
                        nonZeroCount = Features2DTracker.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                        if (nonZeroCount >= 4)
                           homography = Features2DTracker.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask);
                     }
                  }
                  watch.Stop();
               }
            }
         }
         else
         {
            //extract features from the object image
            VectorOfKeyPoint modelKeyPoints = surfParam.DetectKeyPointsRaw(modelImage, null);
            Matrix<float> modelDescriptors = surfParam.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);

            watch = Stopwatch.StartNew();

            // extract features from the observed image
            VectorOfKeyPoint observedKeyPoints = surfParam.DetectKeyPointsRaw(observedImage, null);
            Matrix<float> observedDescriptors = surfParam.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);
            Matrix<int> indices;
            Matrix<float> dist;
            Features2DTracker.DescriptorMatchKnn(modelDescriptors, observedDescriptors, 2, 20, out indices, out dist);
            using (Matrix<byte> mask = new Matrix<byte>(dist.Rows, 1))
            {
               mask.SetValue(255);

               Features2DTracker.VoteForUniqueness(dist, 0.8, mask);

               int nonZeroCount = CvInvoke.cvCountNonZero(mask);
               if (nonZeroCount >= 4)
               {
                  nonZeroCount = Features2DTracker.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                  if (nonZeroCount >= 4)
                     homography = Features2DTracker.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask);
               }
            }
            watch.Stop();
         }
         //Merge the object image and the observed image into one image for display
         Image<Gray, Byte> res = modelImage.ConcateVertical(observedImage);

         #region draw the project region on the image
         if (homography != null)
         {  //draw a rectangle along the projected model
            Rectangle rect = modelImage.ROI;
            PointF[] pts = new PointF[] { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};
            homography.ProjectPoints(pts);

            for (int i = 0; i < pts.Length; i++)
               pts[i].Y += modelImage.Height;

            res.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Gray(255.0), 5);
         }
         #endregion

         ImageViewer.Show(res, String.Format("Matched in {0} milliseconds", watch.ElapsedMilliseconds));
      }

      /// <summary>
      /// Check if both the managed and unmanaged code are compiled for the same architecture
      /// </summary>
      /// <returns>Returns true if both the managed and unmanaged code are compiled for the same architecture</returns>
      static bool IsPlaformCompatable()
      {
         int clrBitness = Marshal.SizeOf(typeof(IntPtr)) * 8;
         if (clrBitness != CvInvoke.UnmanagedCodeBitness)
         {
            MessageBox.Show(String.Format("Platform mismatched: CLR is {0} bit, C++ code is {1} bit." 
               + " Please consider recompiling the executable with the same platform target as C++ code.", 
               clrBitness, CvInvoke.UnmanagedCodeBitness));
            return false;
         }
         return true;
      }
   }
}
