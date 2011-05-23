//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// BRIEF Descriptor
   /// </summary>
   public class BriefDescriptorExtractor : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvBriefDescriptorExtractorCreate(int descriptorSize);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvBriefDescriptorExtractorRelease(ref IntPtr extractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static int CvBriefDescriptorExtractorGetDescriptorSize(IntPtr extractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvBriefDescriptorComputeDescriptors(IntPtr extractor, IntPtr image, IntPtr keypoints, IntPtr descriptors);
      #endregion

      /// <summary>
      /// Create a BRIEF descriptor extractor using descriptor size of 32.
      /// </summary>
      public BriefDescriptorExtractor()
         :this(32)
      {
      }

      /// <summary>
      /// Create a BRIEF descriptor extractor.
      /// </summary>
      /// <param name="descriptorSize">The size of descriptor. It can be equal 16, 32 or 64 bytes. Use 32 for default.</param>
      public BriefDescriptorExtractor(int descriptorSize)
      {
         _ptr = CvBriefDescriptorExtractorCreate(descriptorSize);
      }

      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return CvBriefDescriptorExtractorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public Matrix<Byte> ComputeDescriptorsRaw(Image<Gray, Byte> image, VectorOfKeyPoint keyPoints)
      {
         const float epsilon = 1.192092896e-07f;        // smallest such that 1.0+epsilon != 1.0 
         keyPoints.FilterByImageBorder(image.Size, 48 / 2 + 9 / 2); //this value comes from opencv's BriefDescriptorExtractor::computeImpl implementation
         keyPoints.FilterByKeypointSize(epsilon, float.MaxValue);
         int count = keyPoints.Size;
         if (count == 0) return null;
         Matrix<Byte> descriptors = new Matrix<Byte>(count, DescriptorSize, 1);
         CvBriefDescriptorComputeDescriptors(_ptr, image, keyPoints, descriptors);
         Debug.Assert(keyPoints.Size == descriptors.Rows);
         return descriptors;
      }

      /// <summary>
      /// Release all the unmanaged resource associated with BRIEF
      /// </summary>
      protected override void DisposeObject()
      {
         CvBriefDescriptorExtractorRelease(ref _ptr);
      }
   }
}
