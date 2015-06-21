//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
      /// <returns>Circumscribed box for the object, contains object size and orientation</returns>
      public static RotatedRect CamShift(
         IInputArray probImage,
         ref Rectangle window,
         MCvTermCriteria criteria)
      {
         RotatedRect box = new RotatedRect();
         using (InputArray iaProbImage = probImage.GetInputArray())
         {
            cveCamShift(iaProbImage, ref window, ref criteria, ref box);
         }
         return box;
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCamShift(
         IntPtr probImage,
         ref Rectangle window,
         ref MCvTermCriteria criteria,
         ref RotatedRect box);

      /// <summary>
      /// Iterates to find the object center given its back projection and initial position of search window. The iterations are made until the search window center moves by less than the given value and/or until the function has done the maximum number of iterations. 
      /// </summary>
      /// <param name="probImage">Back projection of object histogram</param>
      /// <param name="window">Initial search window</param>
      /// <param name="criteria">Criteria applied to determine when the window search should be finished. </param>
      /// <returns>The number of iterations made</returns>
      public static int MeanShift(
         IInputArray probImage,
         ref Rectangle window,
         MCvTermCriteria criteria)
      {
         using (InputArray iaProbImage = probImage.GetInputArray())
            return cveMeanShift(iaProbImage, ref window, ref criteria);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int cveMeanShift(
         IntPtr probImage,
         ref Rectangle window,
         ref MCvTermCriteria criteria);

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
      public static void UpdateMotionHistory(IInputArray silhouette, IInputOutputArray mhi, double timestamp, double duration)
      {
         using (InputArray iaSilhouette = silhouette.GetInputArray())
         using (InputOutputArray ioaMhi = mhi.GetInputOutputArray())
            cveUpdateMotionHistory(iaSilhouette, ioaMhi, timestamp, duration);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveUpdateMotionHistory(
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
      public static void CalcMotionGradient(
         IInputArray mhi,
         IOutputArray mask,
         IOutputArray orientation,
         double delta1,
         double delta2,
         int apertureSize = 3)
      {
         using (InputArray iaMhi = mhi.GetInputArray())
         using (OutputArray oaMask = mask.GetOutputArray())
         using (OutputArray oaOrientation = orientation.GetOutputArray())
            cveCalcMotionGradient(iaMhi, oaMask, oaOrientation, delta1, delta2, apertureSize);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCalcMotionGradient(
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
      /// <param name="timestamp">Current time in milliseconds or other units</param>
      /// <param name="segThresh">Segmentation threshold; recommended to be equal to the interval between motion history "steps" or greater</param>
      /// <param name="boundingRects">Vector containing ROIs of motion connected components.</param>
      public static void SegmentMotion(
         IInputArray mhi,
         IOutputArray segMask,
         VectorOfRect boundingRects,
         double timestamp,
         double segThresh)
      {
         using (InputArray iaMhi = mhi.GetInputArray())
         using (OutputArray oaSegMask = segMask.GetOutputArray())
         {
            cveSegmentMotion(iaMhi, oaSegMask, boundingRects, timestamp, segThresh);
         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveSegmentMotion(
          IntPtr mhi,
          IntPtr segMask,
          IntPtr boundingRects,
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
      public static double CalcGlobalOrientation(
         IInputArray orientation,
         IInputArray mask,
         IInputArray mhi,
         double timestamp,
         double duration)
      {
         using (InputArray iaOrientation = orientation.GetInputArray())
         using (InputArray iaMask = mask.GetInputArray())
         using (InputArray iaMhi = mhi.GetInputArray())
            return cveCalcGlobalOrientation(iaOrientation, iaMask, iaMhi, timestamp, duration);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cveCalcGlobalOrientation(IntPtr orientation, IntPtr mask, IntPtr mhi, double timestamp, double duration);
      #endregion

      /*
      #region Kalman Filter
      /// <summary>
      /// Allocates CvKalman and all its matrices and initializes them somehow. 
      /// </summary>
      /// <param name="dynamParams">dimensionality of the state vector</param>
      /// <param name="measureParams">dimensionality of the measurement vector </param>
      /// <param name="controlParams">dimensionality of the control vector </param>
      /// <returns>Pointer to the created Kalman filter</returns>
      [DllImport(OpencvVideoLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateKalman(int dynamParams, int measureParams, int controlParams);

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state.
      /// The function stores adjusted state at kalman->state_post and returns it on output
      /// </summary>
      /// <param name="kalman">Pointer to the structure to be updated</param>
      /// <param name="measurement">Pointer to the structure CvMat containing the measurement vector</param>
      /// <returns>The function stores adjusted state at kalman->state_post and returns it on output</returns>
      [DllImport(OpencvVideoLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvKalmanCorrect(ref MCvKalman kalman, IntPtr measurement);


      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
      /// The function returns the estimated state
      /// </summary>
      /// <param name="kalman">Kalman filter state</param>
      /// <param name="control">Control vector (uk), should be NULL iff there is no external control (controlParams=0). </param>
      /// <returns>the estimated state</returns>
      [DllImport(OpencvVideoLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvKalmanPredict(ref MCvKalman kalman, IntPtr control);

      /// <summary>
      /// Releases the structure CvKalman and all underlying matrices
      /// </summary>
      /// <param name="kalman">reference of the pointer to the Kalman filter structure.</param>
      [DllImport(OpencvVideoLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseKalman(ref IntPtr kalman);
      #endregion
*/
      #region optical flow
      /// <summary>
      /// Calculates optical flow for a sparse feature set using iterative Lucas-Kanade method in pyramids
      /// </summary>
      /// <param name="prev">First frame, at time t</param>
      /// <param name="curr">Second frame, at time t + dt </param>
      /// <param name="prevFeatures">Array of points for which the flow needs to be found</param>
      /// <param name="winSize">Size of the search window of each pyramid level</param>
      /// <param name="level">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc</param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped</param>
      /// <param name="flags">Flags</param>
      /// <param name="currFeatures">Array of 2D points containing calculated new positions of input features in the second image</param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise</param>
      /// <param name="trackError">Array of double numbers containing difference between patches around the original and moved points</param>
      /// <param name="minEigThreshold">the algorithm calculates the minimum eigen value of a 2x2 normal matrix of optical flow equations (this matrix is called a spatial gradient matrix in [Bouguet00]), divided by number of pixels in a window; if this value is less than minEigThreshold, then a corresponding feature is filtered out and its flow is not processed, so it allows to remove bad points and get a performance boost.</param>
      public static void CalcOpticalFlowPyrLK(
         IInputArray prev,
         IInputArray curr,
         PointF[] prevFeatures,
         Size winSize,
         int level,
         MCvTermCriteria criteria,
         out PointF[] currFeatures,
         out Byte[] status,
         out float[] trackError,
         Emgu.CV.CvEnum.LKFlowFlag flags = CvEnum.LKFlowFlag.Default,
         double minEigThreshold = 1.0e-4)
      {
         using (Util.VectorOfPointF prevPts = new Util.VectorOfPointF())
         using (Util.VectorOfPointF nextPts = new Util.VectorOfPointF())
         using (Util.VectorOfByte statusVec = new Util.VectorOfByte())
         using (Util.VectorOfFloat errorVec = new Util.VectorOfFloat())
         {
            prevPts.Push(prevFeatures);

            CalcOpticalFlowPyrLK(
               prev,
               curr,
               prevPts,
               nextPts,
               statusVec,
               errorVec,
               winSize,
               level,
               criteria,
               flags,
               minEigThreshold);
            status = statusVec.ToArray();
            trackError = errorVec.ToArray();
            currFeatures = nextPts.ToArray();
         }
      }
      /// <summary>
      /// Implements sparse iterative version of Lucas-Kanade optical flow in pyramids ([Bouguet00]). It calculates coordinates of the feature points on the current video frame given their coordinates on the previous frame. The function finds the coordinates with sub-pixel accuracy. 
      /// </summary>
      /// <remarks>Both parameters prev_pyr and curr_pyr comply with the following rules: if the image pointer is 0, the function allocates the buffer internally, calculates the pyramid, and releases the buffer after processing. Otherwise, the function calculates the pyramid and stores it in the buffer unless the flag CV_LKFLOW_PYR_A[B]_READY is set. The image should be large enough to fit the Gaussian pyramid data. After the function call both pyramids are calculated and the readiness flag for the corresponding image can be set in the next call (i.e., typically, for all the image pairs except the very first one CV_LKFLOW_PYR_A_READY is set). </remarks>
      /// <param name="prevImg">First frame, at time t. </param>
      /// <param name="nextImg">Second frame, at time t + dt .</param>
      /// <param name="prevPts">Array of points for which the flow needs to be found. </param>
      /// <param name="nextPts">Array of 2D points containing calculated new positions of input </param>
      /// <param name="winSize">Size of the search window of each pyramid level.</param>
      /// <param name="maxLevel">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc. </param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise.</param>
      /// <param name="err">Array of double numbers containing difference between patches around the original and moved points. Optional parameter; can be NULL </param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped.</param>
      /// <param name="flags">Miscellaneous flags</param>
      /// <param name="minEigThreshold">the algorithm calculates the minimum eigen value of a 2x2 normal matrix of optical flow equations (this matrix is called a spatial gradient matrix in [Bouguet00]), divided by number of pixels in a window; if this value is less than minEigThreshold, then a corresponding feature is filtered out and its flow is not processed, so it allows to remove bad points and get a performance boost.</param>
      public static void CalcOpticalFlowPyrLK(
         IInputArray prevImg,
         IInputArray nextImg,
         IInputArray prevPts,
         IInputOutputArray nextPts,
         IOutputArray status,
         IOutputArray err,
         Size winSize,
         int maxLevel,
         MCvTermCriteria criteria,
         CvEnum.LKFlowFlag flags = CvEnum.LKFlowFlag.Default,
         double minEigThreshold = 1.0e-4)
      {
         using (InputArray iaPrevImg = prevImg.GetInputArray())
         using (InputArray iaNextImg = nextImg.GetInputArray())
         using (InputArray iaPrevPts = prevPts.GetInputArray())
         using (InputOutputArray ioaNextPts = nextPts.GetInputOutputArray())
         using (OutputArray oaStatus = status.GetOutputArray())
         using (OutputArray oaErr = err.GetOutputArray())
            cveCalcOpticalFlowPyrLK(iaPrevImg, iaNextImg, iaPrevPts, ioaNextPts, oaStatus, oaErr, ref winSize, maxLevel, ref criteria, flags, minEigThreshold);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCalcOpticalFlowPyrLK(
         IntPtr prevImg,
         IntPtr nextImg,
         IntPtr prevPts,
         IntPtr nextPts,
         IntPtr status,
         IntPtr err,
         ref Size winSize,
         int maxLevel,
         ref MCvTermCriteria criteria,
         CvEnum.LKFlowFlag flags,
         double minEigenThreshold);

      /// <summary>
      /// Computes dense optical flow using Gunnar Farneback's algorithm
      /// </summary>
      /// <param name="prev0">The first 8-bit single-channel input image</param>
      /// <param name="next0">The second input image of the same size and the same type as prevImg</param>
      /// <param name="flowX">The computed flow image for x-velocity; will have the same size as prevImg</param>
      /// <param name="flowY">The computed flow image for y-velocity; will have the same size as prevImg</param>
      /// <param name="pyrScale">Specifies the image scale (!1) to build the pyramids for each image. pyrScale=0.5 means the classical pyramid, where each next layer is twice smaller than the previous</param>
      /// <param name="levels">The number of pyramid layers, including the initial image. levels=1 means that no extra layers are created and only the original images are used</param>
      /// <param name="winSize">The averaging window size; The larger values increase the algorithm robustness to image noise and give more chances for fast motion detection, but yield more blurred motion field</param>
      /// <param name="iterations">The number of iterations the algorithm does at each pyramid level</param>
      /// <param name="polyN">Size of the pixel neighborhood used to find polynomial expansion in each pixel. The larger values mean that the image will be approximated with smoother surfaces, yielding more robust algorithm and more blurred motion field. Typically, poly n=5 or 7</param>
      /// <param name="polySigma">Standard deviation of the Gaussian that is used to smooth derivatives that are used as a basis for the polynomial expansion. For poly n=5 you can set poly sigma=1.1, for poly n=7 a good value would be poly sigma=1.5</param>
      /// <param name="flags">The operation flags</param>
      public static void CalcOpticalFlowFarneback(
         Image<Gray, Byte> prev0,
         Image<Gray, Byte> next0,
         Image<Gray, Single> flowX,
         Image<Gray, Single> flowY,
         double pyrScale,
         int levels,
         int winSize,
         int iterations,
         int polyN,
         double polySigma,
         CvEnum.OpticalflowFarnebackFlag flags)
      {
         using (Mat flow0 = new Mat(prev0.Height, prev0.Width, CvEnum.DepthType.Cv32F, 2))
         using (Util.VectorOfMat vm = new Util.VectorOfMat(new Mat[] { flowX.Mat, flowY.Mat }))
         {
            if ((int)(flags & Emgu.CV.CvEnum.OpticalflowFarnebackFlag.UseInitialFlow) != 0)
            {  //use initial flow
               CvInvoke.Merge(vm, flow0);
            }

            CvInvoke.CalcOpticalFlowFarneback(prev0, next0, flow0, pyrScale, levels, winSize, iterations, polyN, polySigma, flags);
            CvInvoke.Split(flow0, vm);
         }
      }

      /// <summary>
      /// Computes dense optical flow using Gunnar Farneback's algorithm
      /// </summary>
      /// <param name="prev0">The first 8-bit single-channel input image</param>
      /// <param name="next0">The second input image of the same size and the same type as prevImg</param>
      /// <param name="flow">The computed flow image; will have the same size as prevImg and type CV 32FC2</param>
      /// <param name="pyrScale">Specifies the image scale (!1) to build the pyramids for each image. pyrScale=0.5 means the classical pyramid, where each next layer is twice smaller than the previous</param>
      /// <param name="levels">The number of pyramid layers, including the initial image. levels=1 means that no extra layers are created and only the original images are used</param>
      /// <param name="winSize">The averaging window size; The larger values increase the algorithm robustness to image noise and give more chances for fast motion detection, but yield more blurred motion field</param>
      /// <param name="iterations">The number of iterations the algorithm does at each pyramid level</param>
      /// <param name="polyN">Size of the pixel neighborhood used to find polynomial expansion in each pixel. The larger values mean that the image will be approximated with smoother surfaces, yielding more robust algorithm and more blurred motion field. Typically, poly n=5 or 7</param>
      /// <param name="polySigma">Standard deviation of the Gaussian that is used to smooth derivatives that are used as a basis for the polynomial expansion. For poly n=5 you can set poly sigma=1.1, for poly n=7 a good value would be poly sigma=1.5</param>
      /// <param name="flags">The operation flags</param>
      public static void CalcOpticalFlowFarneback(
         IInputArray prev0,
         IInputArray next0,
         IInputOutputArray flow,
         double pyrScale,
         int levels,
         int winSize,
         int iterations,
         int polyN,
         double polySigma,
         CvEnum.OpticalflowFarnebackFlag flags)
      {
         using (InputArray iaPrev0 = prev0.GetInputArray())
         using (InputArray iaNext0 = next0.GetInputArray())
         using (InputOutputArray ioaFlow = flow.GetInputOutputArray())
         cveCalcOpticalFlowFarneback(iaPrev0, iaNext0, ioaFlow, pyrScale, levels, winSize, iterations, polyN, polySigma, flags);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void cveCalcOpticalFlowFarneback(
         IntPtr prev0,
         IntPtr next0,
         IntPtr flow0,
         double pyrScale,
         int levels,
         int winSize,
         int iterations,
         int polyN,
         double polySigma,
         CvEnum.OpticalflowFarnebackFlag flags);

      #endregion


      /// <summary>
      /// Estimate rigid transformation between 2 point sets.
      /// </summary>
      /// <param name="sourcePoints">The points from the source image</param>
      /// <param name="destinationPoints">The corresponding points from the destination image</param>
      /// <param name="fullAffine">Indicates if full affine should be performed</param>
      /// <returns>If success, the 2x3 rotation matrix that defines the Affine transform. Otherwise null is returned.</returns>
      public static Mat EstimateRigidTransform(PointF[] sourcePoints, PointF[] destinationPoints, bool fullAffine)
      {
         using (VectorOfPointF srcVec = new VectorOfPointF(sourcePoints))
         using (VectorOfPointF dstVec = new VectorOfPointF(destinationPoints))
            return EstimateRigidTransform(srcVec, dstVec, fullAffine);
      }

      /// <summary>
      /// Estimate rigid transformation between 2 images or 2 point sets.
      /// </summary>
      /// <param name="src">First image or 2D point set (as a 2 channel Matrix&lt;float&gt;)</param>
      /// <param name="dst">First image or 2D point set (as a 2 channel Matrix&lt;float&gt;)</param>
      /// <param name="fullAffine">Indicates if full affine should be performed</param>
      /// <returns>The resulting Matrix&lt;double&gt; that represent the affine transformation</returns>
      public static Mat EstimateRigidTransform(IInputArray src, IInputArray dst, bool fullAffine)
      {
         Mat result = new Mat();
         using (InputArray iaSrc = src.GetInputArray())
         using (InputArray iaDst = dst.GetInputArray())
            cveEstimateRigidTransform(iaSrc, iaDst, fullAffine, result);
         return result;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern bool cveEstimateRigidTransform(
         IntPtr src,
         IntPtr dst,
         [MarshalAs(CvInvoke.BoolToIntMarshalType)]
         bool fullAffine, 
         IntPtr result);
   }
}
