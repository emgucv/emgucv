//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      /// Create a flann index
      /// </summary>
      /// <param name="values">A row by row matrix of descriptors</param>
      /// <param name="ip">The index parameter</param>
      public Index(IInputArray values, IIndexParams ip)
      {
         using (InputArray iaValues = values.GetInputArray())
            _ptr = cveFlannIndexCreate(iaValues, ip.IndexParamPtr);
      }

      /*
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
      }*/
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
         if (_ptr != IntPtr.Zero)
            CvFlannIndexRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveFlannIndexCreate(IntPtr features, IntPtr ip);

      //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      //internal static extern IntPtr CvFlannIndexCreateComposite(IntPtr features, int numberOfKDTrees, int branching, int iterations, Flann.CenterInitType centersInitType, float cbIndex);

      //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      //internal static extern IntPtr CvFlannIndexCreateAutotuned(IntPtr features, float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvFlannIndexRelease(ref IntPtr index);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvFlannIndexKnnSearch(IntPtr index, IntPtr queries, IntPtr indices, IntPtr dists, int knn, int checks);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int CvFlannIndexRadiusSearch(IntPtr index, IntPtr queries, IntPtr indices, IntPtr dists, float radius, int maxResults, int checks);
   }
}

