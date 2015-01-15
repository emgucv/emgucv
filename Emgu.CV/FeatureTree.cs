/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A wrapper for CvFeatureTree
   /// </summary>
   public class FeatureTree : UnmanagedObject
   {
      private Matrix<float> _descriptorMatrix;

      /// <summary>
      /// Get the descriptor matrix used by this feature tree
      /// </summary>
      public Matrix<float> DescriptorMatrix
      {
         get
         {
            return _descriptorMatrix;
         }
      }

      /// <summary>
      /// Create a k-d tree from the specific feature descriptors
      /// </summary>
      /// <param name="descriptors">The array of feature descriptors</param>
      public FeatureTree(float[][] descriptors)
      {
         _descriptorMatrix = CvToolbox.GetMatrixFromArrays(descriptors);
         _ptr = CvInvoke.cvCreateKDTree(_descriptorMatrix.Ptr);
      }

      /// <summary>
      /// Create a k-d tree from the specific feature descriptors
      /// </summary>
      /// <param name="descriptorMatrix">The array of feature descriptors</param>
      public FeatureTree(Matrix<float> descriptorMatrix)
      {
         _descriptorMatrix = descriptorMatrix.Clone();
         _ptr = CvInvoke.cvCreateKDTree(_descriptorMatrix.Ptr);
      }

      /// <summary>
      /// Create a spill tree from the specific feature descriptors
      /// </summary>
      /// <param name="descriptors">The array of feature descriptors</param>
      /// <param name="naive">A good value is 50</param>
      /// <param name="rho">A good value is .7</param>
      /// <param name="tau">A good value is .1</param>
      public FeatureTree(float[][] descriptors, int naive, double rho, double tau)
      {
         _descriptorMatrix = CvToolbox.GetMatrixFromArrays(descriptors);
         _ptr = CvInvoke.cvCreateSpillTree(_descriptorMatrix.Ptr, naive, rho, tau);
      }

      /// <summary>
      /// Create a spill tree from the specific feature descriptors
      /// </summary>
      /// <param name="descriptors">The array of feature descriptors</param>
      /// <param name="naive">A good value is 50</param>
      /// <param name="rho">A good value is .7</param>
      /// <param name="tau">A good value is .1</param>
      public FeatureTree(Matrix<float> descriptors, int naive, double rho, double tau)
      {
         _descriptorMatrix = descriptors.Clone();
         _ptr = CvInvoke.cvCreateSpillTree(_descriptorMatrix.Ptr, naive, rho, tau);
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
      /// A m x <paramref name="k"/> Matrix of the distances to k nearest neighbors
      /// </param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="emax">For k-d tree only: the maximum number of leaves to visit. Use 20 if not sure</param>
      public void FindFeatures(float[][] descriptors, out Matrix<Int32> results, out Matrix<double> dist, int k, int emax)
      {
         int numberOfDescriptors = descriptors.Length;
         results = new Matrix<Int32>(numberOfDescriptors, k);
         dist = new Matrix<double>(numberOfDescriptors, k);
         FindFeatures(descriptors, results, dist, k, emax);
      }
      
      /// <summary>
      /// Finds (with high probability) the k nearest neighbors in tree for each of the given (row-)vectors in desc, using best-bin-first searching ([Beis97]). The complexity of the entire operation is at most O(m*emax*log2(n)), where n is the number of vectors in the tree
      /// </summary>
      /// <param name="descriptors">The m feature descriptors to be searched from the feature tree</param>
      /// <param name="results">
      /// The results of the best <paramref name="k"/> matched from the feature tree. A m x <paramref name="k"/> matrix. Contains -1 in some columns if fewer than k neighbors found. 
      /// For each row the k neareast neighbors are not sorted. To findout the closet neighbour, look at the output matrix <paramref name="dist"/>.
      /// </param>
      /// <param name="dist">
      /// A m x <paramref name="k"/> Matrix of the distances to k nearest neighbors
      /// </param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="emax">For k-d tree only: the maximum number of leaves to visit. Use 20 if not sure</param>
      private void FindFeatures(float[][] descriptors, Matrix<Int32> results, Matrix<double> dist, int k, int emax)
      {
         using (Matrix<float> descriptorMatrix = CvToolbox.GetMatrixFromArrays(descriptors))
            CvInvoke.cvFindFeatures(Ptr, descriptorMatrix.Ptr, results.Ptr, dist.Ptr, k, emax);
      }

      /// <summary>
      /// Release the unmanaged structure and all the memories associate with it.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseFeatureTree(_ptr);
      }

      /// <summary>
      /// Release the managed resource
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         _descriptorMatrix.Dispose();
      }
   }
}
*/