//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    /// Distance Type
    /// </summary>
    public enum DistType
    {
        /// <summary>
        /// Euclidean
        /// </summary>
        Euclidean = 1,
        /// <summary>
        /// L2
        /// </summary>
        L2 = 1,
        /// <summary>
        /// Manhattan
        /// </summary>
        Manhattan = 2,
        /// <summary>
        /// L1
        /// </summary>
        L1 = 2,
        /// <summary>
        /// Minkowski
        /// </summary>
        Minkowski = 3,
        /// <summary>
        /// Max
        /// </summary>
        Max = 4,
        /// <summary>
        /// HistIntersect
        /// </summary>
        HistIntersect = 5,
        /// <summary>
        /// Hellinger
        /// </summary>
        Hellinger = 6,
        /// <summary>
        /// ChiSquare
        /// </summary>
        ChiSquare = 7,
        /// <summary>
        /// CS
        /// </summary>
        CS = 7,
        /// <summary>
        /// KullbackLeibler
        /// </summary>
        KullbackLeibler = 8,
        /// <summary>
        /// KL
        /// </summary>
        KL = 8,
        /// <summary>
        /// Hamming
        /// </summary>
        Hamming = 9
    }

    /// <summary>
    /// Flann index
    /// </summary>
    public class Index : UnmanagedObject
    {

        #region constructors

        /// <summary>
        /// Create a flann index
        /// </summary>
        /// <param name="values">A row by row matrix of descriptors</param>
        /// <param name="ip">The index parameter</param>
        /// <param name="distType">The distance type</param>
        public Index(IInputArray values, IIndexParams ip, DistType distType = DistType.L2)
        {
            using (InputArray iaValues = values.GetInputArray())
                _ptr = FlannInvoke.cveFlannIndexCreate(iaValues, ip.IndexParamPtr, distType);
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
        /// <param name="eps">The search epsilon</param>
        /// <param name="sorted">If set to true, the search result is sorted</param>
        public void KnnSearch(IInputArray queries, IOutputArray indices, IOutputArray squareDistances, int knn, int checks = 32, float eps = 0, bool sorted = true)
        {
            using (InputArray iaQueries = queries.GetInputArray())
            using (OutputArray oaIndices = indices.GetOutputArray())
            using (OutputArray oaSquareDistances = squareDistances.GetOutputArray())
                FlannInvoke.cveFlannIndexKnnSearch(_ptr, iaQueries, oaIndices, oaSquareDistances, knn, checks, eps, sorted);
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
        /// <param name="eps">The search epsilon</param>
        /// <param name="sorted">If set to true, the search result is sorted</param>
        /// <returns>The number of points in the search radius</returns>
        public int RadiusSearch(IInputArray queries, IOutputArray indices, IOutputArray squareDistances, double radius, int maxResults, int checks = 32, float eps = 0, bool sorted = true)
        {
            using (InputArray iaQueries = queries.GetInputArray())
            using (OutputArray oaIndicies = indices.GetOutputArray())
            using (OutputArray oaSquareDistances = squareDistances.GetOutputArray())
                return FlannInvoke.cveFlannIndexRadiusSearch(_ptr, iaQueries, oaIndicies, oaSquareDistances, radius, maxResults, checks, eps, sorted);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Flann Index
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                FlannInvoke.cveFlannIndexRelease(ref _ptr);
            }
        }
    }
}

