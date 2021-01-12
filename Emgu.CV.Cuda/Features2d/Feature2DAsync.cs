//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
﻿using Emgu.CV.Cuda;
﻿using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// The feature 2D base class
   /// </summary>
   public interface IFeature2DAsync
   {

      /// <summary>
      /// Get the pointer to the Feature2DAsync object
      /// </summary>
      /// <returns>The pointer to the Feature2DAsync object</returns>
      IntPtr Feature2DAsyncPtr { get; }
   }

   /// <summary>
   /// Class that contains extension methods for Feature2DAsync
   /// </summary>
   public static class Feature2DAsyncExtension
   {

      /// <summary>
      /// Detect keypoints in an image and compute the descriptors on the image from the keypoint locations.
      /// </summary>
      /// <param name="feature2DAsync">The Feature2DAsync object</param>
      /// <param name="image">The image</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The detected keypoints will be stored in this vector</param>
      /// <param name="descriptors">The descriptors from the keypoints</param>
      /// <param name="useProvidedKeyPoints">If true, the method will skip the detection phase and will compute descriptors for the provided keypoints</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void DetectAndComputeAsync(this IFeature2DAsync feature2DAsync, IInputArray image, IInputArray mask, IOutputArray keyPoints,
         IOutputArray descriptors, bool useProvidedKeyPoints, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
         using (OutputArray oaKeypoints = keyPoints.GetOutputArray())
         using (OutputArray oaDescriptors = descriptors.GetOutputArray())
            CudaInvoke.cveCudaFeature2dAsyncDetectAndComputeAsync(feature2DAsync.Feature2DAsyncPtr, iaImage, iaMask, oaKeypoints, oaDescriptors, useProvidedKeyPoints, stream);

      }

      /// <summary>
      /// Detect the features in the image
      /// </summary>
      /// <param name="feature2DAsync">The Feature2DAsync object</param>
      /// <param name="keypoints">The result vector of keypoints</param>
      /// <param name="image">The image from which the features will be detected from</param>
      /// <param name="mask">The optional mask.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void DetectAsync(this IFeature2DAsync feature2DAsync, IInputArray image, IOutputArray keypoints, IInputArray mask = null, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaKeypoints = keypoints.GetOutputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            CudaInvoke.cveCudaFeature2dAsyncDetectAsync(feature2DAsync.Feature2DAsyncPtr, iaImage, oaKeypoints, iaMask, stream);
      }

      /// <summary>
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="feature2DAsync">The Feature2DAsync object</param>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keypoints">The keypoints where the descriptor computation is perfromed</param>
      /// <param name="descriptors">The descriptors from the given keypoints</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void ComputeAsync(this IFeature2DAsync feature2DAsync, IInputArray image, IOutputArray keypoints, IOutputArray descriptors, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaKeypoints = keypoints.GetOutputArray())
         using (OutputArray oaDescriptors = descriptors.GetOutputArray())
         {
            CudaInvoke.cveCudaFeature2dAsyncComputeAsync(feature2DAsync.Feature2DAsyncPtr, iaImage, oaKeypoints, oaDescriptors, stream);
         }
      }

      /// <summary>
      /// Converts keypoints array from internal representation to standard vector.
      /// </summary>
      /// <param name="feature2DAsync">The Feature2DAsync object</param>
      /// <param name="gpuKeypoints">GpuMat representation of the keypoints.</param>
      /// <param name="keypoints">Vector of keypoints</param>
      public static void Convert(this IFeature2DAsync feature2DAsync, IInputArray gpuKeypoints,
         VectorOfKeyPoint keypoints)
      {
         using (InputArray iaGpuKeypoints = gpuKeypoints.GetInputArray())
         {
            CudaInvoke.cveCudaFeature2dAsyncConvert(feature2DAsync.Feature2DAsyncPtr, iaGpuKeypoints, keypoints);
         }
      }
   }

   public partial class CudaInvoke
   {

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCudaFeature2dAsyncDetectAsync(
         IntPtr feature2d,
         IntPtr image,
         IntPtr keypoints,
         IntPtr mask,
         IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCudaFeature2dAsyncComputeAsync(
         IntPtr feature2d,
         IntPtr image,
         IntPtr keypoints,
         IntPtr descriptors,
         IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCudaFeature2dAsyncDetectAndComputeAsync(
         IntPtr feature2d,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useProvidedKeypoints,
         IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCudaFeature2dAsyncConvert(
         IntPtr feature2d,
         IntPtr gpuKeypoints,
         IntPtr keypoints);

   }

}
