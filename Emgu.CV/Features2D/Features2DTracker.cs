//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// This class use ImageFeature to match or track object
   /// </summary>
   public class Features2DTracker : DisposableObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool getHomographyMatrixFromMatchedFeatures(IntPtr model, IntPtr observed, IntPtr indices, IntPtr mask, double ransacReprojThreshold, IntPtr homography);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int voteForSizeAndOrientation(IntPtr modelKeyPoints, IntPtr observedKeyPoints, IntPtr indices, IntPtr mask, double scaleIncrement, int rotationBins);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void drawMatchedFeatures(
                                IntPtr img1, IntPtr keypoints1,
                                IntPtr img2, IntPtr keypoints2,
                                IntPtr matchIndices,
                                IntPtr outImg,
                                MCvScalar matchColor, MCvScalar singlePointColor,
                                IntPtr matchesMask,
                                KeypointDrawType flags);
      #endregion

      #region static functions
      /// <summary>
      /// Draw the matched keypoints between the model image and the observered image.
      /// </summary>
      /// <param name="modelImage">The model image</param>
      /// <param name="modelKeypoints">The keypoints in the model image</param>
      /// <param name="observerdImage">The observed image</param>
      /// <param name="observedKeyPoints">The keypoints in the observed image</param>
      /// <param name="matchIndices">The match indices</param>
      /// <param name="matchColor">The color for the match correspondence lines</param>
      /// <param name="singlePointColor">The color for highlighting the keypoints</param>
      /// <param name="matchesMask">The mask for the matches. Use null for all matches.</param>
      /// <param name="flags">The drawing type</param>
      /// <returns>The image where model and observed image is displayed side by side. Matches are drawn as indicated by the flag</returns>
      public static Image<Bgr, Byte> DrawMatches(
         Image<Gray, Byte> modelImage, VectorOfKeyPoint modelKeypoints,
         Image<Gray, Byte> observerdImage, VectorOfKeyPoint observedKeyPoints,
         Matrix<int> matchIndices, Bgr matchColor, Bgr singlePointColor,
         Matrix<Byte> matchesMask, KeypointDrawType flags)
      {
         Image<Bgr, Byte> result = new Image<Bgr, byte>(modelImage.Cols + observerdImage.Cols, Math.Max(modelImage.Rows, observerdImage.Rows));
         drawMatchedFeatures(observerdImage, observedKeyPoints, modelImage, modelKeypoints, matchIndices, result, matchColor.MCvScalar, singlePointColor.MCvScalar, matchesMask, flags);
         return result;
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
         DEFAULT = 0,
         /// <summary>
         /// Single keypoints will not be drawn.
         /// </summary>
         NOT_DRAW_SINGLE_POINTS = 2,
         /// <summary>
         /// For each keypoint the circle around keypoint with keypoint size and
         /// orientation will be drawn.
         /// </summary>
         DRAW_RICH_KEYPOINTS = 4
      };

      /// <summary>
      /// Convert the raw keypoints and descriptors to ImageFeature
      /// </summary>
      /// <param name="keyPointsVec">The raw keypoints vector</param>
      /// <param name="descriptors">The raw descriptor matrix</param>
      /// <returns>An array of image features</returns>
      public static ImageFeature[] ConvertToImageFeature(VectorOfKeyPoint keyPointsVec, Matrix<float> descriptors)
      {
         if (keyPointsVec.Size == 0) return new ImageFeature[0];
         Debug.Assert(keyPointsVec.Size == descriptors.Rows, "Size of keypoints vector do not match the rows of the descriptors matrix.");
         int sizeOfdescriptor = descriptors.Cols;
         MKeyPoint[] keyPoints = keyPointsVec.ToArray();
         ImageFeature[] features = new ImageFeature[keyPoints.Length];
         MCvMat header = descriptors.MCvMat;
         long address = header.data.ToInt64();
         for (int i = 0; i < keyPoints.Length; i++, address += header.step)
         {
            features[i].KeyPoint = keyPoints[i];
            float[] desc = new float[sizeOfdescriptor];
            Marshal.Copy(new IntPtr(address), desc, 0, sizeOfdescriptor);
            features[i].Descriptor = desc;
         }
         return features;
      }

      /// <summary>
      /// Convert the image features to keypoint vector and descriptor matrix
      /// </summary>
      private static void ConvertFromImageFeature(ImageFeature[] features, out VectorOfKeyPoint keyPoints, out Matrix<float> descriptors)
      {
         keyPoints = new VectorOfKeyPoint();
         keyPoints.Push( Array.ConvertAll<ImageFeature, MKeyPoint>(features, delegate(ImageFeature feature) { return feature.KeyPoint; }));
         descriptors = new Matrix<float>(features.Length, features[0].Descriptor.Length);

         int descriptorLength = features[0].Descriptor.Length;
         float[,] data = descriptors.Data;
         for (int i = 0; i < features.Length; i++)
         {
            for (int j = 0; j < descriptorLength; j++)
               data[i, j] = features[i].Descriptor[j];
         }
      }

      /// <summary>
      /// Detect the if the model features exist in the observed features. If true, an homography matrix is returned, otherwise, null is returned.
      /// </summary>
      /// <param name="modelDescriptors">The descriptors from the model image</param>
      /// <param name="modelKeyPoints">The keypoints drom the model image</param>
      /// <param name="observedDescriptors">The descriptors from the descriptor image</param>
      /// <param name="observedKeyPoints">The keypoints from the observed image</param>
      /// <param name="uniquenessThreshold">The distance different ratio which a match is consider unique, a good number will be 0.8</param>
      /// <returns>If the model features exist in the observed features, an homography matrix is returned, otherwise, null is returned.</returns>
      public static HomographyMatrix Detect(
         VectorOfKeyPoint modelKeyPoints, Matrix<float> modelDescriptors,
         VectorOfKeyPoint observedKeyPoints, Matrix<float> observedDescriptors, double uniquenessThreshold)
      {
         using (BruteForceMatcher matcher = new BruteForceMatcher(BruteForceMatcher.DistanceType.L2F32))
         using (Matrix<int> indices = new Matrix<int>(observedKeyPoints.Size, 2))
         using (Matrix<float> dist = new Matrix<float>(indices.Size))
         using (Matrix<byte> mask = new Matrix<byte>(dist.Rows, 1))
         {
            matcher.Add(modelDescriptors);
            matcher.KnnMatch(observedDescriptors, indices, dist, 2, null);

            mask.SetValue(255);

            //Stopwatch w1 = Stopwatch.StartNew();
            VoteForUniqueness(dist, uniquenessThreshold, mask);
            //Trace.WriteLine(w1.ElapsedMilliseconds);

            int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount < 4)
               return null;

            //Stopwatch w2 = Stopwatch.StartNew();
            nonZeroCount = VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
            if (nonZeroCount < 4)
               return null;

            return GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 3);
         }
      }

      /// <summary>
      /// Recover the homography matrix using RANDSAC. If the matrix cannot be recovered, null is returned.
      /// </summary>
      /// <param name="model">The model keypoints</param>
      /// <param name="observed">The observed keypoints</param>
      /// <param name="matchIndices">The match indices</param>
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
      public static HomographyMatrix GetHomographyMatrixFromMatchedFeatures(VectorOfKeyPoint model, VectorOfKeyPoint observed, Matrix<int> matchIndices, Matrix<Byte> mask, double ransacReprojThreshold)
      {
         HomographyMatrix homography = new HomographyMatrix();
         bool found = getHomographyMatrixFromMatchedFeatures(model, observed, matchIndices, mask, ransacReprojThreshold, homography);
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
      /// <param name="distance">The matched distances, should have at lease 2 col</param>
      /// <param name="uniquenessThreshold">The distance different ratio which a match is consider unique, a good number will be 0.8</param>
      /// <param name="mask">This is both input and output. This matrix indicates which row is valid for the matches.</param>
      public static void VoteForUniqueness(Matrix<float> distance, double uniquenessThreshold, Matrix<Byte> mask)
      {
         using (Matrix<float> firstCol = distance.GetCol(0))
         using (Matrix<float> secCol = distance.GetCol(1))
         using (Matrix<float> tmp = new Matrix<float>(firstCol.Size))
         using (Matrix<Byte> maskBuffer = new Matrix<byte>(firstCol.Size))
         {
            CvInvoke.cvDiv(firstCol, secCol, tmp, 1.0);
            CvInvoke.cvCmpS(tmp, uniquenessThreshold, maskBuffer, CvEnum.CMP_TYPE.CV_CMP_LE);
            CvInvoke.cvAnd(maskBuffer, mask, mask, IntPtr.Zero);
         }
      }

      /// <summary>
      /// Eliminate the matched features whose scale and rotation do not aggree with the majority's scale and rotation.
      /// </summary>
      /// <param name="rotationBins">The numbers of bins for rotation, a good value might be 20 (which means each bin covers 18 degree)</param>
      /// <param name="scaleIncrement">This determins the different in scale for neighbour hood bins, a good value might be 1.5 (which means matched features in bin i+1 is scaled 1.5 times larger than matched features in bin i</param>
      /// <param name="modelKeyPoints">The keypoints from the model image</param>
      /// <param name="observedKeyPoints">The keypoints from the observed image</param>
      /// <param name="indices">The match indices matrix. <paramref name="indices"/>[i, k] = j, indicates the j-th model descriptor is the k-th closest match to the i-th observed descriptor</param>
      /// <param name="mask">This is both input and output. This matrix indicates which row is valid for the matches.</param>
      /// <returns> The number of non-zero elements in the resulting mask</returns>
      public static int VoteForSizeAndOrientation(VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints, Matrix<int> indices, Matrix<Byte> mask, double scaleIncrement, int rotationBins)
      {
         return voteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, scaleIncrement, rotationBins);
      }

      /// <summary>
      /// Match the Image feature from the observed image to the features from the model image using brute force matching
      /// </summary>
      /// <param name="modelDescriptors">The descriptors from the model image</param>
      /// <param name="observedDescriptors">The descriptors from the observed image</param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="indices">The match indices matrix. <paramref name="indices"/>[i, k] = j, indicates the j-th model descriptor is the k-th closest match to the i-th observed descriptor</param>
      /// <param name="dist">The distance matrix. <paramref name="dist"/>[i,k] = d, indicates the distance bewtween the corresponding match is d</param>
      [Obsolete("Please use either the BruteForceMatcher or Flann.Index to match descriptors. This function will be removed in the next release.")]
      public static void DescriptorMatchKnn(Matrix<float> modelDescriptors, Matrix<float> observedDescriptors, int k, out Matrix<int> indices, out Matrix<float> dist)
      {
         indices = new Matrix<int>(observedDescriptors.Rows, k);
         dist = new Matrix<float>(observedDescriptors.Rows, k);

         using (BruteForceMatcher matcher = new BruteForceMatcher(BruteForceMatcher.DistanceType.L2F32))
         {
            matcher.Add(modelDescriptors);
            matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
         }
         /*
         using (Flann.Index index = new Flann.Index(modelDescriptors))
         {
            index.KnnSearch(observedDescriptors, indices, dist, k, 0);
            CvInvoke.cvSqrt(dist, dist);
         }*/
      }
      #endregion

      private ImageFeature[] _modelFeatures;
      //private ImageFeatureMatcher _matcher;
      private const int _randsacRequiredMatch = 10;
      private VectorOfKeyPoint _modelKeyPoints;
      private Matrix<float> _modelDescriptors;

      /// <summary>
      /// Create a Image tracker, where Image is matched with flann
      /// </summary>
      /// <param name="modelFeatures">The Image feature from the model image</param>
      public Features2DTracker(ImageFeature[] modelFeatures)
      {
         //_matcher = new ImageFeatureMatcher(modelFeatures);
         ConvertFromImageFeature(modelFeatures, out _modelKeyPoints, out _modelDescriptors);
         _modelFeatures = modelFeatures;
      }

      /// <summary>
      /// Use camshift to track the feature
      /// </summary>
      /// <param name="observedFeatures">The feature found from the observed image</param>
      /// <param name="initRegion">The predicted location of the model in the observed image. If not known, use MCvBox2D.Empty as default</param>
      /// <param name="priorMask">The mask that should be the same size as the observed image. Contains a priori value of the probability a match can be found. If you are not sure, pass an image fills with 1.0s</param>
      /// <returns>If a match is found, the homography projection matrix is returned. Otherwise null is returned</returns>
      public HomographyMatrix CamShiftTrack(ImageFeature[] observedFeatures, MCvBox2D initRegion, Image<Gray, Single> priorMask)
      {
         using (Image<Gray, Single> matchMask = new Image<Gray, Single>(priorMask.Size))
         {
            #region get the list of matched point on the observed image
            Single[, ,] matchMaskData = matchMask.Data;

            //Compute the matched features
            MatchedImageFeature[] matchedFeature = MatchFeature(observedFeatures, 2);
            matchedFeature = VoteForUniqueness(matchedFeature, 0.8);

            foreach (MatchedImageFeature f in matchedFeature)
            {
               PointF p = f.ObservedFeature.KeyPoint.Point; 
               matchMaskData[(int)p.Y, (int)p.X, 0] = 1.0f / (float)f.SimilarFeatures[0].Distance;
            }
            #endregion

            Rectangle startRegion;
            if (initRegion.Equals(MCvBox2D.Empty))
               startRegion = matchMask.ROI;
            else
            {
               startRegion = PointCollection.BoundingRectangle(initRegion.GetVertices());
               if (startRegion.IntersectsWith(matchMask.ROI))
                  startRegion.Intersect(matchMask.ROI);
            }

            CvInvoke.cvMul(matchMask.Ptr, priorMask.Ptr, matchMask.Ptr, 1.0);

            MCvConnectedComp comp;
            MCvBox2D currentRegion;
            //Updates the current location
            CvInvoke.cvCamShift(matchMask.Ptr, startRegion, new MCvTermCriteria(10, 1.0e-8), out comp, out currentRegion);

            #region find the Image features that belongs to the current Region
            MatchedImageFeature[] featuesInCurrentRegion;
            using (MemStorage stor = new MemStorage())
            {
               Contour<System.Drawing.PointF> contour = new Contour<PointF>(stor);
               contour.PushMulti(currentRegion.GetVertices(), Emgu.CV.CvEnum.BACK_OR_FRONT.BACK);

               CvInvoke.cvBoundingRect(contour.Ptr, 1); //this is required before calling the InContour function

               featuesInCurrentRegion = Array.FindAll(matchedFeature,
                  delegate(MatchedImageFeature f)
                  { return contour.InContour(f.ObservedFeature.KeyPoint.Point) >= 0; });
            }
            #endregion

            return GetHomographyMatrixFromMatchedFeatures(VoteForSizeAndOrientation(featuesInCurrentRegion, 1.5, 20));
         }
      }

      /// <summary>
      /// Detect the if the model features exist in the observed features. If true, an homography matrix is returned, otherwise, null is returned.
      /// </summary>
      /// <param name="observedFeatures">The observed features</param>
      /// <param name="uniquenessThreshold">The distance different ratio which a match is consider unique, a good number will be 0.8</param>
      /// <returns>If the model features exist in the observed features, an homography matrix is returned, otherwise, null is returned.</returns>
      public HomographyMatrix Detect(ImageFeature[] observedFeatures, double uniquenessThreshold)
      {
         MatchedImageFeature[] matchedGoodFeatures = MatchFeature(observedFeatures, 2);

         //Stopwatch w1 = Stopwatch.StartNew();
         matchedGoodFeatures = VoteForUniqueness(matchedGoodFeatures, uniquenessThreshold);
         //Trace.WriteLine(w1.ElapsedMilliseconds);

         if (matchedGoodFeatures.Length < 4)
            return null;

         //Stopwatch w2 = Stopwatch.StartNew();
         matchedGoodFeatures = VoteForSizeAndOrientation(matchedGoodFeatures, 1.5, 20);
         //Trace.WriteLine(w2.ElapsedMilliseconds);

         if (matchedGoodFeatures.Length < 4)
            return null;

         return GetHomographyMatrixFromMatchedFeatures(matchedGoodFeatures);
      }

      /// <summary>
      /// Recover the homography matrix using RANDSAC. If the matrix cannot be recovered, null is returned.
      /// </summary>
      /// <param name="matchedFeatures">The Matched Features, only the first ModelFeature will be considered</param>
      /// <returns>The homography matrix, if it cannot be found, null is returned</returns>
      public static HomographyMatrix GetHomographyMatrixFromMatchedFeatures(MatchedImageFeature[] matchedFeatures)
      {
         if (matchedFeatures.Length < 4)
            return null;

         HomographyMatrix homography;
         if (matchedFeatures.Length < _randsacRequiredMatch)
         {  // Too few points for randsac, use 4 points only
            PointF[] pts1 = new PointF[4];
            PointF[] pts2 = new PointF[4];
            for (int i = 0; i < 4; i++)
            {
               pts1[i] = matchedFeatures[i].SimilarFeatures[0].Feature.KeyPoint.Point;
               pts2[i] = matchedFeatures[i].ObservedFeature.KeyPoint.Point;
            }
            homography = CameraCalibration.GetPerspectiveTransform(pts1, pts2);
         }
         else
         {
            //use randsac to find the Homography Matrix
            PointF[] pts1 = new PointF[matchedFeatures.Length];
            PointF[] pts2 = new PointF[matchedFeatures.Length];
            for (int i = 0; i < matchedFeatures.Length; i++)
            {
               pts1[i] = matchedFeatures[i].SimilarFeatures[0].Feature.KeyPoint.Point;
               pts2[i] = matchedFeatures[i].ObservedFeature.KeyPoint.Point;
            }

            homography = CameraCalibration.FindHomography(
               pts1, //points on the model image
               pts2, //points on the observed image
               CvEnum.HOMOGRAPHY_METHOD.RANSAC,
               3);
            if (homography == null)
               return null;
         }

         if (homography.IsValid(10))
            return homography;
         else
         {
            homography.Dispose();
            return null;
         }
      }

      /// <summary>
      /// A similar feature is a structure that contains a Image feature and its corresponding distance to the comparing Image feature
      /// </summary>
      public struct SimilarFeature
      {
         private double _distance;
         private ImageFeature _feature;

         /// <summary>
         /// The distance to the comparing Image feature
         /// </summary>
         public double Distance
         {
            get
            {
               return _distance;
            }
            set
            {
               _distance = value;
            }
         }

         /// <summary>
         /// A similar Image feature
         /// </summary>
         public ImageFeature Feature
         {
            get
            {
               return _feature;
            }
            set
            {
               _feature = value;
            }
         }

         /// <summary>
         /// Create a similar Image feature
         /// </summary>
         /// <param name="distance">The distance to the comparing Image feature</param>
         /// <param name="feature">A similar Image feature</param>
         public SimilarFeature(double distance, ImageFeature feature)
         {
            _distance = distance;
            _feature = feature;
         }
      }

      /// <summary>
      /// Filter the matched Features, such that if a match is not unique, it is rejected.
      /// </summary>
      /// <param name="matchedFeatures">The Matched Image features, each of them has the model feature sorted by distance. (e.g. SortMatchedFeaturesByDistance )</param>
      /// <param name="uniquenessThreshold">The distance different ratio which a match is consider unique, a good number will be 0.8</param>
      /// <returns>The filtered matched Image Features</returns>
      public static MatchedImageFeature[] VoteForUniqueness(MatchedImageFeature[] matchedFeatures, double uniquenessThreshold)
      {
         return Array.FindAll<MatchedImageFeature>(matchedFeatures,
            delegate(MatchedImageFeature f)
            {
               return
                  f.SimilarFeatures.Length == 1 //this is the only match
                  || (f.SimilarFeatures[0].Distance / f.SimilarFeatures[1].Distance <= uniquenessThreshold); //if the first model feature is a good match 
            });
      }

      /// <summary>
      /// Eliminate the matched features whose scale and rotation do not aggree with the majority's scale and rotation.
      /// </summary>
      /// <param name="rotationBins">The numbers of bins for rotation, a good value might be 20 (which means each bin covers 18 degree)</param>
      /// <param name="scaleIncrement">This determins the different in scale for neighbour hood bins, a good value might be 1.5 (which means matched features in bin i+1 is scaled 1.5 times larger than matched features in bin i</param>
      /// <param name="matchedFeatures">The matched feature that will be participated in the voting. For each matchedFeatures, only the zero indexed ModelFeature will be considered.</param>
      public static MatchedImageFeature[] VoteForSizeAndOrientation(MatchedImageFeature[] matchedFeatures, double scaleIncrement, int rotationBins)
      {
         int elementsCount = matchedFeatures.Length;
         if (elementsCount < 1) return matchedFeatures;

         float[] scales = new float[elementsCount];
         float[] rotations = new float[elementsCount];
         float[] flags = new float[elementsCount];
         float minScale = float.MaxValue;
         float maxScale = float.MinValue;

         for (int i = 0; i < matchedFeatures.Length; i++)
         {
            float scale = (float)matchedFeatures[i].ObservedFeature.KeyPoint.Size / (float)matchedFeatures[i].SimilarFeatures[0].Feature.KeyPoint.Size;
            scale = (float)Math.Log10(scale);
            scales[i] = scale;
            if (scale < minScale) minScale = scale;
            if (scale > maxScale) maxScale = scale;

            float rotation = matchedFeatures[i].ObservedFeature.KeyPoint.Angle - matchedFeatures[i].SimilarFeatures[0].Feature.KeyPoint.Angle;
            rotations[i] = rotation < 0.0 ? rotation + 360 : rotation;
         }

         int scaleBinSize = (int)Math.Max(((maxScale - minScale) / Math.Log10(scaleIncrement)), 1);

         if (scaleBinSize == 1)
         {
            //handle the case where there is only one scale bin
            using (DenseHistogram h = new DenseHistogram(new int[] { rotationBins }, new RangeF[] { new RangeF(0, 360) }))
            {
               int count;
               GCHandle rotationHandle = GCHandle.Alloc(rotations, GCHandleType.Pinned);
               GCHandle flagsHandle = GCHandle.Alloc(flags, GCHandleType.Pinned);

               using (Matrix<float> flagsMat = new Matrix<float>(1, elementsCount, flagsHandle.AddrOfPinnedObject()))
               using (Matrix<float> rotationsMat = new Matrix<float>(1, elementsCount, rotationHandle.AddrOfPinnedObject()))
               {
                  h.Calculate(new Matrix<float>[] { rotationsMat }, true, null);

                  float minVal, maxVal;
                  int[] minLoc, maxLoc;
                  h.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

                  h.Threshold(maxVal * 0.5);

                  CvInvoke.cvCalcBackProject(new IntPtr[] { rotationsMat.Ptr }, flagsMat.Ptr, h.Ptr);
                  count = CvInvoke.cvCountNonZero(flagsMat);
               }
               rotationHandle.Free();
               flagsHandle.Free();

               MatchedImageFeature[] matchedGoodFeatures = new MatchedImageFeature[count];
               int index = 0;
               for (int i = 0; i < matchedFeatures.Length; i++)
                  if (flags[i] != 0)
                     matchedGoodFeatures[index++] = matchedFeatures[i];

               return matchedGoodFeatures;
            }
         } else
         {
            using (DenseHistogram h = new DenseHistogram(new int[] { scaleBinSize, rotationBins }, new RangeF[] { new RangeF(minScale, maxScale), new RangeF(0, 360) }))
            {
               int count;
               GCHandle scaleHandle = GCHandle.Alloc(scales, GCHandleType.Pinned);
               GCHandle rotationHandle = GCHandle.Alloc(rotations, GCHandleType.Pinned);
               GCHandle flagsHandle = GCHandle.Alloc(flags, GCHandleType.Pinned);

               using (Matrix<float> flagsMat = new Matrix<float>(1, elementsCount, flagsHandle.AddrOfPinnedObject()))
               using (Matrix<float> scalesMat = new Matrix<float>(1, elementsCount, scaleHandle.AddrOfPinnedObject()))
               using (Matrix<float> rotationsMat = new Matrix<float>(1, elementsCount, rotationHandle.AddrOfPinnedObject()))
               {
                  h.Calculate(new Matrix<float>[] { scalesMat, rotationsMat }, true, null);

                  float minVal, maxVal;
                  int[] minLoc, maxLoc;
                  h.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

                  h.Threshold(maxVal * 0.5);

                  CvInvoke.cvCalcBackProject(new IntPtr[] { scalesMat.Ptr, rotationsMat.Ptr }, flagsMat.Ptr, h.Ptr);
                  count = CvInvoke.cvCountNonZero(flagsMat);
               }
               scaleHandle.Free();
               rotationHandle.Free();
               flagsHandle.Free();

               MatchedImageFeature[] matchedGoodFeatures = new MatchedImageFeature[count];
               int index = 0;
               for (int i = 0; i < matchedFeatures.Length; i++)
                  if (flags[i] != 0)
                     matchedGoodFeatures[index++] = matchedFeatures[i];

               return matchedGoodFeatures;
            }
         }
      }

      /// <summary>
      /// Release unmanaged memory
      /// </summary>
      protected override void DisposeObject()
      {
      }

      /// <summary>
      /// Release the memory assocaited with this Image Tracker
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         _modelDescriptors.Dispose();
         _modelKeyPoints.Dispose();
      }

      /// <summary>
      /// Match the Image feature from the observed image to the features from the model image
      /// </summary>
      /// <param name="observedFeatures">The Image feature from the observed image</param>
      /// <param name="k">The number of neighbors to find</param>
      /// <returns>The matched features</returns>
      public MatchedImageFeature[] MatchFeature(ImageFeature[] observedFeatures, int k)
      {
         VectorOfKeyPoint obsKpts;
         Matrix<float> obsDscpts;
         ConvertFromImageFeature(observedFeatures, out obsKpts, out obsDscpts);

         using (BruteForceMatcher matcher = new BruteForceMatcher(BruteForceMatcher.DistanceType.L2F32))
         using (Matrix<int> indices = new Matrix<int>(obsKpts.Size, k))
         using (Matrix<float> dists = new Matrix<float>(indices.Size))
         {
            matcher.Add(_modelDescriptors);
            matcher.KnnMatch(obsDscpts, indices, dists, k, null);

            MatchedImageFeature[] result = new MatchedImageFeature[observedFeatures.Length];
            for (int i = 0; i < observedFeatures.Length; i++)
            {
               result[i].SimilarFeatures = new SimilarFeature[k];
               for (int j = 0; j < k; j++)
               {
                  result[i].SimilarFeatures[j].Distance = dists.Data[i, j];
                  result[i].SimilarFeatures[j].Feature = _modelFeatures[indices.Data[i, j]];
               }
               result[i].ObservedFeature = observedFeatures[i];
            }
            obsKpts.Dispose();
            obsDscpts.Dispose();
            return result;
         }
      }

      /// <summary>
      /// The matched Image feature
      /// </summary>
      public struct MatchedImageFeature
      {
         /// <summary>
         /// The observed feature
         /// </summary>
         public ImageFeature ObservedFeature;

         private SimilarFeature[] _similarFeatures;

         /// <summary>
         /// An array of similar features from the model image
         /// </summary>
         public SimilarFeature[] SimilarFeatures
         {
            get
            {
               return _similarFeatures;
            }
            set
            {
               _similarFeatures = value;
            }
         }

         /// <summary>
         /// Create a matched feature structure.
         /// </summary>
         /// <param name="observedFeature">The feature from the observed image</param>
         /// <param name="modelFeatures">The matched feature from the model</param>
         /// <param name="dist">The distances between the feature from the observerd image and the matched feature from the model image</param>
         public MatchedImageFeature(ImageFeature observedFeature, ImageFeature[] modelFeatures, double[] dist)
         {
            ObservedFeature = observedFeature;
            _similarFeatures = new SimilarFeature[modelFeatures.Length];
            for (int i = 0; i < modelFeatures.Length; i++)
               _similarFeatures[i] = new SimilarFeature(dist[i], modelFeatures[i]);
         }
      }
   }
}
