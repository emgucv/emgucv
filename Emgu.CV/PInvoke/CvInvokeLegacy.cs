/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      
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
      /// Gradient flag. If != 0, the function calculates gradient magnitude for every image pixel and considers it as the energy field, 
      /// otherwise the input image itself is considered
      /// </param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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

      #region Codebook background model
      /// <summary>
      /// Create a BG code book model
      /// </summary>
      /// <returns>Poionter to BG code book model</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateBGCodeBookModel();

      /// <summary>
      /// Update the BG code book model
      /// </summary>
      /// <param name="model">Pointer to the BGCodeBookModel</param>
      /// <param name="image">The image for update</param>
      /// <param name="roi">The update roi, use Rectangle.Empty for the whole image</param>
      /// <param name="mask">Can be IntPtr.Zero if not needed. The update mask. </param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvBGCodeBookUpdate(
         IntPtr model,
         IntPtr image,
         Rectangle roi,
         IntPtr mask);

      /// <summary>
      /// Update the BG code book model
      /// </summary>
      /// <param name="model">The BGCodeBookModel</param>
      /// <param name="image">The image for update</param>
      /// <param name="roi">The update roi, use Rectangle.Empty for the whole image</param>
      /// <param name="mask">Can be IntPtr.Zero if not needed. The update mask. </param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvBGCodeBookUpdate(
         ref MCvBGCodeBookModel model,
         IntPtr image,
         Rectangle roi,
         IntPtr mask);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="model">Pointer to the BGCodeBookModel</param>
      /// <param name="image">The image to find diff</param>
      /// <param name="fgmask">The returned foreground mask</param>
      /// <param name="roi">The region of interest for the diff. Use Rectangle.Empty for the whole image</param>
      /// <returns></returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int cvBGCodeBookDiff(
         IntPtr model,
         IntPtr image,
         IntPtr fgmask,
         Rectangle roi);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="model">Pointer to the BGCodeBookModel</param>
      /// <param name="image">The image to find diff</param>
      /// <param name="fgmask">The returned foreground mask</param>
      /// <param name="roi">The region of interest for the diff. Use Rectangle.Empty for the whole image</param>
      /// <returns></returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int cvBGCodeBookDiff(
         ref MCvBGCodeBookModel model,
         IntPtr image,
         IntPtr fgmask,
         Rectangle roi);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="model"></param>
      /// <param name="staleThresh"></param>
      /// <param name="roi"></param>
      /// <param name="mask"></param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvBGCodeBookClearStale(
         IntPtr model,
         int staleThresh,
         Rectangle roi,
         IntPtr mask);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="model"></param>
      /// <param name="staleThresh"></param>
      /// <param name="roi"></param>
      /// <param name="mask"></param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvBGCodeBookClearStale(
         ref MCvBGCodeBookModel model,
         int staleThresh,
         Rectangle roi,
         IntPtr mask);


      /// <summary>
      /// Release the BG code book model
      /// </summary>
      /// <param name="model">The BG code book model to be released</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvReleaseBGCodeBookModel(ref IntPtr model);
      #endregion

      #region background / foreground  statistic
      /// <summary>
      /// Releases memory used by BGStatMode
      /// </summary>
      /// <param name="bgModel">The bgModel to be released</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvReleaseBGStatModel(ref IntPtr bgModel);

      /// <summary>
      /// Updates statistical model and returns number of found foreground regions
      /// </summary>
      /// <param name="currentFrame">The current frame</param>
      /// <param name="bgModel">The bg model</param>
      /// <param name="learningRate">The leaning rate, use -1 for default value</param>
      /// <returns>The number of found foreground regions</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int cvUpdateBGStatModel(IntPtr currentFrame, IntPtr bgModel,
                                double learningRate);

      /// <summary>
      /// Create a Gaussian background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="param">Parameters for the background model</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateGaussianBGModel(IntPtr image, IntPtr param);

      /// <summary>
      /// Create a background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="param">Parameters for the background model</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateFGDStatModel(IntPtr image, IntPtr param);

      /// <summary>
      /// Create a foreground model
      /// </summary>
      /// <param name="firstFrame">The first frame</param>
      /// <param name="parameters">The foreground statistic parameters</param>
      /// <returns>Pointer to the foreground model</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateFGDStatModel(IntPtr firstFrame, ref MCvFGDStatModelParams parameters);

      /// <summary>
      /// Create a Gaussian background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="parameters">Parameters for the background model</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateGaussianBGModel(IntPtr image, ref MCvGaussBGStatModelParams parameters);
      #endregion

      #region optical flow

      /// <summary>
      /// Computes flow for every pixel of the first input image using Lucas &amp; Kanade algorithm
      /// </summary>
      /// <param name="prev">First image</param>
      /// <param name="curr">Second image</param>
      /// <param name="winSize">Size of the averaging window used for grouping pixels</param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images</param>
      public static void cvCalcOpticalFlowLK(
         Image<Gray, Byte> prev,
         Image<Gray, Byte> curr,
         Size winSize,
         Image<Gray, Single> velx,
         Image<Gray, Single> vely)
      {
         CvInvoke.cvCalcOpticalFlowLK(prev.Ptr, curr.Ptr, winSize, velx, vely);
      }

      /// <summary>
      /// Computes flow for every pixel of the first input image using Lucas &amp; Kanade algorithm
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel.</param>
      /// <param name="curr">Second image, 8-bit, single-channel.</param>
      /// <param name="winSize">Size of the averaging window used for grouping pixels. </param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      public static void cvCalcOpticalFlowHS(
         Image<Gray, Byte> prev,
         Image<Gray, Byte> curr,
         bool usePrevious,
         Image<Gray, Single> velx,
         Image<Gray, Single> vely,
         double lambda,
         MCvTermCriteria criteria)
      {
         CvInvoke.cvCalcOpticalFlowHS(prev.Ptr, curr.Ptr, usePrevious, velx.Ptr, vely.Ptr, lambda, criteria);
      }

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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcOpticalFlowHS(
              IntPtr prev,
              IntPtr curr,
              bool usePrevious,
              IntPtr velx,
              IntPtr vely,
              double lambda,
              MCvTermCriteria criteria);

      /// <summary>
      /// Calculates optical flow for overlapped blocks block_size.width * block_size.height pixels each, thus the velocity fields are smaller than the original images. 
      /// For every block in prev the functions tries to find a similar block in curr in some neighborhood of the original block or shifted by (velx(x0,y0),vely(x0,y0)) block as has been calculated by previous function call (if use_previous)
      /// </summary>
      /// <param name="prev">First image</param>
      /// <param name="curr">Second image</param>
      /// <param name="blockSize">Size of basic blocks that are compared.</param>
      /// <param name="shiftSize">Block coordinate increments. </param>
      /// <param name="maxRange">Size of the scanned neighborhood in pixels around block.</param>
      /// <param name="usePrevious">Uses previous (input) velocity field. </param>
      /// <param name="velx">Horizontal component of the optical flow of floor((prev->width - block_size.width)/shiftSize.width) x floor((prev->height - block_size.height)/shiftSize.height) size. </param>
      /// <param name="vely">Vertical component of the optical flow of the same size velx.</param>
      public static void cvCalcOpticalFlowBM(
         Image<Gray, Byte> prev,
         Image<Gray, Byte> curr,
         Size blockSize,
         Size shiftSize,
         Size maxRange,
         bool usePrevious,
         Image<Gray, Single> velx,
         Image<Gray, Single> vely)
      {
         cvCalcOpticalFlowBM(prev, curr, blockSize, shiftSize, maxRange, usePrevious, velx, vely);
      }

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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcOpticalFlowBM(
              IntPtr prev,
              IntPtr curr,
              Size blockSize,
              Size shiftSize,
              Size maxRange,
              bool usePrevious,
              IntPtr velx,
              IntPtr vely);
      #endregion

      #region StereoGC
      /// <summary>
      /// Creates the stereo correspondence structure and initializes it. 
      /// </summary>
      /// <param name="numberOfDisparities">The number of disparities. The disparity search range will be state.minDisparity &lt;= disparity &lt; state.minDisparity + state.numberOfDisparities</param>
      /// <param name="maxIters">Maximum number of iterations. On each iteration all possible (or reasonable) alpha-expansions are tried. The algorithm may terminate earlier if it could not find an alpha-expansion that decreases the overall cost function value</param>
      /// <returns>The initialized stereo correspondence structure</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateStereoGCState(
         int numberOfDisparities,
         int maxIters);

      /// <summary>
      /// Releases the stereo correspondence structure and all the associated internal buffers
      /// </summary>
      /// <param name="state">A reference to the pointer of StereoGCState structure</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindStereoCorrespondenceGC(
         IntPtr left,
         IntPtr right,
         IntPtr dispLeft,
         IntPtr dispRight,
         ref MCvStereoGCState state,
         int useDisparityGuess);
      #endregion

      #region Feature Matching
      /// <summary>
      /// Constructs a balanced kd-tree index of the given feature vectors. The lifetime of the desc matrix must exceed that of the returned tree. I.e., no copy is made of the vectors.
      /// </summary>
      /// <param name="desc">n x d matrix of n d-dimensional feature vectors (CV_32FC1 or CV_64FC1)</param>
      /// <returns>A balanced kd-tree index of the given feature vectors</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateKDTree(IntPtr desc);

      /// <summary>
      /// Constructs a spill tree index of the given feature vectors. The lifetime of the desc matrix must exceed that of the returned tree. I.e., no copy is made of the vectors.
      /// </summary>
      /// <param name="desc">n x d matrix of n d-dimensional feature vectors (CV_32FC1 or CV_64FC1)</param>
      /// <param name="naive"></param>
      /// <param name="rho"></param>
      /// <param name="tau"></param>
      /// <returns>A spill tree index of the given feature vectors</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateSpillTree(IntPtr desc, int naive, double rho, double tau);

      /// <summary>
      /// Deallocates the given kd-tree
      /// </summary>
      /// <param name="tr">Pointer to tree being destroyed</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindFeaturesBoxed(
         IntPtr tr,
         IntPtr boundsMin,
         IntPtr boundsMax,
         IntPtr results);
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
         IntPtr subdiv = cvCreateSubdiv2D((int)CvEnum.SeqKind.Subdiv2D,
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvInitSubdivDelaunay2D(IntPtr subdiv, Rectangle rect);

      /// <summary>
      /// Locates input point within subdivision. It finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point. 
      /// </summary>
      /// <param name="subdiv">Delaunay or another subdivision</param>
      /// <param name="pt">Input point</param>
      /// <returns>pointer to the found subdivision vertex (CvSubdiv2DPoint)</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvSubdivDelaunay2DInsert(IntPtr subdiv, PointF pt);

      /// <summary>
      /// Locates input point within subdivision
      /// </summary>
      /// <param name="subdiv">Plannar subdivision</param>
      /// <param name="pt">The point to locate</param>
      /// <param name="edge">The output edge the point falls onto or right to</param>
      /// <param name="vertex">Optional output vertex double pointer the input point coincides with</param>
      /// <returns>The type of location for the point</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern CvEnum.Subdiv2DPointLocationType cvSubdiv2DLocate(IntPtr subdiv, PointF pt,
                                           out IntPtr edge,
                                           ref IntPtr vertex);

      /// <summary>
      /// Calculates coordinates of virtual points. All virtual points corresponding to some vertex of original subdivision form (when connected together) a boundary of Voronoi cell of that point
      /// </summary>
      /// <param name="subdiv">Delaunay subdivision, where all the points are added already</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcSubdivVoronoi2D(IntPtr subdiv);
      #endregion

      #region Eigen Objects
      #region cvEigenDecomposite function
      /// <summary>
      /// Calculates all decomposition coefficients for the input object using the previously calculated eigen objects basis and the averaged object
      /// </summary>
      /// <param name="obj">Input object (Pointer to IplImage)</param>
      /// <param name="eigInput">Pointer to the array of IplImage input objects</param>
      /// <param name="avg">Averaged object (Pointer to IplImage)</param>
      /// <returns>Calculated coefficients; an output parameter</returns>
      public static float[] cvEigenDecomposite(
         IntPtr obj,
         IntPtr[] eigInput,
         IntPtr avg)
      {
         float[] coeffs = new float[eigInput.Length];
         cvEigenDecomposite(
             obj,
             eigInput.Length,
             eigInput,
             CvEnum.EigobjType.NoCallback,
             IntPtr.Zero,
             avg,
             coeffs);
         return coeffs;
      }

      /// <summary>
      /// Calculates all decomposition coefficients for the input object using the previously calculated eigen objects basis and the averaged object
      /// </summary>
      /// <param name="obj">Input object (Pointer to IplImage)</param>
      /// <param name="eigenvecCount">Number of eigen objects</param>
      /// <param name="eigInput">Pointer either to the array of IplImage input objects or to the read callback function according to the value of the parameter <paramref name="ioFlags"/></param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="userData">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="avg">Averaged object (Pointer to IplImage)</param>
      /// <param name="coeffs">Calculated coefficients; an output parameter</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvEigenDecomposite(
         IntPtr obj,
         int eigenvecCount,
         IntPtr[] eigInput,
         CvEnum.EigobjType ioFlags,
         IntPtr userData,
         IntPtr avg,
         float[] coeffs);

      #endregion

      /// <summary>
      /// Calculates orthonormal eigen basis and the averaged object for a group of the input objects.
      /// </summary>
      /// <param name="input">Pointer to the array of IplImage input objects </param>
      /// <param name="calcLimit">Criteria that determine when to stop calculation of eigen objects. Depending on the parameter calcLimit, calculations are finished either after first calcLimit.max_iter dominating eigen objects are retrieved or if the ratio of the current eigenvalue to the largest eigenvalue comes down to calcLimit.epsilon threshold. The value calcLimit -> type must be CV_TERMCRIT_NUMB, CV_TERMCRIT_EPS, or CV_TERMCRIT_NUMB | CV_TERMCRIT_EPS . The function returns the real values calcLimit->max_iter and calcLimit->epsilon</param>
      /// <param name="avg">Averaged object</param>
      /// <param name="eigVals">Pointer to the eigenvalues array in the descending order; may be NULL</param>
      /// <param name="eigVecs">Pointer either to the array of eigen objects</param>
      /// <returns>Pointer either to the array of eigen objects or to the write callback function</returns>
      public static void cvCalcEigenObjects(
         IntPtr[] input,
         ref MCvTermCriteria calcLimit,
         IntPtr[] eigVecs,
         float[] eigVals,
         IntPtr avg)
      {
         cvCalcEigenObjects(
             input.Length,
             input,
             eigVecs,
             CvEnum.EigobjType.NoCallback,
             0,
             IntPtr.Zero,
             ref calcLimit,
             avg,
             eigVals);
      }

      /// <summary>
      /// Calculates orthonormal eigen basis and the averaged object for a group of the input objects.
      /// </summary>
      /// <param name="nObjects">Number of source objects</param>
      /// <param name="input">Pointer either to the array of IplImage input objects or to the read callback function</param>
      /// <param name="output">Pointer either to the array of eigen objects or to the write callback function</param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="ioBufSize">Input/output buffer size in bytes. The size is zero, if unknown</param>
      /// <param name="userData">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="calcLimit">Criteria that determine when to stop calculation of eigen objects. Depending on the parameter calcLimit, calculations are finished either after first calcLimit.max_iter dominating eigen objects are retrieved or if the ratio of the current eigenvalue to the largest eigenvalue comes down to calcLimit.epsilon threshold. The value calcLimit -> type must be CV_TERMCRIT_NUMB, CV_TERMCRIT_EPS, or CV_TERMCRIT_NUMB | CV_TERMCRIT_EPS . The function returns the real values calcLimit->max_iter and calcLimit->epsilon</param>
      /// <param name="avg">Averaged object</param>
      /// <param name="eigVals">Pointer to the eigenvalues array in the descending order; may be NULL</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvCalcEigenObjects(
         int nObjects,
         IntPtr[] input,
         IntPtr[] output,
         CvEnum.EigobjType ioFlags,
         int ioBufSize,
         IntPtr userData,
         ref MCvTermCriteria calcLimit,
         IntPtr avg,
         float[] eigVals);

      /// <summary>
      /// Calculates an object projection to the eigen sub-space or, in other words, restores an object using previously calculated eigen objects basis, averaged object, and decomposition coefficients of the restored object. 
      /// </summary>
      /// <param name="inputVecs">Pointer to either an array of IplImage input objects or to a callback function, depending on io_flags</param>
      /// <param name="coeffs">Previously calculated decomposition coefficients</param>
      /// <param name="avg">Average vector</param>
      /// <param name="proj">Projection to the eigen sub-space</param>
      public static void cvEigenProjection(
         IntPtr[] inputVecs,
         float[] coeffs,
         IntPtr avg,
         IntPtr proj)
      {
         CvInvoke.cvEigenProjection(
             inputVecs,
             inputVecs.Length,
             CvEnum.EigobjType.NoCallback,
             IntPtr.Zero,
             coeffs,
             avg,
             proj);
      }

      /// <summary>
      /// Calculates an object projection to the eigen sub-space or, in other words, restores an object using previously calculated eigen objects basis, averaged object, and decomposition coefficients of the restored object. Depending on io_flags parameter it may be used either in direct access or callback mode.
      /// </summary>
      /// <param name="inputVecs">Pointer to either an array of IplImage input objects or to a callback function, depending on io_flags</param>
      /// <param name="eigenvecCount">Number of eigenvectors</param>
      /// <param name="ioFlags">Input/output flags</param>
      /// <param name="userdata">Pointer to the structure that contains all necessary data for the callback functions</param>
      /// <param name="coeffs">Previously calculated decomposition coefficients</param>
      /// <param name="avg">Average vector</param>
      /// <param name="proj">Projection to the eigen sub-space</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void cvEigenProjection(
         IntPtr[] inputVecs,
         int eigenvecCount,
         CvEnum.EigobjType ioFlags,
         IntPtr userdata,
         float[] coeffs,
         IntPtr avg,
         IntPtr proj);
      #endregion

      #region Condensation
      /// <summary>
      /// Creates CvConDensation structure and returns pointer to the structure
      /// </summary>
      /// <param name="dynamParams">Dimension of the state vector</param>
      /// <param name="measureParams">Dimension of the measurement vector</param>
      /// <param name="sampleCount">Number of samples</param>
      /// <returns>Pointer to the CvConDensation structure</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateConDensation(int dynamParams, int measureParams, int sampleCount);

      /// <summary>
      /// Releases the structure CvConDensation (see cvConDensation) and frees all memory previously allocated for the structure. 
      /// </summary>
      /// <param name="condens">Pointer to the CvConDensation structure</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseConDensation(ref IntPtr condens);

      /// <summary>
      /// Fills the samples arrays in the structure CvConDensation with values within specified ranges. 
      /// </summary>
      /// <param name="condens">Pointer to a structure to be initialized</param>
      /// <param name="lowerBound">Vector of the lower boundary for each dimension</param>
      /// <param name="upperBound">Vector of the upper boundary for each dimension</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvConDensInitSampleSet(IntPtr condens, IntPtr lowerBound, IntPtr upperBound);

      /// <summary>
      /// Estimates the subsequent stochastic model state from its current state
      /// </summary>
      /// <param name="condens">Pointer to the structure to be updated</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvConDensUpdateByTime(IntPtr condens);
      #endregion

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
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPyrSegmentation(
          IntPtr src,
          IntPtr dst,
          IntPtr storage,
          out IntPtr comp,
          int level,
          double threshold1,
          double threshold2);

      /// <summary>
      /// Calculates 2D pair-wise geometrical histogram (PGH), described in [Iivarinen97], for the contour. The algorithm considers every pair of the contour edges. The angle between the edges and the minimum/maximum distances are determined for every pair. To do this each of the edges in turn is taken as the base, while the function loops through all the other edges. When the base edge and any other edge are considered, the minimum and maximum distances from the points on the non-base edge and line of the base edge are selected. The angle between the edges defines the row of the histogram in which all the bins that correspond to the distance between the calculated minimum and maximum distances are incremented (that is, the histogram is transposed relatively to [Iivarninen97] definition). The histogram can be used for contour matching
      /// </summary>
      /// <param name="contour">Input contour. Currently, only integer point coordinates are allowed</param>
      /// <param name="hist">Calculated histogram; must be two-dimensional</param>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcPGH(IntPtr contour, IntPtr hist);

      /// <summary>
      /// Checks planar subdivision for correctness. It is not an absolute check, but it verifies some relations between quad-edges
      /// </summary>
      /// <param name="subdiv">Pointer to the MCvSubdiv2D</param>
      /// <returns>True if valid, false otherwise</returns>
      [DllImport(OpencvLegacyLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool icvSubdiv2DCheck(IntPtr subdiv);

   }
}
*/