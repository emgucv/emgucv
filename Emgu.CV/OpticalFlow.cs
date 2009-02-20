using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

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
         System.Drawing.PointF[] prevFeatures,
         System.Drawing.Size winSize,
         int level,
         MCvTermCriteria criteria,
         out System.Drawing.PointF[] currFeatures,
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
         System.Drawing.PointF[] prevFeatures,
         System.Drawing.Size winSize,
         int level,
         MCvTermCriteria criteria,
         Emgu.CV.CvEnum.LKFLOW_TYPE flags,
         out System.Drawing.PointF[] currFeatures,
         out Byte[] status,
         out float[] trackError)
      {
         if (prevPyrBuffer == null)
         {
            prevPyrBuffer = new Image<Gray, byte>(prev.Width + 8, prev.Height / 3);
         }
         if (currPyrBuffer == null)
         {
            currPyrBuffer = prevPyrBuffer.CopyBlank();
         }

         status = new Byte[prevFeatures.Length];
         trackError = new float[prevFeatures.Length];

         currFeatures = new System.Drawing.PointF[prevFeatures.Length];

         CvInvoke.cvCalcOpticalFlowPyrLK(
            prev,
            curr,
            prevPyrBuffer,
            currPyrBuffer,
            prevFeatures,
            currFeatures,
            prevFeatures.Length,
            winSize,
            level,
            status,
            trackError,
            criteria,
            flags);
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
         System.Drawing.Size winSize,
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
         CvInvoke.cvCalcOpticalFlowHS(prev.Ptr, curr.Ptr, usePrevious ? 1 : 0, velx.Ptr, vely.Ptr, lambda, criteria);
      }
   }
}
