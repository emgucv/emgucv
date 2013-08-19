//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   /// <typeparam name="TDescriptor">The type of descriptor, either float or Byte</typeparam>
   public class Features2DTracker<TDescriptor> : DisposableObject
      where TDescriptor : struct
   {
      #region static functions
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
         VectorOfKeyPoint modelKeyPoints, Matrix<TDescriptor> modelDescriptors,
         VectorOfKeyPoint observedKeyPoints, Matrix<TDescriptor> observedDescriptors, double uniquenessThreshold)
      {
         int k = 2;
         DistanceType dt = typeof(TDescriptor) == typeof(Byte) ? DistanceType.Hamming : DistanceType.L2;
         using (Matrix<int> indices = new Matrix<int>(observedKeyPoints.Size, k))
         using (Matrix<float> dist = new Matrix<float>(indices.Size))
         using (BruteForceMatcher<TDescriptor> matcher = new BruteForceMatcher<TDescriptor>(dt))
         using (Matrix<byte> mask = new Matrix<byte>(dist.Rows, 1))
         {
            matcher.Add(modelDescriptors);
            matcher.KnnMatch(observedDescriptors, indices, dist, k, null);

            mask.SetValue(255);

            //Stopwatch w1 = Stopwatch.StartNew();
            Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
            //Trace.WriteLine(w1.ElapsedMilliseconds);

            int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount < 4)
               return null;

            //Stopwatch w2 = Stopwatch.StartNew();
            nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
            if (nonZeroCount < 4)
               return null;

            return Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 3);

         }
      }
      #endregion

      //private ImageFeature<TDescriptor>[] _modelFeatures;
      private const int _randsacRequiredMatch = 10;
      private VectorOfKeyPoint _modelKeyPoints;
      private Matrix<TDescriptor> _modelDescriptors;

      /// <summary>
      /// Create a Image tracker, where Image is matched with flann
      /// </summary>
      /// <param name="modelFeatures">The Image feature from the model image</param>
      public Features2DTracker(ImageFeature<TDescriptor>[] modelFeatures)
      {
         ImageFeature<TDescriptor>.ConvertToRaw(modelFeatures, out _modelKeyPoints, out _modelDescriptors);
      }

      /// <summary>
      /// Use camshift to track the feature
      /// </summary>
      /// <param name="observedFeatures">The feature found from the observed image</param>
      /// <param name="initRegion">The predicted location of the model in the observed image. If not known, use MCvBox2D.Empty as default</param>
      /// <param name="priorMask">The mask that should be the same size as the observed image. Contains a priori value of the probability a match can be found. If you are not sure, pass an image fills with 1.0s</param>
      /// <returns>If a match is found, the homography projection matrix is returned. Otherwise null is returned</returns>
      public HomographyMatrix CamShiftTrack(ImageFeature<TDescriptor>[] observedFeatures, MCvBox2D initRegion, Image<Gray, Single> priorMask)
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
      public HomographyMatrix Detect(ImageFeature<TDescriptor>[] observedFeatures, double uniquenessThreshold)
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
         private ImageFeature<TDescriptor> _feature;

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
         public ImageFeature<TDescriptor> Feature
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
         public SimilarFeature(double distance, ImageFeature<TDescriptor> feature)
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
         }
         else
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
      /// Match the Image feature from the observed image to the features from the model image, using brute force matcher
      /// </summary>
      /// <param name="observedFeatures">The Image feature from the observed image</param>
      /// <param name="k">The number of neighbors to find</param>
      /// <returns>The matched features</returns>
      public MatchedImageFeature[] MatchFeature(ImageFeature<TDescriptor>[] observedFeatures, int k)
      {
         VectorOfKeyPoint obsKpts;
         Matrix<TDescriptor> observedDescriptors;
         ImageFeature<TDescriptor>.ConvertToRaw(observedFeatures, out obsKpts, out observedDescriptors);

         try
         {
            DistanceType dt = typeof(TDescriptor) == typeof(Byte) ? DistanceType.Hamming : DistanceType.L2;
            using (Matrix<int> indices = new Matrix<int>(observedDescriptors.Rows, k))
            using (Matrix<float> dists = new Matrix<float>(observedDescriptors.Rows, k))
            using (BruteForceMatcher<TDescriptor> matcher = new BruteForceMatcher<TDescriptor>(dt))
            {
               matcher.Add(_modelDescriptors);
               matcher.KnnMatch(observedDescriptors, indices, dists, k, null);
               return ConvertToMatchedImageFeature(_modelKeyPoints, _modelDescriptors, obsKpts, observedDescriptors, indices, dists, null);
            }
         }
         finally
         {
            obsKpts.Dispose();
            observedDescriptors.Dispose();
         }
      }

      /// <summary>
      /// Convert the raw keypoints and descriptors to array of managed structure.
      /// </summary>
      /// <param name="modelKeyPointVec">The model keypoint vector</param>
      /// <param name="modelDescriptorMat">The mode descriptor vector</param>
      /// <param name="observedKeyPointVec">The observerd keypoint vector</param>
      /// <param name="observedDescriptorMat">The observed descriptor vector</param>
      /// <param name="indices">The indices matrix</param>
      /// <param name="dists">The distances matrix</param>
      /// <param name="mask">The mask</param>
      /// <returns>The managed MatchedImageFeature array</returns>
      public static MatchedImageFeature[] ConvertToMatchedImageFeature(
         VectorOfKeyPoint modelKeyPointVec, Matrix<TDescriptor> modelDescriptorMat,
         VectorOfKeyPoint observedKeyPointVec, Matrix<TDescriptor> observedDescriptorMat,
         Matrix<int> indices, Matrix<float> dists, Matrix<Byte> mask)
      {
         MKeyPoint[] modelKeyPoints = modelKeyPointVec.ToArray();
         MKeyPoint[] observedKeyPoints = observedKeyPointVec.ToArray();

         int resultLength = (mask == null) ? observedKeyPoints.Length : CvInvoke.cvCountNonZero(mask);

         MatchedImageFeature[] result = new MatchedImageFeature[resultLength];

         MCvMat modelMat = (MCvMat)Marshal.PtrToStructure(modelDescriptorMat.Ptr, typeof(MCvMat));
         long modelPtr = modelMat.data.ToInt64();
         int modelStep = modelMat.step;

         MCvMat observedMat = (MCvMat)Marshal.PtrToStructure(observedDescriptorMat.Ptr, typeof(MCvMat));
         long observedPtr = observedMat.data.ToInt64();
         int observedStep = observedMat.step;

         int descriptorLength = modelMat.cols;
         int descriptorSizeInByte = descriptorLength * Marshal.SizeOf(typeof(TDescriptor));

         int k = dists.Cols;
         TDescriptor[] tmp = new TDescriptor[descriptorLength];
         GCHandle handle = GCHandle.Alloc(tmp, GCHandleType.Pinned);
         IntPtr address = handle.AddrOfPinnedObject();
         int resultIdx = 0;

         for (int i = 0; i < observedKeyPoints.Length; i++)
         {
            if (mask != null && mask.Data[i, 0] == 0)
               continue;

            SimilarFeature[] features = new SimilarFeature[k];
            for (int j = 0; j < k; j++)
            {
               features[j].Distance = dists.Data[i, j];
               ImageFeature<TDescriptor> imgFeature = new ImageFeature<TDescriptor>();
               int idx = indices.Data[i, j];
               if (idx == -1)
               {
                  Array.Resize(ref features, j);
                  break;
               }
               imgFeature.KeyPoint = modelKeyPoints[idx];
               imgFeature.Descriptor = new TDescriptor[descriptorLength];
               Emgu.Util.Toolbox.memcpy(address, new IntPtr(modelPtr + modelStep * idx), descriptorSizeInByte);
               tmp.CopyTo(imgFeature.Descriptor, 0);
               features[j].Feature = imgFeature;
            }
            result[resultIdx].SimilarFeatures = features;

            ImageFeature<TDescriptor> observedFeature = new ImageFeature<TDescriptor>();
            observedFeature.KeyPoint = observedKeyPoints[i];
            observedFeature.Descriptor = new TDescriptor[descriptorLength];
            Emgu.Util.Toolbox.memcpy(address, new IntPtr(observedPtr + observedStep * i), descriptorSizeInByte);
            tmp.CopyTo(observedFeature.Descriptor, 0);
            result[resultIdx].ObservedFeature = observedFeature;
            resultIdx++;
         }
         handle.Free();
         return result;
      }

      /// <summary>
      /// The matched Image feature
      /// </summary>
      public struct MatchedImageFeature
      {
         /// <summary>
         /// The observed feature
         /// </summary>
         public ImageFeature<TDescriptor> ObservedFeature;

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
         public MatchedImageFeature(ImageFeature<TDescriptor> observedFeature, ImageFeature<TDescriptor>[] modelFeatures, double[] dist)
         {
            ObservedFeature = observedFeature;
            _similarFeatures = new SimilarFeature[modelFeatures.Length];
            for (int i = 0; i < modelFeatures.Length; i++)
               _similarFeatures[i] = new SimilarFeature(dist[i], modelFeatures[i]);
         }
      }
   }

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
      /// <returns>The image with the keypoints drawn</returns>
      /// <typeparam name="TColor">The type of color for the source image. Should be either Gray or Bgr</typeparam>
      public static Image<Bgr, Byte> DrawKeypoints<TColor>(
         Image<TColor, Byte> image,
         VectorOfKeyPoint keypoints,
         Bgr color,
         Features2DToolbox.KeypointDrawType type)
         where TColor : struct, IColor
      {
         Image<Bgr, Byte> result = new Image<Bgr, Byte>(image.Size);
         CvInvoke.drawKeypoints(image, keypoints, result, color.MCvScalar, type);
         return result;
      }

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
      /// <typeparam name="TColor">The type of color for the source images. Should be either Gray or Bgr</typeparam>
      public static Image<Bgr, Byte> DrawMatches<TColor>(
         Image<TColor, Byte> modelImage, VectorOfKeyPoint modelKeypoints,
         Image<TColor, Byte> observerdImage, VectorOfKeyPoint observedKeyPoints,
         Matrix<int> matchIndices, Bgr matchColor, Bgr singlePointColor,
         Matrix<Byte> matchesMask, KeypointDrawType flags)
         where TColor : struct, IColor
      {
         Image<Bgr, Byte> result = new Image<Bgr, byte>(modelImage.Cols + observerdImage.Cols, Math.Max(modelImage.Rows, observerdImage.Rows));
         CvInvoke.drawMatchedFeatures(observerdImage, observedKeyPoints, modelImage, modelKeypoints, matchIndices, result, matchColor.MCvScalar, singlePointColor.MCvScalar, matchesMask, flags);
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
         return CvInvoke.voteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, scaleIncrement, rotationBins);
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
         bool found = CvInvoke.getHomographyMatrixFromMatchedFeatures(model, observed, matchIndices, mask, ransacReprojThreshold, homography);
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

      /*
      /// <summary>
      /// Match the Image feature from the observed image to the features from the model image using brute force matching
      /// </summary>
      /// <param name="modelDescriptors">The descriptors from the model image</param>
      /// <param name="observedDescriptors">The descriptors from the observed image</param>
      /// <param name="k">The number of neighbors to find</param>
      /// <param name="indices">The match indices matrix. <paramref name="indices"/>[i, k] = j, indicates the j-th model descriptor is the k-th closest match to the i-th observed descriptor</param>
      /// <param name="dist">The distance matrix. <paramref name="dist"/>[i,k] = d, indicates the distance bewtween the corresponding match is d</param>
      public static void DescriptorMatchKnn<TDescriptor>(Matrix<TDescriptor> modelDescriptors, Matrix<TDescriptor> observedDescriptors, int k,  Matrix<int> indices,  Matrix<float> dist)
         where TDescriptor : struct 
      {
         indices = new Matrix<int>(observedDescriptors.Rows, k);
         dist = new Matrix<float>(observedDescriptors.Rows, k);

         BruteForceMatcher<TDescriptor>.DistanceType dt = typeof(TDescriptor) == typeof(Byte) ? BruteForceMatcher<TDescriptor>.DistanceType.Hamming : BruteForceMatcher<TDescriptor>.DistanceType.L2F32;
         using (BruteForceMatcher<TDescriptor> matcher = new BruteForceMatcher<TDescriptor>(dt))
         {
            matcher.Add(modelDescriptors);
            matcher.KnnMatch(observedDescriptors, indices, dist, 2, null);
         }
      }*/
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool getHomographyMatrixFromMatchedFeatures(IntPtr model, IntPtr observed, IntPtr indices, IntPtr mask, double ransacReprojThreshold, IntPtr homography);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int voteForSizeAndOrientation(IntPtr modelKeyPoints, IntPtr observedKeyPoints, IntPtr indices, IntPtr mask, double scaleIncrement, int rotationBins);

