//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.Features
{
    /// <summary>
    /// Approximate nearest neighbor index based on Annoy, introduced in OpenCV 5.
    /// </summary>
    public partial class ANNIndex : UnmanagedObject
    {
        /// <summary>
        /// Metrics used to calculate the distance between two feature vectors.
        /// </summary>
        public enum Distance
        {
            /// <summary>
            /// Euclidean distance
            /// </summary>
            Euclidean = 0,
            /// <summary>
            /// Manhattan distance
            /// </summary>
            Manhattan = 1,
            /// <summary>
            /// Angular distance
            /// </summary>
            Angular = 2,
            /// <summary>
            /// Hamming distance
            /// </summary>
            Hamming = 3,
            /// <summary>
            /// Dot product distance
            /// </summary>
            DotProduct = 4
        }

        private IntPtr _sharedPtr;

        /// <summary>
        /// Create an instance of the annoy index.
        /// </summary>
        /// <param name="dim">The dimension of the feature vector.</param>
        /// <param name="distType">Metric to calculate the distance between two feature vectors.</param>
        public ANNIndex(int dim, Distance distType = Distance.Euclidean)
        {
            _ptr = FeaturesInvoke.cveANNIndexCreate(dim, distType, ref _sharedPtr);
        }

        /// <summary>
        /// Add feature vectors to the index.
        /// </summary>
        /// <param name="features">Matrix containing the feature vectors to index. The size of the matrix is num_features x feature_dimension.</param>
        public void AddItems(IInputArray features)
        {
            using (InputArray iaFeatures = features.GetInputArray())
                FeaturesInvoke.cveANNIndexAddItems(_ptr, iaFeatures);
        }

        /// <summary>
        /// Build the index.
        /// </summary>
        /// <param name="trees">Number of trees in the index. If not provided, the number is determined automatically in a way that at most 2x as much memory as the features vectors take is used.</param>
        public void Build(int trees = -1)
        {
            FeaturesInvoke.cveANNIndexBuild(_ptr, trees);
        }

        /// <summary>
        /// Performs a K-nearest neighbor search for given query vector(s) using the index.
        /// </summary>
        /// <param name="query">The query vector(s).</param>
        /// <param name="indices">Matrix that will contain the indices of the K-nearest neighbors found.</param>
        /// <param name="dists">Matrix that will contain the distances to the K-nearest neighbors found.</param>
        /// <param name="knn">Number of nearest neighbors to search for.</param>
        /// <param name="searchK">The maximum number of nodes to inspect, which defaults to trees x knn if not provided.</param>
        public void KnnSearch(IInputArray query, IOutputArray indices, IOutputArray dists, int knn, int searchK = -1)
        {
            using (InputArray iaQuery = query.GetInputArray())
            using (OutputArray oaIndices = indices.GetOutputArray())
            using (OutputArray oaDists = dists.GetOutputArray())
                FeaturesInvoke.cveANNIndexKnnSearch(_ptr, iaQuery, oaIndices, oaDists, knn, searchK);
        }

        /// <summary>
        /// Save the index to disk and loads it. After saving, no more vectors can be added.
        /// </summary>
        /// <param name="fileName">Filename of the index to be saved.</param>
        /// <param name="prefault">If prefault is set to true, it will pre-read the entire file into memory.</param>
        public void Save(String fileName, bool prefault = false)
        {
            using (CvString csFileName = new CvString(fileName))
                FeaturesInvoke.cveANNIndexSave(_ptr, csFileName, prefault);
        }

        /// <summary>
        /// Loads (mmaps) an index from disk.
        /// </summary>
        /// <param name="fileName">Filename of the index to be loaded.</param>
        /// <param name="prefault">If prefault is set to true, it will pre-read the entire file into memory.</param>
        public void Load(String fileName, bool prefault = false)
        {
            using (CvString csFileName = new CvString(fileName))
                FeaturesInvoke.cveANNIndexLoad(_ptr, csFileName, prefault);
        }

        /// <summary>
        /// The number of trees in the index.
        /// </summary>
        public int TreeNumber
        {
            get { return FeaturesInvoke.cveANNIndexGetTreeNumber(_ptr); }
        }

        /// <summary>
        /// The number of feature vectors in the index.
        /// </summary>
        public int ItemNumber
        {
            get { return FeaturesInvoke.cveANNIndexGetItemNumber(_ptr); }
        }

        /// <summary>
        /// Prepare to build the index in the specified file instead of RAM. Execute before adding items; no need to save after build.
        /// </summary>
        /// <param name="fileName">Filename of the index to be built.</param>
        /// <returns>True if successful.</returns>
        public bool SetOnDiskBuild(String fileName)
        {
            using (CvString csFileName = new CvString(fileName))
                return FeaturesInvoke.cveANNIndexSetOnDiskBuild(_ptr, csFileName);
        }

        /// <summary>
        /// Initialize the random number generator with the given seed. Only necessary to pass this before adding the items. Will have no effect after calling Build() or Load().
        /// </summary>
        /// <param name="seed">The given seed of the random number generator.</param>
        public void SetSeed(int seed)
        {
            FeaturesInvoke.cveANNIndexSetSeed(_ptr, seed);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                FeaturesInvoke.cveANNIndexRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class FeaturesInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveANNIndexCreate(int dim, ANNIndex.Distance distType, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANNIndexRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANNIndexAddItems(IntPtr annIndex, IntPtr features);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANNIndexBuild(IntPtr annIndex, int trees);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANNIndexKnnSearch(
            IntPtr annIndex,
            IntPtr query,
            IntPtr indices,
            IntPtr dists,
            int knn,
            int searchK);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANNIndexSave(
            IntPtr annIndex,
            IntPtr fileName,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool prefault);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANNIndexLoad(
            IntPtr annIndex,
            IntPtr fileName,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool prefault);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveANNIndexGetTreeNumber(IntPtr annIndex);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveANNIndexGetItemNumber(IntPtr annIndex);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveANNIndexSetOnDiskBuild(IntPtr annIndex, IntPtr fileName);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANNIndexSetSeed(IntPtr annIndex, int seed);
    }
}
