//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A HOG discriptor
   /// </summary>
   public class HOGDescriptor : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvHOGDescriptorPeopleDetectorCreate(IntPtr seq);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvHOGDescriptorCreate(
         ref Size winSize,
         ref Size blockSize,
         ref Size blockStride,
         ref Size cellSize,
         int nbins,
         int derivAperture,
         double winSigma,
         int histogramNormType,
         double L2HysThreshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool gammaCorrection);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvHOGDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvHOGDescriptorDetectMultiScale(
         IntPtr descriptor,
         IntPtr img,
         IntPtr foundLocations,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvHOGDescriptorCompute(
         IntPtr descriptor,
         IntPtr img,
         IntPtr descriptors,
         Size winStride,
         Size padding,
         IntPtr locations);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static uint CvHOGDescriptorGetDescriptorSize(IntPtr descriptor);

      #endregion

      private VectorOfFloat _vector;

      /// <summary>
      /// Create a new HOGDescriptor
      /// </summary>
      public HOGDescriptor()
      {
         _ptr = CvHOGDescriptorCreateDefault();
         _vector = new VectorOfFloat();
      }

      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters.
      /// </summary>
      /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
      /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
      /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
      /// <param name="gammaCorrection">Do gamma correction preprocessing or not. Use true for default.</param>
      /// <param name="L2HysThreshold">L2-Hys normalization method shrinkage. Use 0.2 for default.</param>
      /// <param name="nbins">Number of bins. Use 9 for default.</param>
      /// <param name="winSigma">Gaussian smoothing window parameter. Use -1 for default. </param>
      /// <param name="winSize">Detection window size. Must be aligned to block size and block stride. Must match the size of the training image. Use (64, 128) for default.</param>
      /// <param name="derivAperture">Use 1 for default.</param>
      public HOGDescriptor(
         Size winSize,
         Size blockSize,
         Size blockStride,
         Size cellSize,
         int nbins,
         int derivAperture,
         double winSigma,
         double L2HysThreshold,
         bool gammaCorrection)
      {
         _ptr = CvHOGDescriptorCreate(
            ref winSize,
            ref blockSize,
            ref blockStride,
            ref cellSize,
            nbins,
            derivAperture,
            winSigma,
            0,
            L2HysThreshold,
            gammaCorrection);
         _vector = new VectorOfFloat();
      }

      /// <summary>
      /// Return the default people detector
      /// </summary>
      /// <returns>The default people detector</returns>
      public static float[] GetDefaultPeopleDetector()
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<float> desc = new Seq<float>(stor);
            CvHOGDescriptorPeopleDetectorCreate(desc);
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
         CvHOGSetSVMDetector(_ptr, _vector);
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The image to search in</param>
      /// <param name="hitThreshold">The threshold for the distance between features and classifying plane.</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding"></param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="groupThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(
         Image<Bgr, Byte> image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<Rectangle> seq = new Seq<Rectangle>(stor);
            CvHOGDescriptorDetectMultiScale(_ptr, image, seq, hitThreshold, winStride, padding, scale, groupThreshold);
            return seq.ToArray();
         }
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The image to search in</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(Image<Bgr, Byte> image)
      {
         return DetectMultiScale(image, 0, new Size(8, 8), new Size(32, 32), 1.05, 2);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride. Use Size.Empty for default</param>
      /// <param name="padding">Padding. Use Size.Empty for default</param>
      /// <param name="locations">Locations for the computation. Can be null if not needed</param>
      /// <returns>The descriptor vector</returns>
      public float[] Compute(Image<Bgr, Byte> image, Size winStride, Size padding, Point[] locations)
      {
         using (VectorOfFloat desc = new VectorOfFloat())
         {
            if (locations == null)
               CvHOGDescriptorCompute(_ptr, image, desc, winStride, padding, IntPtr.Zero);
            else
            {
               using (MemStorage stor = new MemStorage())
               {
                  Seq<Point> locationSeq = new Seq<Point>(stor);
                  CvHOGDescriptorCompute(_ptr, image, desc, winStride, padding, locationSeq);
               }
            }
            return desc.ToArray();
         }
      }

      /// <summary>
      /// Release the managed resources associated with this object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         _vector.Dispose();
         base.ReleaseManagedResources();
      }

      /// <summary>
      /// Release the unmanaged memory associated with this HOGDescriptor
      /// </summary>
      protected override void DisposeObject()
      {
         CvHOGDescriptorRelease(_ptr);
      }

      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public uint DescriptorSize
      {
         get
         {
            return CvHOGDescriptorGetDescriptorSize(_ptr);
         }
      }
   }
}
