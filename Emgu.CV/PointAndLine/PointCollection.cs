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
    public static class PointCollection
    {
        /// <summary>
        /// A comparator which compares only the X value of the point
        /// </summary>
        private class XValueOfPointComparator<T> : IComparer<Point<T>> where T : IComparable, new()
        {
            public int Compare(Point<T> p1, Point<T> p2)
            {
                return p1[0].CompareTo(p2[0]);
            }
        }

        /// <summary>
        /// Perform a first degree interpolation to lookup the y coordinate given the x coordinate
        /// </summary>
        /// <param name="points">The collection of points</param>
        /// <param name="index">the x coordinate</param>
        /// <returns>the y coordinate as the result of the first degree interpolation</returns>
        public static T FirstDegreeInterpolate<T>(Point2D<T>[] points, T index) where T : IComparable, new()
        {
            XValueOfPointComparator<T> comparator = new XValueOfPointComparator<T>();
            int idx = System.Array.BinarySearch<Point<T>>((Point<T>[])points, (Point<T>)new Point2D<T>(index, new T()), comparator);
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

                Line2D<T> line = new Line2D<T>(points[idx], points[idx + 1]);
                return line.YByX(index);
            }
        }

        /// <summary>
        /// Perform a first degree interpolation to lookup the y coordinates given the x coordinates
        /// </summary>
        /// <param name="points">The collection of points</param>
        /// <param name="indexes">the x coordinates</param>
        /// <returns>the y coordinates as the result of the first degree interpolation</returns>
        public static T[] FirstDegreeInterpolate<T>(Point2D<T>[] points, T[] indexes) where T : IComparable, new()
        {
            return System.Array.ConvertAll<T, T>(
                indexes,
                delegate(T d) { return FirstDegreeInterpolate<T>(points, d); });
        }

        /// <summary>
        /// Convert the points to a sequence of CvPoint2D32f
        /// </summary>
        /// <param name="stor">The sotrage</param>
        /// <param name="points">The points to be converted to sequence</param>
        /// <returns>A pointer to the sequence</returns>
        public static Seq<MCvPoint2D32f> To2D32fSequence<D>(MemStorage stor, IEnumerable<Point<D>> points) where D : IComparable, new()
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
        public static Seq<MCvPoint3D32f> To3D32Sequence<D>(MemStorage stor, IEnumerable<Point<D>> points) where D : IComparable, new()
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
        public static Matrix<D> ToMatrix<D>(IEnumerable<Point<D>> points) where D : IComparable, new()
        {
            List<D[]> pts = new List<D[]>();
            foreach (Point<D> pt in points)
                pts.Add(pt.Coordinate);

            Debug.Assert(pts.Count > 0);
            int rows = pts.Count;
            int cols = pts[0].Length;

            D[,] array = new D[rows, cols];
            for (int i = 0; i <= rows; i++)
                for (int j = 0; j <= cols; j++)
                    array[i, j] = pts[i][j];

            return new Matrix<D>(array);
        }

        /// <summary>
        /// Fit a line to the points collection
        /// </summary>
        /// <param name="points">The points to be fitted</param>
        /// <param name="type">The type of the fitting</param>
        /// <returns>A 2D line</returns>
        public static Line2D<float> Line2DFitting<D>(IEnumerable<Point<D>> points, CvEnum.DIST_TYPE type) where D : IComparable, new()
        {
            float[] data = new float[6];
            using (MemStorage stor = new MemStorage())
            {
                Seq<MCvPoint2D32f> seq = To2D32fSequence(stor, points);
                CvInvoke.cvFitLine(seq.Ptr, type, 0.0, 0.01, 0.01, data);
            }
            Line2D<float> res = new Line2D<float>(new Point2D<float>(data[2], data[3]), new Point2D<float>(data[2] + data[0], data[3] + data[1]));
            return res;
        }

        /// <summary>
        /// Fit an ellipse to the points collection
        /// </summary>
        /// <param name="points">The points to be fitted</param>
        /// <returns>An ellipse</returns>
        public static Ellipse<float> LeastSquareEllipseFitting<D>(IEnumerable<Point<D>> points) where D : IComparable, new()
        {
            Ellipse<float> res = new Ellipse<float>();
            using (MemStorage stor = new MemStorage())
            {
                Seq<MCvPoint2D32f> seq = To2D32fSequence(stor, points);
                res.MCvBox2D = CvInvoke.cvFitEllipse2(seq.Ptr);
            }
            return res;
        }

        /// <summary>
        /// convert a series of points to LineSegment2D
        /// </summary>
        /// <typeparam name="D">the depth of the point</typeparam>
        /// <param name="points">the array of points</param>
        /// <param name="closed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
        /// <returns>array of LineSegment2D</returns>
        public static LineSegment2D<D>[] PolyLine<D>(Point2D<D>[] points, bool closed) where D : IComparable, new()
        {
            LineSegment2D<D>[] res;
            if (closed)
            {
                int length = points.Length;
                res = new LineSegment2D<D>[length];
                for (int i = 0; i < length; i++)
                    res[i] = new LineSegment2D<D>(points[i], points[(i + 1) % length]);
            }
            else
            {
                res = new LineSegment2D<D>[points.Length - 1];
                for (int i = 0; i < points.Length - 1; i++)
                    res[i] = new LineSegment2D<D>(points[i], points[(i + 1)]);
            }
            return res;
        }

        /// <summary>
        /// convert a series of MCvPoint to LineSegment2D
        /// </summary>
        /// <typeparam name="D">the depth of the point</typeparam>
        /// <param name="points">the array of points</param>
        /// <param name="closed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
        /// <returns>array of LineSegment2D</returns>
        public static LineSegment2D<D>[] PolyLine<D>(MCvPoint[] points, bool closed) where D : IComparable, new()
        {
            return PolyLine(
                Array.ConvertAll<MCvPoint, Point2D<D>>(points, delegate(MCvPoint p) { Point2D<D> p2d = new Point2D<D>(); p2d.MCvPoint = p; return p2d; }),
                closed);
        }

        public static MCvPoint2D32f[] ConvexHull<D>(IEnumerable< Point<D>> points) where D: IComparable, new()
        {
            using (MemStorage stor = new MemStorage())
            using (Seq<MCvPoint2D32f> sequence = To2D32fSequence<D>(stor, points))
            {
                IntPtr hull = CvInvoke.cvConvexHull2(sequence.Ptr, stor.Ptr, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE, 1);
                using (Seq<MCvPoint2D32f> hullSequence = new Seq<MCvPoint2D32f>(hull, stor))
                {
                    MCvPoint2D32f[] result = new MCvPoint2D32f[hullSequence.Total];
                    int counter = 0;
                    foreach (MCvPoint2D32f p in hullSequence)
                    {
                        result[counter++] = p;
                    }
                    return result;
                }
            }

        }
    };
}
