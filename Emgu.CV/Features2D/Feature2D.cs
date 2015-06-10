//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   /// The feature 2D base class
   /// </summary>
   public abstract class Feature2D : UnmanagedObject, IAlgorithm
   {
      /// <summary>
      /// The pointer to the Feature2D object
      /// </summary>
      protected IntPtr _feature2D;

      /// <summary>
      /// The pointer to the Algorithm object.
      /// </summary>
      protected IntPtr _algorithm;

      /// <summary>
      /// Get the pointer to the Feature2D object
      /// </summary>
      /// <returns>The pointer to the Feature2D object</returns>
      public IntPtr Feature2DPtr
      {
         get { return _feature2D; }
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get
         {
            if (_algorithm != IntPtr.Zero)
               return _algorithm;

            if (_feature2D == IntPtr.Zero)
               return IntPtr.Zero;
            return Feature2DInvoke.CvFeature2DGetAlgorithm(_feature2D);
         }
      }

      /// <summary>
      /// Detect keypoints in an image and compute the descriptors on the image from the keypoint locations.
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The detected keypoints will be stored in this vector</param>
      /// <param name="descriptors">The descriptors from the keypoints</param>
      /// <param name="useProvidedKeyPoints">If true, the method will skip the detection phase and will compute descriptors for the provided keypoints</param>
      public void DetectAndCompute(IInputArray image, IInputArray mask, VectorOfKeyPoint keyPoints, IOutputArray descriptors, bool useProvidedKeyPoints)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
         using (OutputArray oaDescriptors = descriptors.GetOutputArray())
            Feature2DInvoke.CvFeature2DDetectAndCompute(_ptr, iaImage, iaMask, keyPoints, oaDescriptors, useProvidedKeyPoints);
      }

      /// <summary>
      /// Reset the pointers
      /// </summary>
      protected override void DisposeObject()
      {
         _feature2D = IntPtr.Zero;
         _algorithm = IntPtr.Zero;
      }

      /// <summary>
      /// Detect the features in the image
      /// </summary>
      /// <param name="keypoints">The result vector of keypoints</param>
      /// <param name="image">The image from which the features will be detected from</param>
      /// <param name="mask">The optional mask.</param>
      public void DetectRaw(IInputArray image, VectorOfKeyPoint keypoints, IInputArray mask = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            Feature2DInvoke.CvFeature2DDetect(_feature2D, iaImage, keypoints.Ptr, iaMask);
      }

      /// <summary>
      /// Detect the keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract keypoints from</param>
      /// <param name="mask">The optional mask.</param>
      /// <returns>An array of key points</returns>
      public MKeyPoint[] Detect(IInputArray image, IInputArray mask = null)
      {
         using (VectorOfKeyPoint keypoints = new VectorOfKeyPoint())
         {
            DetectRaw(image, keypoints, mask);
            return keypoints.ToArray();
         }
      }

      /*
      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="extractor">The descriptor extractor</param>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public static ImageFeature<TDescriptor>[] Compute<TColor, TDescriptor>(this IDescriptorExtractor<TColor, TDescriptor> extractor, Image<TColor, Byte> image, MKeyPoint[] keyPoints)
         where TColor : struct, IColor
         where TDescriptor : struct
      {
         if (keyPoints.Length == 0) return new ImageFeature<TDescriptor>[0];
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            kpts.Push(keyPoints);
            using (Matrix<TDescriptor> descriptor = extractor.Compute(image, kpts))
            {
               return ImageFeature<TDescriptor>.ConvertFromRaw(kpts, descriptor);
            }
         }
      }*/

      /// <summary>
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <param name="descriptors">The descriptors from the given keypoints</param>
      public void Compute(IInputArray image, VectorOfKeyPoint keyPoints, IOutputArray descriptors)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaDescriptors = descriptors.GetOutputArray())
            Feature2DInvoke.CvFeature2DCompute(_feature2D, iaImage, keyPoints.Ptr, oaDescriptors);
      }

      /// <summary>
      /// Get the number of elements in the descriptor.
      /// </summary>
      /// <returns>The number of elements in the descriptor</returns>
      public int DescriptorSize
      {
         get
         {
            if (_feature2D == IntPtr.Zero)
               return 0;
            return Feature2DInvoke.CvFeature2DGetDescriptorSize(_feature2D);
         }
      }
   }

   internal partial class Feature2DInvoke
   {
      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFeature2DGetAlgorithm(IntPtr detector);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeature2DDetectAndCompute(
         IntPtr feature2D,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useProvidedKeyPoints);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeature2DDetect(
         IntPtr detector,
         IntPtr image,
         IntPtr keypoints,
         IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeature2DCompute(IntPtr extractor, IntPtr image, IntPtr keypoints, IntPtr descriptors);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvFeature2DGetDescriptorSize(IntPtr extractor);
   }

}
