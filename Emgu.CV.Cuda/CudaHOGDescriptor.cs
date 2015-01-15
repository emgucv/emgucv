//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

 using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// A HOG descriptor
   /// </summary>
   public class CudaHOGDescriptor : UnmanagedObject
   {
      /// <summary>
      /// Create a new HOGDescriptor
      /// </summary>
      public CudaHOGDescriptor()
      {
         _ptr = CudaInvoke.cudaHOGDescriptorCreateDefault();
      }

      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters
      /// </summary>
      /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
      /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
      /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
      /// <param name="gammaCorrection">Do gamma correction preprocessing or not.</param>
      /// <param name="L2HysThreshold">L2-Hys normalization method shrinkage.</param>
      /// <param name="nbins">Number of bins.</param>
      /// <param name="nLevels">Maximum number of detection window increases.</param>
      /// <param name="winSigma">Gaussian smoothing window parameter.</param>
      /// <param name="winSize">Detection window size. Must be aligned to block size and block stride. Must match the size of the training image. Use (64, 128) for default.</param>
      public CudaHOGDescriptor(
         Size winSize,
         Size blockSize,
         Size blockStride,
         Size cellSize,
         int nbins = 9,
         double winSigma = -1,
         double L2HysThreshold = 0.2,
         bool gammaCorrection = true,
         int nLevels = 64)
      {
         _ptr = CudaInvoke.cudaHOGDescriptorCreate(
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
            CudaInvoke.cudaHOGDescriptorGetPeopleDetector48x96(f);
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
            CudaInvoke.cudaHOGDescriptorGetPeopleDetector64x128(f);
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
            CudaInvoke.cudaHOGSetSVMDetector(_ptr, vec);
         }
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The CudaImage to search in</param>
      /// <param name="hitThreshold">The threshold for the distance between features and classifying plane.</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding">Mock parameter to keep CPU interface compatibility. Must be (0,0).</param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="groupThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(
         GpuMat image,
         double hitThreshold = 0,
         Size winStride = new Size(),
         Size padding = new Size(),
         double scale = 1.05,
         int groupThreshold = 2)
      {
         using (Util.VectorOfRect vr = new VectorOfRect())
         {
            CudaInvoke.cudaHOGDescriptorDetectMultiScale(_ptr, image, vr.Ptr, hitThreshold, ref winStride, ref padding, scale, groupThreshold);
            return vr.ToArray();
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this HOGDescriptor
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaHOGDescriptorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGDescriptorGetPeopleDetector64x128(IntPtr vector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGDescriptorGetPeopleDetector48x96(IntPtr vector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaHOGDescriptorCreate(
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

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGDescriptorRelease(ref IntPtr descriptor);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGDescriptorDetectMultiScale(
         IntPtr descriptor,
         IntPtr img,
         IntPtr foundLocations,
         double hitThreshold,
         ref Size winStride,
         ref Size padding,
         double scale,
         int groupThreshold);
   }
}
