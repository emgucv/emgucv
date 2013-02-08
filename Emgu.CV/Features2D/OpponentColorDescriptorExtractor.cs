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
   /// Class adapting a descriptor extractor to compute descriptors in the Opponent Color Space (refer to Van de Sande et al.,
   /// CGIV 2008 Color Descriptors for Object Category Recognition). Input RGB image is transformed in the Opponent
   /// Color Space. Then, an unadapted descriptor extractor (set in the constructor) computes descriptors on each of three
   /// channels and concatenates them into a single color descriptor.
   /// </summary>
   /// <typeparam name="TDescriptor">The type of descriptor</typeparam>
   public class OpponentColorDescriptorExtractor<TDescriptor> : UnmanagedObject, IDescriptorExtractor<Bgr, TDescriptor>
            where TDescriptor : struct
   {
      private IDescriptorExtractor<Gray, TDescriptor> _baseExtractor;

      /// <summary>
      /// Create a opponent Color descriptor extractor
      /// </summary>
      /// <param name="extractor">The base descriptor extractor</param>
      public OpponentColorDescriptorExtractor(IDescriptorExtractor<Gray, TDescriptor> extractor)
      {
         _baseExtractor = extractor;
         _ptr = CvInvoke.CvOpponentColorDescriptorExtractorCreate(extractor.DescriptorExtratorPtr);
      }

      /// <summary>
      /// Release the memory associated with this opponent color descriptor extractor
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvOpponentColorDescriptorExtractorRelease(ref _ptr);
      }

      IntPtr IDescriptorExtractor<Bgr, TDescriptor>.DescriptorExtratorPtr
      {
         get { return _ptr; }
      }
   }
}

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvOpponentColorDescriptorExtractorCreate(IntPtr extractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvOpponentColorDescriptorExtractorRelease(ref IntPtr extractor);

   }
}
