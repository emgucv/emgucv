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
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern IntPtr CvFlannIndexCreateLinear(IntPtr features);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern IntPtr CvFlannIndexCreateKDTree(IntPtr features, int numberOfKDTrees);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern IntPtr CvFlannIndexCreateKMeans(IntPtr features, int branching, int iterations, CenterInitType centersInitType, float cbIndex);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern IntPtr CvFlannIndexCreateComposite(IntPtr features, int numberOfKDTrees, int branching, int iterations, CenterInitType centersInitType, float cbIndex);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void CvFlannIndexRelease(IntPtr index);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void CvFlannIndexKnnSearch(IntPtr index, IntPtr queries, IntPtr indices, IntPtr dists, int knn, int checks);
      #endregion

      #region constructors
      /// <summary>
      /// Create a flann index using multiple KDTrees
      /// </summary>
      /// <param name="numberOfKDTrees">The number of KDTrees to be used</param>
      /// <param name="values">A row by row matrix of descriptors</param>
      public Index(Matrix<float> values, int numberOfKDTrees)
      {
         _ptr = CvFlannIndexCreateKDTree(values, numberOfKDTrees);
      }

      /// <summary>
      /// Create a flann index using a composition of KDTreee and KMeans tree
      /// </summary>
      /// <param name="numberOfKDTrees">The number of KDTrees to be used</param>
      /// <param name="values">A row by row matrix of descriptors</param>
      /// <param name="branching">Branching factor (for kmeans tree), use 32 for default</param>
      /// <param name="iterations">Max iterations to perform in one kmeans clustering (kmeans tree), use 11 for deafault</param>
      /// <param name="centersInitType">Algorithm used for picking the initial cluster centers for kmeans tree, use RANDOM for default</param>
      /// <param name="cbIndex">Cluster boundary index. Used when searching the kmeans tree. Use 0.2 for default</param>
      public Index(Matrix<float> values, int numberOfKDTrees, int branching, int iterations, CenterInitType centersInitType, float cbIndex)
      {
         _ptr = CvFlannIndexCreateComposite(values, numberOfKDTrees, branching, iterations, centersInitType, cbIndex);
      }

      /// <summary>
      /// Create a flann index using Kmeans
      /// </summary>
      /// <param name="values">A row by row matrix of descriptors</param>
      /// <param name="branching">Branching factor (for kmeans tree), use 32 for default</param>
      /// <param name="iterations">Max iterations to perform in one kmeans clustering (kmeans tree), use 11 for deafault</param>
      /// <param name="centersInitType">Algorithm used for picking the initial cluster centers for kmeans tree, use RANDOM for default</param>
      /// <param name="cbIndex">Cluster boundary index. Used when searching the kmeans tree. Use 0.2 for default</param>
      public Index(Matrix<float> values, int branching, int iterations, CenterInitType centersInitType, float cbIndex)
      {
         _ptr = CvFlannIndexCreateKMeans(values, branching, iterations, centersInitType, cbIndex);
      }

      /// <summary>
      /// Create a linear flann index
      /// </summary>
      /// <param name="values">A row by row matrix of descriptors</param>
      public Index(Matrix<float> values)
      {
         _ptr = CvFlannIndexCreateLinear(values);
      }
      #endregion

      /// <summary>
      /// Perform k-nearest-neighbours (KNN) search
      /// </summary>
      /// <param name="queries">A row by row matrix of descriptors to be query for nearest neighbours</param>
      /// <param name="indices">The result of the indices of the k-nearest neighbours</param>
      /// <param name="distances">The distance of between the neighbours</param>
      /// <param name="knn">The number of neighbours to be searched</param>
      /// <param name="checks">Use 32 for default</param>
      public void KnnSearch(Matrix<float> queries, Matrix<int> indices, Matrix<float> distances, int knn, int checks)
      {
         CvFlannIndexKnnSearch(_ptr, queries, indices, distances, knn, checks);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this Flann Index
      /// </summary>
      protected override void DisposeObject()
      {
         CvFlannIndexRelease(_ptr);
      }
   }
}
