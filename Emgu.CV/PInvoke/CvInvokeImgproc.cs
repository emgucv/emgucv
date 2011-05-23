//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      #region Sampling, Interpolation and Geometrical Transforms
      /// <summary>
      /// Implements a particular case of application of line iterators. The function reads all the image points lying on the line between pt1 and pt2, including the ending points, and stores them into the buffer
      /// </summary>
      /// <param name="image">Image to sample the line from</param>
      /// <param name="pt1">Starting the line point.</param>
      /// <param name="pt2">Ending the line point</param>
      /// <param name="buffer">Buffer to store the line points; must have enough size to store max( |pt2.x-pt1.x|+1, |pt2.y-pt1.y|+1 ) points in case of 8-connected line and |pt2.x-pt1.x|+|pt2.y-pt1.y|+1 in case of 4-connected line</param>
      /// <param name="connectivity">The line connectivity, 4 or 8</param>
      /// <returns></returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvSampleLine(IntPtr image, Point pt1, Point pt2, IntPtr buffer, CvEnum.CONNECTIVITY connectivity);

      /// <summary>
      /// Extracts pixels from src:
      /// dst(x, y) = src(x + center.x - (width(dst)-1)*0.5, y + center.y - (height(dst)-1)*0.5)
      /// where the values of pixels at non-integer coordinates are retrieved using bilinear interpolation. Every channel of multiple-channel images is processed independently. Whereas the rectangle center must be inside the image, the whole rectangle may be partially occluded. In this case, the replication border mode is used to get pixel values beyond the image boundaries.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Extracted rectangle</param>
      /// <param name="center">Floating point coordinates of the extracted rectangle center within the source image. The center must be inside the image.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetRectSubPix(IntPtr src, IntPtr dst, PointF center);

      /// <summary>
      /// Extracts pixels from src at sub-pixel accuracy and stores them to dst as follows:
      /// dst(x, y)= src( A_11 x'+A_12 y'+ b1, A_21 x'+A_22 y'+ b2),
      /// where A and b are taken from map_matrix:
      /// map_matrix = [ [A11 A12  b1], [ A21 A22  b2 ] ]
      /// x'=x-(width(dst)-1)*0.5, y'=y-(height(dst)-1)*0.5
      /// where the values of pixels at non-integer coordinates A (x,y)^T + b are retrieved using bilinear interpolation. When the function needs pixels outside of the image, it uses replication border mode to reconstruct the values. Every channel of multiple-channel images is processed independently.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Extracted quadrangle</param>
      /// <param name="mapMatrix">The transformation 2 x 3 matrix [A|b]</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetQuadrangleSubPix(IntPtr src, IntPtr dst, IntPtr mapMatrix);

      /// <summary>
      /// Resizes image src so that it fits exactly to dst. If ROI is set, the function consideres the ROI as supported as usual
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image</param>
      /// <param name="interpolation">Interpolation method</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvResize(IntPtr src, IntPtr dst, CvEnum.INTER interpolation);

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapMatrix">2x3 transformation matrix</param>
      /// <param name="flags"> flags </param>
      /// <param name="fillval">A value used to fill outliers</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvWarpAffine(
          IntPtr src,
          IntPtr dst,
          IntPtr mapMatrix,
          int flags,
          MCvScalar fillval);

      /// <summary>
      /// Calculates the matrix of an affine transform such that:
      /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
      /// </summary>
      /// <param name="src">Pointer to an array of PointF, Coordinates of 3 triangle vertices in the source image.</param>
      /// <param name="dst">Pointer to an array of PointF, Coordinates of the 3 corresponding triangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
      /// <returns>Pointer to the destination 2x3 matrix</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetAffineTransform(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix);

      /// <summary>
      /// Calculates the matrix of an affine transform such that:
      /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
      /// </summary>
      /// <param name="src">Coordinates of 3 triangle vertices in the source image.</param>
      /// <param name="dst">Coordinates of the 3 corresponding triangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
      /// <returns>Pointer to the destination 2x3 matrix</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetAffineTransform(
         PointF[] src,
         PointF[] dst,
         IntPtr mapMatrix);

      /// <summary>
      /// Calculates rotation matrix
      /// </summary>
      /// <param name="center">Center of the rotation in the source image. </param>
      /// <param name="angle">The rotation angle in degrees. Positive values mean couter-clockwise rotation (the coordiate origin is assumed at top-left corner).</param>
      /// <param name="scale">Isotropic scale factor</param>
      /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
      /// <returns>Pointer to the destination 2x3 matrix</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cv2DRotationMatrix(
          PointF center,
          double angle,
          double scale,
          IntPtr mapMatrix);

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapMatrix">3? transformation matrix</param>
      /// <param name="flags"></param>
      /// <param name="fillval">A value used to fill outliers</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvWarpPerspective(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix,
         int flags,
         MCvScalar fillval);

      /// <summary>
      /// calculates matrix of perspective transform such that:
      /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
      /// </summary>
      /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
      /// <param name="dst">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 3x3 matrix</param>
      /// <returns>Pointer to the perspective transform matrix</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetPerspectiveTransform(
         PointF[] src,
         PointF[] dst,
         IntPtr mapMatrix);

      /// <summary>
      /// calculates matrix of perspective transform such that:
      /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
      /// </summary>
      /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
      /// <param name="dst">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 3x3 matrix</param>
      /// <returns>Pointer to the perspective transform matrix</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetPerspectiveTransform(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix);

      /// <summary>
      /// Similar to other geometrical transformations, some interpolation method (specified by user) is used to extract pixels with non-integer coordinates.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapx">The map of x-coordinates (32fC1 image)</param>
      /// <param name="mapy">The map of y-coordinates (32fC1 image)</param>
      /// <param name="flags">A combination of interpolation method and the optional flag CV_WARP_FILL_OUTLIERS </param>
      /// <param name="fillval">A value used to fill outliers</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvRemap(IntPtr src, IntPtr dst,
            IntPtr mapx, IntPtr mapy,
            int flags,
            MCvScalar fillval);

      /// <summary>
      /// The function emulates the human "foveal" vision and can be used for fast scale and rotation-invariant template matching, for object tracking etc.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="center">The transformation center, where the output precision is maximal</param>
      /// <param name="M">Magnitude scale parameter</param>
      /// <param name="flags">A combination of interpolation method and the optional flag CV_WARP_FILL_OUTLIERS and/or CV_WARP_INVERSE_MAP</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvLogPolar(
         IntPtr src,
         IntPtr dst,
         PointF center,
         double M,
         int flags);

      /// <summary>
      /// The function emulates the human "foveal" vision and can be used for fast scale and rotation-invariant template matching, for object tracking etc.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="center">The transformation center, where the output precision is maximal</param>
      /// <param name="maxRadius">Maximum radius</param>
      /// <param name="flags">A combination of interpolation method and the optional flag CV_WARP_FILL_OUTLIERS and/or CV_WARP_INVERSE_MAP</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvLinearPolar(
         IntPtr src,
         IntPtr dst,
         PointF center,
         double maxRadius,
         int flags);
      #endregion

      /// <summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. First it convolves source image with the specified filter and then downsamples the image by rejecting even rows and columns.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="filter">Type of the filter used for convolution; only CV_GAUSSIAN_5x5 is currently supported.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPyrDown(IntPtr src, IntPtr dst, CvEnum.FILTER_TYPE filter);

      /// <summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition. First it upsamples the source image by injecting even zero rows and columns and then convolves result with the specified filter multiplied by 4 for interpolation. So the destination image is four times larger than the source image.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="filter">Type of the filter used for convolution; only CV_GAUSSIAN_5x5 is currently supported.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPyrUp(IntPtr src, IntPtr dst, CvEnum.FILTER_TYPE filter);

      /// <summary>
      /// The function cvPyrSegmentation implements image segmentation by pyramids. The pyramid builds up to the level level. The links between any pixel a on level i and its candidate father pixel b on the adjacent level are established if 
      /// p(c(a),c(b))&gt;threshold1. After the connected components are defined, they are joined into several clusters. Any two segments A and B belong to the same cluster, if 
      /// p(c(A),c(B))&gt;threshold2. The input image has only one channel, then 
      /// p(c1,c2)=|c1-c2|. If the input image has three channels (red, green and blue), then 
      /// p(c1,c2)=0.3*(c1r-c2r)+0.59 * (c1g-c2g)+0.11 *(c1b-c2b) . There may be more than one connected component per a cluster.
      /// </summary>
      /// <param name="src">The source image, should be 8-bit single-channel or 3-channel images </param>
      /// <param name="dst">The destination image, should be 8-bit single-channel or 3-channel images, same size as src </param>
      /// <param name="storage">Storage; stores the resulting sequence of connected components</param>
      /// <param name="comp">Pointer to the output sequence of the segmented components</param>
      /// <param name="level">Maximum level of the pyramid for the segmentation</param>
      /// <param name="threshold1">Error threshold for establishing the links</param>
      /// <param name="threshold2">Error threshold for the segments clustering</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPyrSegmentation(
          IntPtr src,
          IntPtr dst,
          IntPtr storage,
          out IntPtr comp,
          int level,
          double threshold1,
          double threshold2);

      /// <summary>
      /// Implements one of the variants of watershed, non-parametric marker-based segmentation algorithm, described in [Meyer92] Before passing the image to the function, user has to outline roughly the desired regions in the image markers with positive (>0) indices, i.e. every region is represented as one or more connected components with the pixel values 1, 2, 3 etc. Those components will be "seeds" of the future image regions. All the other pixels in markers, which relation to the outlined regions is not known and should be defined by the algorithm, should be set to 0's. On the output of the function, each pixel in markers is set to one of values of the "seed" components, or to -1 at boundaries between the regions.
      /// </summary>
      /// <remarks>Note, that it is not necessary that every two neighbor connected components are separated by a watershed boundary (-1's pixels), for example, in case when such tangent components exist in the initial marker image. </remarks>
      /// <param name="image">The input 8-bit 3-channel image</param>
      /// <param name="markers">The input/output Int32 depth single-channel image (map) of markers. </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvWatershed(IntPtr image, IntPtr markers);

      #region Computational Geometry
      /// <summary>
      /// Finds minimum area rectangle that contains both input rectangles inside
      /// </summary>
      /// <param name="rect1">First rectangle </param>
      /// <param name="rect2">Second rectangle </param>
      /// <returns>The minimum area rectangle that contains both input rectangles inside</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern Rectangle cvMaxRect(ref Rectangle rect1, ref Rectangle rect2);

      /// <summary>
      /// Fits line to 2D or 3D point set 
      /// </summary>
      /// <param name="points">Sequence or array of 2D or 3D points with 32-bit integer or floating-point coordinates</param>
      /// <param name="distType">The distance used for fitting </param>
      /// <param name="param">Numerical parameter (C) for some types of distances, if 0 then some optimal value is chosen</param>
      /// <param name="reps">Sufficient accuracy for radius (distance between the coordinate origin and the line),  0.01 would be a good default</param>
      /// <param name="aeps">Sufficient accuracy for angle, 0.01 would be a good default</param>
      /// <param name="line">The output line parameters. In case of 2d fitting it is array of 4 floats (vx, vy, x0, y0) where (vx, vy) is a normalized vector collinear to the line and (x0, y0) is some point on the line. In case of 3D fitting it is array of 6 floats (vx, vy, vz, x0, y0, z0) where (vx, vy, vz) is a normalized vector collinear to the line and (x0, y0, z0) is some point on the line.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFitLine(
          IntPtr points,
          CvEnum.DIST_TYPE distType,
          double param,
          double reps,
          double aeps,
          [Out] float[] line);

      /// <summary>
      /// Calculates vertices of the input 2d box.
      /// </summary>
      /// <param name="box">The box</param>
      /// <param name="pt">An array of size 8, where the coordinate for ith point is: [pt[i&gt;&gt;1], pt[(i&gt;&gt;1)+1]]</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvBoxPoints(
         MCvBox2D box,
         [Out]
         float[] pt);

      /// <summary>
      /// Calculates vertices of the input 2d box.
      /// </summary>
      /// <param name="box">The box</param>
      /// <param name="pt">An array of size 4 points</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvBoxPoints(
         MCvBox2D box,
         [Out]
         PointF[] pt);

      /// <summary>
      /// Calculates ellipse that fits best (in least-squares sense) to a set of 2D points. The meaning of the returned structure fields is similar to those in cvEllipse except that size stores the full lengths of the ellipse axises, not half-lengths
      /// </summary>
      /// <param name="points">Sequence or array of points</param>
      /// <returns>The ellipse that fits best (in least-squares sense) to a set of 2D points</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvBox2D cvFitEllipse2(IntPtr points);

      /// <summary>
      /// The function cvConvexHull2 finds convex hull of 2D point set using Sklansky's algorithm. 
      /// </summary>
      /// <param name="input">Sequence or array of 2D points with 32-bit integer or floating-point coordinates</param>
      /// <param name="hullStorage">The destination array (CvMat*) or memory storage (CvMemStorage*) that will store the convex hull. If it is array, it should be 1d and have the same number of elements as the input array/sequence. On output the header is modified so to truncate the array downto the hull size</param>
      /// <param name="orientation">Desired orientation of convex hull: CV_CLOCKWISE or CV_COUNTER_CLOCKWISE</param>
      /// <param name="returnPoints">If non-zero, the points themselves will be stored in the hull instead of indices if hull_storage is array, or pointers if hull_storage is memory storage</param>
      /// <returns>If hull_storage is memory storage, the function creates a sequence containing the hull points or pointers to them, depending on return_points value and returns the sequence on output</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvConvexHull2(
          IntPtr input,
          IntPtr hullStorage,
          CvEnum.ORIENTATION orientation,
          int returnPoints);

      #endregion

      #region Plannar Subdivisions
      /// <summary>
      /// Creates an empty Delaunay subdivision, where 2d points can be added further using function cvSubdivDelaunay2DInsert. All the points to be added must be within the specified rectangle, otherwise a runtime error will be raised. 
      /// </summary>
      /// <param name="rect">Rectangle that includes all the 2d points that are to be added to subdivision.</param>
      /// <param name="storage">Container for subdivision</param>
      /// <returns></returns>
      public static IntPtr cvCreateSubdivDelaunay2D(Rectangle rect, IntPtr storage)
      {
         IntPtr subdiv = cvCreateSubdiv2D((int)CvEnum.SEQ_KIND.CV_SEQ_KIND_SUBDIV2D,
                 Marshal.SizeOf(typeof(MCvSubdiv2D)),
                 Marshal.SizeOf(typeof(MCvSubdiv2DPoint)),
                 Marshal.SizeOf(typeof(MCvQuadEdge2D)),
                 storage);

         cvInitSubdivDelaunay2D(subdiv, rect);
         return subdiv;
      }

      /// <summary>
      /// Initializes Delaunay triangulation 
      /// </summary>
      /// <param name="subdiv"></param>
      /// <param name="rect"></param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInitSubdivDelaunay2D(IntPtr subdiv, Rectangle rect);

      /// <summary>
      /// Locates input point within subdivision. It finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point. 
      /// </summary>
      /// <param name="subdiv">Delaunay or another subdivision</param>
      /// <param name="pt">Input point</param>
      /// <returns>pointer to the found subdivision vertex (CvSubdiv2DPoint)</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvFindNearestPoint2D(IntPtr subdiv, PointF pt);

      /// <summary>
      /// Creates new subdivision
      /// </summary>
      /// <param name="subdivType"></param>
      /// <param name="headerSize"></param>
      /// <param name="vtxSize"></param>
      /// <param name="quadedgeSize"></param>
      /// <param name="storage"></param>
      /// <returns></returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateSubdiv2D(
          int subdivType,
          int headerSize,
          int vtxSize,
          int quadedgeSize,
          IntPtr storage);

      /// <summary>
      /// Inserts a single point to subdivision and modifies the subdivision topology appropriately. If a points with same coordinates exists already, no new points is added. The function returns pointer to the allocated point. No virtual points coordinates is calculated at this stage.
      /// </summary>
      /// <param name="subdiv">Delaunay subdivision created by function cvCreateSubdivDelaunay2D</param>
      /// <param name="pt">Inserted point.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvSubdivDelaunay2DInsert(IntPtr subdiv, PointF pt);

      /// <summary>
      /// Locates input point within subdivision
      /// </summary>
      /// <param name="subdiv">Plannar subdivision</param>
      /// <param name="pt">The point to locate</param>
      /// <param name="edge">The output edge the point falls onto or right to</param>
      /// <param name="vertex">Optional output vertex double pointer the input point coincides with</param>
      /// <returns>The type of location for the point</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern CvEnum.Subdiv2DPointLocationType cvSubdiv2DLocate(IntPtr subdiv, PointF pt,
                                           out IntPtr edge,
                                           ref IntPtr vertex);

      /// <summary>
      /// Calculates coordinates of virtual points. All virtual points corresponding to some vertex of original subdivision form (when connected together) a boundary of Voronoi cell of that point
      /// </summary>
      /// <param name="subdiv">Delaunay subdivision, where all the points are added already</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcSubdivVoronoi2D(IntPtr subdiv);
      #endregion

      #region Feature Matching
      /// <summary>
      /// Constructs a balanced kd-tree index of the given feature vectors. The lifetime of the desc matrix must exceed that of the returned tree. I.e., no copy is made of the vectors.
      /// </summary>
      /// <param name="desc">n x d matrix of n d-dimensional feature vectors (CV_32FC1 or CV_64FC1)</param>
      /// <returns>A balanced kd-tree index of the given feature vectors</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateKDTree(IntPtr desc);

      /// <summary>
      /// Constructs a spill tree index of the given feature vectors. The lifetime of the desc matrix must exceed that of the returned tree. I.e., no copy is made of the vectors.
      /// </summary>
      /// <param name="desc">n x d matrix of n d-dimensional feature vectors (CV_32FC1 or CV_64FC1)</param>
      /// <param name="naive"></param>
      /// <param name="rho"></param>
      /// <param name="tau"></param>
      /// <returns>A spill tree index of the given feature vectors</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateSpillTree(IntPtr desc, int naive, double rho, double tau);

      /// <summary>
      /// Deallocates the given kd-tree
      /// </summary>
      /// <param name="tr">Pointer to tree being destroyed</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseFeatureTree(IntPtr tr);

      /// <summary>
      /// Searches feature tree for k nearest neighbors of given reference points.
      /// </summary>
      /// <remarks> In case of k-d tree: Finds (with high probability) the k nearest neighbors in tr for each of the given (row-)vectors in desc, using best-bin-first searching ([Beis97]). The complexity of the entire operation is at most O(m*emax*log2(n)), where n is the number of vectors in the tree</remarks>
      /// <param name="tr">Pointer to kd-tree index of reference vectors</param>
      /// <param name="desc">m x d matrix of (row-)vectors to find the nearest neighbors of</param>
      /// <param name="results">m x k set of row indices of matching vectors (referring to matrix passed to cvCreateFeatureTree). Contains -1 in some columns if fewer than k neighbors found</param>
      /// <param name="dist">m x k matrix of distances to k nearest neighbors</param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="emax">For k-d tree only: the maximum number of leaves to visit.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindFeatures(
         IntPtr tr,
         IntPtr desc,
         IntPtr results,
         IntPtr dist,
         int k,
         int emax);

      /// <summary>
      /// Performs orthogonal range searching on the given kd-tree. That is, it returns the set of vectors v in tr that satisfy bounds_min[i] &lt;= v[i] &lt;= bounds_max[i], 0 &lt;= i &lt; d, where d is the dimension of vectors in the tree. The function returns the number of such vectors found
      /// </summary>
      /// <param name="tr">Pointer to kd-tree index of reference vectors</param>
      /// <param name="boundsMin">1 x d or d x 1 vector (CV_32FC1 or CV_64FC1) giving minimum value for each dimension</param>
      /// <param name="boundsMax">1 x d or d x 1 vector (CV_32FC1 or CV_64FC1) giving maximum value for each dimension</param>
      /// <param name="results">1 x m or m x 1 vector (CV_32SC1) to contain output row indices (referring to matrix passed to cvCreateFeatureTree)</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindFeaturesBoxed(
         IntPtr tr,
         IntPtr boundsMin,
         IntPtr boundsMax,
         IntPtr results);

      #endregion

      /// <summary>
      /// Erodes the source image using the specified structuring element that determines the shape of a pixel neighborhood over which the minimum is taken:
      /// dst=erode(src,element):  dst(x,y)=min((x',y') in element)) src(x+x',y+y')
      /// The function supports the in-place mode. Erosion can be applied several (iterations) times. In case of color image each channel is processed independently.
      /// </summary>
      /// <param name="src">Source image. </param>
      /// <param name="dst">Destination image</param>
      /// <param name="element">Structuring element used for erosion. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="iterations">Number of times erosion is applied.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvErode(IntPtr src, IntPtr dst, IntPtr element, int iterations);

      /// <summary>
      /// Dilates the source image using the specified structuring element that determines the shape of a pixel neighborhood over which the maximum is taken
      /// The function supports the in-place mode. Dilation can be applied several (iterations) times. In case of color image each channel is processed independently
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="element">Structuring element used for erosion. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used</param>
      /// <param name="iterations">Number of times erosion is applied</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDilate(IntPtr src, IntPtr dst, IntPtr element, int iterations);

      /// <summary>
      /// Reconstructs the selected image area from the pixel near the area boundary. The function may be used to remove dust and scratches from a scanned photo, or to remove undesirable objects from still images or video.
      /// </summary>
      /// <param name="src">The input 8-bit 1-channel or 3-channel image</param>
      /// <param name="mask">The inpainting mask, 8-bit 1-channel image. Non-zero pixels indicate the area that needs to be inpainted</param>
      /// <param name="dst">The output image of the same format and the same size as input</param>
      /// <param name="flags">The inpainting method</param>
      /// <param name="inpaintRadius">The radius of circlular neighborhood of each point inpainted that is considered by the algorithm</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInpaint(IntPtr src, IntPtr mask, IntPtr dst, double inpaintRadius, CvEnum.INPAINT_TYPE flags);

      /// <summary>
      /// Smooths image using one of several methods. Every of the methods has some features and restrictions listed below
      /// Blur with no scaling works with single-channel images only and supports accumulation of 8-bit to 16-bit format (similar to cvSobel and cvLaplace) and 32-bit floating point to 32-bit floating-point format.
      /// Simple blur and Gaussian blur support 1- or 3-channel, 8-bit and 32-bit floating point images. These two methods can process images in-place.
      /// Median and bilateral filters work with 1- or 3-channel 8-bit images and can not process images in-place.
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="type">Type of the smoothing</param>
      /// <param name="param1">The first parameter of smoothing operation</param>
      /// <param name="param2">The second parameter of smoothing operation. In case of simple scaled/non-scaled and Gaussian blur if param2 is zero, it is set to param1</param>
      /// <param name="param3">In case of Gaussian kernel this parameter may specify Gaussian sigma (standard deviation). If it is zero, it is calculated from the kernel size:
      /// sigma = (n/2 - 1)*0.3 + 0.8, where n=param1 for horizontal kernel,
      /// n=param2 for vertical kernel.
      /// With the standard sigma for small kernels (3x3 to 7x7) the performance is better. If param3 is not zero, while param1 and param2 are zeros, the kernel size is calculated from the sigma (to provide accurate enough operation). 
      /// </param>
      /// <param name="param4">In case of non-square Gaussian kernel the parameter may be used to specify a different (from param3) sigma in the vertical direction</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSmooth(
          IntPtr src,
          IntPtr dst,
          CvEnum.SMOOTH_TYPE type,
          int param1,
          int param2,
          double param3,
          double param4);

      /// <summary>
      /// The Sobel operators combine Gaussian smoothing and differentiation so the result is more or less robust to the noise. Most often, the function is called with (xorder=1, yorder=0, aperture_size=3) or (xorder=0, yorder=1, aperture_size=3) to calculate first x- or y- image derivative. The first case corresponds to
      /// <pre> 
      ///  |-1  0  1|
      ///  |-2  0  2|
      ///  |-1  0  1|</pre>
      /// kernel and the second one corresponds to
      /// <pre>
      ///  |-1 -2 -1|
      ///  | 0  0  0|
      ///  | 1  2  1|</pre>
      /// or
      /// <pre>
      ///  | 1  2  1|
      ///  | 0  0  0|
      ///  |-1 -2 -1|</pre>
      /// kernel, depending on the image origin (origin field of IplImage structure). No scaling is done, so the destination image usually has larger by absolute value numbers than the source image. To avoid overflow, the function requires 16-bit destination image if the source image is 8-bit. The result can be converted back to 8-bit using cvConvertScale or cvConvertScaleAbs functions. Besides 8-bit images the function can process 32-bit floating-point images. Both source and destination must be single-channel images of equal size or ROI size
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image</param>
      /// <param name="xorder">Order of the derivative x </param>
      /// <param name="yorder">Order of the derivative y</param>
      /// <param name="apertureSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. In all cases except 1, <paramref name="apertureSize"/> x <paramref name="apertureSize"/> separable kernel will be used to calculate the derivative. For aperture_size=1 3x1 or 1x3 kernel is used (Gaussian smoothing is not done). There is also special value CV_SCHARR (=-1) that corresponds to 3x3 Scharr filter that may give more accurate results than 3x3 Sobel. Scharr aperture is: 
      /// <pre>
      /// | -3 0  3|
      /// |-10 0 10|
      /// | -3 0  3|</pre>
      /// for x-derivative or transposed for y-derivative. 
      ///</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSobel(IntPtr src, IntPtr dst, int xorder, int yorder, int apertureSize);

      /// <summary>
      /// Calculates Laplacian of the source image by summing second x- and y- derivatives calculated using Sobel operator:
      /// dst(x,y) = d2src/dx2 + d2src/dy2
      /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel:
      /// |0  1  0|
      /// |1 -4  1|
      /// |0  1  0|
      /// Similar to cvSobel function, no scaling is done and the same combinations of input and output formats are supported. 
      /// </summary>
      /// <param name="src">Source image. </param>
      /// <param name="dst">Destination image. </param>
      /// <param name="apertureSize">Aperture size </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvLaplace(IntPtr src, IntPtr dst, int apertureSize);

      /// <summary>
      /// Finds the edges on the input <paramref name="image"/> and marks them in the output image edges using the Canny algorithm. The smallest of threshold1 and threshold2 is used for edge linking, the largest - to find initial segments of strong edges.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      /// <param name="threshold1">The first threshold</param>
      /// <param name="threshold2">The second threshold.</param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCanny(
          IntPtr image,
          IntPtr edges,
          double threshold1,
          double threshold2,
          int apertureSize);

      /// <summary>
      /// Tests whether the input contour is convex or not. The contour must be simple, i.e. without self-intersections. 
      /// </summary>
      /// <param name="contour">Tested contour (sequence or array of points). </param>
      /// <returns>-1 if input is not valid, 1 if convex, 0 otherwise</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvCheckContourConvexity(IntPtr contour);

      /// <summary>
      /// Determines whether the point is inside contour, outside, or lies on an edge (or coinsides with a vertex). It returns positive, negative or zero value, correspondingly
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="pt">The point tested against the contour</param>
      /// <param name="measureDist">If != 0, the function estimates distance from the point to the nearest contour edge</param>
      /// <returns>
      /// When measureDist = 0, the return value is &gt;0 (inside), &lt;0 (outside) and =0 (on edge), respectively. 
      /// When measureDist != 0, it is a signed distance between the point and the nearest contour edge
      /// </returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvPointPolygonTest(
          IntPtr contour,
          PointF pt,
          int measureDist);

      /// <summary>
      /// Finds all convexity defects of the input contour and returns a sequence of the CvConvexityDefect structures. 
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="convexhull">Convex hull obtained using cvConvexHull2 that should contain pointers or indices to the contour points, not the hull points themselves, i.e. return_points parameter in cvConvexHull2 should be 0</param>
      /// <param name="storage">Container for output sequence of convexity defects. If it is NULL, contour or hull (in that order) storage is used</param>
      /// <returns>Pointer to the sequence of the CvConvexityDefect structures. </returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvConvexityDefects(
         IntPtr contour,
         IntPtr convexhull,
         IntPtr storage);

      /// <summary>
      /// Determines whether the point is inside contour, outside, or lies on an edge (or coinsides with a vertex). It returns positive, negative or zero value, correspondingly
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="pt">The point tested against the contour</param>
      /// <param name="measureDist">If true, the function estimates distance from the point to the nearest contour edge</param>
      /// <returns>
      /// When measureDist == false, the return value is &gt;0 (inside), &lt;0 (outside) and =0 (on edge), respectively. 
      /// When measureDist == true, it is a signed distance between the point and the nearest contour edge
      /// </returns>
      public static double cvPointPolygonTest(
         IntPtr contour,
         PointF pt,
         bool measureDist)
      {
         return cvPointPolygonTest(contour, pt, measureDist ? 1 : 0);
      }

      /// <summary>
      /// Finds a circumscribed rectangle of the minimal area for 2D point set by building convex hull for the set and applying rotating calipers technique to the hull.
      /// </summary>
      /// <param name="points">Sequence of points, or two channel int/float depth matrix</param>
      /// <param name="storage">temporary memory storage</param>
      /// <returns>a circumscribed rectangle of the minimal area for 2D point set</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern MCvBox2D cvMinAreaRect2(IntPtr points, IntPtr storage);

      /// <summary>
      /// Finds the minimal circumscribed circle for 2D point set using iterative algorithm. It returns nonzero if the resultant circle contains all the input points and zero otherwise (i.e. algorithm failed)
      /// </summary>
      /// <param name="points">Sequence or array of 2D points</param>
      /// <param name="center">Output parameter. The center of the enclosing circle</param>
      /// <param name="radius">Output parameter. The radius of the enclosing circle.</param>
      /// <returns>True if the resultant circle contains all the input points and false otherwise (i.e. algorithm failed)</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvMinEnclosingCircle(IntPtr points, out PointF center, out float radius);

      #region Contour Processing Functions
      /// <summary>
      /// Approximates one or more curves and returns the approximation result[s]. 
      /// In case of multiple curves approximation the resultant tree will have the same structure as the input one (1:1 correspondence). 
      /// </summary>
      /// <param name="srcSeq">Sequence of array of points</param>
      /// <param name="headerSize">Header size of approximated curve[s].</param>
      /// <param name="storage">Container for approximated contours. If it is NULL, the input sequences' storage is used</param>
      /// <param name="method">Approximation method</param>
      /// <param name="parameter">Desired approximation accuracy</param>
      /// <param name="parameter2">
      /// In case if srcSeq is sequence it means whether the single sequence should be approximated or all sequences on the same level or below srcSeq (see cvFindContours for description of hierarchical contour structures). 
      /// And if srcSeq is array (CvMat*) of points, the parameter specifies whether the curve is closed (parameter2!=0) or not (parameter2=0). 
      /// </param>
      /// <returns>The approximation result</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvApproxPoly(
          IntPtr srcSeq,
          int headerSize,
          IntPtr storage,
          CvEnum.APPROX_POLY_TYPE method,
          double parameter,
          int parameter2);

      /// <summary>
      /// Returns the up-right bounding rectangle for 2d point set
      /// </summary>
      /// <param name="points">Either a 2D point set, represented as a sequence (CvSeq*, CvContour*) or vector (CvMat*) of points, or 8-bit single-channel mask image (CvMat*, IplImage*), in which non-zero pixels are considered</param>
      /// <param name="update">The update flag. Here is list of possible combination of the flag values and type of contour: 
      /// points is CvContour*, update=0: the bounding rectangle is not calculated, but it is read from rect field of the contour header. 
      /// points is CvContour*, update=1: the bounding rectangle is calculated and written to rect field of the contour header. For example, this mode is used by cvFindContours. 
      /// points is CvSeq* or CvMat*: update is ignored, the bounding rectangle is calculated and returned. 
      /// </param>
      /// <returns>The up-right bounding rectangle for 2d point set</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern Rectangle cvBoundingRect(
          IntPtr points,
          int update);

      /// <summary>
      /// Returns the up-right bounding rectangle for 2d point set
      /// </summary>
      /// <param name="points">Either a 2D point set, represented as a sequence (CvSeq*, CvContour*) or vector (CvMat*) of points, or 8-bit single-channel mask image (CvMat*, IplImage*), in which non-zero pixels are considered</param>
      /// <param name="update">The update flag. Here is list of possible combination of the flag values and type of contour: 
      /// points is CvContour*, update=false: the bounding rectangle is not calculated, but it is read from rect field of the contour header. 
      /// points is CvContour*, update=true: the bounding rectangle is calculated and written to rect field of the contour header. For example, this mode is used by cvFindContours. 
      /// points is CvSeq* or CvMat*: update is ignored, the bounding rectangle is calculated and returned. 
      /// </param>
      /// <returns>The up-right bounding rectangle for 2d point set</returns>
      public static Rectangle cvBoundingRect(IntPtr points, bool update)
      {
         return cvBoundingRect(points, update ? 1 : 0);
      }

      /// <summary>
      /// Calculates area of the whole contour or contour section. 
      /// </summary>
      /// <param name="contour">Seq (sequence or array of vertices). </param>
      /// <param name="slice">Starting and ending points of the contour section of interest, by default area of the whole contour is calculated</param>
      /// <param name="oriented">If zero, the absolute area will be returned. Otherwise the returned value mighted be negative</param>
      /// <returns>The area of the whole contour or contour section</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvContourArea(IntPtr contour, MCvSlice slice, int oriented);

      /// <summary>
      /// Calculates length or curve as sum of lengths of segments between subsequent points
      /// </summary>
      /// <param name="curve">Sequence or array of the curve points</param>
      /// <param name="slice">Starting and ending points of the curve, by default the whole curve length is calculated</param>
      /// <param name="isClosed">
      /// Indicates whether the curve is closed or not. There are 3 cases:
      /// isClosed=0 - the curve is assumed to be unclosed. 
      /// isClosed&gt;0 - the curve is assumed to be closed. 
      /// isClosed&lt;0 - if curve is sequence, the flag CV_SEQ_FLAG_CLOSED of ((CvSeq*)curve)-&gt;flags is checked to determine if the curve is closed or not, otherwise (curve is represented by array (CvMat*) of points) it is assumed to be unclosed. 
      /// </param>
      /// <returns></returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvArcLength(IntPtr curve, MCvSlice slice, int isClosed);

      /// <summary>
      /// Find the perimeter of the contour
      /// </summary>
      /// <param name="contour">Pointer to the contour</param>
      /// <returns>the perimeter of the contour</returns>
      public static double cvContourPerimeter(IntPtr contour)
      {
         return cvArcLength(contour, MCvSlice.WholeSeq, 1);
      }

      /// <summary>
      /// Creates binary tree representation for the input contour and returns the pointer to its root.
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="storage">Container for output tree</param>
      /// <param name="threshold">If the parameter threshold is less than or equal to 0, the function creates full binary tree representation. If the threshold is greater than 0, the function creates representation with the precision threshold: if the vertices with the interceptive area of its base line are less than threshold, the tree should not be built any further</param>
      /// <returns>The binary tree representation for the input contour</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateContourTree(
         IntPtr contour,
         IntPtr storage,
         double threshold);

      /// <summary>
      /// Return the contour from its binary tree representation
      /// </summary>
      /// <param name="tree">Contour tree</param>
      /// <param name="storage">Container for the reconstructed contour</param>
      /// <param name="criteria">Criteria, where to stop reconstruction</param>
      /// <returns>The contour represented by this contour tree</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvContourFromContourTree(
         IntPtr tree,
         IntPtr storage,
         MCvTermCriteria criteria);

      /// <summary>
      /// Calculates the value of the matching measure for two contour trees. The similarity measure is calculated level by level from the binary tree roots. If at the certain level difference between contours becomes less than threshold, the reconstruction process is interrupted and the current difference is returned
      /// </summary>
      /// <param name="tree1">First contour tree</param>
      /// <param name="tree2">Second contour tree</param>
      /// <param name="method">Similarity measure, only CV_CONTOUR_TREES_MATCH_I1 is supported</param>
      /// <param name="threshold">Similarity threshold</param>
      /// <returns>The value of the matching measure for two contour trees</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvMatchContourTrees(
         IntPtr tree1,
         IntPtr tree2,
         CvEnum.MATCH_CONTOUR_TREE_METHOD method,
         double threshold);
      #endregion

      /// <summary>
      /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix. If you want to apply different kernels to different channels, split the image using cvSplit into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFilter2D(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor);

      /// <summary>
      /// Copies the source 2D array into interior of destination array and makes a border of the specified type around the copied area. The function is useful when one needs to emulate border type that is different from the one embedded into a specific algorithm implementation. For example, morphological functions, as well as most of other filtering functions in OpenCV, internally use replication border type, while the user may need zero border or a border, filled with 1's or 255's
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="offset">Coordinates of the top-left corner (or bottom-left in case of images with bottom-left origin) of the destination image rectangle where the source image (or its ROI) is copied. Size of the rectangle matches the source image size/ROI size</param>
      /// <param name="bordertype">Type of the border to create around the copied source image rectangle</param>
      /// <param name="value">Value of the border pixels if bordertype=CONSTANT</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCopyMakeBorder(
         IntPtr src,
         IntPtr dst,
         Point offset,
         CvEnum.BORDER_TYPE bordertype,
         MCvScalar value);

      /// <summary>
      /// Applies fixed-level thresholding to single-channel array. The function is typically used to get bi-level (binary) image out of grayscale image (cvCmpS could be also used for this purpose) or for removing a noise, i.e. filtering out pixels with too small or too large values. There are several types of thresholding the function supports that are determined by threshold_type
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="threshold">Threshold value</param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="thresholdType">Thresholding type </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvThreshold(
         IntPtr src,
         IntPtr dst,
         double threshold,
         double maxValue,
         CvEnum.THRESH thresholdType);

      /// <summary>
      /// Transforms grayscale image to binary image. 
      /// Threshold calculated individually for each pixel. 
      /// For the method CV_ADAPTIVE_THRESH_MEAN_C it is a mean of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel
      /// neighborhood, subtracted by param1. 
      /// For the method CV_ADAPTIVE_THRESH_GAUSSIAN_C it is a weighted sum (gaussian) of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel neighborhood, subtracted by param1.
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="adaptiveType">Adaptive_method </param>
      /// <param name="thresholdType">Thresholding type. must be one of CV_THRESH_BINARY, CV_THRESH_BINARY_INV  </param>
      /// <param name="blockSize">The size of a pixel neighborhood that is used to calculate a threshold value for the pixel: 3, 5, 7, ... </param>
      /// <param name="param1">Constant subtracted from mean or weighted mean. It may be negative. </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAdaptiveThreshold(
         IntPtr src,
         IntPtr dst,
         double maxValue,
         CvEnum.ADAPTIVE_THRESHOLD_TYPE adaptiveType,
         CvEnum.THRESH thresholdType,
         int blockSize,
         double param1);

      /// <summary>
      /// Retrieves contours from the binary image and returns the number of retrieved contours. The pointer firstContour is filled by the function. It will contain pointer to the first most outer contour or IntPtr.Zero if no contours is detected (if the image is completely black). Other contours may be reached from firstContour using h_next and v_next links. The sample in cvDrawContours discussion shows how to use contours for connected component detection. Contours can be also used for shape analysis and object recognition - see squares.c in OpenCV sample directory
      /// The function modifies the source image? content
      /// </summary>
      /// <param name="image">The source 8-bit single channel image. Non-zero pixels are treated as 1s, zero pixels remain 0s - that is image treated as binary. To get such a binary image from grayscale, one may use cvThreshold, cvAdaptiveThreshold or cvCanny. The function modifies the source image content</param>
      /// <param name="storage">Container of the retrieved contours</param>
      /// <param name="firstContour">Output parameter, will contain the pointer to the first outer contour</param>
      /// <param name="headerSize">Size of the sequence header, &gt;=sizeof(CvChain) if method=CV_CHAIN_CODE, and &gt;=sizeof(CvContour) otherwise</param>
      /// <param name="mode">Retrieval mode</param>
      /// <param name="method">Approximation method (for all the modes, except CV_RETR_RUNS, which uses built-in approximation). </param>
      /// <param name="offset">Offset, by which every contour point is shifted. This is useful if the contours are extracted from the image ROI and then they should be analyzed in the whole image context</param>
      /// <returns>The number of countours</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvFindContours(
         IntPtr image,
         IntPtr storage,
         ref IntPtr firstContour,
         int headerSize,
         CvEnum.RETR_TYPE mode,
         CvEnum.CHAIN_APPROX_METHOD method,
         Point offset);

      /// <summary>
      /// Initializes and returns a pointer to the contour scanner. The scanner is used in
      /// cvFindNextContour to retrieve the rest of the contours.
      /// </summary>
      /// <param name="image">The 8-bit, single channel, binary source image</param>
      /// <param name="storage">Container of the retrieved contours</param>
      /// <param name="headerSize">Size of the sequence header, &gt;=sizeof(CvChain) if method=CV_CHAIN_CODE, and &gt;=sizeof(CvContour) otherwise</param>
      /// <param name="mode">Retrieval mode</param>
      /// <param name="method">Approximation method (for all the modes, except CV_RETR_RUNS, which uses built-in approximation). </param>
      /// <param name="offset">Offset, by which every contour point is shifted. This is useful if the contours are extracted from the image ROI and then they should be analyzed in the whole image context</param>
      /// <returns>Pointer to the contour scaner</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvStartFindContours(
         IntPtr image,
         IntPtr storage,
         int headerSize,
         CvEnum.RETR_TYPE mode,
         CvEnum.CHAIN_APPROX_METHOD method,
         Point offset);

      /// <summary>
      /// Finds the next contour in the image
      /// </summary>
      /// <param name="scanner">Pointer to the contour scaner</param>
      /// <returns>The next contour in the image</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvFindNextContour(IntPtr scanner);

      /// <summary>
      /// The function replaces the retrieved contour, that was returned from the preceding call of
      /// cvFindNextContour and stored inside the contour scanner state, with the user-specified contour.
      /// The contour is inserted into the resulting structure, list, two-level hierarchy, or tree, depending on
      /// the retrieval mode. If the parameter new contour is IntPtr.Zero, the retrieved contour is not included
      /// in the resulting structure, nor are any of its children that might be added to this structure later.
      /// </summary>
      /// <param name="scanner">Contour scanner initialized by cvStartFindContours</param>
      /// <param name="newContour">Substituting contour</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSubstituteContour(
         IntPtr scanner,
         IntPtr newContour);

      /// <summary>
      /// Finishes the scanning process and returns a pointer to the first contour on the
      /// highest level.
      /// </summary>
      /// <param name="scanner">Reference to the contour scanner</param>
      /// <returns>pointer to the first contour on the highest level</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvEndFindContours(ref IntPtr scanner);

      /// <summary>
      /// Finds circles in grayscale image using some modification of Hough transform
      /// </summary>
      /// <param name="image">The input 8-bit single-channel grayscale image</param>
      /// <param name="circleStorage">The storage for the circles detected. It can be a memory storage (in this case a sequence of circles is created in the storage and returned by the function) or single row/single column matrix (CvMat*) of type CV_32FC3, to which the circles' parameters are written. The matrix header is modified by the function so its cols or rows will contain a number of lines detected. If circle_storage is a matrix and the actual number of lines exceeds the matrix size, the maximum possible number of circles is returned. Every circle is encoded as 3 floating-point numbers: center coordinates (x,y) and the radius</param>
      /// <param name="method">Currently, the only implemented method is CV_HOUGH_GRADIENT</param>
      /// <param name="dp">Resolution of the accumulator used to detect centers of the circles. For example, if it is 1, the accumulator will have the same resolution as the input image, if it is 2 - accumulator will have twice smaller width and height, etc</param>
      /// <param name="minDist">Minimum distance between centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed</param>
      /// <param name="param1">The first method-specific parameter. In case of CV_HOUGH_GRADIENT it is the higher threshold of the two passed to Canny edge detector (the lower one will be twice smaller). </param>
      /// <param name="param2">The second method-specific parameter. In case of CV_HOUGH_GRADIENT it is accumulator threshold at the center detection stage. The smaller it is, the more false circles may be detected. Circles, corresponding to the larger accumulator values, will be returned first</param>
      /// <param name="minRadius">Minimal radius of the circles to search for</param>
      /// <param name="maxRadius">Maximal radius of the circles to search for. By default the maximal radius is set to max(image_width, image_height). </param>
      /// <returns>Pointer to the sequence of circles</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvHoughCircles(
         IntPtr image,
         IntPtr circleStorage,
         CvEnum.HOUGH_TYPE method,
         double dp,
         double minDist,
         double param1,
         double param2,
         int minRadius,
         int maxRadius);

      /// <summary>
      /// Converts input image from one color space to another. The function ignores colorModel and channelSeq fields of IplImage header, so the source image color space should be specified correctly (including order of the channels in case of RGB space, e.g. BGR means 24-bit format with B0 G0 R0 B1 G1 R1 ... layout, whereas RGB means 24-bit format with R0 G0 B0 R1 G1 B1 ... layout). 
      /// </summary>
      /// <param name="src">The source 8-bit (8u), 16-bit (16u) or single-precision floating-point (32f) image</param>
      /// <param name="dst">The destination image of the same data type as the source one. The number of channels may be different</param>
      /// <param name="code">Color conversion operation that can be specifed using CV_src_color_space2dst_color_space constants </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCvtColor(IntPtr src, IntPtr dst, CvEnum.COLOR_CONVERSION code);

      /// <summary>
      /// The function cvHoughLines2 implements a few variants of Hough transform for line detection
      /// </summary>
      /// <param name="image">The input 8-bit single-channel binary image. In case of probabilistic method the image is modified by the function</param>
      /// <param name="lineStorage">The storage for the lines detected. It can be a memory storage (in this case a sequence of lines is created in the storage and returned by the function) or single row/single column matrix (CvMat*) of a particular type (see below) to which the lines' parameters are written. The matrix header is modified by the function so its cols or rows will contain a number of lines detected. If line_storage is a matrix and the actual number of lines exceeds the matrix size, the maximum possible number of lines is returned (in case of standard hough transform the lines are sorted by the accumulator value). </param>
      /// <param name="method">The Hough transform variant</param>
      /// <param name="rho">Distance resolution in pixel-related units</param>
      /// <param name="theta">Angle resolution measured in radians</param>
      /// <param name="threshold">Threshold parameter. A line is returned by the function if the corresponding accumulator value is greater than threshold</param>
      /// <param name="param1">The first method-dependent parameter:
      /// For classical Hough transform it is not used (0). 
      /// For probabilistic Hough transform it is the minimum line length. 
      /// For multi-scale Hough transform it is divisor for distance resolution rho. (The coarse distance resolution will be rho and the accurate resolution will be (rho / param1))
      /// </param>
      /// <param name="param2">The second method-dependent parameter:
      /// For classical Hough transform it is not used (0). 
      /// For probabilistic Hough transform it is the maximum gap between line segments lieing on the same line to treat them as the single line segment (i.e. to join them). 
      /// For multi-scale Hough transform it is divisor for angle resolution theta. (The coarse angle resolution will be theta and the accurate resolution will be (theta / param2)). 
      /// </param>
      /// <returns>Pointer to the decetected lines</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvHoughLines2(
         IntPtr image,
         IntPtr lineStorage,
         CvEnum.HOUGH_TYPE method,
         double rho,
         double theta,
         int threshold,
         double param1,
         double param2);

      /// <summary>
      /// Calculates spatial and central moments up to the third order and writes them to moments. The moments may be used then to calculate gravity center of the shape, its area, main axises and various shape characeteristics including 7 Hu invariants.
      /// </summary>
      /// <param name="arr">Image (1-channel or 3-channel with COI set) or polygon (CvSeq of points or a vector of points)</param>
      /// <param name="moments">Pointer to returned moment state structure</param>
      /// <param name="binary">(For images only) If the flag is non-zero, all the zero pixel values are treated as zeroes, all the others are treated as 1s</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMoments(IntPtr arr, ref MCvMoments moments, int binary);

      /// <summary>
      /// Finds corners with big eigenvalues in the image. 
      /// </summary>
      /// <remarks>
      /// The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. 
      /// Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). 
      /// The next step is rejecting the corners with the minimal eigenvalue less than quality_level*max(eigImage(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features.
      /// </remarks>
      /// <param name="image">The source 8-bit or floating-point 32-bit, single-channel image</param>
      /// <param name="eigImage">Temporary floating-point 32-bit image of the same size as image</param>
      /// <param name="tempImage">Another temporary image of the same size and same format as eig_image</param>
      /// <param name="corners">Output parameter. Detected corners</param>
      /// <param name="cornerCount">Output parameter. Number of detected corners</param>
      /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
      /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used</param>
      /// <param name="mask">Region of interest. The function selects points either in the specified region or in the whole image if the mask is IntPtr.Zero</param>
      /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
      /// <param name="useHarris">If nonzero, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal.</param>
      /// <param name="k">Free parameter of Harris detector; used only if <paramref name="useHarris"/> != 0</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGoodFeaturesToTrack(
          IntPtr image,
          IntPtr eigImage,
          IntPtr tempImage,
          IntPtr corners,
          ref int cornerCount,
          double qualityLevel,
          double minDistance,
          IntPtr mask,
          int blockSize,
          int useHarris,
          double k);

      /*
      /// <summary>
      /// Finds corners with big eigenvalues in the image. 
      /// </summary>
      /// <remarks>
      /// The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. 
      /// Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). 
      /// The next step is rejecting the corners with the minimal eigenvalue less than quality_level*max(eigImage(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features.
      /// </remarks>
      /// <param name="image">The source 8-bit or floating-point 32-bit, single-channel image</param>
      /// <param name="eigImage">Temporary floating-point 32-bit image of the same size as image</param>
      /// <param name="tempImage">Another temporary image of the same size and same format as eig_image</param>
      /// <param name="corners">Output parameter. Detected corners</param>
      /// <param name="cornerCount">Output parameter. Number of detected corners</param>
      /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
      /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used</param>
      /// <param name="mask">Region of interest. The function selects points either in the specified region or in the whole image if the mask is IntPtr.Zero</param>
      /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
      /// <param name="useHarris">If nonzero, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal.</param>
      /// <param name="k">Free parameter of Harris detector; used only if <paramref name="useHarris"/> != 0</param>
      [DllImport(CV_LIBRARY, CallingConvention=CvInvoke.CvCallingConvention)]
      public static extern void cvGoodFeaturesToTrack(
         IntPtr image,
         IntPtr eigImage,
         IntPtr tempImage,
         [Out]
         PointF[] corners,
         ref int cornerCount,
         double qualityLevel,
         double minDistance,
         IntPtr mask,
         int blockSize,
         int useHarris,
         double k);
      */

      /// <summary>
      /// This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMatchTemplate(
          IntPtr image,
          IntPtr templ,
          IntPtr result,
          CvEnum.TM_TYPE method);

      /// <summary>
      /// Compares two shapes. The 3 implemented methods all use Hu moments
      /// </summary>
      /// <param name="object1">First contour or grayscale image</param>
      /// <param name="object2">Second contour or grayscale image</param>
      /// <param name="method">Comparison method</param>
      /// <param name="parameter">Method-specific parameter (is not used now)</param>
      /// <returns>The result of the comparison</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvMatchShapes(
         IntPtr object1,
         IntPtr object2,
         CvEnum.CONTOURS_MATCH_TYPE method,
         double parameter);

      /// <summary>
      /// Creates an structuring element.
      /// </summary>
      /// <param name="cols">Number of columns in the structuring element.</param>
      /// <param name="rows">Number of rows in the structuring element.</param>
      /// <param name="anchorX">Relative horizontal offset of the anchor point.</param>
      /// <param name="anchorY">Relative vertical offset of the anchor point.</param>
      /// <param name="shape">Shape of the structuring element.</param>
      /// <param name="values">
      /// Pointer to the structuring element data, representing row-by-row scanning of the element matrix.
      /// Non-zero values indicate points that belong to the element.
      /// If the pointer is IntPtr.Zero, then all values are considered non-zero, that is, the element is of a rectangular shape.
      /// This parameter is considered only if the shape is CV_SHAPE_CUSTOM.
      /// </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateStructuringElementEx(
          int cols,
          int rows,
          int anchorX,
          int anchorY,
          CvEnum.CV_ELEMENT_SHAPE shape,
          IntPtr values);

      /// <summary>
      /// Releases the structuring element.
      /// </summary>
      /// <param name="ppElement">Pointer to the deallocated structuring element.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseStructuringElement(ref IntPtr ppElement);

      /// <summary>
      /// Performs advanced morphological transformations.
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="temp">
      /// Temporary image, required in some cases.
      /// The temporary image temp is required for morphological gradient and, in case of in-place operation, for "top hat" and "black hat".
      /// </param>
      /// <param name="element">Structuring element.</param>
      /// <param name="operation">Type of morphological operation.</param>
      /// <param name="iterations">Number of times erosion and dilation are applied.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMorphologyEx(
         IntPtr src,
         IntPtr dst,
         IntPtr temp,
         IntPtr element,
         CvEnum.CV_MORPH_OP operation,
         int iterations);

      #region Histograms
      /// <summary>
      /// Creates a histogram of the specified size and returns the pointer to the created histogram. If the array ranges is 0, the histogram bin ranges must be specified later via The function cvSetHistBinRanges, though cvCalcHist and cvCalcBackProject may process 8-bit images without setting bin ranges, they assume equally spaced in 0..255 bins
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="type">Histogram representation format: CV_HIST_ARRAY means that histogram data is represented as an multi-dimensional dense array CvMatND; CV_HIST_SPARSE means that histogram data is represented as a multi-dimensional sparse array CvSparseMat</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if != 0, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform == 0, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject</param>
      /// <returns>A pointer to the histogram</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateHist(
         int dims,
         [In]
         int[] sizes,
         CvEnum.HIST_TYPE type,
         [In]
         IntPtr[] ranges,
         int uniform);

      /// <summary>
      /// Finds the minimum and maximum histogram bins and their positions
      /// </summary>
      /// <remarks>
      /// Among several extremums with the same value the ones with minimum index (in lexicographical order). 
      /// In case of several maximums or minimums the earliest in lexicographical order extrema locations are returned.
      /// </remarks>
      /// <param name="hist">Histogram</param>
      /// <param name="minValue">Pointer to the minimum value of the histogram </param>
      /// <param name="maxValue">Pointer to the maximum value of the histogram </param>
      /// <param name="minIdx">Pointer to the array of coordinates for minimum </param>
      /// <param name="maxIdx">Pointer to the array of coordinates for maximum </param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetMinMaxHistValue(
         IntPtr hist,
         ref float minValue,
         ref float maxValue,
         int[] minIdx,
         int[] maxIdx);

      /// <summary>
      /// Normalizes the histogram bins by scaling them, such that the sum of the bins becomes equal to factor
      /// </summary>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="factor">Normalization factor</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvNormalizeHist(IntPtr hist, double factor);

      /// <summary>
      /// Clears histogram bins that are below the specified threshold
      /// </summary>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="threshold">Threshold level</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvThreshHist(IntPtr hist, double threshold);


      /// <summary>
      /// Sets all histogram bins to 0 in case of dense histogram and removes all histogram bins in case of sparse array
      /// </summary>
      /// <param name="hist">Histogram</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvClearHist(IntPtr hist);

      /// <summary>
      /// initializes the histogram, which header and bins are allocated by user. No cvReleaseHist need to be called afterwards. Only dense histograms can be initialized this way. 
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="data">The underline memory storage (pointer to array of float)</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if true, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform=false, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject
      /// </param>
      /// <returns>Pointer to the histogram</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvMakeHistHeaderForArray(
         int dims,
         [In] int[] sizes,
         IntPtr hist,
         IntPtr data,
         [In] IntPtr[] ranges,
         int uniform);

      /// <summary>
      /// Creates a histogram of the specified size and returns the pointer to the created histogram. If the array ranges is 0, the histogram bin ranges must be specified later via The function cvSetHistBinRanges, though cvCalcHist and cvCalcBackProject may process 8-bit images without setting bin ranges, they assume equally spaced in 0..255 bins
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="type">Histogram representation format: CV_HIST_ARRAY means that histogram data is represented as an multi-dimensional dense array CvMatND; CV_HIST_SPARSE means that histogram data is represented as a multi-dimensional sparse array CvSparseMat</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if true, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform=false, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject
      /// </param>
      /// <returns>A pointer to the histogram</returns>
      public static IntPtr cvCreateHist(
         int dims,
         [In]
         int[] sizes,
         CvEnum.HIST_TYPE type,
         [In]
         IntPtr[] ranges,
         bool uniform)
      {
         return cvCreateHist(dims, sizes, type, ranges, uniform ? 1 : 0);
      }

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcArrHist(
          IntPtr[] image,
          IntPtr hist,
          int accumulate,
          IntPtr mask);

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      public static void cvCalcArrHist(IntPtr[] image, IntPtr hist, bool accumulate, IntPtr mask)
      {
         cvCalcArrHist(image, hist, accumulate ? 1 : 0, mask);
      }

      /// <summary>
      /// Makes a copy of the histogram. If the second histogram pointer *dst is NULL, a new histogram of the same size as src is created. Otherwise, both histograms must have equal types and sizes. Then the function copies the source histogram bins values to destination histogram and sets the same bin values ranges as in src.
      /// </summary>
      /// <param name="src">The source histogram</param>
      /// <param name="dst">The destination histogram</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCopyHist(IntPtr src, ref IntPtr dst);

      /// <summary>
      /// Compares two dense histograms
      /// </summary>
      /// <param name="hist1">The first dense histogram. </param>
      /// <param name="hist2">The second dense histogram.</param>
      /// <param name="method">Comparison method</param>
      /// <returns>Result of the comparison</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvCompareHist(
         IntPtr hist1,
         IntPtr hist2,
         CvEnum.HISTOGRAM_COMP_METHOD method);

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      public static void cvCalcHist(
          IntPtr[] image,
          IntPtr hist,
          bool accumulate,
          IntPtr mask)
      {
         cvCalcArrHist(image, hist, accumulate ? 1 : 0, mask);
      }

      /// <summary>
      /// Calculates the back project of the histogram. 
      /// For each tuple of pixels at the same position of all input single-channel images the function puts the value of the histogram bin, corresponding to the tuple, to the destination image. 
      /// In terms of statistics, the value of each output image pixel is probability of the observed tuple given the distribution (histogram). 
      /// </summary>
      /// <example>
      /// To find a red object in the picture, one may do the following: 
      /// 1. Calculate a hue histogram for the red object assuming the image contains only this object. The histogram is likely to have a strong maximum, corresponding to red color. 
      /// 2. Calculate back projection of a hue plane of input image where the object is searched, using the histogram. Threshold the image. 
      /// 3. Find connected components in the resulting picture and choose the right component using some additional criteria, for example, the largest connected component. 
      /// That is the approximate algorithm of Camshift color object tracker, except for the 3rd step, instead of which CAMSHIFT algorithm is used to locate the object on the back projection given the previous object position. 
      /// </example>
      /// <param name="image">Source images (though you may pass CvMat** as well), all are of the same size and type </param>
      /// <param name="backProject">Destination back projection image of the same type as the source images</param>
      /// <param name="hist">Histogram</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcArrBackProject(IntPtr[] image, IntPtr backProject, IntPtr hist);

      /// <summary>
      /// The algorithm normalizes brightness and increases contrast of the image
      /// </summary>
      /// <param name="src">The input 8-bit single-channel image</param>
      /// <param name="dst">The output image of the same size and the same data type as src</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvEqualizeHist(IntPtr src, IntPtr dst);

      /// <summary>
      /// Calculates the back project of the histogram. 
      /// For each tuple of pixels at the same position of all input single-channel images the function puts the value of the histogram bin, corresponding to the tuple, to the destination image. 
      /// In terms of statistics, the value of each output image pixel is probability of the observed tuple given the distribution (histogram). 
      /// </summary>
      /// <example>
      /// To find a red object in the picture, one may do the following: 
      /// 1. Calculate a hue histogram for the red object assuming the image contains only this object. The histogram is likely to have a strong maximum, corresponding to red color. 
      /// 2. Calculate back projection of a hue plane of input image where the object is searched, using the histogram. Threshold the image. 
      /// 3. Find connected components in the resulting picture and choose the right component using some additional criteria, for example, the largest connected component. 
      /// That is the approximate algorithm of Camshift color object tracker, except for the 3rd step, instead of which CAMSHIFT algorithm is used to locate the object on the back projection given the previous object position. 
      /// </example>
      /// <param name="image">Source images (though you may pass CvMat** as well), all are of the same size and type </param>
      /// <param name="backProject">Destination back projection image of the same type as the source images</param>
      /// <param name="hist">Histogram</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvCalcArrBackProject")]
      public static extern void cvCalcBackProject(
         IntPtr[] image,
         IntPtr backProject,
         IntPtr hist);

      /// <summary>
      /// Compares histogram, computed over each possible rectangular patch of the specified size in the input images, and stores the results to the output map dst.
      /// </summary>
      /// <remarks>In pseudo-code the operation may be written as:
      ///for (x,y) in images (until (x+patch_size.width-1,y+patch_size.height-1) is inside the images) do
      ///    compute histogram over the ROI (x,y,x+patch_size.width,y+patch_size.height) in images
      ///       (see cvCalcHist)
      ///    normalize the histogram using the factor
      ///       (see cvNormalizeHist)
      ///    compare the normalized histogram with input histogram hist using the specified method
      ///       (see cvCompareHist)
      ///    store the result to dst(x,y)
      ///end for
      ///</remarks>
      /// <param name="images">Source images (though, you may pass CvMat** as well), all of the same size</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="patchSize">Size of patch slid though the source images. </param>
      /// <param name="hist">Histogram </param>
      /// <param name="method">Comparison methof</param>
      /// <param name="factor">Normalization factor for histograms, will affect normalization scale of destination image, pass 1. if unsure.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvCalcArrBackProjectPatch")]
      public static extern void cvCalcBackProjectPatch(
         IntPtr[] images,
         IntPtr dst,
         Size patchSize,
         IntPtr hist,
         CvEnum.HISTOGRAM_COMP_METHOD method,
         double factor);

      /// <summary>
      /// calculates the object probability density from the two histograms as:
      /// dist_hist(I)=0,      if hist1(I)==0;
      /// dist_hist(I)=scale,  if hist1(I)!=0 &amp;&amp; hist2(I)&gt;hist1(I);
      /// dist_hist(I)=hist2(I)*scale/hist1(I), if hist1(I)!=0 &amp;&amp; hist2(I)&lt;=hist1(I)
      /// </summary>
      /// <param name="hist1">First histogram (the divisor)</param>
      /// <param name="hist2">Second histogram.</param>
      /// <param name="dstHist">Destination histogram. </param>
      /// <param name="scale">Scale factor for the destination histogram.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcProbDensity(
         IntPtr hist1,
         IntPtr hist2,
         IntPtr dstHist,
         double scale);

      /// <summary>
      /// Releases the histogram (header and the data). 
      /// The pointer to histogram is cleared by the function. 
      /// If *hist pointer is already NULL, the function does nothing.
      /// </summary>
      /// <param name="hist">Double pointer to the released histogram</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseHist(ref IntPtr hist);
      #endregion

      /// <summary>
      /// Retrieves the spatial moment, which in case of image moments is defined as:
      /// M_{x_order,y_order}=sum_{x,y}(I(x,y) * x^{x_order} * y^{y_order})
      /// where I(x,y) is the intensity of the pixel (x, y). 
      /// </summary>
      /// <param name="moments">The moment state</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0. </param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The spatial moment</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetSpatialMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      /// <summary>
      /// Retrieves the central moment, which in case of image moments is defined as:
      /// mu_{x_order,y_order}=sum_{x,y}(I(x,y)*(x-x_c)^{x_order} * (y-y_c)^{y_order}),
      /// where x_c=M10/M00, y_c=M01/M00 - coordinates of the gravity center
      /// </summary>
      /// <param name="moments">Reference to the moment state structure</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0.</param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The center moment</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetCentralMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      /// <summary>
      /// Retrieves normalized central moment, which in case of image moments is defined as:
      /// eta_{x_order,y_order}=mu_{x_order,y_order} / M00^{(y_order+x_order)/2+1},
      /// where mu_{x_order,y_order} is the central moment
      /// </summary>
      /// <param name="moments">Reference to the moment state structure</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0.</param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The normalized center moment</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetNormalizedCentralMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      #region Accumulation of Background Statistics
      /// <summary>
      /// Adds the whole image or its selected region to accumulator sum
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point. (each channel of multi-channel image is processed independently). </param>
      /// <param name="sum">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point. </param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvAcc(IntPtr image, IntPtr sum, IntPtr mask);

      /// <summary>
      /// Adds the input <paramref name="image"/> or its selected region, raised to power 2, to the accumulator sqsum
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="sqsum">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSquareAcc(IntPtr image, IntPtr sqsum, IntPtr mask);

      /// <summary>
      /// Adds product of 2 images or thier selected regions to accumulator acc
      /// </summary>
      /// <param name="image1">First input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="image2">Second input image, the same format as the first one</param>
      /// <param name="acc">Accumulator of the same number of channels as input images, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvMultiplyAcc(IntPtr image1, IntPtr image2, IntPtr acc, IntPtr mask);

      /// <summary>
      /// Calculates weighted sum of input <paramref name="image"/> and the accumulator acc so that acc becomes a running average of frame sequence:
      /// acc(x,y)=(1-<paramref name="alpha"/>) * acc(x,y) + <paramref name="alpha"/> * image(x,y) if mask(x,y)!=0
      /// where <paramref name="alpha"/> regulates update speed (how fast accumulator forgets about previous frames). 
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently). </param>
      /// <param name="acc">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point. </param>
      /// <param name="alpha">Weight of input image</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvRunningAvg(IntPtr image, IntPtr acc, double alpha, IntPtr mask);
      #endregion

      /// <summary>
      /// Calculates seven Hu invariants
      /// </summary>
      /// <param name="moments">Pointer to the moment state structure</param>
      /// <param name="huMoments">Pointer to Hu moments structure.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetHuMoments(ref MCvMoments moments, ref MCvHuMoments huMoments);

      /// <summary>
      /// Runs the Harris edge detector on image. Similarly to cvCornerMinEigenVal and cvCornerEigenValsAndVecs, for each pixel it calculates 2x2 gradient covariation matrix M over block_size x block_size neighborhood. Then, it stores
      /// det(M) - k*trace(M)^2
      /// to the destination image. Corners in the image can be found as local maxima of the destination image.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="harrisResponce">Image to store the Harris detector responces. Should have the same size as image </param>
      /// <param name="blockSize">Neighborhood size </param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator (see cvSobel). format. In the case of floating-point input format this parameter is the number of the fixed float filter used for differencing. </param>
      /// <param name="k">Harris detector free parameter.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCornerHarris(
          IntPtr image,
          IntPtr harrisResponce,
          int blockSize,
          int apertureSize,
          double k);

      /// <summary>
      /// Iterates to find the sub-pixel accurate location of corners, or radial saddle points
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="corners">Initial coordinates of the input corners and refined coordinates on output</param>
      /// <param name="count">Number of corners</param>
      /// <param name="win">Half sizes of the search window. For example, if win=(5,5) then 5*2+1 x 5*2+1 = 11 x 11 search window is used</param>
      /// <param name="zeroZone">Half size of the dead region in the middle of the search zone over which the summation in formulae below is not done. It is used sometimes to avoid possible singularities of the autocorrelation matrix. The value of (-1,-1) indicates that there is no such size</param>
      /// <param name="criteria">Criteria for termination of the iterative process of corner refinement. That is, the process of corner position refinement stops either after certain number of iteration or when a required accuracy is achieved. The criteria may specify either of or both the maximum number of iteration and the required accuracy</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindCornerSubPix(
         IntPtr image,
         IntPtr corners,
         int count,
         Size win,
         Size zeroZone,
         MCvTermCriteria criteria);

      /// <summary>
      /// Iterates to find the sub-pixel accurate location of corners, or radial saddle points
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="corners">Initial coordinates of the input corners and refined coordinates on output</param>
      /// <param name="count">Number of corners</param>
      /// <param name="win">Half sizes of the search window. For example, if win=(5,5) then 5*2+1 x 5*2+1 = 11 x 11 search window is used</param>
      /// <param name="zeroZone">Half size of the dead region in the middle of the search zone over which the summation in formulae below is not done. It is used sometimes to avoid possible singularities of the autocorrelation matrix. The value of (-1,-1) indicates that there is no such size</param>
      /// <param name="criteria">Criteria for termination of the iterative process of corner refinement. That is, the process of corner position refinement stops either after certain number of iteration or when a required accuracy is achieved. The criteria may specify either of or both the maximum number of iteration and the required accuracy</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindCornerSubPix(
         IntPtr image,
         [In, Out]
         PointF[] corners,
         int count,
         Size win,
         Size zeroZone,
         MCvTermCriteria criteria);

      /// <summary>
      /// Calculates one or more integral images for the source image 
      /// Using these integral images, one may calculate sum, mean, standard deviation over arbitrary up-right or rotated rectangular region of the image in a constant time.
      /// It makes possible to do a fast blurring or fast block correlation with variable window size etc. In case of multi-channel images sums for each channel are accumulated independently. 
      /// </summary>
      /// <param name="image">The source image, WxH, 8-bit or floating-point (32f or 64f) image.</param>
      /// <param name="sum">The integral image, W+1xH+1, 32-bit integer or double precision floating-point (64f). </param>
      /// <param name="sqsum">The integral image for squared pixel values, W+1xH+1, double precision floating-point (64f). </param>
      /// <param name="tiltedSum">The integral for the image rotated by 45 degrees, W+1xH+1, the same data type as sum.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvIntegral(
         IntPtr image,
         IntPtr sum,
         IntPtr sqsum,
         IntPtr tiltedSum);

      /// <summary>
      /// Calculates distance to closest zero pixel for all non-zero pixels of source image
      /// </summary>
      /// <param name="src">Source 8-bit single-channel (binary) image.</param>
      /// <param name="dst">Output image with calculated distances (32-bit floating-point, single-channel). </param>
      /// <param name="distanceType">Type of distance</param>
      /// <param name="maskSize">Size of distance transform mask; can be 3 or 5.
      /// In case of CV_DIST_L1 or CV_DIST_C the parameter is forced to 3, because 3x3 mask gives the same result as 5x5 yet it is faster.</param>
      /// <param name="userMask">User-defined mask in case of user-defined distance.
      /// It consists of 2 numbers (horizontal/vertical shift cost, diagonal shift cost) in case of 3x3 mask
      /// and 3 numbers (horizontal/vertical shift cost, diagonal shift cost, knights move cost) in case of 5x5 mask.</param>
      /// <param name="labels">The optional output 2d array of labels of integer type and the same size as src and dst.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDistTransform(
         IntPtr src,
         IntPtr dst,
         CvEnum.DIST_TYPE distanceType,
         int maskSize,
         float[] userMask,
         IntPtr labels);

      /// <summary>
      /// Fills a connected component with given color.
      /// </summary>
      /// <param name="src">Input 1- or 3-channel, 8-bit or floating-point image. It is modified by the function unless CV_FLOODFILL_MASK_ONLY flag is set.</param>
      /// <param name="seedPoint">The starting point.</param>
      /// <param name="newVal">New value of repainted domain pixels.</param>
      /// <param name="loDiff">Maximal lower brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="upDiff">Maximal upper brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="comp">Pointer to structure the function fills with the information about the repainted domain.</param>
      /// <param name="flags">The operation flags.
      /// Lower bits contain connectivity value, 4 (by default) or 8, used within the function.
      /// Connectivity determines which neighbors of a pixel are considered.
      /// Upper bits can be 0 or combination of the following flags:
      /// CV_FLOODFILL_FIXED_RANGE - if set the difference between the current pixel and seed pixel is considered,
      /// otherwise difference between neighbor pixels is considered (the range is floating).
      /// CV_FLOODFILL_MASK_ONLY - if set, the function does not fill the image (new_val is ignored),
      /// but the fills mask (that must be non-NULL in this case). </param>
      /// <param name="mask">Operation mask,
      /// should be singe-channel 8-bit image, 2 pixels wider and 2 pixels taller than image.
      /// If not IntPtr.Zero, the function uses and updates the mask, so user takes responsibility of initializing mask content.
      /// Floodfilling can't go across non-zero pixels in the mask, for example, an edge detector output can be used as a mask to stop filling at edges.
      /// Or it is possible to use the same mask in multiple calls to the function to make sure the filled area do not overlap.
      /// Note: because mask is larger than the filled image, pixel in mask that corresponds to (x,y) pixel in image will have coordinates (x+1,y+1).</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFloodFill(
         IntPtr src,
         Point seedPoint,
         MCvScalar newVal,
         MCvScalar loDiff,
         MCvScalar upDiff,
         out MCvConnectedComp comp,
         int flags,
         IntPtr mask);

      /// <summary>
      /// Fills a connected component with given color.
      /// </summary>
      /// <param name="src">Input 1- or 3-channel, 8-bit or floating-point image. It is modified by the function unless CV_FLOODFILL_MASK_ONLY flag is set.</param>
      /// <param name="seedPoint">The starting point.</param>
      /// <param name="newVal">New value of repainted domain pixels.</param>
      /// <param name="loDiff">Maximal lower brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="upDiff">Maximal upper brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="comp">Pointer to structure the function fills with the information about the repainted domain.</param>
      /// <param name="mask">Operation mask,
      /// should be singe-channel 8-bit image, 2 pixels wider and 2 pixels taller than image.
      /// If not IntPtr.Zero, the function uses and updates the mask, so user takes responsibility of initializing mask content.
      /// Floodfilling can't go across non-zero pixels in the mask, for example, an edge detector output can be used as a mask to stop filling at edges.
      /// Or it is possible to use the same mask in multiple calls to the function to make sure the filled area do not overlap.
      /// Note: because mask is larger than the filled image, pixel in mask that corresponds to (x,y) pixel in image will have coordinates (x+1,y+1).</param>
      /// <param name="connectivity">The connectivity of flood fill</param>
      /// <param name="flags">The flood fill types</param>
      public static void cvFloodFill(
         IntPtr src,
         Point seedPoint,
         MCvScalar newVal,
         MCvScalar loDiff,
         MCvScalar upDiff,
         out MCvConnectedComp comp,
         CvEnum.CONNECTIVITY connectivity,
         CvEnum.FLOODFILL_FLAG flags,
         IntPtr mask)
      {
         cvFloodFill(src, seedPoint, newVal, loDiff, upDiff, out comp, (int)connectivity | (int)flags, mask);
      }

      /// <summary>
      /// Filters image using meanshift algorithm
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Result image</param>
      /// <param name="sp"></param>
      /// <param name="sr"></param>
      /// <param name="max_level">Use 1 as default value</param>
      /// <param name="termcrit">Use new MCvTermCriteria(5, 1) as default value</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPyrMeanShiftFiltering(
         IntPtr src, IntPtr dst,
         double sp, double sr, int max_level,
         MCvTermCriteria termcrit);

      #region image undistortion
      /// <summary>
      /// Transforms the image to compensate radial and tangential lens distortion. The camera matrix and distortion parameters can be determined using cvCalibrateCamera2. For every pixel in the output image the function computes coordinates of the corresponding location in the input image using the formulae in the section beginning. Then, the pixel value is computed using bilinear interpolation. If the resolution of images is different from what was used at the calibration stage, fx, fy, cx and cy need to be adjusted appropriately, while the distortion coefficients remain the same.
      /// </summary>
      /// <param name="src">The input (distorted) image</param>
      /// <param name="dst">The output (corrected) image</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1].</param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2].</param>
      /// <param name="newIntrinsicMatrix">Camera matrix of the distorted image. By default it is the same as cameraMatrix, but you may additionally scale and shift the result by using some different matrix</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvUndistort2(
          IntPtr src,
          IntPtr dst,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs,
          IntPtr newIntrinsicMatrix);

      /// <summary>
      /// Pre-computes the undistortion map - coordinates of the corresponding pixel in the distorted image for every pixel in the corrected image. Then, the map (together with input and output images) can be passed to cvRemap function. 
      /// </summary>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. </param>
      /// <param name="mapx">The output array of x-coordinates of the map</param>
      /// <param name="mapy">The output array of y-coordinates of the map</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInitUndistortMap(
         IntPtr intrinsicMatrix,
         IntPtr distortionCoeffs,
         IntPtr mapx,
         IntPtr mapy);

      /// <summary>
      /// This function is an extended version of cvInitUndistortMap. That is, in addition to the correction of lens distortion, the function can also apply arbitrary perspective transformation R and finally it can scale and shift the image according to the new camera matrix
      /// </summary>
      /// <param name="cameraMatrix">The camera matrix A=[fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="distCoeffs">The vector of distortion coefficients, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="R">The rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used</param>
      /// <param name="newCameraMatrix">The new camera matrix A'=[fx' 0 cx'; 0 fy' cy'; 0 0 1]</param>
      /// <param name="mapx">The output array of x-coordinates of the map</param>
      /// <param name="mapy">The output array of y-coordinates of the map</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInitUndistortRectifyMap(
         IntPtr cameraMatrix,
         IntPtr distCoeffs,
         IntPtr R,
         IntPtr newCameraMatrix,
         IntPtr mapx,
         IntPtr mapy);

      /// <summary>
      /// Similar to cvInitUndistortRectifyMap and is opposite to it at the same time. 
      /// The functions are similar in that they both are used to correct lens distortion and to perform the optional perspective (rectification) transformation. 
      /// They are opposite because the function cvInitUndistortRectifyMap does actually perform the reverse transformation in order to initialize the maps properly, while this function does the forward transformation. 
      /// </summary>
      /// <param name="src">The observed point coordinates</param>
      /// <param name="dst">The ideal point coordinates, after undistortion and reverse perspective transformation. </param>
      /// <param name="camera_matrix">The camera matrix A=[fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="dist_coeffs">The vector of distortion coefficients, 4x1, 1x4, 5x1 or 1x5. </param>
      /// <param name="R">The rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used.</param>
      /// <param name="P">The new camera matrix (3x3) or the new projection matrix (3x4). P1 or P2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used.</param>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvUndistortPoints(
         IntPtr src,
         IntPtr dst,
         IntPtr camera_matrix,
         IntPtr dist_coeffs,
         IntPtr R,
         IntPtr P);
      #endregion

      #region CalcEMD
      /// <summary>
      /// Wrapped Opencv's CvDistanceFunction
      /// </summary>
      /// <param name="f1">Pointer to an array of float</param>
      /// <param name="f2">Pointer to an array of float</param>
      /// <param name="userParams">User passed parameters</param>
      /// <returns>The distance between f1 and f2</returns>
      public delegate float CvDistanceFunction(IntPtr f1, IntPtr f2, IntPtr userParams);

      /// <summary>
      /// Computes earth mover distance and/or a lower boundary of the distance
      /// between the two weighted point configurations. One of the application
      /// described in [RubnerSept98] is multi-dimensional histogram comparison
      /// for image retrieval. EMD is a transportation problem that is solved
      /// using some modification of simplex algorithm, thus the complexity is
      /// exponential in the worst case, though, it is much faster in average.
      /// In case of a real metric the lower boundary can be calculated even
      /// faster (using linear-time algorithm) and it can be used to determine
      /// roughly whether the two signatures are far enough so that they cannot
      /// relate to the same object.
      /// </summary>
      /// <param name="signature1">
      /// First signature, size1*dims+1 floating-point matrix. Each row stores the point weight followed by the point coordinates. The matrix is allowed to have a single column (weights only) if the user-defined cost matrix is used.
      /// </param>
      /// <param name="signature2">Second signature of the same format as signature1, though the number of rows may be different. The total weights may be different, in this case an extra "dummy" point is added to either signature1 or signature2. </param>
      /// <param name="distType">Metrics used; CV_DIST_L1, CV_DIST_L2, and CV_DIST_C stand for one of the standard metrics; CV_DIST_USER means that a user-defined function distance_func or pre-calculated cost_matrix is used. </param>
      /// <param name="distFunc">The user-defined distance function. It takes coordinates of two points and returns the distance between the points.</param>
      /// <param name="costMatrix">The user-defined size1*size2 cost matrix. At least one of cost_matrix and distance_func must be NULL. Also, if a cost matrix is used, lower boundary (see below) can not be calculated, because it needs a metric function.</param>
      /// <param name="flow">The resultant size1*size2 flow matrix: flow,,ij,, is a flow from i-th point of signature1 to j-th point of signature2</param>
      /// <param name="lowerBound">
      /// Optional input/output parameter: lower boundary of
      /// distance between the two signatures that is a distance between mass centers.
      /// The lower boundary may not be calculated if the user-defined cost matrix
      /// is used, the total weights of point configurations are not equal, or there
      /// is the signatures consist of weights only (i.e. the signature matrices have
      /// a single column). User must initialize *lower_bound. If the calculated
      /// distance between mass centers is greater or equal to *lower_bound
      /// (it means that the signatures are far enough) the function does not
      /// calculate EMD. In any case *lower_bound is set to the calculated
      /// distance between mass centers on return. Thus, if user wants to
      /// calculate both distance between mass centers and EMD, *lower_bound should
      /// be set to 0.
      /// </param>
      /// <param name="userParam">Pointer to optional data that is passed into the user-defined distance function. </param>
      /// <returns>"minimal work" distance between two weighted point configurations</returns>
      [DllImport(OPENCV_IMGPROC_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float cvCalcEMD2(
         IntPtr signature1,
         IntPtr signature2,
         CvEnum.DIST_TYPE distType,
         CvDistanceFunction distFunc,
         IntPtr costMatrix,
         IntPtr flow,
         IntPtr lowerBound,
         IntPtr userParam);
      #endregion

      /*
              /// <summary>
              ///  Fits a line into set of 2d points in a robust way (M-estimator technique) 
              /// </summary>
              public static void cvFitLine2D(IntPtr points, int count, int dist,
                                    float param, float reps, float aeps, IntPtr line)
              {
                  MCvMat mat = cvMat(1, count, CV_MAKETYPE((int)MAT_DEPTH.CV_32F, 2), points);
                  //float _param = (param != IntPtr.Zero )? *(float*)param : 0.f;
                  //assert( dist != CV_DIST_USER );
                  IntPtr l = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 1000);
                  cvFitLine(mat, dist, param, reps, aeps, line);
                  Marshal.FreeHGlobal(l);
              }
      */
   }
}
