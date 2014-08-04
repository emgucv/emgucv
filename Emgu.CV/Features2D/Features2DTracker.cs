//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /*
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
         DistanceType dt = typeof (TDescriptor) == typeof (Byte) ? DistanceType.Hamming : DistanceType.L2;
         using (Matrix<int> indices = new Matrix<int>(observedKeyPoints.Size, k))
         using (Matrix<float> dist = new Matrix<float>(indices.Size))
         using (BFMatcher matcher = new BFMatcher(dt))
         using (Matrix<byte> mask = new Matrix<byte>(dist.Rows, 1))
         using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
         {
            matcher.Add(modelDescriptors);
            matcher.KnnMatch(observedDescriptors, matches, k, null);

            mask.SetValue(255);

            //Stopwatch w1 = Stopwatch.StartNew();
            Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
            //Trace.WriteLine(w1.ElapsedMilliseconds);

            int nonZeroCount = CvInvoke.CountNonZero(mask);
            if (nonZeroCount < 4)
               return null;

            //Stopwatch w2 = Stopwatch.StartNew();
            nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask,
               1.5, 20);
            if (nonZeroCount < 4)
               return null;

            return Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices,
               mask, 3);

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
      /// <param name="initRegion">The predicted location of the model in the observed image. If not known, use RotatedRect.Empty as default</param>
      /// <param name="priorMask">The mask that should be the same size as the observed image. Contains a priori value of the probability a match can be found. If you are not sure, pass an image fills with 1.0s</param>
      /// <returns>If a match is found, the homography projection matrix is returned. Otherwise null is returned</returns>
      public HomographyMatrix CamShiftTrack(ImageFeature<TDescriptor>[] observedFeatures, RotatedRect initRegion, Image<Gray, Single> priorMask)
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
            if (initRegion.Equals(RotatedRect.Empty))
               startRegion = matchMask.ROI;
            else
            {
               startRegion = PointCollection.BoundingRectangle(initRegion.GetVertices());
               if (startRegion.IntersectsWith(matchMask.ROI))
                  startRegion.Intersect(matchMask.ROI);
            }

            CvInvoke.Multiply(matchMask, priorMask, matchMask, 1.0, CvInvoke.GetDepthType(typeof(float)));

            //Updates the current location
            RotatedRect currentRegion = CvInvoke.CamShift(matchMask, ref startRegion, new MCvTermCriteria(10, 1.0e-8));

            #region find the Image features that belongs to the current Region
            MatchedImageFeature[] featuesInCurrentRegion;
            using (VectorOfPointF contour = new VectorOfPointF(currentRegion.GetVertices()))
            {
               featuesInCurrentRegion = Array.FindAll(matchedFeature,
                  f => CvInvoke.PointPolygonTest(contour, f.ObservedFeature.KeyPoint.Point, false) >= 0);
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
               CvEnum.HomographyMethod.Ransac,
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
            using(Mat h = new Mat())
            {
               int count;
               GCHandle rotationHandle = GCHandle.Alloc(rotations, GCHandleType.Pinned);
               GCHandle flagsHandle = GCHandle.Alloc(flags, GCHandleType.Pinned);

               using (Matrix<float> flagsMat = new Matrix<float>(1, elementsCount, flagsHandle.AddrOfPinnedObject()))
               using (Matrix<float> rotationsMat = new Matrix<float>(1, elementsCount, rotationHandle.AddrOfPinnedObject()))
               using (VectorOfMat vm = new VectorOfMat())
               {
                  vm.Push(rotationsMat);
                  CvInvoke.CalcHist(vm, new int[] { 0 }, null, h, new int[] { rotationBins }, new float[] { 0, 360 }, false);
                  double[] minVal, maxVal;
                  Point[] minLoc, maxLoc;
                 
                  h.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

                  CvInvoke.Threshold(h, h, maxVal[0] * 0.5, 0, CvEnum.ThresholdType.ToZero);
                  CvInvoke.CalcBackProject(vm, new int[] { 0 }, h, flagsMat.Mat, new float[] { 0, 360 });
                  //CvInvoke.cvCalcBackProject(new IntPtr[] { rotationsMat.Ptr }, flagsMat.Ptr, h.Ptr);
                  count = CvInvoke.CountNonZero(flagsMat);
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
            //using (DenseHistogram h = new DenseHistogram(new int[] { scaleBinSize, rotationBins }, new RangeF[] { new RangeF(minScale, maxScale), new RangeF(0, 360) }))
            {
               int count;
               GCHandle scaleHandle = GCHandle.Alloc(scales, GCHandleType.Pinned);
               GCHandle rotationHandle = GCHandle.Alloc(rotations, GCHandleType.Pinned);
               GCHandle flagsHandle = GCHandle.Alloc(flags, GCHandleType.Pinned);

               using (Matrix<float> flagsMat = new Matrix<float>(1, elementsCount, flagsHandle.AddrOfPinnedObject()))
               using (Matrix<float> scalesMat = new Matrix<float>(1, elementsCount, scaleHandle.AddrOfPinnedObject()))
               using (Matrix<float> rotationsMat = new Matrix<float>(1, elementsCount, rotationHandle.AddrOfPinnedObject()))
               using (Mat h = new Mat())
               using (VectorOfMat vm = new VectorOfMat())
               {
                  vm.Push(scalesMat);
                  vm.Push(rotationsMat);
                  CvInvoke.CalcHist(vm, new int[] { 0, 1 }, null, h, new int[] { scaleBinSize, rotationBins }, new float[] { minScale, maxScale, 0, 360 }, false);
                  //h.Calculate(new Matrix<float>[] { scalesMat, rotationsMat }, true, null);

                  double[] minVal, maxVal;
                  Point[] minLoc, maxLoc;
                  h.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

                  CvInvoke.Threshold(h, h, Math.Max( maxVal[0], maxVal[1]) * 0.5, 0, CvEnum.ThresholdType.ToZero);
                  CvInvoke.CalcBackProject(vm, new int[] { 0, 1 }, h, flagsMat, new float[] { minScale, maxScale, 0, 360 });
                  //CvInvoke.cvCalcBackProject(new IntPtr[] { scalesMat.Ptr, rotationsMat.Ptr }, flagsMat.Ptr, h.Ptr);
                  count = CvInvoke.CountNonZero(flagsMat);
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
            //using (Matrix<int> indices = new Matrix<int>(observedDescriptors.Rows, k))
            //using (Matrix<float> dists = new Matrix<float>(observedDescriptors.Rows, k))
            using (BFMatcher matcher = new BFMatcher(dt))
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())            
            {
               matcher.Add(_modelDescriptors);
               matcher.KnnMatch(observedDescriptors, matches, k, null);
               return ConvertToMatchedImageFeature(_modelKeyPoints, _modelDescriptors, obsKpts, observedDescriptors, matches, null);
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
      /// <param name="mask">The mask</param>
      /// <returns>The managed MatchedImageFeature array</returns>
      public static MatchedImageFeature[] ConvertToMatchedImageFeature(
         VectorOfKeyPoint modelKeyPointVec, Matrix<TDescriptor> modelDescriptorMat,
         VectorOfKeyPoint observedKeyPointVec, Matrix<TDescriptor> observedDescriptorMat,
         VectorOfVectorOfDMatch matches, Matrix<Byte> mask)
      {
         MKeyPoint[] modelKeyPoints = modelKeyPointVec.ToArray();
         MKeyPoint[] observedKeyPoints = observedKeyPointVec.ToArray();

         int resultLength = (mask == null) ? observedKeyPoints.Length : CvInvoke.CountNonZero(mask);

         MatchedImageFeature[] result = new MatchedImageFeature[resultLength];

         MCvMat modelMat = (MCvMat)Marshal.PtrToStructure(modelDescriptorMat.Ptr, typeof(MCvMat));
         long modelPtr = modelMat.Data.ToInt64();
         int modelStep = modelMat.Step;

         MCvMat observedMat = (MCvMat)Marshal.PtrToStructure(observedDescriptorMat.Ptr, typeof(MCvMat));
         long observedPtr = observedMat.Data.ToInt64();
         int observedStep = observedMat.Step;

         int descriptorLength = modelMat.Cols;
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
         return Array.FindAll(result, r => r.SimilarFeatures.Length != 0);
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
   }*/

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
            Features2DToolbox.KeypointDrawType type)
         {
            MCvScalar c = color.MCvScalar;
            using (InputArray iaImage = image.GetInputArray())
            using (InputOutputArray ioaOutImage = outImage.GetInputOutputArray())
            CvInvoke.drawKeypoints(iaImage, keypoints, ioaOutImage, ref c, type);
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
            CvInvoke.drawMatchedFeatures(iaObserverdImage, observedKeyPoints, iaModelImage,
               modelKeypoints, matches, ioaResult, ref matchColor, ref singlePointColor, iaMask , flags);
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
         /// <param name="scaleIncrement">This determins the different in scale for neighbour hood bins, a good value might be 1.5 (which means matched features in bin i+1 is scaled 1.5 times larger than matched features in bin i</param>
         /// <param name="modelKeyPoints">The keypoints from the model image</param>
         /// <param name="observedKeyPoints">The keypoints from the observed image</param>
         /// <param name="mask">This is both input and output. This matrix indicates which row is valid for the matches.</param>
         /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
         /// <returns> The number of non-zero elements in the resulting mask</returns>
         public static int VoteForSizeAndOrientation(VectorOfKeyPoint modelKeyPoints, VectorOfKeyPoint observedKeyPoints,
            VectorOfVectorOfDMatch matches, Matrix<Byte> mask, double scaleIncrement, int rotationBins)
         {
            return CvInvoke.voteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, matches, mask, scaleIncrement,
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
         public static HomographyMatrix GetHomographyMatrixFromMatchedFeatures(VectorOfKeyPoint model,
            VectorOfKeyPoint observed, VectorOfVectorOfDMatch matches, Matrix<Byte> mask, double ransacReprojThreshold)
         {
            HomographyMatrix homography = new HomographyMatrix();
            bool found = CvInvoke.getHomographyMatrixFromMatchedFeatures(model, observed, matches, mask,
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
         public static void VoteForUniqueness(VectorOfVectorOfDMatch matches, double uniquenessThreshold, Matrix<Byte> mask)
         {
            MDMatch[][] mArr = matches.ToArrayOfArray();
            byte[] maskData = new byte[mArr.Length];
            GCHandle maskHandle = GCHandle.Alloc(maskData, GCHandleType.Pinned);
            using (Mat m = new Mat(mArr.Length, 1, DepthType.Cv8U, 1, maskHandle.AddrOfPinnedObject(), 1))
            {
               mask.Mat.CopyTo(m);
               for (int i = 0; i < mArr.Length; i++)
               {
                  if (maskData[i] != 0 && (mArr[i][0].Distance / mArr[i][1].Distance) <= uniquenessThreshold)
                  {
                     maskData[i] = (byte)255;
                  }
               }

               m.CopyTo(mask);
            }
            maskHandle.Free();

         }
      }
   }

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool getHomographyMatrixFromMatchedFeatures(IntPtr model, IntPtr observed, IntPtr indices, IntPtr mask, double ransacReprojThreshold, IntPtr homography);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int voteForSizeAndOrientation(IntPtr modelKeyPoints, IntPtr observedKeyPoints, IntPtr indices, IntPtr mask, double scaleIncrement, int rotationBins);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void drawMatchedFeatures(
         IntPtr img1, IntPtr keypoints1,
         IntPtr img2, IntPtr keypoints2,
         IntPtr matchIndices,
         IntPtr outImg,
         ref MCvScalar matchColor, ref MCvScalar singlePointColor,
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