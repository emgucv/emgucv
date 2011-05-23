//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /// <summary>
      /// Implements CAMSHIFT object tracking algorithm ([Bradski98]). First, it finds an object center using cvMeanShift and, after that, calculates the object size and orientation. 
      /// </summary>
      /// <param name="probImage">Back projection of object histogram </param>
      /// <param name="window">Initial search window</param>
      /// <param name="criteria">Criteria applied to determine when the window search should be finished</param>
      /// <param name="comp">Resultant structure that contains converged search window coordinates (comp->rect field) and sum of all pixels inside the window (comp->area field).</param>
      /// <param name="box">Circumscribed box for the object. If not IntPtr.Zero, contains object size and orientation</param>
      /// <returns>The number of iterations made within cvMeanShift</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvCamShift(
         IntPtr probImage,
         Rectangle window,
         MCvTermCriteria criteria,
         out MCvConnectedComp comp,
         out MCvBox2D box);

      /// <summary>
      /// Iterates to find the object center given its back projection and initial position of search window. The iterations are made until the search window center moves by less than the given value and/or until the function has done the maximum number of iterations. 
      /// </summary>
      /// <param name="probImage">Back projection of object histogram</param>
      /// <param name="window">Initial search window</param>
      /// <param name="criteria">Criteria applied to determine when the window search should be finished. </param>
      /// <param name="comp">Resultant structure that contains converged search window coordinates (comp->rect field) and sum of all pixels inside the window (comp->area field). </param>
      /// <returns>The number of iterations made</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvMeanShift(
         IntPtr probImage,
         Rectangle window,
         MCvTermCriteria criteria,
         out MCvConnectedComp comp);

      #region Optical flow
      /// <summary>
      /// Computes flow for every pixel of the first input image using Lucas &amp; Kanade algorithm
      /// </summary>
      /// <param name="prev">First image, 8-bit, single-channel.</param>
      /// <param name="curr">Second image, 8-bit, single-channel.</param>
      /// <param name="winSize">Size of the averaging window used for grouping pixels. </param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images, 32-bit floating-point, single-channel.</param>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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

      #region motion history
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcMotionGradient(
          IntPtr mhi,
          IntPtr mask,
          IntPtr orientation,
          double delta1,
          double delta2,
          int apertureSize);

      /// <summary>
      /// Finds all the motion segments and marks them in segMask with individual values each (1,2,...). It also returns a sequence of CvConnectedComp structures, one per each motion components. After than the motion direction for every component can be calculated with cvCalcGlobalOrientation using extracted mask of the particular component (using cvCmp) 
      /// </summary>
      /// <param name="mhi">Motion history image</param>
      /// <param name="segMask">Image where the mask found should be stored, single-channel, 32-bit floating-point</param>
      /// <param name="storage">Memory storage that will contain a sequence of motion connected components</param>
      /// <param name="timestamp">Current time in milliseconds or other units</param>
      /// <param name="segThresh">Segmentation threshold; recommended to be equal to the interval between motion history "steps" or greater</param>
      /// <returns>Pointer to the sequence of MCvConnectedComp</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvCalcGlobalOrientation(
                  IntPtr orientation,
                  IntPtr mask,
                  IntPtr mhi,
                  double timestamp,
                  double duration);
      #endregion

      #region background / foreground  statistic
      /// <summary>
      /// Create a Gaussian background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="param">Parameters for the background model</param>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateGaussianBGModel(IntPtr image, IntPtr param);

      /// <summary>
      /// Create a background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="param">Parameters for the background model</param>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateFGDStatModel(IntPtr image, IntPtr param);

      /// <summary>
      /// Create a foreground model
      /// </summary>
      /// <param name="firstFrame">The first frame</param>
      /// <param name="parameters">The foreground statistic parameters</param>
      /// <returns>Pointer to the foreground model</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateFGDStatModel(IntPtr firstFrame, ref MCvFGDStatModelParams parameters);

      /// <summary>
      /// Create a Gaussian background model
      /// </summary>
      /// <param name="image">Background image</param>
      /// <param name="parameters">Parameters for the background model</param>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateGaussianBGModel(IntPtr image, ref MCvGaussBGStatModelParams parameters);
      #endregion

      #region Codebook background model
      /// <summary>
      /// Create a BG code book model
      /// </summary>
      /// <returns>Poionter to BG code book model</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvCreateBGCodeBookModel();

      /// <summary>
      /// Update the BG code book model
      /// </summary>
      /// <param name="model">Pointer to the BGCodeBookModel</param>
      /// <param name="image">The image for update</param>
      /// <param name="roi">The update roi, use Rectangle.Empty for the whole image</param>
      /// <param name="mask">Can be IntPtr.Zero if not needed. The update mask. </param>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvBGCodeBookClearStale(
         ref MCvBGCodeBookModel model,
         int staleThresh,
         Rectangle roi,
         IntPtr mask);


      /// <summary>
      /// Release the BG code book model
      /// </summary>
      /// <param name="model">The BG code book model to be released</param>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvReleaseBGCodeBookModel(ref IntPtr model);
      #endregion

      #region Kalman Filter
      /// <summary>
      /// Allocates CvKalman and all its matrices and initializes them somehow. 
      /// </summary>
      /// <param name="dynamParams">dimensionality of the state vector</param>
      /// <param name="measureParams">dimensionality of the measurement vector </param>
      /// <param name="controlParams">dimensionality of the control vector </param>
      /// <returns>Pointer to the created Kalman filter</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateKalman(int dynamParams, int measureParams, int controlParams);

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state.
      /// The function stores adjusted state at kalman->state_post and returns it on output
      /// </summary>
      /// <param name="kalman">Pointer to the structure to be updated</param>
      /// <param name="measurement">Pointer to the structure CvMat containing the measurement vector</param>
      /// <returns>The function stores adjusted state at kalman->state_post and returns it on output</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvKalmanCorrect(IntPtr kalman, IntPtr measurement);

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state.
      /// The function stores adjusted state at kalman->state_post and returns it on output
      /// </summary>
      /// <param name="kalman">Pointer to the structure to be updated</param>
      /// <param name="measurement">Pointer to the structure CvMat containing the measurement vector</param>
      /// <returns>The function stores adjusted state at kalman->state_post and returns it on output</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvKalmanCorrect(ref MCvKalman kalman, IntPtr measurement);

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
      /// The function returns the estimated state
      /// </summary>
      /// <param name="kalman">Kalman filter state</param>
      /// <param name="control">Control vector (uk), should be NULL iff there is no external control (controlParams=0). </param>
      /// <returns>the estimated state</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvKalmanPredict(IntPtr kalman, IntPtr control);

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
      /// The function returns the estimated state
      /// </summary>
      /// <param name="kalman">Kalman filter state</param>
      /// <param name="control">Control vector (uk), should be NULL iff there is no external control (controlParams=0). </param>
      /// <returns>the estimated state</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvKalmanPredict(ref MCvKalman kalman, IntPtr control);

      /// <summary>
      /// Releases the structure CvKalman and all underlying matrices
      /// </summary>
      /// <param name="kalman">reference of the pointer to the Kalman filter structure.</param>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseKalman(ref IntPtr kalman);
      #endregion

      /// <summary>
      /// Estimate rigid transformation between 2 images or 2 point sets.
      /// </summary>
      /// <param name="A">First image or 2D point set (as a 2 channel Matrix&lt;float&gt;)</param>
      /// <param name="B">First image or 2D point set (as a 2 channel Matrix&lt;float&gt;)</param>
      /// <param name="M">The resulting Matrix&lt;double&gt; that represent the affine transformation</param>
      /// <param name="fullAffine">Indicates if full affine should be performed</param>
      /// <returns>True if eatimated sucessfully, false otherwise</returns>
      [DllImport(OPENCV_VIDEO_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvEstimateRigidTransform(
         IntPtr A, 
         IntPtr B, 
         IntPtr M, 
         [MarshalAs(CvInvoke.BoolToIntMarshalType)]
         bool fullAffine);
   }
}
