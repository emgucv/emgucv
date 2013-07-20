//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// A HOG descriptor
   /// </summary>
   public class OclHOGDescriptor : UnmanagedObject
   {
      /// <summary>
      /// Create a new HOGDescriptor
      /// </summary>
      public OclHOGDescriptor()
      {
         _ptr = OclInvoke.oclHOGDescriptorCreateDefault();
      }

      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters
      /// </summary>
      /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
      /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
      /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
      /// <param name="gammaCorrection">Do gamma correction preprocessing or not. Use true for default.</param>
      /// <param name="L2HysThreshold">L2-Hys normalization method shrinkage. Use 0.2 for default.</param>
      /// <param name="nbins">Number of bins. Use 9 bins per cell for deafault.</param>
      /// <param name="nLevels">Maximum number of detection window increases. Use 64 for default</param>
      /// <param name="winSigma">Gaussian smoothing window parameter. Use -1 for default.</param>
      /// <param name="winSize">Detection window size. Must be aligned to block size and block stride. Must match the size of the training image. Use (64, 128) for default.</param>
      public OclHOGDescriptor(
         Size winSize,
         Size blockSize,
         Size blockStride,
         Size cellSize,
         int nbins,
         double winSigma,
         double L2HysThreshold,
         bool gammaCorrection,
         int nLevels)
      {
         _ptr = OclInvoke.oclHOGDescriptorCreate(
            ref winSize,
            ref blockSize,
            ref blockStride,
            ref cellSize,
            nbins,
            winSigma,
            L2HysThreshold,
            gammaCorrection,
            nLevels);
      }

      /// <summary>
      /// Returns coefficients of the classifier trained for people detection (for default window size).
      /// </summary>
      /// <returns>The default people detector</returns>
      public static float[] GetDefaultPeopleDetector()
      {
         return GetPeopleDetector64x128();
      }

      /// <summary>
      /// Returns coefficients of the classifier trained for people detection (for size 64x128). Only compatible with HOG detector with the same windows size.
      /// </summary>
      /// <returns>The people detector of 48x96 resolution</returns>
      public static float[] GetPeopleDetector48x96()
      {
         using (VectorOfFloat f = new VectorOfFloat())
         {
            OclInvoke.oclHOGDescriptorGetPeopleDetector48x96(f);
            return f.ToArray();
         }
      }

      /// <summary>
      /// Returns coefficients of the classifier trained for people detection (for size 64x128).
      /// </summary>
      /// <returns>The people detector of 64x128 resolution.</returns>
      public static float[] GetPeopleDetector64x128()
      {
         using (VectorOfFloat f = new VectorOfFloat())
         {
            OclInvoke.oclHOGDescriptorGetPeopleDetector64x128(f);
            return f.ToArray();
         }
      }

      /// <summary>
      /// Set the SVM detector 
      /// </summary>
      /// <param name="detector">The SVM detector</param>
      public void SetSVMDetector(float[] detector)
      {
         using (VectorOfFloat vec = new VectorOfFloat())
         {
            vec.Push(detector);
            OclInvoke.oclHOGSetSVMDetector(_ptr, vec);
         }
      }

      private Rectangle[] DetectMultiScale(
         IntPtr image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold)
      {
         using (MemStorage storage = new MemStorage())
         {
            Seq<Rectangle> rectSeq = new Seq<Rectangle>(storage);
            OclInvoke.oclHOGDescriptorDetectMultiScale(_ptr, image, rectSeq, hitThreshold, winStride, padding, scale, groupThreshold);
            return rectSeq.ToArray();
         }
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The OclImage to search in</param>
      /// <param name="hitThreshold">The threshold for the distance between features and classifying plane.</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding">Mock parameter to keep CPU interface compatibility. Must be (0,0).</param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="groupThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(
         OclImage<Bgra, Byte> image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold)
      {
         return DetectMultiScale(image.Ptr, hitThreshold, winStride, padding, scale, groupThreshold);
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The OclImage to search in</param>
      /// <param name="hitThreshold">The threshold for the distance between features and classifying plane.</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding">Mock parameter to keep CPU interface compatibility. Must be (0,0).</param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="groupThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(
         OclImage<Gray, Byte> image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold)
      {
         return DetectMultiScale(image.Ptr, hitThreshold, winStride, padding, scale, groupThreshold);
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The OclImage to search in</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(OclImage<Bgra, Byte> image)
      {
         return DetectMultiScale(image, 0, new Size(0, 0), new Size(0, 0), 1.05, 2);
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The GpuImage to search in</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(OclImage<Gray, Byte> image)
      {
         return DetectMultiScale(image, 0, new Size(0, 0), new Size(0, 0), 1.05, 2);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this HOGDescriptor
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.oclHOGDescriptorRelease(ref _ptr);
      }
   }

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclHOGDescriptorGetPeopleDetector64x128(IntPtr vector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclHOGDescriptorGetPeopleDetector48x96(IntPtr vector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr oclHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr oclHOGDescriptorCreate(
         ref Size winSize,
         ref Size blockSize,
         ref Size blockStride,
         ref Size cellSize,
         int nbins,
         double winSigma,
         double L2HysThreshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool gammaCorrection,
         int nLevels);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclHOGDescriptorRelease(ref IntPtr descriptor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclHOGDescriptorDetectMultiScale(
         IntPtr descriptor,
         IntPtr img,
         IntPtr foundLocations,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold);
   }
}
