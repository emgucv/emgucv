using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Emgu;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// A collection of points
    /// </summary>
    public class PointCollection<D> where D:IComparable, new()
    {
        /// <summary>
        /// A comparator which compares only the X value of the point
        /// </summary>
        private class XValueOfPointComparator : IComparer<Point<D>>
        {
            public int Compare(Point<D> p1, Point<D> p2)
            {
                return p1[0].CompareTo(p2[0]);
            }
        }

        /// <summary>
        /// A comparator for used in the indexor
        /// </summary>
        private static XValueOfPointComparator _xValueOfPointComparator = new XValueOfPointComparator();

        /// <summary>
        /// Perform a first degree interpolation to lookup the y coordinate given the x coordinate
        /// </summary>
        /// <param name="points">The collection of points</param>
        /// <param name="index">the x coordinate</param>
        /// <returns>the y coordinate as the result of the first degree interpolation</returns>
        public static D FirstDegreeInterpolate(Point2D<D>[] points, D index)
        {
            int idx = System.Array.BinarySearch<Point<D>>( (Point<D>[]) points, (Point<D>) new Point2D<D>(index, new D()), _xValueOfPointComparator);
            if (idx >= 0)
            {   // an exact index is matched
                return points[idx].Y;
            }
            else
            {   // the index fall into a range, in this case we do interpolation
                idx = -idx;
                if (idx == 1)
                {   // the specific index is smaller than all indexes
                    idx = 0;
                }
                else if (idx == points.Length + 1)
                {   // the specific index is larger than all indexes
                    idx = points.Length - 2;
                }
                else
                {
                    idx -= 2;
                }

                Line2D<D> line = new Line2D<D>(points[idx], points[idx + 1]);
                return line.YByX(index);
            }
        }

        /// <summary>
        /// Perform a first degree interpolation to lookup the y coordinates given the x coordinates
        /// </summary>
        /// <param name="points">The collection of points</param>
        /// <param name="indexes">the x coordinates</param>
        /// <returns>the y coordinates as the result of the first degree interpolation</returns>
        public static D[] FirstDegreeInterpolate(Point2D<D>[] points, D[] indexes)
        {
            return System.Array.ConvertAll<D, D>(
                indexes,
                delegate(D d) { return FirstDegreeInterpolate(points, d); });
        }

        /// <summary>
        /// Convert the points to a sequence of CvPoint2D32f
        /// </summary>
        /// <param name="stor">The sotrage</param>
        /// <param name="points">The points to be converted to sequence</param>
        /// <returns>A pointer to the sequence</returns>
        public static Seq<MCvPoint2D32f> To2D32fSequence(MemStorage stor, IEnumerable<Point<D>> points)
        {
            Seq<MCvPoint2D32f> seq = new Seq<MCvPoint2D32f>(
                CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2),
                stor);

            if (typeof(D) == typeof(float)) // if the depth of the point is float
            {
                foreach (Point<D> p in points)
                {
                    GCHandle handle = GCHandle.Alloc(p.Coordinate, GCHandleType.Pinned);
                    CvInvoke.cvSeqPush(seq.Ptr, handle.AddrOfPinnedObject());
                    handle.Free();
                }
            }
            else //if the depth of the point is not float
            {
                foreach (Point<D> p in points)
                {
                    Point<float> pf = p.Convert<float>(); //convert the point to float
                    GCHandle handle = GCHandle.Alloc(pf.Coordinate, GCHandleType.Pinned);
                    CvInvoke.cvSeqPush(seq.Ptr, handle.AddrOfPinnedObject());
                    handle.Free();
                }
            }
            return seq;
        }

        /// <summary>
        /// Convert the points to a sequence of CvPoint3D32f
        /// </summary>
        /// <param name="stor">The sotrage</param>
        /// <param name="points">The points to be converted to sequence</param>
        /// <returns>A pointer to the sequence</returns>
        public Seq<MCvPoint3D32f> To3D32Sequence(MemStorage stor, IEnumerable<Point<D>> points)
        {
            Seq<MCvPoint3D32f> seq = new Seq<MCvPoint3D32f>(
                CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 3),
                stor);

            if (typeof(D) == typeof(float)) // if the depth of the point is float
            {
                foreach (Point<D> p in points)
                {
                    GCHandle handle = GCHandle.Alloc(p.Coordinate, GCHandleType.Pinned);
                    CvInvoke.cvSeqPush(seq.Ptr, handle.AddrOfPinnedObject());
                    handle.Free();
                }
            }
            else //if the depth of the point is not float
            {
                foreach (Point<D> p in points)
                {
                    Point<float> pf = p.Convert<float>(); //convert the point to float
                    GCHandle handle = GCHandle.Alloc(pf.Coordinate, GCHandleType.Pinned);
                    CvInvoke.cvSeqPush(seq.Ptr, handle.AddrOfPinnedObject());
                    handle.Free();
                }
            }
            return seq;
        }

        /// <summary>
        /// Convert a collection of N Points to an (N x k) matrix, k is determined by the dimension of the first point
        /// </summary>
        /// <param name="points">The points which will be converted to matrix</param>
        /// <returns>the matrix representing the collection of points</returns>
        public static Matrix<D> ToMatrix(IEnumerable< Point<D> > points)
        {
            List<D[]> pts = new List<D[]>();
            foreach (Point<D> pt in points)
                pts.Add(pt.Coordinate);

            Debug.Assert(pts.Count > 0);
            int rows = pts.Count;
            int cols = pts[0].Length;

            D[,] array = new D[rows,cols];
            for (int i = 0; i <= rows; i++)
                for (int j = 0; j <= cols; j++)
                    array[i, j] = pts[i][j];

            return  new Matrix<D>(array);
        }

        /// <summary>
        /// Fit a line to the points collection
        /// </summary>
        /// <param name="points">The points to be fitted</param>
        /// <param name="type">The type of the fitting</param>
        /// <returns>A 2D line</returns>
        public static Line2D<float> Line2DFitting(IEnumerable<Point<D>> points, CvEnum.DIST_TYPE type)
        {
            float[] data = new float[6];
            GCHandle handle2 = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (MemStorage stor = new MemStorage())
            {
                Seq<MCvPoint2D32f> seq = To2D32fSequence(stor, points);
                CvInvoke.cvFitLine(seq.Ptr, type, 0.0, 0.01, 0.01, handle2.AddrOfPinnedObject());
            }
            handle2.Free();
            Line2D<float> res = new Line2D<float>(new Point2D<float>(data[2], data[3]), new Point2D<float>(data[2] + data[0], data[3] + data[1]));
            return res;
        }

        /// <summary>
        /// Fit an ellipse to the points collection
        /// </summary>
        /// <param name="points">The points to be fitted</param>
        /// <returns>An ellipse</returns>
        public static Ellipse<float> LeastSquareEllipseFitting(IEnumerable<Point<D>> points)
        {
            Ellipse<float> res = new Ellipse<float>();
            using (MemStorage stor = new MemStorage())
            {
                Seq<MCvPoint2D32f> seq = To2D32fSequence(stor, points);
                res.MCvBox2D = CvInvoke.cvFitEllipse2(seq.Ptr);
            }
            return res;
        }
    };
}
