//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
   /// <summary>
   /// BRIEF Descriptor
   /// </summary>
   public class BriefDescriptorExtractor : Feature2D
   {
      /// <summary>
      /// Create a BRIEF descriptor extractor.
      /// </summary>
      /// <param name="descriptorSize">The size of descriptor. It can be equal 16, 32 or 64 bytes.</param>
      public BriefDescriptorExtractor(int descriptorSize = 32)
      {
         _ptr = Emgu.CV.ContribInvoke.cveBriefDescriptorExtractorCreate(descriptorSize, ref _feature2D);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with BRIEF
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            Emgu.CV.ContribInvoke.cveBriefDescriptorExtractorRelease(ref _ptr);
         base.DisposeObject();
      }
   }
}

namespace Emgu.CV
{

   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveBriefDescriptorExtractorCreate(int descriptorSize, ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveBriefDescriptorExtractorRelease(ref IntPtr extractor);
   }
}
