//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
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

      /*
      /// <summary>
      /// Fit a line to the points collection
      /// </summary>
      /// <param name="points">The points to be fitted</param>
      /// <param name="type">The type of the fitting</param>
      /// <param name="normalizedDirection">The normalized direction of the fitted line</param>
      /// <param name="aPointOnLine">A point on the fitted line</param>
      public static void Line2DFitting(PointF[] points, CvEnum.DistType type, out PointF normalizedDirection, out PointF aPointOnLine)
      {
         float[] data = new float[6];
         IntPtr seq = Marshal.AllocHGlobal(StructSize.MCvSeq);
         IntPtr block = Marshal.AllocHGlobal(StructSize.MCvSeqBlock);
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);

         CvInvoke.cvMakeSeqHeaderForArray(
            CvInvoke.MakeType(CvEnum.DepthType.Cv32F, 2),
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
      }*/

      /// <summary>
      /// Fit an ellipse to the points collection
      /// </summary>
      /// <param name="points">The points to be fitted</param>
      /// <returns>An ellipse</returns>
      public static Ellipse EllipseLeastSquareFitting(PointF[] points)
      {
         using (VectorOfPointF vp = new VectorOfPointF(points))
         {
            Ellipse e = new Ellipse(CvInvoke.FitEllipse(vp));

            //The angle returned by cvFitEllipse2 has the wrong sign.
            //Returned angle is clock wise rotation, what we need for the definition of MCvBox is the counter clockwise rotation.
            //For this, we needs to change the sign of the angle
            RotatedRect b = e.RotatedRect;
            b.Angle = -b.Angle;
            if (b.Angle < 0) b.Angle += 360;
            e.RotatedRect = b;

            return e;
         }
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
      /// Find the bounding rectangle for the specific array of points
      /// </summary>
      /// <param name="points">The collection of points</param>
      /// <returns>The bounding rectangle for the array of points</returns>
      public static Rectangle BoundingRectangle(PointF[] points)
      {
         using (VectorOfPointF ptVec = new VectorOfPointF(points))
         return CvInvoke.BoundingRectangle(ptVec);
      }

      /// <summary>
      /// Re-project pixels on a 1-channel disparity map to array of 3D points.
      /// </summary>
      /// <param name="disparity">Disparity map</param>
      /// <param name="Q">The re-projection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
      /// <returns>The reprojected 3D points</returns>
      public static MCvPoint3D32f[] ReprojectImageTo3D(IInputArray disparity, IInputArray Q)
      {
         Size size;
         using (InputArray ia = disparity.GetInputArray())
            size = ia.GetSize();

         MCvPoint3D32f[] points3D = new MCvPoint3D32f[size.Width * size.Height];
         GCHandle handle = GCHandle.Alloc(points3D, GCHandleType.Pinned);

         using (Matrix<float> pts = new Matrix<float>(size.Height, size.Width, 3, handle.AddrOfPinnedObject(), 0))
            CvInvoke.ReprojectImageTo3D(disparity, pts, Q, false, CvEnum.DepthType.Cv32F);

         handle.Free();
         return points3D;
      }

      /*
      /// <summary>
      /// Re-project pixels on a 1-channel disparity map to array of 3D points.
      /// </summary>
      /// <param name="disparity">Disparity map</param>
      /// <param name="Q">The re-projection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
      /// <returns>The reprojected 3D points</returns>
      public static MCvPoint3D32f[] ReprojectImageTo3D(Image<Gray, Byte> disparity, Matrix<double> Q)
      {
         Size size = disparity.Size;
         MCvPoint3D32f[] points3D = new MCvPoint3D32f[size.Width * size.Height];
         GCHandle handle = GCHandle.Alloc(points3D, GCHandleType.Pinned);

         using (Matrix<float> pts = new Matrix<float>(size.Height, size.Width, 3, handle.AddrOfPinnedObject(), 0))
            CvInvoke.ReprojectImageTo3D(disparity, pts, Q, false, CvEnum.DepthType.Cv32F);

         handle.Free();
         return points3D;
      }*/

      
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
         using (RotationMatrix2D rotation = new RotationMatrix2D(e.RotatedRect.Center, e.RotatedRect.Angle, 1.0))
         using (Mat tmp = new Mat())
         {
            rotation.ConvertTo(tmp, DepthType.Cv32F);
            xValues.SetRandNormal(new MCvScalar(e.RotatedRect.Center.X), new MCvScalar(e.RotatedRect.Size.Width / 2.0f));
            yValues.SetRandNormal(new MCvScalar(e.RotatedRect.Center.Y), new MCvScalar(e.RotatedRect.Size.Height / 2.0f));
            rotation.RotatePoints(points);
         }
         handle.Free();
         return cloud;
      }
   }
}
