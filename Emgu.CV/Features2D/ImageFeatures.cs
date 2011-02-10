using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// This class use ImageFeature to match or track object
   /// </summary>
   public static class ImageFeatures 
   {
      /// <summary>
      /// Detect the if the model features exist in the observed features. If true, an homography matrix is returned, otherwise, null is returned.
      /// </summary>
      /// <param name="uniquenessThreshold">The distance different ratio which a match is consider unique, a good number will be 0.8</param>
      /// <returns>If the model features exist in the observed features, an homography matrix is returned, otherwise, null is returned.</returns>
      public static HomographyMatrix Detect(
         VectorOfKeyPoint modelKeyPoints, Matrix<float> modelDescriptors,
         VectorOfKeyPoint observedKeyPoints, Matrix<float> observedDescriptors, double uniquenessThreshold)
      {
         Matrix<int> indices;
         Matrix<float> dist;
         DescriptorMatchKnn(modelDescriptors, observedDescriptors, 2, 20, out indices, out dist);
         using (Matrix<byte> mask = new Matrix<byte>(dist.Rows, 1))
         {
            mask.SetValue(255);

            //Stopwatch w1 = Stopwatch.StartNew();
            VoteForUniqueness(dist, uniquenessThreshold, mask);
            //Trace.WriteLine(w1.ElapsedMilliseconds);

            int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount < 4)
               return null;

            //Stopwatch w2 = Stopwatch.StartNew();
            VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
            //Trace.WriteLine(w2.ElapsedMilliseconds);
            nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount < 4)
               return null;

            return GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask);
         }
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool getHomographyMatrixFromMatchedFeatures(IntPtr model, IntPtr observed, IntPtr indices, IntPtr mask, IntPtr homography);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int voteForSizeAndOrientation(IntPtr modelKeyPoints, IntPtr observedKeyPoints, IntPtr indices, IntPtr mask, double scaleIncrement, int rotationBins);

      /// <summary>
      /// Recover the homography matrix using RANDSAC. If the matrix cannot be recovered, null is returned.
      /// </summary>
      /// <returns>The homography matrix, if it cannot be found, null is returned</returns>
      public static HomographyMatrix GetHomographyMatrixFromMatchedFeatures(VectorOfKeyPoint model, VectorOfKeyPoint observed, Matrix<int> matchIndices, Matrix<Byte> mask)
      {
         HomographyMatrix homography = new HomographyMatrix();
         bool found = getHomographyMatrixFromMatchedFeatures(model, observed, matchIndices, mask, homography);
         if (found)
         {
            return homography;
         }
         else
         {
            homography.Dispose();
            return null;
         }
      }

      /// <summary>
      /// Filter the matched Features, such that if a match is not unique, it is rejected.
      /// </summary>
      /// <param name="distance">The matched distances, should have at lease 2 col</param>
      /// <param name="uniquenessThreshold">The distance different ratio which a match is consider unique, a good number will be 0.8</param>
      /// <param name="mask">This is both input and output. This matrix indicates which row is valid for the matches.</param>
      public static void VoteForUniqueness(Matrix<float> distance, double uniquenessThreshold, Matrix<Byte> mask)
      {
         using (Matrix<float> firstCol = distance.GetCol(0))
         using (Matrix<float> secCol = distance.GetCol(1))
         using (Matrix<float> tmp = new Matrix<float>(firstCol.Size))
         using (Matrix<Byte> maskBuffer = new Matrix<byte>(firstCol.Size))
         {
            CvInvoke.cvDiv(firstCol, secCol, tmp, 1.0);
            CvInvoke.cvCmpS(tmp, uniquenessThreshold, maskBuffer, CvEnum.CMP_TYPE.CV_CMP_LE);
            CvInvoke.cvAnd(maskBuffer, mask, mask, IntPtr.Zero);
         }
      }

      /// <summary>
      /// Eliminate the matched features whose scale and rotation do not aggree with the majority's scale and rotation.
      /// </summary>
      /// <param name="rotationBins">The numbers of bins for rotation, a good value might be 20 (which means each bin covers 18 degree)</param>
      /// <param name="scaleIncrement">This determins the different in scale for neighbour hood bins, a good value might be 1.5 (which means matched features in bin i+1 is scaled 1.5 times larger than matched features in bin i</param>
      /// <param name="matchedFeatures">The matched feature that will be participated in the voting. For each matchedFeatures, only the zero indexed ModelFeature will be considered.</param>
      public static int VoteForSizeAndOrientation(VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints, Matrix<int> indices, Matrix<Byte> mask, double scaleIncrement, int rotationBins)
      {
         return voteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, scaleIncrement, rotationBins);
      }

      /// <summary>
      /// Match the Image feature from the observed image to the features from the model image
      /// </summary>
      /// <param name="observedDescriptors">The Image feature from the observed image</param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="emax">For k-d tree only: the maximum number of leaves to visit.</param>
      /// <returns>The matched features</returns>
      public static void DescriptorMatchKnn(Matrix<float> modelDescriptors, Matrix<float> observedDescriptors, int k, int emax, out Matrix<int> indices, out Matrix<float> dist)
      {
         indices = new Matrix<int>(observedDescriptors.Rows, k);
         dist = new Matrix<float>(observedDescriptors.Rows, k);
         Flann.Index index = new Flann.Index(
            modelDescriptors,
            1);
         index.KnnSearch(observedDescriptors, indices, dist, k, emax);
         CvInvoke.cvSqrt(dist, dist);
      }
   }
}