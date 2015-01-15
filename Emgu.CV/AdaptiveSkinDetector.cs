/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Adaptive Skin Detector
   /// </summary>
   public class AdaptiveSkinDetector : UnmanagedObject
   {
      static AdaptiveSkinDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Morphing method
      /// </summary>
      public enum MorphingMethod
      {
         /// <summary>
         /// None
         /// </summary>
         None = 0,
         /// <summary>
         /// Erode
         /// </summary>
         Erode = 1,
         /// <summary>
         /// Double Erode 
         /// </summary>
         ErodeErode = 2,
         /// <summary>
         /// Erode dilate
         /// </summary>
         ErodeDilate = 3
      }

      /// <summary>
      /// Create an Adaptive Skin Detector
      /// </summary>
      /// <param name="samplingDivider">Use 1 for default</param>
      /// <param name="morphingMethod">The morphine method for the skin detector</param>
      public AdaptiveSkinDetector(int samplingDivider, MorphingMethod morphingMethod)
      {
         _ptr = CvAdaptiveSkinDetectorCreate(samplingDivider, morphingMethod);
      }

      /// <summary>
      /// Process the image to produce a hue mask
      /// </summary>
      /// <param name="image">The input image</param>
      /// <param name="hueMask">The resulting mask</param>
      public void Process(Image<Bgr, Byte> image, Image<Gray, Byte> hueMask)
      {
         CvAdaptiveSkinDetectorProcess(_ptr, image.Ptr, hueMask.Ptr);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvAdaptiveSkinDetectorRelease(_ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvAdaptiveSkinDetectorCreate(int samplingDivider, AdaptiveSkinDetector.MorphingMethod morphingMethod);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvAdaptiveSkinDetectorRelease(IntPtr detector);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvAdaptiveSkinDetectorProcess(IntPtr detector, IntPtr inputBGRImage, IntPtr outputHueMask);
   }
}
*/