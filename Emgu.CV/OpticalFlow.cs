//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// Contains a collection of optical flow methods
   /// </summary>
   public static class OpticalFlow
   {
      /// <summary>
      /// Calculates optical flow for a sparse feature set using iterative Lucas-Kanade method in pyramids
      /// </summary>
      /// <param name="prev">First frame, at time t</param>
      /// <param name="curr">Second frame, at time t + dt </param>
      /// <param name="prevFeatures">Array of points for which the flow needs to be found</param>
      /// <param name="winSize">Size of the search window of each pyramid level</param>
      /// <param name="level">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc</param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped</param>
      /// <param name="currFeatures">Array of 2D points containing calculated new positions of input features in the second image</param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise</param>
      /// <param name="trackError">Array of double numbers containing difference between patches around the original and moved points</param>
      public static void PyrLK(
         Image<Gray, Byte> prev,
         Image<Gray, Byte> curr,
         PointF[] prevFeatures,
         Size winSize,
         int level,
         MCvTermCriteria criteria,
         out PointF[] currFeatures,
         out Byte[] status,
         out float[] trackError)
      {
         PyrLK(prev, curr, null, null, prevFeatures, winSize, level, criteria, Emgu.CV.CvEnum.LKFLOW_TYPE.DEFAULT, out currFeatures, out status, out trackError);
      }

      /// <summary>
      /// Calculates optical flow for a sparse feature set using iterative Lucas-Kanade method in pyramids
      /// </summary>
      /// <param name="prev">First frame, at time t</param>
      /// <param name="curr">Second frame, at time t + dt </param>
      /// <param name="prevPyrBuffer">Buffer for the pyramid for the first frame. If it is not NULL, the buffer must have a sufficient size to store the pyramid from level 1 to level #level ; the total size of (image_width+8)*image_height/3 bytes is sufficient</param>
      /// <param name="currPyrBuffer">Similar to prev_pyr, used for the second frame</param>
      /// <param name="prevFeatures">Array of points for which the flow needs to be found</param>
      /// <param name="winSize">Size of the search window of each pyramid level</param>
      /// <param name="level">Maximal pyramid level number. If 0 , pyramids are not used (single level), if 1 , two levels are used, etc</param>
      /// <param name="criteria">Specifies when the iteration process of finding the flow for each point on each pyramid level should be stopped</param>
      /// <param name="flags">Flags</param>
      /// <param name="currFeatures">Array of 2D points containing calculated new positions of input features in the second image</param>
      /// <param name="status">Array. Every element of the array is set to 1 if the flow for the corresponding feature has been found, 0 otherwise</param>
      /// <param name="trackError">Array of double numbers containing difference between patches around the original and moved points</param>
      public static void PyrLK(
         Image<Gray, Byte> prev,
         Image<Gray, Byte> curr,
         Image<Gray, Byte> prevPyrBuffer,
         Image<Gray, Byte> currPyrBuffer,
         PointF[] prevFeatures,
         Size winSize,
         int level,
         MCvTermCriteria criteria,
         Emgu.CV.CvEnum.LKFLOW_TYPE flags,
         out PointF[] currFeatures,
         out Byte[] status,
         out float[] trackError)
      {
         Image<Gray, Byte> prevPyrBufferParam = prevPyrBuffer ?? new Image<Gray, byte>(prev.Width + 8, prev.Height / 3);
         Image<Gray, Byte> currPyrBufferParam = currPyrBuffer ?? prevPyrBufferParam.CopyBlank();

         status = new Byte[prevFeatures.Length];
         trackError = new float[prevFeatures.Length];

         currFeatures = new PointF[prevFeatures.Length];

         CvInvoke.cvCalcOpticalFlowPyrLK(
            prev,
            curr,
            prevPyrBufferParam,
            currPyrBufferParam,
            prevFeatures,
            currFeatures,
            prevFeatures.Length,
            winSize,
            level,
            status,
            trackError,
            criteria,
            flags);

         #region Release buffer images if they are create within this function call
         if (!object.ReferenceEquals(prevPyrBufferParam, prevPyrBuffer)) prevPyrBufferParam.Dispose();
         if (!object.ReferenceEquals(currPyrBufferParam, currPyrBuffer)) currPyrBufferParam.Dispose();
         #endregion
      }

      /// <summary>
      /// Computes flow for every pixel of the first input image using Lucas &amp; Kanade algorithm
      /// </summary>
      /// <param name="prev">First image</param>
      /// <param name="curr">Second image</param>
      /// <param name="winSize">Size of the averaging window used for grouping pixels</param>
      /// <param name="velx">Horizontal component of the optical flow of the same size as input images</param>
      /// <param name="vely">Vertical component of the optical flow of the same size as input images</param>
      public static void LK(
         Image<Gray, Byte> prev,
         Image<Gray, Byte> curr,
         Size winSize,
         Image<Gray, Single> velx,
         Image<Gray, Single> vely)
      {
         CvInvoke.cvCalcOpticalFlowLK(prev.Ptr, curr.Ptr, winSize, velx, vely);
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
      public static void HS(
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
      public static void BM(
         Image<Gray, Byte> prev,
         Image<Gray, Byte> curr,
         Size blockSize,
         Size shiftSize,
         Size maxRange,
         bool usePrevious,
         Image<Gray, Single> velx,
         Image<Gray, Single> vely)
      {
         CvInvoke.cvCalcOpticalFlowBM(prev, curr, blockSize, shiftSize, maxRange, usePrevious, velx, vely);
      }

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
      public static void Farneback(
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
         CvEnum.OPTICALFLOW_FARNEBACK_FLAG flags)
      {

         IntPtr flow0 = CvInvoke.cvCreateImage(prev0.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
         try
         {
            if ((int) (flags  & Emgu.CV.CvEnum.OPTICALFLOW_FARNEBACK_FLAG.USE_INITIAL_FLOW) != 0)
            {  //use initial flow
               CvInvoke.cvMerge(flowX.Ptr, flowY.Ptr, IntPtr.Zero, IntPtr.Zero, flow0);
            }

            CvInvoke.cvCalcOpticalFlowFarneback(prev0, next0, flow0, pyrScale, levels, winSize, iterations, polyN, polySigma, flags);
            CvInvoke.cvSplit(flow0, flowX.Ptr, flowY.Ptr, IntPtr.Zero, IntPtr.Zero);
         }
         finally
         {
            CvInvoke.cvReleaseImage(ref flow0);
         }
      }
   }
}
