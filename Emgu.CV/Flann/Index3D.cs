//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace Emgu.CV.Flann
{
    /// <summary>
    /// Create index for 3D points
    /// </summary>
    public class Index3D : UnmanagedObject
    {
        private MCvPoint3D32f[] _points;
        private Index _flannIndex;
        private Mat _dataMat;
        private GCHandle _dataHandle;

        /// <summary>
        /// Create a flann index for 3D points
        /// </summary>
        /// <param name="points">The IPosition3D array</param>
        /// <param name="ip">The index parameters</param>
        public Index3D(MCvPoint3D32f[] points, IIndexParams ip)
        {
            _points = points;

            _dataHandle = GCHandle.Alloc(_points, GCHandleType.Pinned);
            _dataMat = new Mat(
                _points.Length,
                3,
                DepthType.Cv32F,
                1,
                _dataHandle.AddrOfPinnedObject(),
                3 * sizeof(float));

            _flannIndex = new Index(_dataMat, ip);
        }

        /// <summary>
        /// A neighbor point
        /// </summary>
        public struct Neighbor
        {
            /// <summary>
            /// The index of the point
            /// </summary>
            public int Index;
            /// <summary>
            /// The square distance
            /// </summary>
            public float SquareDist;

        }

        /// <summary>
        /// Find the approximate nearest position in 3D
        /// </summary>
        /// <param name="position">The position to start the search from</param>
        /// <returns>The nearest neighbor (may be an approximation, depends in the index type).</returns>
        public Neighbor NearestNeighbor(MCvPoint3D32f position)
        {
            using (Mat query = new Mat(1, 3, DepthType.Cv32F, 1))
            using (Mat index = new Mat(1, 1, DepthType.Cv32S, 1))
            using (Mat distance = new Mat(1, 1, DepthType.Cv32F, 1))
            {
                float[] queryData = new float[] { position.X, position.Y, position.Z };
                Marshal.Copy(queryData, 0, query.DataPointer, queryData.Length);

                _flannIndex.KnnSearch(query, index, distance, 1, 1);

                int[] indexVal = new int[1];
                index.CopyTo(indexVal);
                float[] distanceVal = new float[1];
                distance.CopyTo(distanceVal);

                Neighbor n = new Neighbor();
                n.Index = indexVal[0];
                n.SquareDist = distanceVal[0];

                return n;
            }
        }

        /// <summary>
        /// Perform a search within the given radius
        /// </summary>
        /// <param name="position">The center of the search area</param>
        /// <param name="radius">The radius of the search</param>
        /// <param name="maxResults">The maximum number of results to return</param>
        /// <returns>The neighbors found</returns>
        public Neighbor[] RadiusSearch(MCvPoint3D32f position, double radius, int maxResults)
        {
            using (Mat query = new Mat(1, 3, DepthType.Cv32F, 1))
            {
                float[] queryData = new float[] { position.X, position.Y, position.Z };
                Marshal.Copy(queryData, 0, query.DataPointer, queryData.Length);

                using (Mat indicies = new Mat(new Size(maxResults, 1), DepthType.Cv32S, 1))
                using (Mat sqrDistances = new Mat(new Size(maxResults, 1), DepthType.Cv32F, 1))
                {
                    indicies.SetTo(new MCvScalar(-1));
                    sqrDistances.SetTo(new MCvScalar(-1));
                    _flannIndex.RadiusSearch(query, indicies, sqrDistances, radius, maxResults);
                    int[] indiciesVal = new int[indicies.Rows * indicies.Cols];
                    indicies.CopyTo(indiciesVal);
                    float[] sqrDistancesVal = new float[sqrDistances.Rows * sqrDistances.Cols];
                    sqrDistances.CopyTo(sqrDistancesVal);

                    List<Neighbor> neighbors = new List<Neighbor>();
                    int resultCount = Math.Min(maxResults, indiciesVal.Length);
                    for (int j = 0; j < resultCount; j++)
                    {
                        if (indiciesVal[j] < 0)
                            break;
                        Neighbor n = new Neighbor();
                        n.Index = indiciesVal[j];
                        n.SquareDist = sqrDistancesVal[j];
                        neighbors.Add(n);
                    }

                    return neighbors.ToArray();
                }

            }
        }

        /// <summary>
        /// Release the resource used by this object
        /// </summary>
        protected override void DisposeObject()
        {
            _flannIndex.Dispose();
            _dataMat.Dispose();
            _dataHandle.Free();
        }
    }
}
