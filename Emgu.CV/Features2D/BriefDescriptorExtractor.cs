//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
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
      /// Create a BRIEF descriptor extractor.
      /// </summary>
      public BriefDescriptorExtractor()
         :this(32)
      {
      }

      /// <summary>
      /// Create a BRIEF descriptor extractor.
      /// </summary>
      /// <param name="descriptorSize">The size of descriptor. It can be equal 16, 32 or 64 bytes. Use 32 for deafault.</param>
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
         int count = keyPoints.Size;
         if (count == 0) return null;
         Matrix<Byte> descriptors = new Matrix<Byte>(count, DescriptorSize, 1);
         CvBriefDescriptorComputeDescriptors(_ptr, image, keyPoints, descriptors);
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
