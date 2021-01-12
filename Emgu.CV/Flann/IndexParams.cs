//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// When passing an object of this type, the index will perform a linear, brute-force search.
    /// </summary>
    public class LinearIndexParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearIndexParams"/> class.
        /// </summary>
        public LinearIndexParams()
        {
            _ptr = FlannInvoke.cveLinearIndexParamsCreate(ref _indexParamPtr);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveLinearIndexParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// When passing an object of this type the index constructed will consist of a set of randomized kd-trees which will be searched in parallel.
    /// </summary>
    public class KdTreeIndexParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="KdTreeIndexParams"/> class.
        /// </summary>
        /// <param name="trees">The number of parallel kd-trees to use. Good values are in the range [1..16]</param>
        public KdTreeIndexParams(int trees = 4)
        {
            _ptr = FlannInvoke.cveKDTreeIndexParamsCreate(ref _indexParamPtr, trees);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveKDTreeIndexParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// When using a parameters object of this type the index created uses multi-probe LSH (by Multi-Probe LSH: Efficient Indexing for High-Dimensional Similarity Search by Qin Lv, William Josephson, Zhe Wang, Moses Charikar, Kai Li., Proceedings of the 33rd International Conference on Very Large Data Bases (VLDB). Vienna, Austria. September 2007)
    /// </summary>
    public class LshIndexParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="LshIndexParams"/> class.
        /// </summary>
        /// <param name="tableNumber">The number of hash tables to use (between 10 and 30 usually).</param>
        /// <param name="keySize">The size of the hash key in bits (between 10 and 20 usually).</param>
        /// <param name="multiProbeLevel">The number of bits to shift to check for neighboring buckets (0 is regular LSH, 2 is recommended).</param>
        public LshIndexParams(int tableNumber, int keySize, int multiProbeLevel)
        {
            _ptr = FlannInvoke.cveLshIndexParamsCreate(ref _indexParamPtr, tableNumber, keySize, multiProbeLevel);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveLshIndexParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// When passing an object of this type the index constructed will be a hierarchical k-means tree.
    /// </summary>
    public class KMeansIndexParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="KMeansIndexParams"/> class.
        /// </summary>
        /// <param name="branching">The branching factor to use for the hierarchical k-means tree</param>
        /// <param name="iterations"> The maximum number of iterations to use in the k-means clustering stage when building the k-means tree. A value of -1 used here means that the k-means clustering should be iterated until convergence</param>
        /// <param name="centersInit">The algorithm to use for selecting the initial centers when performing a k-means clustering step. The possible values are CENTERS_RANDOM (picks the initial cluster centers randomly), CENTERS_GONZALES (picks the initial centers using Gonzales’ algorithm) and CENTERS_KMEANSPP (picks the initial centers using the algorithm suggested in arthur_kmeanspp_2007 )</param>
        /// <param name="cbIndex">This parameter (cluster boundary index) influences the way exploration is performed in the hierarchical kmeans tree. When cb_index is zero the next kmeans domain to be explored is chosen to be the one with the closest center. A value greater then zero also takes into account the size of the domain.</param>
        public KMeansIndexParams(int branching = 32, int iterations = 11, Flann.CenterInitType centersInit = CenterInitType.Random, float cbIndex = 0.2f)
        {
            _ptr = FlannInvoke.cveKMeansIndexParamsCreate(ref _indexParamPtr, branching, iterations, centersInit, cbIndex);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveKMeansIndexParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// When using a parameters object of this type the index created combines the randomized kd-trees and the hierarchical k-means tree.
    /// </summary>
    public class CompositeIndexParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeIndexParams"/> class.
        /// </summary>
        /// <param name="trees">The number of parallel kd-trees to use. Good values are in the range [1..16]</param>
        /// <param name="branching">The branching factor to use for the hierarchical k-means tree</param>
        /// <param name="iterations"> The maximum number of iterations to use in the k-means clustering stage when building the k-means tree. A value of -1 used here means that the k-means clustering should be iterated until convergence</param>
        /// <param name="centersInit">The algorithm to use for selecting the initial centers when performing a k-means clustering step. The possible values are CENTERS_RANDOM (picks the initial cluster centers randomly), CENTERS_GONZALES (picks the initial centers using Gonzales’ algorithm) and CENTERS_KMEANSPP (picks the initial centers using the algorithm suggested in arthur_kmeanspp_2007 )</param>
        /// <param name="cbIndex">This parameter (cluster boundary index) influences the way exploration is performed in the hierarchical kmeans tree. When cb_index is zero the next kmeans domain to be explored is chosen to be the one with the closest center. A value greater then zero also takes into account the size of the domain.</param>
        public CompositeIndexParams(int trees = 4, int branching = 32, int iterations = 11, Flann.CenterInitType centersInit = CenterInitType.Random, float cbIndex = 0.2f)
        {
            _ptr = FlannInvoke.cveCompositeIndexParamsCreate(ref _indexParamPtr, trees, branching, iterations, centersInit, cbIndex);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveCompositeIndexParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// When passing an object of this type the index created is automatically tuned to offer the best performance, by choosing the optimal index type (randomized kd-trees, hierarchical kmeans, linear) and parameters for the dataset provided.
    /// </summary>
    public class AutotunedIndexParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutotunedIndexParams"/> class.
        /// </summary>
        /// <param name="targetPrecision"> Is a number between 0 and 1 specifying the percentage of the approximate nearest-neighbor searches that return the exact nearest-neighbor. Using a higher value for this parameter gives more accurate results, but the search takes longer. The optimum value usually depends on the application.</param>
        /// <param name="buildWeight">Specifies the importance of the index build time reported to the nearest-neighbor search time. In some applications it’s acceptable for the index build step to take a long time if the subsequent searches in the index can be performed very fast. In other applications it’s required that the index be build as fast as possible even if that leads to slightly longer search times.</param>
        /// <param name="memoryWeight">Is used to specify the trade off between time (index build time and search time) and memory used by the index. A value less than 1 gives more importance to the time spent and a value greater than 1 gives more importance to the memory usage.</param>
        /// <param name="sampleFraction">Is a number between 0 and 1 indicating what fraction of the dataset to use in the automatic parameter configuration algorithm. Running the algorithm on the full dataset gives the most accurate results, but for very large datasets can take longer than desired. In such case using just a fraction of the data helps speeding up this algorithm while still giving good approximations of the optimum parameters.</param>
        public AutotunedIndexParams(float targetPrecision = 0.8f, float buildWeight = 0.01f, float memoryWeight = 0, float sampleFraction = 0.1f)
        {
            _ptr = FlannInvoke.cveAutotunedIndexParamsCreate(ref _indexParamPtr, targetPrecision, buildWeight, memoryWeight, sampleFraction);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveAutotunedIndexParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Hierarchical Clustering Index Parameters
    /// </summary>
    public class HierarchicalClusteringIndexParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalClusteringIndexParams"/>.
        /// </summary>
        /// <param name="branching">branching</param>
        /// <param name="centersInit">Center initialization method</param>
        /// <param name="trees">Trees</param>
        /// <param name="leafSize">Leaf Size</param>
        public HierarchicalClusteringIndexParams(
            int branching = 32,
            Flann.CenterInitType centersInit = CenterInitType.Random,
            int trees = 4,
            int leafSize = 100)
        {
            _ptr = FlannInvoke.cveHierarchicalClusteringIndexParamsCreate(ref _indexParamPtr, branching, centersInit, trees, leafSize);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveHierarchicalClusteringIndexParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Search parameters
    /// </summary>
    public class SearchParams : UnmanagedObject, IIndexParams
    {
        private IntPtr _indexParamPtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchParams"/> class.
        /// </summary>
        /// <param name="checks">how many leafs to visit when searching for neighbors (-1 for unlimited)</param>
        /// <param name="eps">Search for eps-approximate neighbors </param>
        /// <param name="sorted">Only for radius search, require neighbors sorted by distance </param>
        public SearchParams(int checks = 32, float eps = 0, bool sorted = true)
        {
            _ptr = FlannInvoke.cveSearchParamsCreate(ref _indexParamPtr, checks, eps, sorted);
        }

        IntPtr IIndexParams.IndexParamPtr
        {
            get { return _indexParamPtr; }
        }

        /// <summary>
        /// Release all the memory associated with this IndexParam
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                FlannInvoke.cveSearchParamsRelease(ref _ptr);
                _indexParamPtr = IntPtr.Zero;
            }
        }
    }

}
