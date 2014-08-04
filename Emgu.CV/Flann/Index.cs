//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Flann
{
   /// <summary>
   /// Flann index
   /// </summary>
   public class Index : UnmanagedObject
   {
      static Index()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      #region constructors
      /// <summary>
      /// Create a flann index using multiple KDTrees
      /// </summary>
      /// <param name="numberOfKDTrees">The number of KDTrees to be used</param>
      /// <param name="values">A row by row matrix of descriptors</param>
      public Index(IInputArray values, int numberOfKDTrees)
      {
         using (InputArray iaValues = values.GetInputArray())
            _ptr = CvFlannIndexCreateKDTree(iaValues, numberOfKDTrees);
      }

      /// <summary>
      /// Create a flann index using LSH
      /// </summary>
      /// <param name="values">A row by row matrix of descriptors</param>
      /// <param name="tableNumber">The number of hash tables to use (between 10 and 30 usually).</param>
      /// <param name="keySize">The size of the hash key in bits (between 10 and 20 usually).</param>
      /// <param name="multiProbeLevel">The number of bits to shift to check for neighboring buckets (0 is regular LSH, 2 is recommended).</param>
      public Index(IInputArray values, int tableNumber, int keySize, int multiProbeLevel)
      {
         using (InputArray iaValues = values.GetInputArray())
            _ptr = CvFlannIndexCreateLSH(iaValues, tableNumber, keySize, multiProbeLevel);
      }

      /// <summary>
      /// Create a flann index using a composition of KDTreee and KMeans tree
      /// </summary>
      /// <param name="numberOfKDTrees">The number of KDTrees to be used</param>
      /// <param name="values">A row by row matrix of descriptors</param>
      /// <param name="branching">Branching factor (for kmeans tree), use 32 for default</param>
      /// <param name="iterations">Max iterations to perform in one kmeans clustering (kmeans tree), use 11 for deafault</param>
      /// <param name="centersInitType">Algorithm used for picking the initial cluster centers for kmeans tree, use Random for default</param>
      /// <param name="cbIndex">Cluster boundary index. Used when searching the kmeans tree. Use 0.2 for default</param>
      public Index(IInputArray values, int numberOfKDTrees, int branching, int iterations, CenterInitType centersInitType, float cbIndex)
      {
         using (InputArray iaValues = values.GetInputArray())
            _ptr = CvFlannIndexCreateComposite(iaValues, numberOfKDTrees, branching, iterations, centersInitType, cbIndex);
      }

      /// <summary>
      /// Create a flann index using Kmeans
      /// </summary>
      /// <param name="values">A row by row matrix of descriptors</param>
      /// <param name="branching">Branching factor (for kmeans tree), use 32 for default</param>
      /// <param name="iterations">Max iterations to perform in one kmeans clustering (kmeans tree), use 11 for deafault</param>
      /// <param name="centersInitType">Algorithm used for picking the initial cluster centers for kmeans tree, use Random for default</param>
      /// <param name="cbIndex">Cluster boundary index. Used when searching the kmeans tree. Use 0.2 for default</param>
      public Index(IInputArray values, int branching, int iterations, CenterInitType centersInitType, float cbIndex)
      {
         using (InputArray iaValues = values.GetInputArray())
            _ptr = CvFlannIndexCreateKMeans(iaValues, branching, iterations, centersInitType, cbIndex);
      }

      /// <summary>
      /// Create a linear flann index
      /// </summary>
      /// <param name="values">A row by row matrix of descriptors</param>
      public Index(IInputArray values)
      {
         using (InputArray iaValues = values.GetInputArray())
            _ptr = CvFlannIndexCreateLinear(iaValues);
      }

      /// <summary>
      /// Create an auto-tuned flann index
      /// </summary>
      /// <param name="values">A row by row matrix of descriptors</param>
      /// <param name="targetPrecision">Precision desired, use 0.9 if not sure</param>
      /// <param name="buildWeight">build tree time weighting factor, use 0.01 if not sure</param>
      /// <param name="memoryWeight">index memory weighting factor, use 0 if not sure</param>
      /// <param name="sampleFraction">what fraction of the dataset to use for autotuning, use 0.1 if not sure</param>
      public Index(IInputArray values, float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction)
      {
         using (InputArray iaValues = values.GetInputArray())
            _ptr = CvFlannIndexCreateAutotuned(iaValues, targetPrecision, buildWeight, memoryWeight, sampleFraction);
      }
      #endregion

      /// <summary>
      /// Perform k-nearest-neighbours (KNN) search
      /// </summary>
      /// <param name="queries">A row by row matrix of descriptors to be query for nearest neighbours</param>
      /// <param name="indices">The result of the indices of the k-nearest neighbours</param>
      /// <param name="squareDistances">The square of the Eculidean distance between the neighbours</param>
      /// <param name="knn">Number of nearest neighbors to search for</param>
      /// <param name="checks">The number of times the tree(s) in the index should be recursively traversed. A
      /// higher value for this parameter would give better search precision, but also take more
      /// time. If automatic configuration was used when the index was created, the number of
      /// checks required to achieve the specified precision was also computed, in which case
      /// this parameter is ignored </param>
      public void KnnSearch(IInputArray queries, IOutputArray indices, IOutputArray squareDistances, int knn, int checks)
      {
         using (InputArray iaQueries = queries.GetInputArray())
         using (OutputArray oaIndices = indices.GetOutputArray())
         using (OutputArray oaSquareDistances = squareDistances.GetOutputArray())
         CvFlannIndexKnnSearch(_ptr, iaQueries, oaIndices, oaSquareDistances, knn, checks);
      }

      /// <summary>
      /// Performs a radius nearest neighbor search for multiple query points
      /// </summary>
      /// <param name="queries">The query points, one per row</param>
      /// <param name="indices">Indices of the nearest neighbors found</param>
      /// <param name="squareDistances">The square of the Eculidean distance between the neighbours</param>
      /// <param name="radius">The search radius</param>
      /// <param name="maxResults">The maximum number of results</param>
      /// <param name="checks">The number of times the tree(s) in the index should be recursively traversed. A
      /// higher value for this parameter would give better search precision, but also take more
      /// time. If automatic configuration was used when the index was created, the number of
      /// checks required to achieve the specified precision was also computed, in which case
      /// this parameter is ignored </param>
      /// <returns>The number of points in the search radius</returns>
      public int RadiusSearch(IInputArray queries, IOutputArray indices, IOutputArray squareDistances, float radius, int maxResults, int checks)
      {
         using (InputArray iaQueries = queries.GetInputArray())
         using (OutputArray oaIndicies = indices.GetOutputArray())
         using (OutputArray oaSquareDistances = squareDistances.GetOutputArray())
         return CvFlannIndexRadiusSearch(_ptr, iaQueries, oaIndicies, oaSquareDistances, radius, maxResults, checks);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Flann Index
      /// </summary>
      protected override void DisposeObject()
      {
         CvFlannIndexRelease(_ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvFlannIndexCreateLinear(IntPtr features);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvFlannIndexCreateLSH(IntPtr features, int tableNumber, int keySize, int multiProbeLevel);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvFlannIndexCreateKDTree(IntPtr features, int numberOfKDTrees);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvFlannIndexCreateKMeans(IntPtr features, int branching, int iterations, Flann.CenterInitType centersInitType, float cbIndex);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvFlannIndexCreateComposite(IntPtr features, int numberOfKDTrees, int branching, int iterations, Flann.CenterInitType centersInitType, float cbIndex);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvFlannIndexCreateAutotuned(IntPtr features, float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvFlannIndexRelease(IntPtr index);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvFlannIndexKnnSearch(IntPtr index, IntPtr queries, IntPtr indices, IntPtr dists, int knn, int checks);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int CvFlannIndexRadiusSearch(IntPtr index, IntPtr queries, IntPtr indices, IntPtr dists, float radius, int maxResults, int checks);
   }
}

