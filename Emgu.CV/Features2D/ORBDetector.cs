//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped ORB detector
   /// </summary>
   public class ORBDetector : Feature2DBase<Byte>
   {
      /// <summary>
      /// The score type
      /// </summary>
      public enum ScoreType
      {
         /// <summary>
         /// Harris
         /// </summary>
         Harris,
         /// <summary>
         /// Fast
         /// </summary>
         Fast
      }

      /// <summary>
      /// Create a ORBDetector using the specific values
      /// </summary>
      /// <param name="numberOfFeatures">The number of desired features. Use 500 for default.</param>
      /// <param name="scaleFactor">Coefficient by which we divide the dimensions from one scale pyramid level to the next. Use 1.2f for default value</param>
      /// <param name="nLevels">The number of levels in the scale pyramid. Use 8 for default value.</param>
      /// <param name="firstLevel">The level at which the image is given. If 1, that means we will also look at the image.<paramref name="scaleFactor"/> times bigger</param>
      /// <param name="edgeThreshold">How far from the boundary the points should be. Use 0 for default.</param>
      /// <param name="WTK_A">How many random points are used to produce each cell of the descriptor (2, 3, 4 ...). Use 2 for default.</param>
      /// <param name="scoreType">Type of the score to use. Use Harris for default.</param>
      /// <param name="patchSize">Patch size. Use 31 for default.</param>
      public ORBDetector(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTK_A, ScoreType scoreType, int patchSize)
      {
         _ptr = CvInvoke.CvOrbDetectorCreate(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTK_A, scoreType, patchSize, ref _featureDetectorPtr, ref _descriptorExtractorPtr);
      }

      /// <summary>
      /// Create a ORB detector with the default parameters
      /// </summary>
      /// <param name="numberOfFeatures">The number of desired features. Use 500 for default.</param>
      public ORBDetector(int numberOfFeatures)
         : this(numberOfFeatures, 1.2f, 8, 31, 0, 2, ScoreType.Harris, 31)
      {
      }

      /*
      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public override int DescriptorSize
      {
         get
         {
            return CvInvoke.CvOrbDetectorGetDescriptorSize(_ptr);
         }
      }*/

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvOrbDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

      /*
      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      private Matrix<byte> ComputeDescriptorsRawHelper(CvArray<Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         if (mask != null)
            keyPoints.FilterByPixelsMask(mask);
         int count = keyPoints.Size;
         if (count == 0) return null;
         Matrix<byte> descriptors = new Matrix<byte>(count, DescriptorSize * image.NumberOfChannels, 1);
         CvInvoke.CvOrbDetectorComputeDescriptors(_ptr, image, keyPoints, descriptors);
         return descriptors;
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location, using oppponent color (CGIV 2008 "Color Descriptors for Object Category Recognition").
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public Matrix<byte> ComputeDescriptorsRaw(Image<Bgr, Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         return ComputeDescriptorsRawHelper(image, mask, keyPoints);
      }*/
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvOrbDetectorGetDescriptorSize(IntPtr detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvOrbDetectorComputeDescriptors(
         IntPtr detector,
         IntPtr image,
         IntPtr keypoints,
         IntPtr descriptors);*/

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvOrbDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTK_A, Features2D.ORBDetector.ScoreType scoreType, int patchSize, ref IntPtr featureDetector, ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvOrbDetectorRelease(ref IntPtr detector);
   }
}
