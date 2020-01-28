//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        private Matrix<float> _dataMatrix;
        private Matrix<float> _query;
        private Matrix<float> _distance;
        private Matrix<int> _index;
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
            _dataMatrix = new Matrix<float>(_points.Length, 3, _dataHandle.AddrOfPinnedObject());

            _flannIndex = new Index(_dataMatrix, ip);

            _query = new Matrix<float>(1, 3);
            _distance = new Matrix<float>(1, 1);
            _index = new Matrix<int>(1, 1);
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
            _query.Data[0, 0] = position.X;
            _query.Data[0, 1] = position.Y;
            _query.Data[0, 2] = position.Z;
            _flannIndex.KnnSearch(_query, _index, _distance, 1, 1);

            Neighbor n = new Neighbor();
            n.Index = _index.Data[0, 0];
            n.SquareDist = _distance.Data[0, 0];

            return n;
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
            _query.Data[0, 0] = position.X;
            _query.Data[0, 1] = position.Y;
            _query.Data[0, 2] = position.Z;
            using (Mat indicies = new Mat(new Size(maxResults, 1), DepthType.Cv32S, 1))
            using (Mat sqrDistances = new Mat(new Size(maxResults, 1), DepthType.Cv32F, 1))
            {
                indicies.SetTo(new MCvScalar(-1));
                sqrDistances.SetTo(new MCvScalar(-1));
                _flannIndex.RadiusSearch(_query, indicies, sqrDistances, radius, maxResults);
                int[] indiciesVal = new int[indicies.Rows * indicies.Cols];
                indicies.CopyTo(indiciesVal);
                float[] sqrDistancesVal = new float[sqrDistances.Rows * sqrDistances.Cols];
                sqrDistances.CopyTo(sqrDistancesVal);

                List<Neighbor> neighbors = new List<Neighbor>();
                for (int j = 0; j < maxResults; j++)
                {
                    if (indiciesVal[j] <= 0)
                        break;
                    Neighbor n = new Neighbor();
                    n.Index = indiciesVal[j];
                    n.SquareDist = sqrDistancesVal[j];
                    neighbors.Add(n);
                }

                return neighbors.ToArray();
            }

        }

        /// <summary>
        /// Release the resource used by this object
        /// </summary>
        protected override void DisposeObject()
        {
            _index.Dispose();
            _dataHandle.Free();
            _dataMatrix.Dispose();
            _query.Dispose();
            _distance.Dispose();
        }
    }
}
