//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        /// <summary>
        /// Calculates the matrix of an affine transform such that:
        /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
        /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
        /// </summary>
        /// <param name="src">Coordinates of 3 triangle vertices in the source image. If the array contains more than 3 points, only the first 3 will be used</param>
        /// <param name="dest">Coordinates of the 3 corresponding triangle vertices in the destination image. If the array contains more than 3 points, only the first 3 will be used</param>
        /// <returns>The 2x3 rotation matrix that defines the Affine transform</returns>
        public static Mat GetAffineTransform(PointF[] src, PointF[] dest)
        {
            Debug.Assert(src.Length >= 3, "The source should contain at least 3 points");
            Debug.Assert(dest.Length >= 3, "The destination should contain at least 3 points");

            using (VectorOfPointF ptSrc = src.Length == 3
                       ? new VectorOfPointF(src)
                       : new VectorOfPointF(new PointF[] {src[0], src[1], src[2]}))
            using (VectorOfPointF ptDest = dest.Length == 3
                       ? new VectorOfPointF(dest)
                       : new VectorOfPointF(new PointF[] {dest[0], dest[1], dest[2]}))
                return CvInvoke.GetAffineTransform(ptSrc, ptDest);
        }

        /// <summary>
        /// Calculates the matrix of an affine transform such that:
        /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
        /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
        /// </summary>
        /// <param name="src">Pointer to an array of PointF, Coordinates of 3 triangle vertices in the source image.</param>
        /// <param name="dst">Pointer to an array of PointF, Coordinates of the 3 corresponding triangle vertices in the destination image</param>
        /// <returns>The destination 2x3 matrix</returns>
        public static Mat GetAffineTransform(
            IInputArray src,
            IOutputArray dst)
        {
            Mat affine = new Mat();
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveGetAffineTransform(iaSrc, oaDst, affine);
            return affine;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetAffineTransform(IntPtr src, IntPtr dst, IntPtr result);

        /// <summary>
        /// Calculates rotation matrix
        /// </summary>
        /// <param name="center">Center of the rotation in the source image. </param>
        /// <param name="angle">The rotation angle in degrees. Positive values mean couter-clockwise rotation (the coordiate origin is assumed at top-left corner).</param>
        /// <param name="scale">Isotropic scale factor</param>
        /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
        public static void GetRotationMatrix2D(PointF center, double angle, double scale, IOutputArray mapMatrix)
        {
            using (OutputArray oaMapMatrix = mapMatrix.GetOutputArray())
                cveGetRotationMatrix2D(ref center, angle, scale, oaMapMatrix);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetRotationMatrix2D(ref PointF center, double angle, double scale, IntPtr mapMatrix);

        /// <summary>
        /// calculates matrix of perspective transform such that:
        /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)T
        /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
        /// </summary>
        /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
        /// <param name="dst">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
        /// <returns>The perspective transform matrix</returns>
        public static Mat GetPerspectiveTransform(IInputArray src, IInputArray dst)
        {
            Mat m = new Mat();
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaDst = dst.GetInputArray())
                cveGetPerspectiveTransform(iaSrc, iaDst, m);
            return m;
        }

        /// <summary>
        /// calculates matrix of perspective transform such that:
        /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)^T
        /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
        /// </summary>
        /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
        /// <param name="dest">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
        /// <returns>The 3x3 Homography matrix</returns>
        public static Mat GetPerspectiveTransform(PointF[] src, PointF[] dest)
        {
            Debug.Assert(src.Length >= 4, "The source should contain at least 4 points");
            Debug.Assert(dest.Length >= 4, "The destination should contain at least 4 points");

            Mat rot;
            GCHandle handleSrc = GCHandle.Alloc(src, GCHandleType.Pinned);
            GCHandle handleDest = GCHandle.Alloc(dest, GCHandleType.Pinned);
            using (Mat mSrc = new Mat(src.Length, 2, DepthType.Cv32F, 1, handleSrc.AddrOfPinnedObject(), 8))
            using (Mat mDst = new Mat(dest.Length, 2, DepthType.Cv32F, 1, handleDest.AddrOfPinnedObject(), 8))
                rot = CvInvoke.GetPerspectiveTransform(mSrc, mDst);
            handleSrc.Free();
            handleDest.Free();
            return rot;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetPerspectiveTransform(IntPtr src, IntPtr dst, IntPtr mapMatrix);

        /// <summary>
        /// Inverts an affine transformation
        /// </summary>
        /// <param name="m">Original affine transformation</param>
        /// <param name="im">Output reverse affine transformation.</param>
        public static void InvertAffineTransform(IInputArray m, IOutputArray im)
        {
            using (InputArray iaM = m.GetInputArray())
            using (OutputArray oaIm = im.GetOutputArray())
                cveInvertAffineTransform(iaM, oaIm);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveInvertAffineTransform(IntPtr m, IntPtr im);

        /// <summary>
        /// Fits line to 2D or 3D point set
        /// </summary>
        /// <param name="points">Input vector of 2D points.</param>
        /// <param name="distType">The distance used for fitting </param>
        /// <param name="param">Numerical parameter (C) for some types of distances, if 0 then some optimal value is chosen</param>
        /// <param name="reps">Sufficient accuracy for radius (distance between the coordinate origin and the line), 0.01 would be a good default</param>
        /// <param name="aeps">Sufficient accuracy for angle, 0.01 would be a good default</param>
        /// <param name="line">Output line parameters.</param>
        public static void FitLine(
            IInputArray points,
            IOutputArray line,
            CvEnum.DistType distType,
            double param,
            double reps,
            double aeps)
        {
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaLine = line.GetOutputArray())
                cveFitLine(iaPoints, oaLine, distType, param, reps, aeps);
        }

        /// <summary>
        /// Fits line to 2D or 3D point set
        /// </summary>
        /// <param name="points">Input vector of 2D points.</param>
        /// <param name="distType">The distance used for fitting </param>
        /// <param name="param">Numerical parameter (C) for some types of distances, if 0 then some optimal value is chosen</param>
        /// <param name="reps">Sufficient accuracy for radius (distance between the coordinate origin and the line), 0.01 would be a good default</param>
        /// <param name="aeps">Sufficient accuracy for angle, 0.01 would be a good default</param>
        /// <param name="direction">A normalized vector collinear to the line </param>
        /// <param name="pointOnLine">A point on the line.</param>
        public static void FitLine(
            PointF[] points,
            out PointF direction,
            out PointF pointOnLine,
            CvEnum.DistType distType,
            double param,
            double reps,
            double aeps)
        {
            using (VectorOfPointF pv = new VectorOfPointF(points))
            using (VectorOfFloat line = new VectorOfFloat())
            using (InputArray iaPv = pv.GetInputArray())
            using (OutputArray oaLine = line.GetOutputArray())
            {
                cveFitLine(iaPv, oaLine, distType, param, reps, aeps);
                float[] values = line.ToArray();
                direction = new PointF(values[0], values[1]);
                pointOnLine = new PointF(values[2], values[3]);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFitLine(IntPtr points, IntPtr line, CvEnum.DistType distType, double param, double reps, double aeps);

        /// <summary>
        /// Finds out if there is any intersection between two rotated rectangles.
        /// </summary>
        /// <param name="rect1">First rectangle</param>
        /// <param name="rect2">Second rectangle</param>
        /// <param name="intersectingRegion">The output array of the vertices of the intersecting region.</param>
        /// <returns>The intersect type</returns>
        public static CvEnum.RectIntersectType RotatedRectangleIntersection(RotatedRect rect1, RotatedRect rect2, IOutputArray intersectingRegion)
        {
            using (OutputArray oaIntersectingRegion = intersectingRegion.GetOutputArray())
                return cveRotatedRectangleIntersection(ref rect1, ref rect2, oaIntersectingRegion);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern CvEnum.RectIntersectType cveRotatedRectangleIntersection(ref RotatedRect rect1, ref RotatedRect rect2, IntPtr intersectingRegion);

        /// <summary>
        /// Calculates vertices of the input 2d box.
        /// </summary>
        /// <param name="box">The box</param>
        /// <returns>The four vertices of rectangles.</returns>
        public static PointF[] BoxPoints(RotatedRect box)
        {
            PointF[] pts = new PointF[4];
            GCHandle handle = GCHandle.Alloc(pts, GCHandleType.Pinned);
            using (Mat vp = new Mat(4, 2, DepthType.Cv32F, 1, handle.AddrOfPinnedObject(), 8))
            using (OutputArray oaVp = vp.GetOutputArray())
            {
                cveBoxPoints(ref box, oaVp);
            }
            handle.Free();
            return pts;
        }

        /// <summary>
        /// Calculates vertices of the input 2d box.
        /// </summary>
        /// <param name="box">The box</param>
        /// <param name="points">The output array of four vertices of rectangles.</param>
        public static void BoxPoints(RotatedRect box, IOutputArray points)
        {
            using (OutputArray oaPoints = points.GetOutputArray())
                cveBoxPoints(ref box, oaPoints);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBoxPoints(ref RotatedRect box, IntPtr pt);

        /// <summary>
        /// Fits an ellipse around a set of 2D points.
        /// </summary>
        /// <param name="points">Input 2D point set</param>
        /// <returns>The ellipse that fits best (in least-squares sense) to a set of 2D points</returns>
        public static RotatedRect FitEllipse(IInputArray points)
        {
            RotatedRect ellipse = new RotatedRect();
            using (InputArray iaPoints = points.GetInputArray())
                cveFitEllipse(iaPoints, ref ellipse);
            return ellipse;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFitEllipse(IntPtr points, ref RotatedRect ellipse);

        /// <summary>
        /// The function calculates the ellipse that fits a set of 2D points. The Approximate Mean Square (AMS) is used.
        /// </summary>
        /// <param name="points">Input 2D point set</param>
        /// <returns>The rotated rectangle in which the ellipse is inscribed</returns>
        public static RotatedRect FitEllipseAMS(IInputArray points)
        {
            RotatedRect ellipse = new RotatedRect();
            using (InputArray iaPoints = points.GetInputArray())
                cveFitEllipseAMS(iaPoints, ref ellipse);
            return ellipse;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFitEllipseAMS(IntPtr points, ref RotatedRect ellipse);

        /// <summary>
        /// The function calculates the ellipse that fits a set of 2D points. The Direct least square (Direct) method is used.
        /// </summary>
        /// <param name="points">Input 2D point set</param>
        /// <returns>The rotated rectangle in which the ellipse is inscribed</returns>
        public static RotatedRect FitEllipseDirect(IInputArray points)
        {
            RotatedRect ellipse = new RotatedRect();
            using (InputArray iaPoints = points.GetInputArray())
                cveFitEllipseDirect(iaPoints, ref ellipse);
            return ellipse;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFitEllipseDirect(IntPtr points, ref RotatedRect ellipse);

        /// <summary>
        /// Compute for each 2d point the nearest 2d point located on a given ellipse.
        /// </summary>
        /// <param name="ellipseParams">Ellipse parameters</param>
        /// <param name="points">Input 2d points</param>
        /// <param name="closestPts">For each 2d point, their corresponding closest 2d point located on a given ellipse</param>
        public static void GetClosestEllipsePoints(RotatedRect ellipseParams, IInputArray points, IOutputArray closestPts)
        {
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaClosestPts = closestPts.GetOutputArray())
                cveGetClosestEllipsePoints(ref ellipseParams, iaPoints, oaClosestPts);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetClosestEllipsePoints(ref RotatedRect ellipseParams, IntPtr points, IntPtr closestPts);

        /// <summary>
        /// Finds convex hull of 2D point set using Sklansky's algorithm
        /// </summary>
        /// <param name="points">The points to find convex hull from</param>
        /// <param name="clockwise">Orientation flag.</param>
        /// <returns>The convex hull of the points</returns>
        public static PointF[] ConvexHull(PointF[] points, bool clockwise = false)
        {
            using (VectorOfPointF vpf = new VectorOfPointF(points))
            using (VectorOfPointF hull = new VectorOfPointF())
            {
                CvInvoke.ConvexHull(vpf, hull, clockwise);
                return hull.ToArray();
            }
        }

        /// <summary>
        /// The function finds convex hull of 2D point set using Sklansky's algorithm.
        /// </summary>
        /// <param name="points">Input 2D point set</param>
        /// <param name="hull">Output convex hull.</param>
        /// <param name="clockwise">Orientation flag.</param>
        /// <param name="returnPoints">Operation flag.</param>
        public static void ConvexHull(IInputArray points, IOutputArray hull, bool clockwise = false, bool returnPoints = true)
        {
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaHull = hull.GetOutputArray())
                cveConvexHull(iaPoints, oaHull, clockwise, returnPoints);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveConvexHull(
            IntPtr points,
            IntPtr hull,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool clockwise,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool returnPoints);

        /// <summary>
        /// The function tests whether the input contour is convex or not.
        /// </summary>
        /// <param name="contour">Input vector of 2D points </param>
        /// <returns>true if input is convex</returns>
        public static bool IsContourConvex(IInputArray contour)
        {
            using (InputArray iaContour = contour.GetInputArray())
                return cveIsContourConvex(iaContour);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveIsContourConvex(IntPtr contour);

        /// <summary>
        /// Finds intersection of two convex polygons
        /// </summary>
        /// <param name="p1">The first convex polygon</param>
        /// <param name="p2">The second convex polygon</param>
        /// <param name="p12">The intersection of the convex polygon</param>
        /// <param name="handleNested">Handle nested polygons</param>
        /// <returns>Absolute value of area of intersecting polygon.</returns>
        public static float IntersectConvexConvex(IInputArray p1, IInputArray p2, IOutputArray p12, bool handleNested = true)
        {
            using (InputArray iaP1 = p1.GetInputArray())
            using (InputArray iaP2 = p2.GetInputArray())
            using (OutputArray oaP12 = p12.GetOutputArray())
                return cveIntersectConvexConvex(iaP1, iaP2, oaP12, handleNested);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern float cveIntersectConvexConvex(
            IntPtr p1,
            IntPtr p2,
            IntPtr p12,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool handleNested);

        /// <summary>
        /// Determines whether the point is inside contour, outside, or lies on an edge (or coincides with a vertex).
        /// </summary>
        /// <param name="contour">Input contour</param>
        /// <param name="pt">The point tested against the contour</param>
        /// <param name="measureDist">If true, the function estimates distance from the point to the nearest contour edge</param>
        /// <returns>Positive (inside), negative (outside) or zero (on edge)</returns>
        public static double PointPolygonTest(IInputArray contour, PointF pt, bool measureDist)
        {
            using (InputArray iaContour = contour.GetInputArray())
                return cvePointPolygonTest(iaContour, ref pt, measureDist);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cvePointPolygonTest(
            IntPtr contour,
            ref PointF pt,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool measureDist);

        /// <summary>
        /// Finds the convexity defects of a contour.
        /// </summary>
        /// <param name="contour">Input contour</param>
        /// <param name="convexhull">Convex hull obtained using ConvexHull</param>
        /// <param name="convexityDefects">The output vector of convexity defects.</param>
        public static void ConvexityDefects(IInputArray contour, IInputArray convexhull, IOutputArray convexityDefects)
        {
            using (InputArray iaContour = contour.GetInputArray())
            using (InputArray iaConvexhull = convexhull.GetInputArray())
            using (OutputArray oaConvecxityDefects = convexityDefects.GetOutputArray())
                cveConvexityDefects(iaContour, iaConvexhull, oaConvecxityDefects);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveConvexityDefects(IntPtr contour, IntPtr convexhull, IntPtr convexityDefects);

        /// <summary>
        /// Find the bounding rectangle for the specific array of points
        /// </summary>
        /// <param name="points">The collection of points</param>
        /// <returns>The bounding rectangle for the array of points</returns>
        public static Rectangle BoundingRectangle(Point[] points)
        {
            using (VectorOfPoint vp = new VectorOfPoint(points))
                return BoundingRectangle(vp);
        }

        /// <summary>
        /// Returns the up-right bounding rectangle for 2d point set
        /// </summary>
        /// <param name="points">Input 2D point set, stored in std::vector or Mat.</param>
        /// <returns>The up-right bounding rectangle for 2d point set</returns>
        public static Rectangle BoundingRectangle(IInputArray points)
        {
            Rectangle rectangle = new Rectangle();
            using (InputArray iaPoints = points.GetInputArray())
                cveBoundingRectangle(iaPoints, ref rectangle);
            return rectangle;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBoundingRectangle(IntPtr points, ref Rectangle boundingRect);

        /// <summary>
        /// Calculates area of the whole contour or contour section.
        /// </summary>
        /// <param name="contour">Input vector of 2D points (contour vertices).</param>
        /// <param name="oriented">Oriented area flag.</param>
        /// <returns>The area of the whole contour or contour section</returns>
        public static double ContourArea(IInputArray contour, bool oriented = false)
        {
            using (InputArray iaContour = contour.GetInputArray())
                return cveContourArea(iaContour, oriented);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveContourArea(
            IntPtr contour,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool oriented);

        /// <summary>
        /// Calculates a contour perimeter or a curve length
        /// </summary>
        /// <param name="curve">Sequence or array of the curve points</param>
        /// <param name="isClosed">Indicates whether the curve is closed or not.</param>
        /// <returns>Contour perimeter or a curve length</returns>
        public static double ArcLength(IInputArray curve, bool isClosed)
        {
            using (InputArray iaCurve = curve.GetInputArray())
                return cveArcLength(iaCurve, isClosed);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveArcLength(
            IntPtr curve,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool isClosed);

        /// <summary>
        /// Find the bounding rectangle for the specific array of points
        /// </summary>
        /// <param name="points">The collection of points</param>
        /// <returns>The bounding rectangle for the array of points</returns>
        public static RotatedRect MinAreaRect(PointF[] points)
        {
            using (VectorOfPointF vpf = new VectorOfPointF(points))
                return MinAreaRect(vpf);
        }

        /// <summary>
        /// Finds a rotated rectangle of the minimum area enclosing the input 2D point set.
        /// </summary>
        /// <param name="points">Input vector of 2D points</param>
        /// <returns>a circumscribed rectangle of the minimal area for 2D point set</returns>
        public static RotatedRect MinAreaRect(IInputArray points)
        {
            RotatedRect rect = new RotatedRect();
            using (InputArray iaPoints = points.GetInputArray())
                cveMinAreaRect(iaPoints, ref rect);
            return rect;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMinAreaRect(IntPtr points, ref RotatedRect box);

        /// <summary>
        /// Finds the minimal circumscribed circle for 2D point set using iterative algorithm.
        /// </summary>
        /// <param name="points">Sequence or array of 2D points</param>
        /// <returns>The minimal circumscribed circle for 2D point set</returns>
        public static CircleF MinEnclosingCircle(PointF[] points)
        {
            using (VectorOfPointF vp = new VectorOfPointF(points))
                return MinEnclosingCircle(vp);
        }

        /// <summary>
        /// Finds the minimal circumscribed circle for 2D point set using iterative algorithm.
        /// </summary>
        /// <param name="points">Sequence or array of 2D points</param>
        /// <returns>The minimal circumscribed circle for 2D point set</returns>
        public static CircleF MinEnclosingCircle(IInputArray points)
        {
            PointF center = new PointF();
            float radius = 0;
            using (InputArray iaPoints = points.GetInputArray())
                cveMinEnclosingCircle(iaPoints, ref center, ref radius);
            return new CircleF(center, radius);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMinEnclosingCircle(IntPtr points, ref PointF center, ref float radius);

        /// <summary>
        /// Finds a triangle of minimum area enclosing a 2D point set and returns its area.
        /// </summary>
        /// <param name="points">Input vector of 2D points with depth CV_32S or CV_32F</param>
        /// <param name="triangles">Output vector of three 2D points defining the vertices of the triangle.</param>
        /// <returns>The triangle's area</returns>
        public static double MinEnclosingTriangle(IInputArray points, IOutputArray triangles)
        {
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaTriangles = triangles.GetOutputArray())
                return cveMinEnclosingTriangle(iaPoints, oaTriangles);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveMinEnclosingTriangle(IntPtr points, IntPtr triangle);

        /// <summary>
        /// Approximates a polygonal curve(s) with the specified precision.
        /// </summary>
        /// <param name="curve">Input vector of a 2D point</param>
        /// <param name="approxCurve">Result of the approximation.</param>
        /// <param name="epsilon">Parameter specifying the approximation accuracy.</param>
        /// <param name="closed">If true, the approximated curve is closed.</param>
        public static void ApproxPolyDP(IInputArray curve, IOutputArray approxCurve, double epsilon, bool closed)
        {
            using (InputArray iaCurve = curve.GetInputArray())
            using (OutputArray oaApproxCurve = approxCurve.GetOutputArray())
                cveApproxPolyDP(iaCurve, oaApproxCurve, epsilon, closed);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveApproxPolyDP(
            IntPtr curve,
            IntPtr approxCurve,
            double epsilon,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool closed);

        /// <summary>
        /// Approximates a polygon with a convex hull with a specified accuracy and number of sides.
        /// </summary>
        /// <param name="curve">Input vector of a 2D points stored in std::vector or Mat.</param>
        /// <param name="approxCurve">Result of the approximation.</param>
        /// <param name="nsides">The parameter defines the number of sides of the result polygon.</param>
        /// <param name="epsilonPercentage">Defines the percentage of the maximum of additional area.</param>
        /// <param name="ensureConvex">If true, algorithm creates a convex hull of input contour.</param>
        public static void ApproxPolyN(IInputArray curve, IOutputArray approxCurve, int nsides, float epsilonPercentage = -1.0f, bool ensureConvex = true)
        {
            using (InputArray iaCurve = curve.GetInputArray())
            using (OutputArray oaApproxCurve = approxCurve.GetOutputArray())
                cveApproxPolyN(iaCurve, oaApproxCurve, nsides, epsilonPercentage, ensureConvex);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveApproxPolyN(
            IntPtr curve,
            IntPtr approxCurve,
            int nsides,
            float epsilonPercentage,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool ensureConvex);

        /// <summary>
        /// Calculates spatial and central moments up to the third order.
        /// </summary>
        /// <param name="arr">Image or polygon</param>
        /// <param name="binaryImage">If true, all non-zero pixel values are treated as 1s</param>
        /// <returns>The moment</returns>
        public static Moments Moments(IInputArray arr, bool binaryImage = false)
        {
            Moments m = new Moments();
            using (InputArray iaArr = arr.GetInputArray())
                cveMoments(iaArr, binaryImage, m);
            return m;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMoments(
            IntPtr arr,
            [MarshalAs(CvInvoke.BoolMarshalType)] bool binaryImage,
            IntPtr moments);

        /// <summary>
        /// Compares two shapes using Hu invariants
        /// </summary>
        /// <param name="contour1">First contour or grayscale image</param>
        /// <param name="contour2">Second contour or grayscale image</param>
        /// <param name="method">Comparison method</param>
        /// <param name="parameter">Method-specific parameter (not used now)</param>
        /// <returns>The result of the comparison</returns>
        public static double MatchShapes(IInputArray contour1, IInputArray contour2, CvEnum.ContoursMatchType method, double parameter = 0)
        {
            using (InputArray iaContour1 = contour1.GetInputArray())
            using (InputArray iaContour2 = contour2.GetInputArray())
                return cveMatchShapes(iaContour1, iaContour2, method, parameter);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveMatchShapes(IntPtr contour1, IntPtr contour2, CvEnum.ContoursMatchType method, double parameter);

        /// <summary>
        /// Calculates seven Hu invariants
        /// </summary>
        /// <param name="m">The image moment</param>
        /// <param name="hu">The output Hu moments.</param>
        public static void HuMoments(Moments m, IOutputArray hu)
        {
            using (OutputArray oaHu = hu.GetOutputArray())
                CvInvoke.cveHuMoments(m, oaHu);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHuMoments(IntPtr moments, IntPtr huMoments);

        /// <summary>
        /// Calculates seven Hu invariants
        /// </summary>
        /// <param name="m">The image moment</param>
        /// <returns>The output Hu moments.</returns>
        public static double[] HuMoments(Moments m)
        {
            double[] hu = new double[7];
            GCHandle handle = GCHandle.Alloc(hu, GCHandleType.Pinned);
            CvInvoke.cveHuMoments2(m, handle.AddrOfPinnedObject());
            handle.Free();
            return hu;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHuMoments2(IntPtr moments, IntPtr hu);
    }
}
