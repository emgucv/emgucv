//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
   /// Class to compute an image descriptor using the bag of visual words. Such a computation consists of the following
   /// steps:
   /// 1. Compute descriptors for a given image and its keypoints set.
   /// 2. Find the nearest visual words from the vocabulary for each keypoint descriptor.
   /// 3. Compute the bag-of-words image descriptor as is a normalized histogram of vocabulary words encountered in
   /// the image. The i-th bin of the histogram is a frequency of i-th word of the vocabulary in the given image.
   /// </summary>
   /// <typeparam name="T">The type of values in vocabulary</typeparam>
   public class BOWImgDescriptorExtractor<T> : UnmanagedObject
      where T: struct
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="descriptorExtractor">Descriptor extractor that is used to compute descriptors for an input image and its keypoints.</param>
      /// <param name="descriptorMatcher">Descriptor matcher that is used to find the nearest word of the trained vocabulary for each keypoint descriptor of the image.</param>
      public BOWImgDescriptorExtractor(IDescriptorExtractor<Gray, T> descriptorExtractor, DescriptorMatcher<T> descriptorMatcher)
      {
         _ptr = CvInvoke.CvBOWImgDescriptorExtractorCreate(descriptorExtractor.DescriptorExtratorPtr, descriptorMatcher);
      }

      /// <summary>
      /// Sets a visual vocabulary.
      /// </summary>
      /// <param name="vocabulary">The vocabulary</param>
      public void SetVocabulary(Matrix<T> vocabulary)
      {
         CvInvoke.CvBOWImgDescriptorExtractorSetVocabulary(_ptr, vocabulary);
      }

      /// <summary>
      /// Computes an image descriptor using the set visual vocabulary.
      /// </summary>
      /// <param name="image">Image, for which the descriptor is computed</param>
      /// <param name="keypoints">Keypoints detected in the input image.</param>
      /// <returns>Image descriptor.</returns>
      public Matrix<float> Compute(Image<Gray, Byte> image, VectorOfKeyPoint keypoints)
      {
         using (Mat m = new Mat())
         {
            CvInvoke.CvBOWImgDescriptorExtractorCompute(_ptr, image, keypoints, m);
            Matrix<float> result = new Matrix<float>(m.Size);
            CvInvoke.cvMatCopyToCvArr(m, result);
            return result;
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBOWImgDescriptorExtractorRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBOWImgDescriptorExtractorCreate(IntPtr descriptorExtractor, IntPtr descriptorMatcher);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWImgDescriptorExtractorRelease(ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWImgDescriptorExtractorCompute(IntPtr bowImgDescriptorExtractor, IntPtr image, IntPtr keypoints, IntPtr imgDescriptor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWImgDescriptorExtractorSetVocabulary(IntPtr bowImgDescriptorExtractor, IntPtr vocabulary);

   }
}