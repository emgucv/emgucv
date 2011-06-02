//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// A HOG discriptor
   /// </summary>
   public class GpuHOGDescriptor : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuHOGDescriptorPeopleDetectorCreate(IntPtr seq);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr gpuHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr gpuHOGDescriptorCreate(
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
      private extern static void gpuHOGDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuHOGDescriptorDetectMultiScale(
         IntPtr descriptor,
         IntPtr img,
         IntPtr foundLocations,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold);
      #endregion

      private MemStorage _rectStorage;
      private Seq<Rectangle> _rectSeq;
      private VectorOfFloat _vector;

      /// <summary>
      /// Create a new HOGDescriptor
      /// </summary>
      public GpuHOGDescriptor()
      {
         _ptr = gpuHOGDescriptorCreateDefault();
         _rectStorage = new MemStorage();
         _rectSeq = new Seq<Rectangle>(_rectStorage);
         _vector = new VectorOfFloat();
      }

      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters
      /// </summary>
      /// <param name="blockSize">Block size in cells. Only (2,2) is supported for now.</param>
      /// <param name="cellSize">Cell size. Only (8, 8) is supported for now.</param>
      /// <param name="blockStride">Block stride. Must be a multiple of cell size.</param>
      /// <param name="gammaCorrection">Do gamma correction preprocessing or not.</param>
      /// <param name="L2HysThreshold">L2-Hys normalization method shrinkage.</param>
      /// <param name="nbins">Number of bins. Only 9 bins per cell is supported for now.</param>
      /// <param name="nLevels">Maximum number of detection window increases.</param>
      /// <param name="winSigma">Gaussian smoothing window parameter.</param>
      /// <param name="winSize">Detection window size. Must be aligned to block size and block stride.</param>
      public GpuHOGDescriptor(
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
         _ptr = gpuHOGDescriptorCreate(
            ref winSize,
            ref blockSize,
            ref blockStride,
            ref cellSize,
            nbins,
            winSigma,
            L2HysThreshold,
            gammaCorrection,
            nLevels);

         _rectStorage = new MemStorage();
         _rectSeq = new Seq<Rectangle>(_rectStorage);
      }

      /// <summary>
      /// Returns coefficients of the classifier trained for people detection (for default window size).
      /// </summary>
      /// <returns>The default people detector</returns>
      public static float[] GetDefaultPeopleDetector()
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<float> desc = new Seq<float>(stor);
            gpuHOGDescriptorPeopleDetectorCreate(desc);
            return desc.ToArray();
         }
      }

      /// <summary>
      /// Set the SVM detector 
      /// </summary>
      /// <param name="detector">The SVM detector</param>
      public void SetSVMDetector(float[] detector)
      {
         _vector.Clear();
         _vector.Push(detector);
         gpuHOGSetSVMDetector(_ptr, _vector);
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The GpuImage to search in</param>
      /// <param name="hitThreshold">The threshold for the distance between features and classifying plane.</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding">Mock parameter to keep CPU interface compatibility. Must be (0,0).</param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="groupThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(
         GpuImage<Bgra, Byte> image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold)
      {
         gpuHOGDescriptorDetectMultiScale(_ptr, image, _rectSeq, hitThreshold, winStride, padding, scale, groupThreshold);
         return _rectSeq.ToArray();
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The GpuImage to search in</param>
      /// <param name="hitThreshold">The threshold for the distance between features and classifying plane.</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding">Mock parameter to keep CPU interface compatibility. Must be (0,0).</param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="groupThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(
         GpuImage<Gray, Byte> image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold)
      {
         gpuHOGDescriptorDetectMultiScale(_ptr, image, _rectSeq, hitThreshold, winStride, padding, scale, groupThreshold);
         return _rectSeq.ToArray();
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The GpuImage to search in</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(GpuImage<Bgra, Byte> image)
      {
         return DetectMultiScale(image, 0, new Size(8, 8), new Size(0, 0), 1.05, 2);
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The GpuImage to search in</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(GpuImage<Gray, Byte> image)
      {
         return DetectMultiScale(image, 0, new Size(8, 8), new Size(0, 0), 1.05, 2);
      }

      /// <summary>
      /// Release the managed resources associated with this object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         _rectStorage.Dispose();
         _vector.Dispose();
         base.ReleaseManagedResources();
      }

      /// <summary>
      /// Release the unmanaged memory associated with this HOGDescriptor
      /// </summary>
      protected override void DisposeObject()
      {
         gpuHOGDescriptorRelease(_ptr);
      }
   }
}