#if ANDROID
      internal static void drawMatchedFeatures(
         IntPtr img1, IntPtr keypoints1,
         IntPtr img2, IntPtr keypoints2,
         IntPtr matchIndices,
         IntPtr outImg,
         MCvScalar matchColor, MCvScalar singlePointColor,
         IntPtr matchesMask,
         Features2D.Features2DToolbox.KeypointDrawType flags)
      {
         drawMatchedFeatures(
            img1, keypoints1, img2, keypoints2, matchIndices, outImg,
            matchColor.v0, matchColor.v1, matchColor.v2, matchColor.v3,
            singlePointColor.v0, singlePointColor.v1, singlePointColor.v2, singlePointColor.v3,
            matchesMask, flags);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void drawMatchedFeatures(
         IntPtr img1, IntPtr keypoints1,
         IntPtr img2, IntPtr keypoints2,
         IntPtr matchIndices,
         IntPtr outImg,
         double mc0, double mc1, double mc2, double mc3,
         double spc0, double spc1, double spc2, double spc3,
         IntPtr matchesMask,
         Features2D.Features2DToolbox.KeypointDrawType flags);

      internal static void drawKeypoints(
                          IntPtr image,
                          IntPtr vectorOfKeypoints,
                          IntPtr outImage,
                          MCvScalar color,
                          Features2D.Features2DToolbox.KeypointDrawType flags)
      {
         drawKeypoints(image, vectorOfKeypoints, outImage, color.v0, color.v1, color.v2, color.v3, flags);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void drawKeypoints(
                          IntPtr image,
                          IntPtr vectorOfKeypoints,
                          IntPtr outImage,
                          double v0, double v1, double v2, double v3,
                          Features2D.Features2DToolbox.KeypointDrawType flags);

#else
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void drawMatchedFeatures(
         IntPtr img1, IntPtr keypoints1,
         IntPtr img2, IntPtr keypoints2,
         IntPtr matchIndices,
         IntPtr outImg,
         MCvScalar matchColor, MCvScalar singlePointColor,
         IntPtr matchesMask,
         Features2D.Features2DToolbox.KeypointDrawType flags);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void drawKeypoints(
                          IntPtr image,
                          IntPtr vectorOfKeypoints,
                          IntPtr outImage,
                          MCvScalar color,
                          Features2D.Features2DToolbox.KeypointDrawType flags);
#endif


   }
}