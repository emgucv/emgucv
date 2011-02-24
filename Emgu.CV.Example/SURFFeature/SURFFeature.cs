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
         SURFDetector surfParam = new SURFDetector(500, false);

         Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png");
         //extract features from the object image
         VectorOfKeyPoint modelKeyPoints = surfParam.DetectKeyPointsRaw(modelImage, null);
         Matrix<float> modelDescriptors = surfParam.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);

         Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");
         Stopwatch watch = Stopwatch.StartNew();
         
         // extract features from the observed image
         VectorOfKeyPoint observedKeyPoints = surfParam.DetectKeyPointsRaw(observedImage, null);
         Matrix<float> observedDescriptors = surfParam.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);
         HomographyMatrix homography = null;
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

         //Merge the object image and the observed image into one image for display
         Image<Gray, Byte> res = modelImage.ConcateVertical(observedImage);

         /*
         #region draw lines between the matched features
         foreach (Features2DTracker.MatchedImageFeature matchedFeature in matchedFeatures)
         {
            PointF p = matchedFeature.ObservedFeature.KeyPoint.Point;
            p.Y += modelImage.Height;
            res.Draw(new LineSegment2DF(matchedFeature.SimilarFeatures[0].Feature.KeyPoint.Point, p), new Gray(0), 1);
         }
         #endregion
         */
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
