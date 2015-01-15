/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   public class OpponentColorDescriptorExtractor : UnmanagedObject, IDescriptorExtractor
   {
      private IDescriptorExtractor _baseExtractor;

      /// <summary>
      /// Create a opponent Color descriptor extractor
      /// </summary>
      /// <param name="extractor">The base descriptor extractor</param>
      public OpponentColorDescriptorExtractor(IDescriptorExtractor extractor)
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

      IntPtr IDescriptorExtractor.DescriptorExtratorPtr
      {
         get { return _ptr; }
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get
         {
            return CvInvoke.AlgorithmPtrFromDescriptorExtractorPtr(((IDescriptorExtractor)this).DescriptorExtratorPtr);
         }
      }
   }
}

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvOpponentColorDescriptorExtractorCreate(IntPtr extractor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvOpponentColorDescriptorExtractorRelease(ref IntPtr extractor);

   }
}
*/