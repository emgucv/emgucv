using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// A wrapper for CvFeatureTree
   /// </summary>
   public class FeatureTree : UnmanagedObject
   {
      private Matrix<float> _descriptorMatrix;

      /// <summary>
      /// Create a feature tree from the specific feature descriptors
      /// </summary>
      /// <param name="descriptors">The array of feature descriptors</param>
      public FeatureTree(Matrix<float>[] descriptors)
      {
         _descriptorMatrix = DescriptorsToMatrix(descriptors);
         _ptr = CvInvoke.cvCreateFeatureTree(_descriptorMatrix.Ptr);
      }

      private static Matrix<float> DescriptorsToMatrix(Matrix<float>[] descriptors)
      {
         Matrix<float> res = new Matrix<float>(descriptors.Length, descriptors[0].Rows);
         IntPtr rowHeader = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MCvMat)));
         for (int i = 0; i < descriptors.Length; i++)
         {
            CvInvoke.cvGetRows(res, rowHeader, i, i + 1, 1);
            CvInvoke.cvTranspose(descriptors[i], rowHeader);
         }
         Marshal.FreeHGlobal(rowHeader);
         return res;
      }

      /// <summary>
      /// Finds (with high probability) the k nearest neighbors in tr for each of the given (row-)vectors in desc, using best-bin-first searching ([Beis97]). The complexity of the entire operation is at most O(m*emax*log2(n)), where n is the number of vectors in the tree
      /// </summary>
      /// <param name="descriptors">The m feature descriptors to be searched from the feature tree</param>
      /// <param name="results">
      /// The results of the best <paramref name="k"/> matched from the feature tree. A m x <paramref name="k"/> matrix. Contains -1 in some columns if fewer than k neighbors found. 
      /// For each row the k neareast neighbors are not sorted. To findout the closet neighbour, look at the output matrix <paramref name="dist"/>.
      /// </param>
      /// <param name="dist">
      /// A m x <paramref name="k"/> matrix of the distances to k nearest neighbors
      /// </param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="emax">The maximum number of leaves to visit</param>
      public void FindFeatures(Matrix<float>[] descriptors, out Matrix<Int32> results, out Matrix<double> dist, int k, int emax)
      {
         int numberOfDescriptors = descriptors.Length;
         results = new Matrix<Int32>(numberOfDescriptors, k);
         dist = new Matrix<double>(numberOfDescriptors, k);

         Matrix<float> descriptorMatrix = DescriptorsToMatrix(descriptors);
         CvInvoke.cvFindFeatures(Ptr, descriptorMatrix.Ptr, results.Ptr, dist.Ptr, k, emax);
      }

      /// <summary>
      /// Release the unmanaged structure and all the memories associate with it.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseFeatureTree(_ptr);
         _ptr = IntPtr.Zero;
      }
   }
}
