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
      [DllImport(CV_LIBRARY)]
      public static extern int cvSampleLine(IntPtr image, Point pt1, Point pt2, IntPtr buffer, CvEnum.CONNECTIVITY connectivity);

      /// <summary>
      /// Extracts pixels from src:
      /// dst(x, y) = src(x + center.x - (width(dst)-1)*0.5, y + center.y - (height(dst)-1)*0.5)
      /// where the values of pixels at non-integer coordinates are retrieved using bilinear interpolation. Every channel of multiple-channel images is processed independently. Whereas the rectangle center must be inside the image, the whole rectangle may be partially occluded. In this case, the replication border mode is used to get pixel values beyond the image boundaries.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Extracted rectangle</param>
      /// <param name="center">Floating point coordinates of the extracted rectangle center within the source image. The center must be inside the image.</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvGetQuadrangleSubPix(IntPtr src, IntPtr dst, IntPtr mapMatrix);

      /// <summary>
      /// Resizes image src so that it fits exactly to dst. If ROI is set, the function consideres the ROI as supported as usual
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image</param>
      /// <param name="interpolation">Interpolation method</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvResize(IntPtr src, IntPtr dst, CvEnum.INTER interpolation);

      /// <summary>
      /// Transforms source image using the specified matrix
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapMatrix">2x3 transformation matrix</param>
      /// <param name="flags"> flags </param>
      /// <param name="fillval">A value used to fill outliers</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvLinearPolar(
         IntPtr src,
         IntPtr dst,
         PointF center,
         double maxRadius,
         int flags);
      #endregion

      /// <summary>
      /// Finds all the motion segments and marks them in segMask with individual values each (1,2,...). It also returns a sequence of CvConnectedComp structures, one per each motion components. After than the motion direction for every component can be calculated with cvCalcGlobalOrientation using extracted mask of the particular component (using cvCmp) 
      /// </summary>
      /// <param name="mhi">Motion history image</param>
      /// <param name="segMask">Image where the mask found should be stored, single-channel, 32-bit floating-point</param>
      /// <param name="storage">Memory storage that will contain a sequence of motion connected components</param>
      /// <param name="timestamp">Current time in milliseconds or other units</param>
      /// <param name="segThresh">Segmentation threshold; recommended to be equal to the interval between motion history "steps" or greater</param>
      /// <returns>Pointer to the sequence of MCvConnectedComp</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvSegmentMotion(
          IntPtr mhi,
          IntPtr segMask,
          IntPtr storage,
          double timestamp,
          double segThresh);

      /// <summary>
      /// Calculates the general motion direction in the selected region and returns the angle between 0 and 360. At first the function builds the orientation histogram and finds the basic orientation as a coordinate of the histogram maximum. After that the function calculates the shift relative to the basic orientation as a weighted sum of all orientation vectors: the more recent is the motion, the greater is the weight. The resultant angle is a circular sum of the basic orientation and the shift. 
      /// </summary>
      /// <param name="orientation">Motion gradient orientation image; calculated by the function cvCalcMotionGradient.</param>
      /// <param name="mask">Mask image. It may be a conjunction of valid gradient mask, obtained with cvCalcMotionGradient and mask of the region, whose direction needs to be calculated. </param>
      /// <param name="mhi">Motion history image.</param>
      /// <param name="timestamp">Current time in milliseconds or other units, it is better to store time passed to cvUpdateMotionHistory before and reuse it here, because running cvUpdateMotionHistory and cvCalcMotionGradient on large images may take some time.</param>
      /// <param name="duration">Maximal duration of motion track in milliseconds, the same as in cvUpdateMotionHistory</param>
      /// <returns>The angle</returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvCalcGlobalOrientation(
                  IntPtr orientation,
                  IntPtr mask,
                  IntPtr mhi,
                  double timestamp,
                  double duration);

      /// <summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. First it convolves source image with the specified filter and then downsamples the image by rejecting even rows and columns.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="filter">Type of the filter used for convolution; only CV_GAUSSIAN_5x5 is currently supported.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvPyrDown(IntPtr src, IntPtr dst, CvEnum.FILTER_TYPE filter);

      /// <summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition. First it upsamples the source image by injecting even zero rows and columns and then convolves result with the specified filter multiplied by 4 for interpolation. So the destination image is four times larger than the source image.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="filter">Type of the filter used for convolution; only CV_GAUSSIAN_5x5 is currently supported.</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvWatershed(IntPtr image, IntPtr markers);

      #region Computational Geometry
      /// <summary>
      /// Finds minimum area rectangle that contains both input rectangles inside
      /// </summary>
      /// <param name="rect1">First rectangle </param>
      /// <param name="rect2">Second rectangle </param>
      /// <returns>The minimum area rectangle that contains both input rectangles inside</returns>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvBoxPoints(
         MCvBox2D box,
         [Out]
         float[] pt);

      /// <summary>
      /// Calculates vertices of the input 2d box.
      /// </summary>
      /// <param name="box">The box</param>
      /// <param name="pt">An array of size 4 points</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvBoxPoints(
         MCvBox2D box,
         [Out]
         PointF[] pt);

      /// <summary>
      /// Calculates ellipse that fits best (in least-squares sense) to a set of 2D points. The meaning of the returned structure fields is similar to those in cvEllipse except that size stores the full lengths of the ellipse axises, not half-lengths
      /// </summary>
      /// <param name="points">Sequence or array of points</param>
      /// <returns>The ellipse that fits best (in least-squares sense) to a set of 2D points</returns>
      [DllImport(CV_LIBRARY)]
      public static extern MCvBox2D cvFitEllipse2(IntPtr points);

      /// <summary>
      /// The function cvConvexHull2 finds convex hull of 2D point set using Sklansky's algorithm. 
      /// </summary>
      /// <param name="input">Sequence or array of 2D points with 32-bit integer or floating-point coordinates</param>
      /// <param name="hullStorage">The destination array (CvMat*) or memory storage (CvMemStorage*) that will store the convex hull. If it is array, it should be 1d and have the same number of elements as the input array/sequence. On output the header is modified so to truncate the array downto the hull size</param>
      /// <param name="orientation">Desired orientation of convex hull: CV_CLOCKWISE or CV_COUNTER_CLOCKWISE</param>
      /// <param name="returnPoints">If non-zero, the points themselves will be stored in the hull instead of indices if hull_storage is array, or pointers if hull_storage is memory storage</param>
      /// <returns>If hull_storage is memory storage, the function creates a sequence containing the hull points or pointers to them, depending on return_points value and returns the sequence on output</returns>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvInitSubdivDelaunay2D(IntPtr subdiv, Rectangle rect);

      /// <summary>
      /// Locates input point within subdivision. It finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point. 
      /// </summary>
      /// <param name="subdiv">Delaunay or another subdivision</param>
      /// <param name="pt">Input point</param>
      /// <returns>pointer to the found subdivision vertex (CvSubdiv2DPoint)</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvFindNearestPoint2D(IntPtr subdiv, PointF pt);

      /// <summary>
      /// Creates new subdivision
      /// </summary>
      /// <param name="subdiv_type"></param>
      /// <param name="header_size"></param>
      /// <param name="vtx_size"></param>
      /// <param name="quadedge_size"></param>
      /// <param name="storage"></param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateSubdiv2D(
          int subdiv_type,
          int header_size,
          int vtx_size,
          int quadedge_size,
          IntPtr storage);

      /// <summary>
      /// Inserts a single point to subdivision and modifies the subdivision topology appropriately. If a points with same coordinates exists already, no new points is added. The function returns pointer to the allocated point. No virtual points coordinates is calculated at this stage.
      /// </summary>
      /// <param name="subdiv">Delaunay subdivision created by function cvCreateSubdivDelaunay2D</param>
      /// <param name="pt">Inserted point.</param>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvSubdivDelaunay2DInsert(IntPtr subdiv, PointF pt);

      /// <summary>
      /// Locates input point within subdivision
      /// </summary>
      /// <param name="subdiv">Plannar subdivision</param>
      /// <param name="pt">The point to locate</param>
      /// <param name="edge">The output edge the point falls onto or right to</param>
      /// <param name="vertex">Optional output vertex double pointer the input point coincides with</param>
      /// <returns>The type of location for the point</returns>
      [DllImport(CV_LIBRARY)]
      public static extern CvEnum.Subdiv2DPointLocationType cvSubdiv2DLocate(IntPtr subdiv, PointF pt,
                                           out IntPtr edge,
                                           ref IntPtr vertex);

      /// <summary>
      /// Calculates coordinates of virtual points. All virtual points corresponding to some vertex of original subdivision form (when connected together) a boundary of Voronoi cell of that point
      /// </summary>
      /// <param name="subdiv">Delaunay subdivision, where all the points are added already</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcSubdivVoronoi2D(IntPtr subdiv);

      #endregion

      #region Pose Estimation
      /// <summary>
      /// Allocates memory for the object structure and computes the object inverse matrix. 
      /// </summary>
      /// <remarks>The preprocessed object data is stored in the structure CvPOSITObject, internal for OpenCV, which means that the user cannot directly access the structure data. The user may only create this structure and pass its pointer to the function. 
      /// Object is defined as a set of points given in a coordinate system. The function cvPOSIT computes a vector that begins at a camera-related coordinate system center and ends at the points[0] of the object. 
      /// Once the work with a given object is finished, the function cvReleasePOSITObject must be called to free memory</remarks>
      /// <param name="points3D">A two dimensional array contains the points of the 3D object model, the second dimension must be 3. </param>
      /// <param name="pointCount">Number of object points</param>
      /// <returns>A pointer to the CvPOSITObject</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreatePOSITObject(float[,] points3D, int pointCount);

      /// <summary>
      /// Implements POSIT algorithm. Image coordinates are given in a camera-related coordinate system. The focal length may be retrieved using camera calibration functions. At every iteration of the algorithm new perspective projection of estimated pose is computed. 
      /// </summary>
      /// <remarks>Difference norm between two projections is the maximal distance between corresponding points. </remarks>
      /// <param name="positObject">Pointer to the object structure</param>
      /// <param name="imagePoints">2D array to the object points projections on the 2D image plane, the second dimension must be 2.</param>
      /// <param name="focalLength">Focal length of the camera used</param>
      /// <param name="criteria">Termination criteria of the iterative POSIT algorithm. The parameter criteria.epsilon serves to stop the algorithm if the difference is small.</param>
      /// <param name="rotationMatrix">A vector which contains the 9 elements of the 3x3 rotation matrix</param>
      /// <param name="translationVector">Translation vector (3x1)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvPOSIT(IntPtr positObject, float[,] imagePoints, double focalLength,
              MCvTermCriteria criteria, float[] rotationMatrix, float[] translationVector);

      /// <summary>
      /// Implements POSIT algorithm. Image coordinates are given in a camera-related coordinate system. The focal length may be retrieved using camera calibration functions. At every iteration of the algorithm new perspective projection of estimated pose is computed. 
      /// </summary>
      /// <remarks>Difference norm between two projections is the maximal distance between corresponding points. </remarks>
      /// <param name="positObject">Pointer to the object structure</param>
      /// <param name="imagePoints">2D array to the object points projections on the 2D image plane, the second dimension must be 2.</param>
      /// <param name="focalLength">Focal length of the camera used</param>
      /// <param name="criteria">Termination criteria of the iterative POSIT algorithm. The parameter criteria.epsilon serves to stop the algorithm if the difference is small.</param>
      /// <param name="rotationMatrix">A vector which contains the 9 elements of the 3x3 rotation matrix</param>
      /// <param name="translationVector">Translation vector (3x1)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvPOSIT(IntPtr positObject, IntPtr imagePoints, double focalLength,
              MCvTermCriteria criteria, IntPtr rotationMatrix, IntPtr translationVector);

      /// <summary>
      /// The function cvReleasePOSITObject releases memory previously allocated by the function cvCreatePOSITObject. 
      /// </summary>
      /// <param name="positObject">pointer to CvPOSIT structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleasePOSITObject(ref IntPtr positObject);
      #endregion

      #region Feature Matching
      /// <summary>
      /// Constructs a balanced kd-tree index of the given feature vectors. The lifetime of the desc matrix must exceed that of the returned tree. I.e., no copy is made of the vectors.
      /// </summary>
      /// <param name="desc">n x d matrix of n d-dimensional feature vectors (CV_32FC1 or CV_64FC1)</param>
      /// <returns>A balanced kd-tree index of the given feature vectors</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateKDTree(IntPtr desc);

      /// <summary>
      /// Constructs a spill tree index of the given feature vectors. The lifetime of the desc matrix must exceed that of the returned tree. I.e., no copy is made of the vectors.
      /// </summary>
      /// <param name="desc">n x d matrix of n d-dimensional feature vectors (CV_32FC1 or CV_64FC1)</param>
      /// <param name="naive"></param>
      /// <param name="rho"></param>
      /// <param name="tau"></param>
      /// <returns>A spill tree index of the given feature vectors</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateSpillTree(IntPtr desc, int naive, double rho, double tau);

      /// <summary>
      /// Deallocates the given kd-tree
      /// </summary>
      /// <param name="tr">Pointer to tree being destroyed</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindFeatures(
         IntPtr tr,
         IntPtr desc,
         IntPtr results,
         IntPtr dist,
         int k,
         int emax);

      /// <summary>
      /// Performs orthogonal range seaching on the given kd-tree. That is, it returns the set of vectors v in tr that satisfy bounds_min[i] &lt;= v[i] &lt;= bounds_max[i], 0 &lt;= i &lt; d, where d is the dimension of vectors in the tree. The function returns the number of such vectors found
      /// </summary>
      /// <param name="tr">Pointer to kd-tree index of reference vectors</param>
      /// <param name="boundsMin">1 x d or d x 1 vector (CV_32FC1 or CV_64FC1) giving minimum value for each dimension</param>
      /// <param name="boundsMax">1 x d or d x 1 vector (CV_32FC1 or CV_64FC1) giving maximum value for each dimension</param>
      /// <param name="results">1 x m or m x 1 vector (CV_32SC1) to contain output row indices (referring to matrix passed to cvCreateFeatureTree)</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvErode(IntPtr src, IntPtr dst, IntPtr element, int iterations);

      /// <summary>
      /// Dilates the source image using the specified structuring element that determines the shape of a pixel neighborhood over which the maximum is taken
      /// The function supports the in-place mode. Dilation can be applied several (iterations) times. In case of color image each channel is processed independently
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="element">Structuring element used for erosion. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used</param>
      /// <param name="iterations">Number of times erosion is applied</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvDilate(IntPtr src, IntPtr dst, IntPtr element, int iterations);

      /// <summary>
      /// Deallocates the cascade that has been created manually or loaded using cvLoadHaarClassifierCascade or cvLoad
      /// </summary>
      /// <param name="cascade">Double pointer to the released cascade. The pointer is cleared by the function. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseHaarClassifierCascade(ref IntPtr cascade);

      /// <summary>
      /// Reconstructs the selected image area from the pixel near the area boundary. The function may be used to remove dust and scratches from a scanned photo, or to remove undesirable objects from still images or video.
      /// </summary>
      /// <param name="src">The input 8-bit 1-channel or 3-channel image</param>
      /// <param name="mask">The inpainting mask, 8-bit 1-channel image. Non-zero pixels indicate the area that needs to be inpainted</param>
      /// <param name="dst">The output image of the same format and the same size as input</param>
      /// <param name="flags">The inpainting method</param>
      /// <param name="inpaintRadius">The radius of circlular neighborhood of each point inpainted that is considered by the algorithm</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      /// <param name="apertureSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. In all cases except 1, <paramref name="appertureSize"/> x <paramref name="appertureSize"/> separable kernel will be used to calculate the derivative. For aperture_size=1 3x1 or 1x3 kernel is used (Gaussian smoothing is not done). There is also special value CV_SCHARR (=-1) that corresponds to 3x3 Scharr filter that may give more accurate results than 3x3 Sobel. Scharr aperture is: 
      /// <pre>
      /// | -3 0  3|
      /// |-10 0 10|
      /// | -3 0  3|</pre>
      /// for x-derivative or transposed for y-derivative. 
      ///</param>
      [DllImport(CV_LIBRARY)]
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
      /// <param name="aperture_size">Aperture size </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvLaplace(IntPtr src, IntPtr dst, int aperture_size);

      /// <summary>
      /// Finds the edges on the input image image and marks them in the output image edges using the Canny algorithm. The smallest of threshold1 and threshold2 is used for edge linking, the largest - to find initial segments of strong edges.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      /// <param name="threshold1">The first threshold</param>
      /// <param name="threshold2">The second threshold.</param>
      /// <param name="aperture_size">Aperture parameter for Sobel operator </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCanny(
          IntPtr image,
          IntPtr edges,
          double threshold1,
          double threshold2,
          int aperture_size);

      /// <summary>
      /// Tests whether the input contour is convex or not. The contour must be simple, i.e. without self-intersections. 
      /// </summary>
      /// <param name="contour">Tested contour (sequence or array of points). </param>
      /// <returns>true if convex</returns>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern MCvBox2D cvMinAreaRect2(IntPtr points, IntPtr storage);

      /// <summary>
      /// Finds the minimal circumscribed circle for 2D point set using iterative algorithm. It returns nonzero if the resultant circle contains all the input points and zero otherwise (i.e. algorithm failed)
      /// </summary>
      /// <param name="points">Sequence or array of 2D points</param>
      /// <param name="center">Output parameter. The center of the enclosing circle</param>
      /// <param name="radius">Output parameter. The radius of the enclosing circle.</param>
      /// <returns>Nonzero if the resultant circle contains all the input points and zero otherwise (i.e. algorithm failed)</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvMinEnclosingCircle(IntPtr points, out PointF center, out float radius);

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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern double cvContourArea(IntPtr contour, MCvSlice slice, int oriented);

      /// <summary>
      /// Calculates length or curve as sum of lengths of segments between subsequent points
      /// </summary>
      /// <param name="curve">Sequence or array of the curve points</param>
      /// <param name="slice">Starting and ending points of the curve, by default the whole curve length is calculated</param>
      /// <param name="is_closed">
      /// Indicates whether the curve is closed or not. There are 3 cases:
      /// is_closed=0 - the curve is assumed to be unclosed. 
      /// is_closed&gt;0 - the curve is assumed to be closed. 
      /// is_closed&lt;0 - if curve is sequence, the flag CV_SEQ_FLAG_CLOSED of ((CvSeq*)curve)-&gt;flags is checked to determine if the curve is closed or not, otherwise (curve is represented by array (CvMat*) of points) it is assumed to be unclosed. 
      /// </param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern double cvArcLength(IntPtr curve, MCvSlice slice, int is_closed);

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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvFilter2D(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor);

      /// <summary>
      /// Copies the source 2D array into interior of destination array and makes a border of the specified type around the copied area. The function is useful when one needs to emulate border type that is different from the one embedded into a specific algorithm implementation. For example, morphological functions, as well as most of other filtering functions in OpenCV, internally use replication border type, while the user may need zero border or a border, filled with 1's or 255's
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="offset">Coordinates of the top-left corner (or bottom-left in case of images with bottom-left origin) of the destination image rectangle where the source image (or its ROI) is copied. Size of the rectangle matches the source image size/ROI size</param>
      /// <param name="bordertype">Type of the border to create around the copied source image rectangle</param>
      /// <param name="value">Value of the border pixels if bordertype=CONSTANT</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvAdaptiveThreshold(
         IntPtr src,
         IntPtr dst,
         double maxValue,
         CvEnum.ADAPTIVE_THRESHOLD_TYPE adaptiveType,
         CvEnum.THRESH thresholdType,
         int blockSize,
         double param1);

      /// <summary>
      /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles. The function scans the image several times at different scales (see cvSetImagesForHaarClassifierCascade). Each time it considers overlapping regions in the image and applies the classifiers to the regions using cvRunHaarClassifierCascade. It may also apply some heuristics to reduce number of analyzed regions, such as Canny prunning. After it has proceeded and collected the candidate rectangles (regions that passed the classifier cascade), it groups them and returns a sequence of average rectangles for each large enough group. The default parameters (scale_factor=1.1, min_neighbors=3, flags=0) are tuned for accurate yet slow object detection. For a faster operation on real video images the settings are: scale_factor=1.2, min_neighbors=2, flags=CV_HAAR_DO_CANNY_PRUNING, min_size=&lt;minimum possible face size&gt; (for example, ~1/4 to 1/16 of the image area in case of video conferencing). 
      /// </summary>
      /// <param name="image">Image to detect objects in.</param>
      /// <param name="cascade">Haar classifier cascade in internal representation</param>
      /// <param name="storage">Memory storage to store the resultant sequence of the object candidate rectangles</param>
      /// <param name="scaleFactor">Use 1.1 as default. The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%</param>
      /// <param name="minNeighbors">Use 3 as default. Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure</param>
      /// <param name="flags">Mode of operation. Currently the only flag that may be specified is CV_HAAR_DO_CANNY_PRUNING. If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing</param>
      /// <param name="minSize">Use Size.Empty as default. Minimum window size. By default, it is set to the size of samples the classifier has been trained on (~20x20 for face detection). </param>
      /// <returns>Rectangular regions in the given image that are likely to contain objects the cascade has been trained for</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvHaarDetectObjects(
         IntPtr image,
         IntPtr cascade,
         IntPtr storage,
         double scaleFactor,
         int minNeighbors,
         CvEnum.HAAR_DETECTION_TYPE flags,
         Size minSize);

      /// <summary>
      /// Retrieves contours from the binary image and returns the number of retrieved contours. The pointer first_contour is filled by the function. It will contain pointer to the first most outer contour or IntPtr.Zero if no contours is detected (if the image is completely black). Other contours may be reached from first_contour using h_next and v_next links. The sample in cvDrawContours discussion shows how to use contours for connected component detection. Contours can be also used for shape analysis and object recognition - see squares.c in OpenCV sample directory
      /// </summary>
      /// <param name="image">The source 8-bit single channel image. Non-zero pixels are treated as 1s, zero pixels remain 0s - that is image treated as binary. To get such a binary image from grayscale, one may use cvThreshold, cvAdaptiveThreshold or cvCanny. The function modifies the source image content</param>
      /// <param name="storage">Container of the retrieved contours</param>
      /// <param name="firstContour">Output parameter, will contain the pointer to the first outer contour</param>
      /// <param name="headerSize">Size of the sequence header, &gt;=sizeof(CvChain) if method=CV_CHAIN_CODE, and &gt;=sizeof(CvContour) otherwise</param>
      /// <param name="mode">Retrieval mode</param>
      /// <param name="method">Approximation method (for all the modes, except CV_RETR_RUNS, which uses built-in approximation). </param>
      /// <param name="offset">Offset, by which every contour point is shifted. This is useful if the contours are extracted from the image ROI and then they should be analyzed in the whole image context</param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindContours(
         IntPtr image,
         IntPtr storage,
         ref IntPtr firstContour,
         int headerSize,
         CvEnum.RETR_TYPE mode,
         CvEnum.CHAIN_APPROX_METHOD method,
         Point offset);

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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      /// Finds robust features in the image. For each feature it returns its location, size, orientation and optionally the descriptor, basic or extended. The function can be used for object tracking and localization, image stitching etc
      /// </summary>
      /// <param name="image">The input 8-bit grayscale image</param>
      /// <param name="mask">The optional input 8-bit mask. The features are only found in the areas that contain more than 50% of non-zero mask pixels</param>
      /// <param name="keypoints">The output parameter; double pointer to the sequence of keypoints. This will be the sequence of MCvSURFPoint structures</param>
      /// <param name="descriptors">The optional output parameter; double pointer to the sequence of descriptors; Depending on the params.extended value, each element of the sequence will be either 64-element or 128-element floating-point (CV_32F) vector. If the parameter is IntPtr.Zero, the descriptors are not computed</param>
      /// <param name="storage">Memory storage where keypoints and descriptors will be stored</param>
      /// <param name="parameters">Various algorithm parameters put to the structure CvSURFParams</param>
      /// <param name="useProvidedKeyPoints">If 1, the provided key points are locations for computing SURF descriptors</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvExtractSURF(
         IntPtr image, IntPtr mask,
         ref IntPtr keypoints,
         ref IntPtr descriptors,
         IntPtr storage,
         MCvSURFParams parameters,
         int useProvidedKeyPoints);

      /// <summary>
      /// Create a CvSURFParams using the specific values
      /// </summary>
      /// <param name="hessianThreshold">      
      /// only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extended">      
      /// 0 means basic descriptors (64 elements each),
      /// 1 means extended descriptors (128 elements each)
      /// </param>
      /// <returns>The MCvSURFParams structure</returns>
      [DllImport(CV_LIBRARY)]
      public static extern MCvSURFParams cvSURFParams(double hessianThreshold, int extended);

      /// <summary>
      /// Extracts the contours of Maximally Stable Extremal Regions
      /// </summary>
      /// <param name="img">The image where MSER will be extracted</param>
      /// <param name="mask">The mask for region of interest</param>
      /// <param name="contours">The contours where MSER will be stored</param>
      /// <param name="storage">Memory storage</param>
      /// <param name="parameters">MSER parameters</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvExtractMSER(
         IntPtr img,
         IntPtr mask,
         ref IntPtr contours,
         IntPtr storage,
         MCvMSERParams parameters);

      #region Camera Calibration
      /// <summary>
      /// Computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters.
      /// Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points). 
      /// </summary>
      /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
      /// <param name="rotationVector">The rotation vector, 1x3 or 3x1</param>
      /// <param name="translationVector">The translation vector, 1x3 or 3x1</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is IntPtr.Zero, all distortion coefficients are considered 0's</param>
      /// <param name="imagePoints">The output array of image points, 2xN or Nx2, where N is the total number of points in the view</param>
      /// <param name="dpdrot">Optional Nx3 matrix of derivatives of image points with respect to components of the rotation vector</param>
      /// <param name="dpdt">Optional Nx3 matrix of derivatives of image points w.r.t. components of the translation vector</param>
      /// <param name="dpdf">Optional Nx2 matrix of derivatives of image points w.r.t. fx and fy</param>
      /// <param name="dpdc">Optional Nx2 matrix of derivatives of image points w.r.t. cx and cy</param>
      /// <param name="dpddist">Optional Nx4 matrix of derivatives of image points w.r.t. distortion coefficients</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvProjectPoints2(
          IntPtr objectPoints,
          IntPtr rotationVector,
          IntPtr translationVector,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs,
          IntPtr imagePoints,
          IntPtr dpdrot,
          IntPtr dpdt,
          IntPtr dpdf,
          IntPtr dpdc,
          IntPtr dpddist);

      /// <summary>
      /// Finds perspective transformation H=||hij|| between the source and the destination planes
      /// </summary>
      /// <param name="srcPoints">Point coordinates in the original plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates), where N is the number of points. </param>
      /// <param name="dstPoints">Point coordinates in the destination plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates) </param>
      /// <param name="homography">Output 3x3 homography matrix. Homography matrix is determined up to a scale, thus it is normalized to make h33=1</param>
      /// <param name="method">The type of the method</param>
      /// <param name="ransacReprojThreshold">The maximum allowed reprojection error to treat a point pair as an inlier. The parameter is only used in RANSAC-based homography estimation. E.g. if dst_points coordinates are measured in pixels with pixel-accurate precision, it makes sense to set this parameter somewhere in the range ~1..3</param>
      /// <param name="mask">The optional output mask set by a robust method (RANSAC or LMEDS). </param>
      /// <returns>1 if the homography matrix is found, 0 otherwise.</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindHomography(
         IntPtr srcPoints,
         IntPtr dstPoints,
         IntPtr homography,
         CvEnum.HOMOGRAPHY_METHOD method,
         double ransacReprojThreshold,
         IntPtr mask);

      /// <summary>
      /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
      /// </summary>
      /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
      /// <param name="imagePoints">The joint matrix of corresponding image points, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="pointCounts">Vector containing numbers of points in each particular view, 1xM or Mx1, where M is the number of a scene views</param>
      /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="intrinsicMatrix">The output camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATION are specified, some or all of fx, fy, cx, cy must be initialized</param>
      /// <param name="distortionCoeffs">The output 4x1 or 1x4 vector of distortion coefficients [k1, k2, p1, p2]</param>
      /// <param name="rotationVectors">The output 3xM or Mx3 array of rotation vectors (compact representation of rotation matrices, see cvRodrigues2). </param>
      /// <param name="translationVectors">The output 3xM or Mx3 array of translation vectors</param>
      /// <param name="flags">Different flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalibrateCamera2(
          IntPtr objectPoints,
          IntPtr imagePoints,
          IntPtr pointCounts,
          Size imageSize,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs,
          IntPtr rotationVectors,
          IntPtr translationVectors,
          CvEnum.CALIB_TYPE flags);

      /// <summary>
      /// Computes various useful camera (sensor/lens) characteristics using the computed camera calibration matrix, image frame resolution in pixels and the physical aperture size
      /// </summary>
      /// <param name="calibMatr">The matrix of intrinsic parameters</param>
      /// <param name="imgWidth">Image width in pixels</param>
      /// <param name="imgHeight">Image height in pixels</param>
      /// <param name="apertureWidth">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="apertureHeight">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="fovx">Field of view angle in x direction in degrees</param>
      /// <param name="fovy">Field of view angle in y direction in degrees </param>
      /// <param name="focalLength">Focal length in realworld units </param>
      /// <param name="principalPoint">The principal point in realworld units </param>
      /// <param name="pixelAspectRatio">The pixel aspect ratio ~ fy/f</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalibrationMatrixValues(
         IntPtr calibMatr,
         int imgWidth,
         int imgHeight,
         double apertureWidth,
         double apertureHeight,
         ref double fovx,
         ref double fovy,
         ref double focalLength,
         ref MCvPoint2D64f principalPoint,
         ref double pixelAspectRatio);


      /// <summary>
      /// Estimates extrinsic camera parameters using known intrinsic parameters and and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error
      /// </summary>
      /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
      /// <param name="imagePoints">The array of corresponding image points, 2xN or Nx2, where N is the number of points in the view</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is IntPtr.Zero, all distortion coefficients are considered 0's.</param>
      /// <param name="rotationVector">The output 3x1 or 1x3 rotation vector (compact representation of a rotation matrix, see cvRodrigues2). </param>
      /// <param name="translationVector">The output 3x1 or 1x3 translation vector</param>
      /// <param name="useExtrinsicGuess">Use the input rotation and translation parameters as a guess</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindExtrinsicCameraParams2(
         IntPtr objectPoints,
         IntPtr imagePoints,
         IntPtr intrinsicMatrix,
         IntPtr distortionCoeffs,
         IntPtr rotationVector,
         IntPtr translationVector,
         int useExtrinsicGuess);

      /// <summary>
      /// Estimates transformation between the 2 cameras making a stereo pair. If we have a stereo camera, where the relative position and orientatation of the 2 cameras is fixed, and if we computed poses of an object relative to the fist camera and to the second camera, (R1, T1) and (R2, T2), respectively (that can be done with cvFindExtrinsicCameraParams2), obviously, those poses will relate to each other, i.e. given (R1, T1) it should be possible to compute (R2, T2) - we only need to know the position and orientation of the 2nd camera relative to the 1st camera. That's what the described function does. It computes (R, T) such that:
      /// R2=R*R1,
      /// T2=R*T1 + T
      /// </summary>
      /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
      /// <param name="imagePoints1">The joint matrix of corresponding image points in the views from the 1st camera, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="imagePoints2">The joint matrix of corresponding image points in the views from the 2nd camera, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="pointCounts">Vector containing numbers of points in each view, 1xM or Mx1, where M is the number of views</param>
      /// <param name="cameraMatrix1">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
      /// <param name="distCoeffs1">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="cameraMatrix2">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
      /// <param name="distCoeffs2">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="R">The rotation matrix between the 1st and the 2nd cameras' coordinate systems </param>
      /// <param name="T">The translation vector between the cameras' coordinate systems</param>
      /// <param name="E">The optional output essential matrix</param>
      /// <param name="F">The optional output fundamental matrix </param>
      /// <param name="termCrit">Termination criteria for the iterative optimiziation algorithm</param>
      /// <param name="flags">The calibration flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvStereoCalibrate(
         IntPtr objectPoints,
         IntPtr imagePoints1,
         IntPtr imagePoints2,
         IntPtr pointCounts,
         IntPtr cameraMatrix1,
         IntPtr distCoeffs1,
         IntPtr cameraMatrix2,
         IntPtr distCoeffs2,
         Size imageSize,
         IntPtr R,
         IntPtr T,
         IntPtr E,
         IntPtr F,
         MCvTermCriteria termCrit,
         CvEnum.CALIB_TYPE flags);

      /// <summary>
      /// computes the rectification transformations without knowing intrinsic parameters of the cameras and their relative position in space, hence the suffix "Uncalibrated". Another related difference from cvStereoRectify is that the function outputs not the rectification transformations in the object (3D) space, but the planar perspective transformations, encoded by the homography matrices H1 and H2. The function implements the following algorithm [Hartley99]. 
      /// </summary>
      /// <remarks>
      /// Note that while the algorithm does not need to know the intrinsic parameters of the cameras, it heavily depends on the epipolar geometry. Therefore, if the camera lenses have significant distortion, it would better be corrected before computing the fundamental matrix and calling this function. For example, distortion coefficients can be estimated for each head of stereo camera separately by using cvCalibrateCamera2 and then the images can be corrected using cvUndistort2
      /// </remarks>
      /// <param name="points1">The array of 2D points</param>
      /// <param name="points2">The array of 2D points</param>
      /// <param name="F">Fundamental matrix. It can be computed using the same set of point pairs points1 and points2 using cvFindFundamentalMat</param>
      /// <param name="imageSize">Size of the image</param>
      /// <param name="H1">The rectification homography matrices for the first images</param>
      /// <param name="H2">The rectification homography matrices for the second images</param>
      /// <param name="threshold">If the parameter is greater than zero, then all the point pairs that do not comply the epipolar geometry well enough (that is, the points for which fabs(points2[i]T*F*points1[i])>threshold) are rejected prior to computing the homographies</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvStereoRectifyUncalibrated(
         IntPtr points1,
         IntPtr points2,
         IntPtr F,
         Size imageSize,
         IntPtr H1,
         IntPtr H2,
         double threshold);

      /// <summary>
      /// computes the rotation matrices for each camera that (virtually) make both camera image planes the same plane. Consequently, that makes all the epipolar lines parallel and thus simplifies the dense stereo correspondence problem. On input the function takes the matrices computed by cvStereoCalibrate and on output it gives 2 rotation matrices and also 2 projection matrices in the new coordinates. The function is normally called after cvStereoCalibrate that computes both camera matrices, the distortion coefficients, R and T
      /// </summary>
      /// <param name="cameraMatrix1">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
      /// <param name="cameraMatrix2">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
      /// <param name="distCoeffs1">The vectors of distortion coefficients for first camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="distCoeffs2">The vectors of distortion coefficients for second camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="imageSize">Size of the image used for stereo calibration</param>
      /// <param name="R">The rotation matrix between the 1st and the 2nd cameras' coordinate systems</param>
      /// <param name="T">The translation vector between the cameras' coordinate systems</param>
      /// <param name="R1">3x3 Rectification transforms (rotation matrices) for the first camera</param>
      /// <param name="R2">3x3 Rectification transforms (rotation matrices) for the second camera</param>
      /// <param name="P1">3x4 Projection matrices in the new (rectified) coordinate systems</param>
      /// <param name="P2">3x4 Projection matrices in the new (rectified) coordinate systems</param>
      /// <param name="Q">The optional output disparity-to-depth mapping matrix, 4x4, see cvReprojectImageTo3D. </param>
      /// <param name="flags">The operation flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvStereoRectify(
         IntPtr cameraMatrix1,
         IntPtr cameraMatrix2,
         IntPtr distCoeffs1,
         IntPtr distCoeffs2,
         Size imageSize,
         IntPtr R,
         IntPtr T,
         IntPtr R1,
         IntPtr R2,
         IntPtr P1,
         IntPtr P2,
         IntPtr Q,
         CvEnum.STEREO_RECTIFY_TYPE flags);

      /// <summary>
      /// Transforms the image to compensate radial and tangential lens distortion. The camera matrix and distortion parameters can be determined using cvCalibrateCamera2. For every pixel in the output image the function computes coordinates of the corresponding location in the input image using the formulae in the section beginning. Then, the pixel value is computed using bilinear interpolation. If the resolution of images is different from what was used at the calibration stage, fx, fy, cx and cy need to be adjusted appropriately, while the distortion coefficients remain the same.
      /// </summary>
      /// <param name="src">The input (distorted) image</param>
      /// <param name="dst">The output (corrected) image</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1].</param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2].</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvUndistort2(
          IntPtr src,
          IntPtr dst,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs);

      /// <summary>
      /// Pre-computes the undistortion map - coordinates of the corresponding pixel in the distorted image for every pixel in the corrected image. Then, the map (together with input and output images) can be passed to cvRemap function. 
      /// </summary>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. </param>
      /// <param name="mapx">The output array of x-coordinates of the map</param>
      /// <param name="mapy">The output array of y-coordinates of the map</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvUndistortPoints(
         IntPtr src,
         IntPtr dst,
         IntPtr camera_matrix,
         IntPtr dist_coeffs,
         IntPtr R,
         IntPtr P);

      /// <summary>
      /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
      /// </summary>
      /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The output array of corners detected</param>
      /// <param name="cornerCount">The output corner counter. If it is not IntPtr.Zero, the function stores there the number of corners found</param>
      /// <param name="flags">Various operation flags</param>
      /// <returns>Non-zero value if all the corners have been found and they have been placed in a certain order (row by row, left to right in every row), otherwise, if the function fails to find all the corners or reorder them, it returns 0</returns>
      /// <remarks>The coordinates detected are approximate, and to determine their position more accurately, the user may use the function cvFindCornerSubPix</remarks>
      [DllImport(CV_LIBRARY)]
      [Obsolete("Use the other cvFindChessboardCorners function instead, will be removed in the next version")]
      public static extern int cvFindChessboardCorners(
         IntPtr image,
         Size patternSize,
         float[,] corners,
         ref int cornerCount,
         CvEnum.CALIB_CB_TYPE flags);

      /// <summary>
      /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
      /// </summary>
      /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">Pointer to the output array of corners(PointF) detected</param>
      /// <param name="cornerCount">The output corner counter. If it is not IntPtr.Zero, the function stores there the number of corners found</param>
      /// <param name="flags">Various operation flags</param>
      /// <returns>Non-zero value if all the corners have been found and they have been placed in a certain order (row by row, left to right in every row), otherwise, if the function fails to find all the corners or reorder them, it returns 0</returns>
      /// <remarks>The coordinates detected are approximate, and to determine their position more accurately, the user may use the function cvFindCornerSubPix</remarks>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindChessboardCorners(
         IntPtr image,
         Size patternSize,
         IntPtr corners,
         ref int cornerCount,
         CvEnum.CALIB_CB_TYPE flags);

      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
      /// </summary>
      /// <param name="image">The destination image; it must be 8-bit color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected</param>
      /// <param name="count">The number of corners</param>
      /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvDrawChessboardCorners(
         IntPtr image,
         Size patternSize,
         float[,] corners,
         int count,
         int patternWasFound);

      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
      /// </summary>
      /// <param name="image">The destination image; it must be 8-bit color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected</param>
      /// <param name="count">The number of corners</param>
      /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvDrawChessboardCorners(
         IntPtr image,
         Size patternSize,
         [In]
         PointF[] corners,
         int count,
         int patternWasFound);

      #endregion

      #region Epipolar Geometry, Stereo Correspondence
      /// <summary>
      /// Calculates fundamental matrix using one of four methods listed above and returns the number of fundamental matrices found (1 or 3) and 0, if no matrix is found. 
      /// </summary>
      /// <param name="points1">Array of the first image points of 2xN, Nx2, 3xN or Nx3 size (where N is number of points). Multi-channel 1xN or Nx1 array is also acceptable. The point coordinates should be floating-point (single or double precision) </param>
      /// <param name="points2">Array of the second image points of the same size and format as points1</param>
      /// <param name="fundamentalMatrix">The output fundamental matrix or matrices. The size should be 3x3 or 9x3 (7-point method may return up to 3 matrices).</param>
      /// <param name="method">Method for computing the fundamental matrix </param>
      /// <param name="param1">Use 3.0 for default. The parameter is used for RANSAC method only. It is the maximum distance from point to epipolar line in pixels, beyond which the point is considered an outlier and is not used for computing the final fundamental matrix. Usually it is set somewhere from 1 to 3. </param>
      /// <param name="param2">Use 0.99 for default. The parameter is used for RANSAC or LMedS methods only. It denotes the desirable level of confidence of the fundamental matrix estimate. </param>
      /// <param name="status">The optional pointer to output array of N elements, every element of which is set to 0 for outliers and to 1 for the "inliers", i.e. points that comply well with the estimated epipolar geometry. The array is computed only in RANSAC and LMedS methods. For other methods it is set to all 1?.</param>
      /// <returns>the number of fundamental matrices found (1 or 3) and 0, if no matrix is found. </returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvFindFundamentalMat(IntPtr points1,
         IntPtr points2,
         IntPtr fundamentalMatrix,
         CvEnum.CV_FM method,
         double param1,
         double param2,
         IntPtr status);

      /// <summary>
      /// For every point in one of the two images of stereo-pair the function cvComputeCorrespondEpilines finds equation of a line that contains the corresponding point (i.e. projection of the same 3D point) in the other image. Each line is encoded by a vector of 3 elements l=[a,b,c]^T, so that: 
      /// l^T*[x, y, 1]^T=0, or
      /// a*x + b*y + c = 0
      /// From the fundamental matrix definition (see cvFindFundamentalMatrix discussion), line l2 for a point p1 in the first image (which_image=1) can be computed as: 
      /// l2=F*p1 and the line l1 for a point p2 in the second image (which_image=1) can be computed as: 
      /// l1=F^T*p2Line coefficients are defined up to a scale. They are normalized (a2+b2=1) are stored into correspondent_lines
      /// </summary>
      /// <param name="points">The input points. 2xN, Nx2, 3xN or Nx3 array (where N number of points). Multi-channel 1xN or Nx1 array is also acceptable.</param>
      /// <param name="whichImage">Index of the image (1 or 2) that contains the points</param>
      /// <param name="fundamentalMatrix">Fundamental matrix </param>
      /// <param name="correspondentLines">Computed epilines, 3xN or Nx3 array </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvComputeCorrespondEpilines(
         IntPtr points,
         int whichImage,
         IntPtr fundamentalMatrix,
         IntPtr correspondentLines);

      /// <summary>
      /// Converts 2D or 3D points from/to homogenious coordinates, or simply copies or transposes the array. In case if the input array dimensionality is larger than the output, each point coordinates are divided by the last coordinate
      /// </summary>
      /// <param name="src">The input point array, 2xN, Nx2, 3xN, Nx3, 4xN or Nx4 (where N is the number of points). Multi-channel 1xN or Nx1 array is also acceptable</param>
      /// <param name="dst">The output point array, must contain the same number of points as the input; The dimensionality must be the same, 1 less or 1 more than the input, and also within 2..4.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvConvertPointsHomogeneous(IntPtr src, IntPtr dst);

      /// <summary>
      /// Creates the stereo correspondence structure and initializes it. It is possible to override any of the parameters at any time between the calls to cvFindStereoCorrespondenceBM
      /// </summary>
      /// <param name="type">ID of one of the pre-defined parameter sets. Any of the parameters can be overridden after creating the structure.</param>
      /// <param name="numberOfDisparities">The number of disparities. If the parameter is 0, it is taken from the preset, otherwise the supplied value overrides the one from preset. </param>
      /// <returns>Pointer to the stereo correspondece structure</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateStereoBMState(
         CvEnum.STEREO_BM_TYPE type,
         int numberOfDisparities);

      /// <summary>
      /// Releases the stereo correspondence structure and all the associated internal buffers
      /// </summary>
      /// <param name="state">The state to be released</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseStereoBMState(ref IntPtr state);

      /// <summary>
      /// computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <param name="state">Stereo correspondence structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceBM(
         IntPtr left,
         IntPtr right,
         IntPtr disparity,
         IntPtr state);

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <param name="state">Stereo correspondence structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceBM(
         IntPtr left,
         IntPtr right,
         IntPtr disparity,
         ref MCvStereoBMState state);

      /// <summary>
      /// Creates the stereo correspondence structure and initializes it. 
      /// </summary>
      /// <param name="numberOfDisparities">The number of disparities. The disparity search range will be state.minDisparity &lt;= disparity &lt; state.minDisparity + state.numberOfDisparities</param>
      /// <param name="maxIters">Maximum number of iterations. On each iteration all possible (or reasonable) alpha-expansions are tried. The algorithm may terminate earlier if it could not find an alpha-expansion that decreases the overall cost function value</param>
      /// <returns>The initialized stereo correspondence structure</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateStereoGCState(
         int numberOfDisparities,
         int maxIters);

      /// <summary>
      /// Releases the stereo correspondence structure and all the associated internal buffers
      /// </summary>
      /// <param name="state">A reference to the pointer of StereoGCState structure</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseStereoGCState(ref IntPtr state);

      /// <summary>
      /// Computes disparity maps for the input rectified stereo pair
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="dispLeft">The optional output single-channel 16-bit signed left disparity map of the same size as input images.</param>
      /// <param name="dispRight">The optional output single-channel 16-bit signed right disparity map of the same size as input images</param>
      /// <param name="state">Stereo correspondence structure</param>
      /// <param name="useDisparityGuess">If the parameter is not zero, the algorithm will start with pre-defined disparity maps. Both dispLeft and dispRight should be valid disparity maps. Otherwise, the function starts with blank disparity maps (all pixels are marked as occlusions)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceGC(
         IntPtr left,
         IntPtr right,
         IntPtr dispLeft,
         IntPtr dispRight,
         IntPtr state,
         int useDisparityGuess);

      /// <summary>
      /// Computes disparity maps for the input rectified stereo pair
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="dispLeft">The optional output single-channel 16-bit signed left disparity map of the same size as input images.</param>
      /// <param name="dispRight">The optional output single-channel 16-bit signed right disparity map of the same size as input images</param>
      /// <param name="state">Stereo correspondence structure</param>
      /// <param name="useDisparityGuess">If the parameter is not zero, the algorithm will start with pre-defined disparity maps. Both dispLeft and dispRight should be valid disparity maps. Otherwise, the function starts with blank disparity maps (all pixels are marked as occlusions)</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindStereoCorrespondenceGC(
         IntPtr left,
         IntPtr right,
         IntPtr dispLeft,
         IntPtr dispRight,
         ref MCvStereoGCState state,
         int useDisparityGuess);

      /// <summary>
      /// Transforms 1-channel disparity map to 3-channel image, a 3D surface.
      /// </summary>
      /// <param name="disparity">Disparity map</param>
      /// <param name="image3D">3-channel, 16-bit integer or 32-bit floating-point image - the output map of 3D points</param>
      /// <param name="Q">The reprojection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReprojectImageTo3D(
         IntPtr disparity,
         IntPtr image3D,
         IntPtr Q);

      #endregion

      /// <summary>
      /// Iterates to find the object center given its back projection and initial position of search window. The iterations are made until the search window center moves by less than the given value and/or until the function has done the maximum number of iterations. 
      /// </summary>
      /// <param name="probImage">Back projection of object histogram</param>
      /// <param name="window">Initial search window</param>
      /// <param name="criteria">Criteria applied to determine when the window search should be finished. </param>
      /// <param name="comp">Resultant structure that contains converged search window coordinates (comp->rect field) and sum of all pixels inside the window (comp->area field). </param>
      /// <returns>the number of iterations made</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvMeanShift(
         IntPtr probImage,
         Rectangle window,
         MCvTermCriteria criteria,
         out MCvConnectedComp comp);

      /// <summary>
      /// Implements CAMSHIFT object tracking algrorithm ([Bradski98]). First, it finds an object center using cvMeanShift and, after that, calculates the object size and orientation. 
      /// </summary>
      /// <param name="probImage">Back projection of object histogram </param>
      /// <param name="window">Initial search window</param>
      /// <param name="criteria">Criteria applied to determine when the window search should be finished</param>
      /// <param name="comp">Resultant structure that contains converged search window coordinates (comp->rect field) and sum of all pixels inside the window (comp->area field).</param>
      /// <param name="box">Circumscribed box for the object. If not IntPtr.Zero, contains object size and orientation</param>
      /// <returns>number of iterations made within cvMeanShift</returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvCamShift(
         IntPtr probImage,
         Rectangle window,
         MCvTermCriteria criteria,
         out MCvConnectedComp comp,
         out MCvBox2D box);

      /// <summary>
      /// This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern double cvMatchShapes(
         IntPtr object1,
         IntPtr object2,
         CvEnum.CONTOURS_MATCH_TYPE method,
         double parameter);


      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If != 0, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSnakeImage(
          IntPtr image,
          IntPtr points,
          int length,
          [MarshalAs(UnmanagedType.LPArray)] float[] alpha,
          [MarshalAs(UnmanagedType.LPArray)] float[] beta,
          [MarshalAs(UnmanagedType.LPArray)] float[] gamma,
          int coeffUsage,
          Size win,
          MCvTermCriteria criteria,
          int calcGradient);

      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If != 0, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSnakeImage(
         IntPtr image,
         [In, Out]
         Point[] points,
         int length,
         [MarshalAs(UnmanagedType.LPArray)] float[] alpha,
         [MarshalAs(UnmanagedType.LPArray)] float[] beta,
         [MarshalAs(UnmanagedType.LPArray)] float[] gamma,
         int coeffUsage,
         Size win,
         MCvTermCriteria criteria,
         int calcGradient);

      /// <summary>
      /// Updates snake in order to minimize its total energy that is a sum of internal energy that depends on contour shape (the smoother contour is, the smaller internal energy is) and external energy that depends on the energy field and reaches minimum at the local energy extremums that correspond to the image edges in case of image gradient.
      /// </summary>
      /// <param name="image">The source image or external energy field</param>
      /// <param name="points">Seq points (snake). </param>
      /// <param name="length">Number of points in the contour</param>
      /// <param name="alpha">Weight[s] of continuity energy, single float or array of length floats, one per each contour point</param>
      /// <param name="beta">Weight[s] of curvature energy, similar to alpha</param>
      /// <param name="gamma">Weight[s] of image energy, similar to alpha</param>
      /// <param name="coeffUsage">Variant of usage of the previous three parameters: 
      /// CV_VALUE indicates that each of alpha, beta, gamma is a pointer to a single value to be used for all points; 
      /// CV_ARRAY indicates that each of alpha, beta, gamma is a pointer to an array of coefficients different for all the points of the snake. All the arrays must have the size equal to the contour size.
      /// </param>
      /// <param name="win">Size of neighborhood of every point used to search the minimum, both win.width and win.height must be odd</param>
      /// <param name="criteria">Termination criteria</param>
      /// <param name="calcGradient">
      /// Gradient flag. If true, the function calculates gradient magnitude for every image pixel and consideres it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      public static void cvSnakeImage(
           IntPtr image,
           IntPtr points,
           int length,
           float[] alpha,
           float[] beta,
           float[] gamma,
           int coeffUsage,
           Size win,
           MCvTermCriteria criteria,
           bool calcGradient)
      {
         cvSnakeImage(
            image,
            points,
            length,
            alpha,
            beta,
            gamma,
            coeffUsage,
            win,
            criteria,
            calcGradient ? 1 : 0);
      }

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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvNormalizeHist(IntPtr hist, double factor);

      /// <summary>
      /// Clears histogram bins that are below the specified threshold
      /// </summary>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="threshold">Threshold level</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvThreshHist(IntPtr hist, double threshold);


      /// <summary>
      /// Sets all histogram bins to 0 in case of dense histogram and removes all histogram bins in case of sparse array
      /// </summary>
      /// <param name="hist">Histogram</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvCopyHist(IntPtr src, ref IntPtr dst);

      /// <summary>
      /// Compares two dense histograms
      /// </summary>
      /// <param name="hist1">The first dense histogram. </param>
      /// <param name="hist2">The second dense histogram.</param>
      /// <param name="method">Comparison method</param>
      /// <returns>Result of the comparison</returns>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcArrBackProject(IntPtr[] image, IntPtr backProject, IntPtr hist);

      /// <summary>
      /// The algorithm normalizes brightness and increases contrast of the image
      /// </summary>
      /// <param name="src">The input 8-bit single-channel image</param>
      /// <param name="dst">The output image of the same size and the same data type as src</param>
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY, EntryPoint = "cvCalcArrBackProject")]
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
      [DllImport(CV_LIBRARY, EntryPoint = "cvCalcArrBackProjectPatch")]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseHist(ref IntPtr hist);
      #endregion

      #region Optical flow
      /// <summary>
      /// Computes flow for every pixel of the first input image using Lucas &amp; Kanade algorithm
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel.</param>
      /// <param name="curr">Second image, 8-bit, single-channel.</param>
      /// <param name="winSize">Size of the averaging window used for grouping pixels. </param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowLK(
              IntPtr prev,
              IntPtr curr,
              Size winSize,
              IntPtr velx,
              IntPtr vely);

      /// <summary>
      /// Computes flow for every pixel of the first input image using Horn &amp; Schunck algorithm 
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel</param>
      /// <param name="curr">Second image, 8-bit, single-channel</param>
      /// <param name="usePrevious">Uses previous (input) velocity field</param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images, 32-bit floating-point, single-channel</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images, 32-bit floating-point, single-channel</param>
      /// <param name="lambda">Lagrangian multiplier</param>
      /// <param name="criteria">Criteria of termination of velocity computing</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowHS(
              IntPtr prev,
              IntPtr curr,
              int usePrevious,
              IntPtr velx,
              IntPtr vely,
              double lambda,
              MCvTermCriteria criteria);

      /// <summary>
      /// Calculates optical flow for overlapped blocks block_size.width * block_size.height pixels each, thus the velocity fields are smaller than the original images. For every block in prev the functions tries to find a similar block in curr in some neighborhood of the original block or shifted by (velx(x0,y0),vely(x0,y0)) block as has been calculated by previous function call (if use_previous=1)
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel.</param>
      /// <param name="curr">Second image, 8-bit, single-channel. </param>
      /// <param name="blockSize">Size of basic blocks that are compared.</param>
      /// <param name="shiftSize">Block coordinate increments. </param>
      /// <param name="maxRange">Size of the scanned neighborhood in pixels around block.</param>
      /// <param name="usePrevious">Uses previous (input) velocity field. </param>
      /// <param name="velx">Horizontal component of the optical flow of floor((prev->width - block_size.width)/shiftSize.width) x floor((prev->height - block_size.height)/shiftSize.height) size, 32-bit floating-point, single-channel. </param>
      /// <param name="vely">Vertical component of the optical flow of the same size velx, 32-bit floating-point, single-channel.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowBM(
              IntPtr prev,
              IntPtr curr,
              Size blockSize,
              Size shiftSize,
              Size maxRange,
              int usePrevious,
              IntPtr velx,
              IntPtr vely);

      /// <summary>
      /// Implements sparse iterative version of Lucas-Kanade optical flow in pyramids ([Bouguet00]). It calculates coordinates of the feature points on the current video frame given their coordinates on the previous frame. The function finds the coordinates with sub-pixel accuracy. 
      /// </summary>
      /// <remarks>Both parameters prev_pyr and curr_pyr comply with the following rules: if the image pointer is 0, the function allocates the buffer internally, calculates the pyramid, and releases the buffer after processing. Otherwise, the function calculates the pyramid and stores it in the buffer unless the flag CV_LKFLOW_PYR_A[B]_READY is set. The image should be large enough to fit the Gaussian pyramid data. After the function call both pyramids are calculated and the readiness flag for the corresponding image can be set in the next call (i.e., typically, for all the image pairs except the very first one CV_LKFLOW_PYR_A_READY is set). </remarks>
      /// <param name="prev">First frame, at time t. </param>
      /// <param name="curr">Second frame, at time t + dt .</param>
      /// <param name="prevPyr">Buffer for the pyramid for the first frame. If the pointer is not NULL , the buffer must have a sufficient size to store the pyramid from level 1 to level #level ; the total size of (image_width+8)*image_height/3 bytes is sufficient. </param>
      /// <param name="currPyr">Similar to prev_pyr, used for the second frame. </param>
      /// <param name="prevFeatures">Array of points for which the flow needs to be found. </param>
      /// <param name="currFeatures">Array of 2D points containing calculated new positions of input </param>
      /// <param name="count">Number of feature points.</param>
      /// <param name="winSize">Size of the search window of each pyramid level.</param>
      /// <param name="level">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc. </param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise.</param>
      /// <param name="trackError">Array of double numbers containing difference between patches around the original and moved points. Optional parameter; can be NULL </param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped.</param>
      /// <param name="flags">Miscellaneous flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowPyrLK(
          IntPtr prev,
          IntPtr curr,
          IntPtr prevPyr,
          IntPtr currPyr,
          float[,] prevFeatures,
          float[,] currFeatures,
          int count,
          Size winSize,
          int level,
          Byte[] status,
          float[] trackError,
          MCvTermCriteria criteria,
          CvEnum.LKFLOW_TYPE flags);

      /// <summary>
      /// Implements sparse iterative version of Lucas-Kanade optical flow in pyramids ([Bouguet00]). It calculates coordinates of the feature points on the current video frame given their coordinates on the previous frame. The function finds the coordinates with sub-pixel accuracy. 
      /// </summary>
      /// <remarks>Both parameters prev_pyr and curr_pyr comply with the following rules: if the image pointer is 0, the function allocates the buffer internally, calculates the pyramid, and releases the buffer after processing. Otherwise, the function calculates the pyramid and stores it in the buffer unless the flag CV_LKFLOW_PYR_A[B]_READY is set. The image should be large enough to fit the Gaussian pyramid data. After the function call both pyramids are calculated and the readiness flag for the corresponding image can be set in the next call (i.e., typically, for all the image pairs except the very first one CV_LKFLOW_PYR_A_READY is set). </remarks>
      /// <param name="prev">First frame, at time t. </param>
      /// <param name="curr">Second frame, at time t + dt .</param>
      /// <param name="prevPyr">Buffer for the pyramid for the first frame. If the pointer is not NULL , the buffer must have a sufficient size to store the pyramid from level 1 to level #level ; the total size of (image_width+8)*image_height/3 bytes is sufficient. </param>
      /// <param name="currPyr">Similar to prev_pyr, used for the second frame. </param>
      /// <param name="prevFeatures">Array of points for which the flow needs to be found. </param>
      /// <param name="currFeatures">Array of 2D points containing calculated new positions of input </param>
      /// <param name="count">Number of feature points.</param>
      /// <param name="winSize">Size of the search window of each pyramid level.</param>
      /// <param name="level">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc. </param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise.</param>
      /// <param name="trackError">Array of double numbers containing difference between patches around the original and moved points. Optional parameter; can be NULL </param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped.</param>
      /// <param name="flags">Miscellaneous flags</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcOpticalFlowPyrLK(
         IntPtr prev,
         IntPtr curr,
         IntPtr prevPyr,
         IntPtr currPyr,
         [In]
         PointF[] prevFeatures,
         [Out]
         PointF[] currFeatures,
         int count,
         Size winSize,
         int level,
         Byte[] status,
         float[] trackError,
         MCvTermCriteria criteria,
         CvEnum.LKFLOW_TYPE flags);

      /// <summary>
      /// Computes dense optical flow using Gunnar Farneback's algorithm
      /// </summary>
      /// <param name="prev0">The first 8-bit single-channel input image</param>
      /// <param name="next0">The second input image of the same size and the same type as prevImg</param>
      /// <param name="flow0">The computed flow image; will have the same size as prevImg and type CV 32FC2</param>
      /// <param name="pyrScale">Specifies the image scale (!1) to build the pyramids for each image. pyrScale=0.5 means the classical pyramid, where each next layer is twice smaller than the previous</param>
      /// <param name="levels">The number of pyramid layers, including the initial image. levels=1 means that no extra layers are created and only the original images are used</param>
      /// <param name="winSize">The averaging window size; The larger values increase the algorithm robustness to image noise and give more chances for fast motion detection, but yield more blurred motion field</param>
      /// <param name="iterations">The number of iterations the algorithm does at each pyramid level</param>
      /// <param name="polyN">Size of the pixel neighborhood used to find polynomial expansion in each pixel. The larger values mean that the image will be approximated with smoother surfaces, yielding more robust algorithm and more blurred motion field. Typically, poly n=5 or 7</param>
      /// <param name="polySigma">Standard deviation of the Gaussian that is used to smooth derivatives that are used as a basis for the polynomial expansion. For poly n=5 you can set poly sigma=1.1, for poly n=7 a good value would be poly sigma=1.5</param>
      /// <param name="flags">The operation flags</param>
      [DllImport(CV_LIBRARY)]
      public extern static void cvCalcOpticalFlowFarneback(
         IntPtr prev0,
         IntPtr next0,
         IntPtr flow0,
         double pyrScale,
         int levels,
         int winSize,
         int iterations,
         int polyN,
         double polySigma,
         CvEnum.OPTICALFLOW_FARNEBACK_FLAG flags);

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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvAcc(IntPtr image, IntPtr sum, IntPtr mask);

      /// <summary>
      /// Adds the input image image or its selected region, raised to power 2, to the accumulator sqsum
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="sqsum">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvSquareAcc(IntPtr image, IntPtr sqsum, IntPtr mask);

      /// <summary>
      /// Adds product of 2 images or thier selected regions to accumulator acc
      /// </summary>
      /// <param name="image1">First input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="image2">Second input image, the same format as the first one</param>
      /// <param name="acc">Accumulator of the same number of channels as input images, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvMultiplyAcc(IntPtr image1, IntPtr image2, IntPtr acc, IntPtr mask);

      /// <summary>
      /// Calculates weighted sum of input image image and the accumulator acc so that acc becomes a running average of frame sequence:
      /// acc(x,y)=(1-<paramref name="alpha"/>) * acc(x,y) + <paramref name="alpha"/> * image(x,y) if mask(x,y)!=0
      /// where <paramref name="alpha"/> regulates update speed (how fast accumulator forgets about previous frames). 
      /// </summary>
      /// <param name="image">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently). </param>
      /// <param name="acc">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point. </param>
      /// <param name="alpha">Weight of input image</param>
      /// <param name="mask">Optional operation mask</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvRunningAvg(IntPtr image, IntPtr acc, double alpha, IntPtr mask);
      #endregion

      /// <summary>
      /// Converts a rotation vector to rotation matrix or vice versa. Rotation vector is a compact representation of rotation matrix. Direction of the rotation vector is the rotation axis and the length of the vector is the rotation angle around the axis. 
      /// </summary>
      /// <param name="src">The input rotation vector (3x1 or 1x3) or rotation matrix (3x3). </param>
      /// <param name="dst">The output rotation matrix (3x3) or rotation vector (3x1 or 1x3), respectively</param>
      /// <param name="jacobian">Optional output Jacobian matrix, 3x9 or 9x3 - partial derivatives of the output array components w.r.t the input array components</param>
      /// <returns></returns>
      [DllImport(CV_LIBRARY)]
      public static extern int cvRodrigues2(IntPtr src, IntPtr dst, IntPtr jacobian);

      /// <summary>
      /// Calculates seven Hu invariants
      /// </summary>
      /// <param name="moments">Pointer to the moment state structure</param>
      /// <param name="hu_moments">Pointer to Hu moments structure.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvGetHuMoments(ref MCvMoments moments, ref MCvHuMoments hu_moments);

      #region Kalman Filter
      /// <summary>
      /// Allocates CvKalman and all its matrices and initializes them somehow. 
      /// </summary>
      /// <param name="dynamParams">dimensionality of the state vector</param>
      /// <param name="measureParams">dimensionality of the measurement vector </param>
      /// <param name="controlParams">dimensionality of the control vector </param>
      /// <returns>Pointer to the created Kalman filter</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvCreateKalman(int dynamParams, int measureParams, int controlParams);

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state.
      /// The function stores adjusted state at kalman->state_post and returns it on output
      /// </summary>
      /// <param name="kalman">Pointer to the structure to be updated</param>
      /// <param name="measurement">Pointer to the structure CvMat containing the measurement vector</param>
      /// <returns>The function stores adjusted state at kalman->state_post and returns it on output</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanCorrect(IntPtr kalman, IntPtr measurement);

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state.
      /// The function stores adjusted state at kalman->state_post and returns it on output
      /// </summary>
      /// <param name="kalman">Pointer to the structure to be updated</param>
      /// <param name="measurement">Pointer to the structure CvMat containing the measurement vector</param>
      /// <returns>The function stores adjusted state at kalman->state_post and returns it on output</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanCorrect(ref MCvKalman kalman, IntPtr measurement);

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
      /// The function returns the estimated state
      /// </summary>
      /// <param name="kalman">Kalman filter state</param>
      /// <param name="control">Control vector (uk), should be NULL iff there is no external control (controlParams=0). </param>
      /// <returns>the estimated state</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanPredict(IntPtr kalman, IntPtr control);

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
      /// The function returns the estimated state
      /// </summary>
      /// <param name="kalman">Kalman filter state</param>
      /// <param name="control">Control vector (uk), should be NULL iff there is no external control (controlParams=0). </param>
      /// <returns>the estimated state</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvKalmanPredict(ref MCvKalman kalman, IntPtr control);

      /// <summary>
      /// Releases the structure CvKalman and all underlying matrices
      /// </summary>
      /// <param name="kalman">reference of the pointer to the Kalman filter structure.</param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvReleaseKalman(ref IntPtr kalman);
      #endregion

      /// <summary>
      /// Updates the motion history image as following:
      /// mhi(x,y)=timestamp  if silhouette(x,y)!=0
      ///         0          if silhouette(x,y)=0 and mhi(x,y)&lt;timestamp-duration
      ///         mhi(x,y)   otherwise
      /// That is, MHI pixels where motion occurs are set to the current timestamp, while the pixels where motion happened far ago are cleared. 
      /// </summary>
      /// <param name="silhouette">Silhouette mask that has non-zero pixels where the motion occurs. </param>
      /// <param name="mhi">Motion history image, that is updated by the function (single-channel, 32-bit floating-point) </param>
      /// <param name="timestamp">Current time in milliseconds or other units. </param>
      /// <param name="duration">Maximal duration of motion track in the same units as timestamp. </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvUpdateMotionHistory(
          IntPtr silhouette,
          IntPtr mhi,
          double timestamp,
          double duration);

      /// <summary>
      /// Calculates the derivatives Dx and Dy of mhi and then calculates gradient orientation as:
      ///orientation(x,y)=arctan(Dy(x,y)/Dx(x,y))
      ///where both Dx(x,y)' and Dy(x,y)' signs are taken into account (as in cvCartToPolar function). After that mask is filled to indicate where the orientation is valid (see delta1 and delta2 description). 
      /// </summary>
      /// <param name="mhi">Motion history image</param>
      /// <param name="mask">Mask image; marks pixels where motion gradient data is correct. Output parameter.</param>
      /// <param name="orientation">Motion gradient orientation image; contains angles from 0 to ~360. </param>
      /// <param name="delta1">The function finds minimum (m(x,y)) and maximum (M(x,y)) mhi values over each pixel (x,y) neihborhood and assumes the gradient is valid only if min(delta1,delta2) &lt;= M(x,y)-m(x,y) &lt;= max(delta1,delta2). </param>
      /// <param name="delta2">The function finds minimum (m(x,y)) and maximum (M(x,y)) mhi values over each pixel (x,y) neihborhood and assumes the gradient is valid only if min(delta1,delta2) &lt;= M(x,y)-m(x,y) &lt;= max(delta1,delta2).</param>
      /// <param name="apertureSize">Aperture size of derivative operators used by the function: CV_SCHARR, 1, 3, 5 or 7 (see cvSobel). </param>
      [DllImport(CV_LIBRARY)]
      public static extern void cvCalcMotionGradient(
          IntPtr mhi,
          IntPtr mask,
          IntPtr orientation,
          double delta1,
          double delta2,
          int apertureSize);

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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
      public static extern void cvFindCornerSubPix(
         IntPtr image,
         float[,] corners,
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      [DllImport(CV_LIBRARY)]
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
      /// desctibed in [RubnerSept98] is multi-dimensional histogram comparison
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
      [DllImport(CV_LIBRARY)]
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

      /// <summary>
      /// Retrieve the star keypoint location from the specific image
      /// </summary>
      /// <param name="img">The image to detect start keypoints</param>
      /// <param name="storage">The storage for the returned sequence</param>
      /// <param name="param">The star detector parameters</param>
      /// <returns>Pointer to the sequence of star keypoint locations</returns>
      [DllImport(CV_LIBRARY)]
      public static extern IntPtr cvGetStarKeypoints(
         IntPtr img,
         IntPtr storage,
         StarDetector param);

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
