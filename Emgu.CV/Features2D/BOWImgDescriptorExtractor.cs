//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Class to compute an image descriptor using the bag of visual words. Such a computation consists of the following
   /// steps:
   /// 1. Compute descriptors for a given image and its key points set.
   /// 2. Find the nearest visual words from the vocabulary for each key point descriptor.
   /// 3. Compute the bag-of-words image descriptor as is a normalized histogram of vocabulary words encountered in
   /// the image. The i-th bin of the histogram is a frequency of i-th word of the vocabulary in the given image.
   /// </summary>
   public class BOWImgDescriptorExtractor: UnmanagedObject
   {

      /// <summary>
      /// 
      /// </summary>
      /// <param name="descriptorExtractor">Descriptor extractor that is used to compute descriptors for an input image and its key points.</param>
      /// <param name="descriptorMatcher">Descriptor matcher that is used to find the nearest word of the trained vocabulary for each key point descriptor of the image.</param>
      public BOWImgDescriptorExtractor(Feature2D descriptorExtractor, DescriptorMatcher descriptorMatcher)
      {
         _ptr = BOWImgDescriptorExtractorInvoke.CvBOWImgDescriptorExtractorCreate(descriptorExtractor.Feature2DPtr, descriptorMatcher);
      }

      /// <summary>
      /// Sets a visual vocabulary.
      /// </summary>
      /// <param name="vocabulary">The vocabulary</param>
      public void SetVocabulary(Mat vocabulary)
      {
         BOWImgDescriptorExtractorInvoke.CvBOWImgDescriptorExtractorSetVocabulary(_ptr, vocabulary);
      }

      /// <summary>
      /// Computes an image descriptor using the set visual vocabulary.
      /// </summary>
      /// <param name="image">Image, for which the descriptor is computed</param>
      /// <param name="keypoints">Key points detected in the input image.</param>
      /// <param name="imgDescriptors">The output image descriptors.</param>
      public void Compute(IInputArray image, VectorOfKeyPoint keypoints, Mat imgDescriptors)
      {
         using (InputArray iaImage = image.GetInputArray())
            BOWImgDescriptorExtractorInvoke.CvBOWImgDescriptorExtractorCompute(_ptr, iaImage, keypoints, imgDescriptors);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         BOWImgDescriptorExtractorInvoke.CvBOWImgDescriptorExtractorRelease(ref _ptr);
      }
   }

   internal static class BOWImgDescriptorExtractorInvoke
   {
      static BOWImgDescriptorExtractorInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBOWImgDescriptorExtractorCreate(IntPtr descriptorExtractor, IntPtr descriptorMatcher);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWImgDescriptorExtractorRelease(ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWImgDescriptorExtractorCompute(IntPtr bowImgDescriptorExtractor, IntPtr image, IntPtr keypoints, IntPtr imgDescriptor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBOWImgDescriptorExtractorSetVocabulary(IntPtr bowImgDescriptorExtractor, IntPtr vocabulary);
   }
}
