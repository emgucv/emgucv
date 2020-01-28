//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
//using System.Runtime.Remoting.Messaging;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
    /// <summary>
    /// Tools for features 2D
    /// </summary>
    public static class Features2DToolbox
    {
        /// <summary>
        /// Draw the keypoints found on the image.
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="keypoints">The keypoints to be drawn</param>
        /// <param name="color">The color used to draw the keypoints</param>
        /// <param name="type">The drawing type</param>
        /// <param name="outImage">The image with the keypoints drawn</param> 
        public static void DrawKeypoints(
           IInputArray image,
           VectorOfKeyPoint keypoints,
           IInputOutputArray outImage,
           Bgr color,
           Features2DToolbox.KeypointDrawType type = KeypointDrawType.Default)
        {
            MCvScalar c = color.MCvScalar;
            using (InputArray iaImage = image.GetInputArray())
            using (InputOutputArray ioaOutImage = outImage.GetInputOutputArray())
                Features2DInvoke.drawKeypoints(iaImage, keypoints, ioaOutImage, ref c, type);
        }

        /// <summary>
        /// Draw the matched keypoints between the model image and the observered image.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="modelKeypoints">The keypoints in the model image</param>
        /// <param name="observerdImage">The observed image</param>
        /// <param name="observedKeyPoints">The keypoints in the observed image</param>
        /// <param name="matchColor">The color for the match correspondence lines</param>
        /// <param name="singlePointColor">The color for highlighting the keypoints</param>
        /// <param name="mask">The mask for the matches. Use null for all matches.</param>
        /// <param name="flags">The drawing type</param>
        /// <param name="result">The image where model and observed image is displayed side by side. Matches are drawn as indicated by the flag</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        public static void DrawMatches(
           IInputArray modelImage, 
           VectorOfKeyPoint modelKeypoints,
           IInputArray observerdImage, 
           VectorOfKeyPoint observedKeyPoints,
           VectorOfVectorOfDMatch matches,
           IInputOutputArray result,
           MCvScalar matchColor, 
           MCvScalar singlePointColor,
           VectorOfVectorOfByte mask = null,
           KeypointDrawType flags = KeypointDrawType.Default)
        {
            using (InputArray iaModelImage = modelImage.GetInputArray())
            using (InputArray iaObserverdImage = observerdImage.GetInputArray())
            using (InputOutputArray ioaResult = result.GetInputOutputArray())
                Features2DInvoke.drawMatchedFeatures2(iaObserverdImage, observedKeyPoints, iaModelImage,
               modelKeypoints, matches, ioaResult, ref matchColor, ref singlePointColor, mask, flags);
        }

        /// <summary>
        /// Draw the matched keypoints between the model image and the observered image.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="modelKeypoints">The keypoints in the model image</param>
        /// <param name="observerdImage">The observed image</param>
        /// <param name="observedKeyPoints">The keypoints in the observed image</param>
        /// <param name="matchColor">The color for the match correspondence lines</param>
        /// <param name="singlePointColor">The color for highlighting the keypoints</param>
        /// <param name="mask">The mask for the matches. Use null for all matches.</param>
        /// <param name="flags">The drawing type</param>
        /// <param name="result">The image where model and observed image is displayed side by side. Matches are drawn as indicated by the flag</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        public static void DrawMatches(
           IInputArray modelImage,
           VectorOfKeyPoint modelKeypoints,
           IInputArray observerdImage,
           VectorOfKeyPoint observedKeyPoints,
           VectorOfDMatch matches,
           IInputOutputArray result,
           MCvScalar matchColor,
           MCvScalar singlePointColor,
           VectorOfByte mask = null,
           KeypointDrawType flags = KeypointDrawType.Default)
        {
            using (InputArray iaModelImage = modelImage.GetInputArray())
            using (InputArray iaObserverdImage = observerdImage.GetInputArray())
            using (InputOutputArray ioaResult = result.GetInputOutputArray())
                Features2DInvoke.drawMatchedFeatures1(iaObserverdImage, observedKeyPoints, iaModelImage,
               modelKeypoints, matches, ioaResult, ref matchColor, ref singlePointColor, mask, flags);
        }

        /// <summary>
        /// Draw the matched keypoints between the model image and the observed image.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="modelKeypoints">The keypoints in the model image</param>
        /// <param name="observerdImage">The observed image</param>
        /// <param name="observedKeyPoints">The keypoints in the observed image</param>
        /// <param name="matchColor">The color for the match correspondence lines</param>
        /// <param name="singlePointColor">The color for highlighting the keypoints</param>
        /// <param name="mask">The mask for the matches. Use null for all matches.</param>
        /// <param name="flags">The drawing type</param>
        /// <param name="result">The image where model and observed image is displayed side by side. Matches are drawn as indicated by the flag</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        public static void DrawMatches(
           IInputArray modelImage, VectorOfKeyPoint modelKeypoints,
           IInputArray observerdImage, VectorOfKeyPoint observedKeyPoints,
           VectorOfVectorOfDMatch matches,
           IInputOutputArray result,
           MCvScalar matchColor, MCvScalar singlePointColor,
           IInputArray mask = null,
           KeypointDrawType flags = KeypointDrawType.Default)
        {
            using (InputArray iaModelImage = modelImage.GetInputArray())
            using (InputArray iaObserverdImage = observerdImage.GetInputArray())
            using (InputOutputArray ioaResult = result.GetInputOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                Features2DInvoke.drawMatchedFeatures3(iaObserverdImage, observedKeyPoints, iaModelImage,
               modelKeypoints, matches, ioaResult, ref matchColor, ref singlePointColor, iaMask, flags);
        }

        /// <summary>
        /// Define the Keypoint draw type
        /// </summary>
        public enum KeypointDrawType
        {
            /// <summary>
            /// Two source image, matches and single keypoints will be drawn.
            /// For each keypoint only the center point will be drawn (without
            /// the circle around keypoint with keypoint size and orientation).
            /// </summary>
            Default = 0,

            /// <summary>
            /// Single keypoints will not be drawn.
            /// </summary>
            NotDrawSinglePoints = 2,

            /// <summary>
            /// For each keypoint the circle around keypoint with keypoint size and
            /// orientation will be drawn.
            /// </summary>
            DrawRichKeypoints = 4
        }

        /// <summary>
        /// Eliminate the matched features whose scale and rotation do not aggree with the majority's scale and rotation.
        /// </summary>
        /// <param name="rotationBins">The numbers of bins for rotation, a good value might be 20 (which means each bin covers 18 degree)</param>
        /// <param name="scaleIncrement">This determines the different in scale for neighbor hood bins, a good value might be 1.5 (which means matched features in bin i+1 is scaled 1.5 times larger than matched features in bin i</param>
        /// <param name="modelKeyPoints">The keypoints from the model image</param>
        /// <param name="observedKeyPoints">The keypoints from the observed image</param>
        /// <param name="mask">This is both input and output. This matrix indicates which row is valid for the matches.</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        /// <returns> The number of non-zero elements in the resulting mask</returns>
        public static int VoteForSizeAndOrientation(VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints,
           VectorOfVectorOfDMatch matches, Mat mask, double scaleIncrement, int rotationBins)
        {
            return Features2DInvoke.voteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, matches, mask, scaleIncrement,
               rotationBins);
        }

        /// <summary>
        /// Recover the homography matrix using RANDSAC. If the matrix cannot be recovered, null is returned.
        /// </summary>
        /// <param name="model">The model keypoints</param>
        /// <param name="observed">The observed keypoints</param>
        /// <param name="ransacReprojThreshold">
        /// The maximum allowed reprojection error to treat a point pair as an inlier. 
        /// If srcPoints and dstPoints are measured in pixels, it usually makes sense to set this parameter somewhere in the range 1 to 10.
        /// </param>
        /// <param name="mask">
        /// The mask matrix of which the value might be modified by the function. 
        /// As input, if the value is 0, the corresponding match will be ignored when computing the homography matrix. 
        /// If the value is 1 and RANSAC determine the match is an outlier, the value will be set to 0.
        /// </param>
        /// <returns>The homography matrix, if it cannot be found, null is returned</returns>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        public static Mat GetHomographyMatrixFromMatchedFeatures(VectorOfKeyPoint model,
           VectorOfKeyPoint observed, VectorOfVectorOfDMatch matches, Mat mask, double ransacReprojThreshold)
        {
            Mat homography = new Mat();
            bool found = Features2DInvoke.getHomographyMatrixFromMatchedFeatures(model, observed, matches, mask,
               ransacReprojThreshold, homography);
            if (found)
            {
                return homography;
            }
            else
            {
                homography.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Filter the matched Features, such that if a match is not unique, it is rejected.
        /// </summary>
        /// <param name="uniquenessThreshold">The distance different ratio which a match is consider unique, a good number will be 0.8</param>
        /// <param name="mask">This is both input and output. This matrix indicates which row is valid for the matches.</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param> 
        public static void VoteForUniqueness(VectorOfVectorOfDMatch matches, double uniquenessThreshold, Mat mask)
        {
            MDMatch[][] mArr = matches.ToArrayOfArray();
            byte[] maskData = new byte[mArr.Length];
            GCHandle maskHandle = GCHandle.Alloc(maskData, GCHandleType.Pinned);
            using (Mat m = new Mat(mArr.Length, 1, DepthType.Cv8U, 1, maskHandle.AddrOfPinnedObject(), 1))
            {
                mask.CopyTo(m);
                for (int i = 0; i < mArr.Length; i++)
                {
                    if (maskData[i] != 0 && (mArr[i][0].Distance / mArr[i][1].Distance) > uniquenessThreshold)
                    {
                        maskData[i] = (byte)0;
                    }
                }

                m.CopyTo(mask);
            }
            maskHandle.Free();

        }
    }

    public static partial class Features2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool getHomographyMatrixFromMatchedFeatures(IntPtr model, IntPtr observed, IntPtr indices, IntPtr mask, double ransacReprojThreshold, IntPtr homography);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int voteForSizeAndOrientation(IntPtr modelKeyPoints, IntPtr observedKeyPoints, IntPtr indices, IntPtr mask, double scaleIncrement, int rotationBins);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void drawMatchedFeatures1(
           IntPtr img1, 
           IntPtr keypoints1,
           IntPtr img2, 
           IntPtr keypoints2,
           IntPtr matchIndices,
           IntPtr outImg,
           ref MCvScalar matchColor, 
           ref MCvScalar singlePointColor,
           IntPtr matchesMask,
           Features2D.Features2DToolbox.KeypointDrawType flags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void drawMatchedFeatures2(
            IntPtr img1, 
            IntPtr keypoints1,
            IntPtr img2, 
            IntPtr keypoints2,
            IntPtr matchIndices,
            IntPtr outImg,
            ref MCvScalar matchColor, 
            ref MCvScalar singlePointColor,
            IntPtr matchesMask,
            Features2D.Features2DToolbox.KeypointDrawType flags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void drawMatchedFeatures3(
            IntPtr img1, 
            IntPtr keypoints1,
            IntPtr img2, 
            IntPtr keypoints2,
            IntPtr matchIndices,
            IntPtr outImg,
            ref MCvScalar matchColor, 
            ref MCvScalar singlePointColor,
            IntPtr matchesMask,
            Features2D.Features2DToolbox.KeypointDrawType flags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void drawKeypoints(
                            IntPtr image,
                            IntPtr vectorOfKeypoints,
                            IntPtr outImage,
                            ref MCvScalar color,
                            Features2D.Features2DToolbox.KeypointDrawType flags);


    }
}