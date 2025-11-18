//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Diagnostics;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Emgu.CV
{
    
    /// <summary>
    /// A (2x3) 2D rotation matrix. This Matrix defines an Affine Transform
    /// </summary>
    [Serializable]
    public class RotationMatrix2D : Mat
    {
        /// <summary>
        /// Create an empty (2x3) 2D rotation matrix
        /// </summary>
        public RotationMatrix2D()
           : base()
        { }

        /// <summary>
        /// Create a (2x3) 2D rotation matrix
        /// </summary>
        /// <param name="center">Center of the rotation in the source image</param>
        /// <param name="angle">The rotation angle in degrees. Positive values mean counter-clockwise rotation (the coordinate origin is assumed at top-left corner). </param>
        /// <param name="scale">Isotropic scale factor.</param>
        public RotationMatrix2D(PointF center, double angle, double scale)
           : this()
        {
            SetRotation(center, angle, scale);
        }

        /// <summary>
        /// Set the values of the rotation matrix
        /// </summary>
        /// <param name="center">Center of the rotation in the source image</param>
        /// <param name="angle">The rotation angle in degrees. Positive values mean counter-clockwise rotation (the coordinate origin is assumed at top-left corner). </param>
        /// <param name="scale">Isotropic scale factor.</param>
        public void SetRotation(PointF center, double angle, double scale)
        {
            CvInvoke.GetRotationMatrix2D(center, angle, scale, this);
        }

        /// <summary>
        /// Rotate the <paramref name="points"/>, the value of the input <paramref name="points"/> will be changed.
        /// </summary>
        /// <param name="points">The points to be rotated, its value will be modified</param>
        public void RotatePoints(MCvPoint2D64f[] points)
        {
            GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
            using (Mat mat = new Mat(points.Length, 2, DepthType.Cv64F, 1, handle.AddrOfPinnedObject(), Marshal.SizeOf<MCvPoint2D64f>()))
                RotatePoints(mat);
            handle.Free();
        }

        /// <summary>
        /// Rotate the <paramref name="points"/>, the value of the input <paramref name="points"/> will be changed.
        /// </summary>
        /// <param name="points">The points to be rotated, its value will be modified</param>
        public void RotatePoints(PointF[] points)
        {
            GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
            using (Mat mat = new Mat(points.Length, 2, DepthType.Cv32F, 1, handle.AddrOfPinnedObject(), Marshal.SizeOf<PointF>()))
                RotatePoints(mat);
            handle.Free();
        }

        /// <summary>
        /// Rotate the <paramref name="lineSegments"/>, the value of the input <paramref name="lineSegments"/> will be changed.
        /// </summary>
        /// <param name="lineSegments">The line segments to be rotated</param>
        public void RotateLines(LineSegment2DF[] lineSegments)
        {
            GCHandle handle = GCHandle.Alloc(lineSegments, GCHandleType.Pinned);
            using (Mat mat = new Mat(lineSegments.Length * 2, 2, DepthType.Cv32F, 1, handle.AddrOfPinnedObject(), Marshal.SizeOf<PointF>()))
                RotatePoints(mat);
            handle.Free();
        }

        /// <summary>
        /// Rotate the single channel Nx2 matrix where N is the number of 2D points. The value of the matrix is changed after rotation. The points value must be double or float.
        /// </summary>
        /// <param name="points">The N 2D-points to be rotated</param>
        public void RotatePoints(Mat points)
        {
            CvEnum.DepthType dt = points.Depth;
            Debug.Assert(dt == DepthType.Cv32F || dt == DepthType.Cv64F, "Only type of double or float is supported");
            Debug.Assert(points.NumberOfChannels == 1 && points.Cols == 2, "The matrix must be a single channel Nx2 matrix where N is the number of points");

            using (Mat tmp = new Mat(points.Rows, 3, dt, 1))
            using (Mat rotationMatrix = new Mat(Rows, Cols, dt, 1))
            {
                CvInvoke.CopyMakeBorder(points, tmp, 0, 0, 0, 1, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar(1.0));

                if (dt == DepthType.Cv64F)
                    CopyTo(rotationMatrix);
                else
                    ConvertTo(rotationMatrix, dt);

                CvInvoke.Gemm(
                   tmp,
                   rotationMatrix,
                   1.0,
                   null,
                   0.0,
                   points,
                   Emgu.CV.CvEnum.GemmType.Src2Transpose);

            }
        }

        /// <summary>
        /// Return a clone of the Matrix
        /// </summary>
        /// <returns>A clone of the Matrix</returns>
        public new RotationMatrix2D Clone()
        {
            RotationMatrix2D clone = new RotationMatrix2D();
            CopyTo(clone);
            //CvInvoke.cvCopy(_ptr, clone, IntPtr.Zero);
            return clone;
        }

        /// <summary>
        /// Create a rotation matrix for rotating an image
        /// </summary>
        /// <param name="angle">The rotation angle in degrees. Positive values mean couter-clockwise rotation (the coordiate origin is assumed at image centre). </param>
        /// <param name="center">The rotation center</param>
        /// <param name="srcImageSize">The source image size</param>
        /// <param name="dstImageSize">The minimun size of the destination image</param>
        /// <returns>The rotation matrix that rotate the source image to the destination image.</returns>
        public static RotationMatrix2D CreateRotationMatrix(PointF center, double angle, Size srcImageSize, out Size dstImageSize)
        {
            RotationMatrix2D rotationMatrix = new RotationMatrix2D(center, angle, 1);
            PointF[] corners = new PointF[] {
                  new PointF(0, 0),
                  new PointF(srcImageSize.Width - 1 , 0),
                  new PointF(srcImageSize.Width - 1, srcImageSize.Height -1),
                  new PointF(0, srcImageSize.Height -1)};
            rotationMatrix.RotatePoints(corners);
            int minX = (int)Math.Round(Math.Min(Math.Min(corners[0].X, corners[1].X), Math.Min(corners[2].X, corners[3].X)));
            int maxX = (int)Math.Round(Math.Max(Math.Max(corners[0].X, corners[1].X), Math.Max(corners[2].X, corners[3].X)));
            int minY = (int)Math.Round(Math.Min(Math.Min(corners[0].Y, corners[1].Y), Math.Min(corners[2].Y, corners[3].Y)));
            int maxY = (int)Math.Round(Math.Max(Math.Max(corners[0].Y, corners[1].Y), Math.Max(corners[2].Y, corners[3].Y)));

            using (Mat offset = new Mat(2, 3, DepthType.Cv64F, 1))
            {
                offset.SetTo(new double[]
                {
                    0.0, 0.0, minX,
                    0.0, 0.0, minY
                });
                /*
                offset[0, 2] = minX;
                offset[1, 2] = minY;
                */
                CvInvoke.Subtract(rotationMatrix, offset, rotationMatrix);
                //rotationMatrix[0, 2] -= minX;
                //rotationMatrix[1, 2] -= minY;
            }
            dstImageSize = new Size(maxX - minX + 1, maxY - minY + 1);
            return rotationMatrix;
        }
    }
}
