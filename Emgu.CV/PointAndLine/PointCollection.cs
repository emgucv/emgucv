//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A collection of points
   /// </summary>
   public static class PointCollection
   {
      /*
      /// <summary>
      /// A comparator which compares only the X value of the point
      /// </summary>
      private class XValueOfPointComparator : IComparer<PointF>
      {
         public int Compare(PointF p1, PointF p2)
         {
            return p1.X.CompareTo(p2.X);
         }
      }

      /// <summary>
      /// Perform a first degree interpolation to lookup the y coordinate given the x coordinate
      /// </summary>
      /// <param name="points">The collection of points. Must be sorted by the x value.</param>
      /// <param name="index">the x coordinate</param>
      /// <returns>the y coordinate as the result of the first degree interpolation</returns>
      public static float FirstDegreeInterpolate(PointF[] points, float index)
      {
         XValueOfPointComparator comparator = new XValueOfPointComparator();
         int idx = Array.BinarySearch<PointF>(points, new PointF(index, 0.0f), comparator);
         
         if (idx >= 0) // an exact index is matched
            return points[idx].Y;

         // the index fall into a range, in this case we do interpolation
         idx = -idx;

         if (idx == 1)
            // the specific index is smaller than all indexes
            idx = 0;
         else if (idx == points.Length + 1)
            // the specific index is larger than all indexes
            idx = points.Length - 2;
         else
            idx -= 2;

         LineSegment2DF line = new LineSegment2DF(points[idx], points[idx + 1]);
         return line.YByX(index);         
      }

      /// <summary>
      /// Perform a first degree interpolation to lookup the y coordinates given the x coordinates
      /// </summary>
      /// <param name="points">The collection of points, Must be sorted by x value</param>
      /// <param name="indexes">the x coordinates</param>
      /// <returns>The y coordinates as the result of the first degree interpolation</returns>
      public static float[] FirstDegreeInterpolate(PointF[] points, float[] indexes)
      {
         return Array.ConvertAll<float, float>(
             indexes,
             delegate(float d) { return FirstDegreeInterpolate(points, d); });
      }*/

      /// <summary>
      /// Fit a line to the points collection
      /// </summary>
      /// <param name="points">The points to be fitted</param>
      /// <param name="type">The type of the fitting</param>
      /// <param name="normalizedDirection">The normalized direction of the fitted line</param>
      /// <param name="aPointOnLine">A point on the fitted line</param>
      public static void Line2DFitting(PointF[] points, CvEnum.DIST_TYPE type, out PointF normalizedDirection, out PointF aPointOnLine)
      {
         float[] data = new float[6];
         IntPtr seq = Marshal.AllocHGlobal(StructSize.MCvSeq);
         IntPtr block = Marshal.AllocHGlobal(StructSize.MCvSeqBlock);
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);

         CvInvoke.cvMakeSeqHeaderForArray(
            CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2),
            StructSize.MCvSeq,
            StructSize.PointF,
            handle.AddrOfPinnedObject(),
            points.Length,
            seq,
            block); 

         CvInvoke.cvFitLine(seq, type, 0.0, 0.01, 0.01, data);

         handle.Free();
         Marshal.FreeHGlobal(seq);
         Marshal.FreeHGlobal(block);
         normalizedDirection = new PointF(data[0], data[1]);
         aPointOnLine = new PointF(data[2], data[3]);
      }

      /// <summary>
      /// Fit an ellipse to the points collection
      /// </summary>
      /// <param name="points">The points to be fitted</param>
      /// <returns>An ellipse</returns>
      public static Ellipse EllipseLeastSquareFitting(PointF[] points)
      {
         IntPtr seq = Marshal.AllocHGlobal(StructSize.MCvSeq);
         IntPtr block = Marshal.AllocHGlobal(StructSize.MCvSeqBlock);
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         CvInvoke.cvMakeSeqHeaderForArray(
            CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2),
            StructSize.MCvSeq,
            StructSize.PointF,
            handle.AddrOfPinnedObject(),
            points.Length,
            seq,
            block);
         Ellipse e = new Ellipse(CvInvoke.cvFitEllipse2(seq));

         //The angle returned by cvFitEllipse2 has the wrong sign.
         //Returned angle is clock wise rotation, what we need for the definition of MCvBox is the counter clockwise rotation.
         //For this, we needs to change the sign of the angle
         MCvBox2D b = e.MCvBox2D;
         b.angle = -b.angle;
         if (b.angle < 0) b.angle += 360;
         e.MCvBox2D = b;
         
         handle.Free();
         Marshal.FreeHGlobal(seq);
         Marshal.FreeHGlobal(block);
         return e;
      }

      /// <summary>
      /// convert a series of points to LineSegment2D
      /// </summary>
      /// <param name="points">the array of points</param>
      /// <param name="closed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
      /// <returns>array of LineSegment2D</returns>
      public static LineSegment2DF[] PolyLine(PointF[] points, bool closed)
      {
         LineSegment2DF[] res;
         int length = points.Length;
         if (closed)
         {
            res = new LineSegment2DF[length];
            PointF lastPoint = points[length - 1];
            for (int i = 0; i < res.Length; i++)
            {
               res[i] = new LineSegment2DF(lastPoint, points[i]);
               lastPoint = points[i];
            }
         }
         else
         {
            res = new LineSegment2DF[length - 1];
            PointF lastPoint = points[0];
            for (int i = 1; i < res.Length; i++)
            {
               res[i] = new LineSegment2DF(lastPoint, points[i]);
               lastPoint = points[i];
            }
         }

         return res;
      }


      /// <summary>
      /// convert a series of System.Drawing.Point to LineSegment2D
      /// </summary>
      /// <param name="points">the array of points</param>
      /// <param name="closed">if true, the last line segment is defined by the last point of the array and the first point of the array</param>
      /// <returns>array of LineSegment2D</returns>
      public static LineSegment2D[] PolyLine(Point[] points, bool closed)
      {
         LineSegment2D[] res;
         int length = points.Length;
         if (closed)
         {
            res = new LineSegment2D[length];
            for (int i = 0; i < res.Length; i++)
               res[i] = new LineSegment2D(points[i], points[(i + 1) % length]);
         }
         else
         {
            res = new LineSegment2D[length - 1];
            for (int i = 0; i < res.Length; i++)
               res[i] = new LineSegment2D(points[i], points[(i + 1)]);
         }
         return res;
      }

      /// <summary>
      /// Finds convex hull of 2D point set using Sklansky's algorithm
      /// </summary>
      /// <param name="points">The points to find convex hull from</param>
      /// <param name="storage">the storage used by the resulting sequence</param>
      /// <param name="orientation">The orientation of the convex hull</param>
      /// <returns>The convex hull of the points</returns>
      public static Seq<PointF> ConvexHull(PointF[] points, MemStorage storage, CvEnum.ORIENTATION orientation)
      {
         IntPtr seq = Marshal.AllocHGlobal(StructSize.MCvSeq);
         IntPtr block = Marshal.AllocHGlobal(StructSize.MCvSeqBlock);
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         CvInvoke.cvMakeSeqHeaderForArray(
            CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2),
            StructSize.MCvSeq,
            StructSize.PointF,
            handle.AddrOfPinnedObject(),
            points.Length,
            seq,
            block);

         Seq<PointF> convexHull = new Seq<PointF>(CvInvoke.cvConvexHull2(seq, storage.Ptr, orientation, 1), storage);
         handle.Free();
         Marshal.FreeHGlobal(seq);
         Marshal.FreeHGlobal(block);
         return convexHull;
      }

      /// <summary>
      /// Find the bounding rectangle for the specific array of points
      /// </summary>
      /// <param name="points">The collection of points</param>
      /// <returns>The bounding rectangle for the array of points</returns>
      public static Rectangle BoundingRectangle(PointF[] points)
      {
         IntPtr seq = Marshal.AllocHGlobal(StructSize.MCvContour);
         IntPtr block = Marshal.AllocHGlobal(StructSize.MCvSeqBlock);
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         CvInvoke.cvMakeSeqHeaderForArray(
            CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2),
            StructSize.MCvSeq,
            StructSize.PointF,
            handle.AddrOfPinnedObject(),
            points.Length,
            seq,
            block);
         Rectangle rect = CvInvoke.cvBoundingRect(seq, true);
         handle.Free();
         Marshal.FreeHGlobal(seq);
         Marshal.FreeHGlobal(block);
         return rect;
      }

      /// <summary>
      /// Find the bounding rectangle for the specific array of points
      /// </summary>
      /// <param name="points">The collection of points</param>
      /// <returns>The bounding rectangle for the array of points</returns>
      public static MCvBox2D MinAreaRect(PointF[] points)
      {
         IntPtr seq = Marshal.AllocHGlobal(StructSize.MCvContour);
         IntPtr block = Marshal.AllocHGlobal(StructSize.MCvSeqBlock);
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         CvInvoke.cvMakeSeqHeaderForArray(
            CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2),
            StructSize.MCvSeq,
            StructSize.PointF,
            handle.AddrOfPinnedObject(),
            points.Length,
            seq,
            block);
         MCvBox2D rect = CvInvoke.cvMinAreaRect2(seq, IntPtr.Zero);
         handle.Free();
         Marshal.FreeHGlobal(seq);
         Marshal.FreeHGlobal(block);
         return rect;
      }

      /// <summary>
      /// Find the minimum enclosing circle for the specific array of points
      /// </summary>
      /// <param name="points">The collection of points</param>
      /// <returns>The minimum enclosing circle for the array of points</returns>
      public static CircleF MinEnclosingCircle(PointF[] points)
      {
         IntPtr seq = Marshal.AllocHGlobal(StructSize.MCvContour);
         IntPtr block = Marshal.AllocHGlobal(StructSize.MCvSeqBlock);
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         CvInvoke.cvMakeSeqHeaderForArray(
            CvInvoke.CV_MAKETYPE((int)CvEnum.MAT_DEPTH.CV_32F, 2),
            StructSize.MCvSeq,
            StructSize.PointF,
            handle.AddrOfPinnedObject(),
            points.Length,
            seq,
            block);
         PointF center;
         float radius;
         CvInvoke.cvMinEnclosingCircle(seq, out center, out radius);
         handle.Free();
         Marshal.FreeHGlobal(seq);
         Marshal.FreeHGlobal(block);
         return new CircleF(center, radius);
      }

      /// <summary>
      /// Reproject pixels on a 1-channel disparity map to array of 3D points.
      /// </summary>
      /// <param name="disparity">Disparity map</param>
      /// <param name="Q">The reprojection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
      /// <returns>The reprojected 3D points</returns>
      public static MCvPoint3D32f[] ReprojectImageTo3D(Image<Gray, Int16> disparity, Matrix<double> Q)
      {
         Size size = disparity.Size;
         MCvPoint3D32f[] points3D = new MCvPoint3D32f[size.Width * size.Height];
         GCHandle handle = GCHandle.Alloc(points3D, GCHandleType.Pinned);

         using (Matrix<float> pts = new Matrix<float>(size.Height, size.Width, 3, handle.AddrOfPinnedObject(), 0))
            CvInvoke.cvReprojectImageTo3D(disparity.Ptr, pts.Ptr, Q);

         handle.Free();
         return points3D;
      }

      /// <summary>
      /// Reproject pixels on a 1-channel disparity map to array of 3D points.
      /// </summary>
      /// <param name="disparity">Disparity map</param>
      /// <param name="Q">The reprojection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
      /// <returns>The reprojected 3D points</returns>
      public static MCvPoint3D32f[] ReprojectImageTo3D(Image<Gray, Byte> disparity, Matrix<double> Q)
      {
         Size size = disparity.Size;
         MCvPoint3D32f[] points3D = new MCvPoint3D32f[size.Width * size.Height];
         GCHandle handle = GCHandle.Alloc(points3D, GCHandleType.Pinned);

         using (Matrix<float> pts = new Matrix<float>(size.Height, size.Width, 3, handle.AddrOfPinnedObject(), 0))
            CvInvoke.cvReprojectImageTo3D(disparity.Ptr, pts.Ptr, Q);

         handle.Free();
         return points3D;
      }

      /// <summary>
      /// Generate a random point cloud around the ellipse. 
      /// </summary>
      /// <param name="e">The region where the point cloud will be generated. The axes of e corresponds to std of the random point cloud.</param>
      /// <param name="numberOfPoints">The number of points to be generated</param>
      /// <returns>A random point cloud around the ellipse</returns>
      public static PointF[] GeneratePointCloud(Ellipse e, int numberOfPoints)
      {
         PointF[] cloud = new PointF[numberOfPoints];
         GCHandle handle = GCHandle.Alloc(cloud, GCHandleType.Pinned);
         using (Matrix<float> points = new Matrix<float>(numberOfPoints, 2, handle.AddrOfPinnedObject()))
         using (Matrix<float> xValues = points.GetCol(0))
         using (Matrix<float> yValues = points.GetCol(1))
         using (RotationMatrix2D<float> rotation = new RotationMatrix2D<float>(e.MCvBox2D.center, e.MCvBox2D.angle, 1.0))
         {
            xValues.SetRandNormal(new MCvScalar(e.MCvBox2D.center.X), new MCvScalar(e.MCvBox2D.size.Width / 2.0f));
            yValues.SetRandNormal(new MCvScalar(e.MCvBox2D.center.Y), new MCvScalar(e.MCvBox2D.size.Height / 2.0f));
            rotation.RotatePoints(points);
         }
         handle.Free();
         return cloud;
      }
   }
}
