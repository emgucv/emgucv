//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public class BriefDescriptorExtractor : UnmanagedObject, IDescriptorExtractor
   {
      static BriefDescriptorExtractor()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a BRIEF descriptor extractor.
      /// </summary>
      /// <param name="descriptorSize">The size of descriptor. It can be equal 16, 32 or 64 bytes.</param>
      public BriefDescriptorExtractor(int descriptorSize = 32)
      {
         _ptr = CvBriefDescriptorExtractorCreate(descriptorSize);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with BRIEF
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvBriefDescriptorExtractorRelease(ref _ptr);
      }

      IntPtr IDescriptorExtractor.DescriptorExtratorPtr
      {
         get { return _ptr; }
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBriefDescriptorExtractorCreate(int descriptorSize);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBriefDescriptorExtractorRelease(ref IntPtr extractor);
   }
}
