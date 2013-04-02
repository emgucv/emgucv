//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A HOG discriptor
   /// </summary>
   public class HOGDescriptor : UnmanagedObject
   {
      /// <summary>
      /// Create a new HOGDescriptor
      /// </summary>
      public HOGDescriptor()
      {
         _ptr = CvInvoke.CvHOGDescriptorCreateDefault();
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
         _ptr = CvInvoke.CvHOGDescriptorCreate(
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
      }

      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters.
      /// </summary>
      /// <param name="template">The template image to be detected.</param>
      /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
      /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
      /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
      /// <param name="gammaCorrection">Do gamma correction preprocessing or not. Use true for default.</param>
      /// <param name="L2HysThreshold">L2-Hys normalization method shrinkage. Use 0.2 for default.</param>
      /// <param name="nbins">Number of bins. Use 9 for default.</param>
      /// <param name="winSigma">Gaussian smoothing window parameter. Use -1 for default. </param>
      /// <param name="derivAperture">Use 1 for default.</param>
      public HOGDescriptor(
         Image<Bgr, Byte> template,
         Size blockSize,
         Size blockStride,
         Size cellSize,
         int nbins,
         int derivAperture,
         double winSigma,
         double L2HysThreshold,
         bool gammaCorrection)
         : this(template.Size, blockSize, blockStride, cellSize, nbins, derivAperture, winSigma, L2HysThreshold, gammaCorrection)
      {

         float[] descriptor = Compute(
            template,
            Size.Empty, //new Size(8, 8), //winStride
            Size.Empty, //new Size(16, 16), //padding
            null);
         SetSVMDetector(descriptor);
      }

      /// <summary>
      /// Create a new HogDescriptor using the specific template and default parameters.
      /// </summary>
      /// <param name="template">The template image to be detected.</param>
      public HOGDescriptor(Image<Bgr, Byte> template)
         : this(template, new Size(16, 16), new Size(8, 8), new Size(8, 8), 9, 1, -1, 0.2, true)
      {
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
            CvInvoke.CvHOGDescriptorPeopleDetectorCreate(desc);
            return desc.ToArray();
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
            CvInvoke.CvHOGSetSVMDetector(_ptr, vec);
         }
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The image to search in</param>
      /// <param name="hitThreshold">
      /// Threshold for the distance between features and SVM classifying plane.
      /// Usually it is 0 and should be specfied in the detector coefficients (as the last free coefficient).
      /// But if the free coefficient is omitted (which is allowed), you can specify it manually here.
      ///</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding"></param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="finalThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping. Should be an integer if not using meanshift grouping. Use 2.0 for default</param>
      /// <param name="useMeanshiftGrouping">If true, it will use meanshift grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public Rectangle[] DetectMultiScale(
         Image<Bgr, Byte> image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int finalThreshold,
         bool useMeanshiftGrouping)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<MCvObjectDetection> seq = new Seq<MCvObjectDetection>(stor);
            CvInvoke.CvHOGDescriptorDetectMultiScale(_ptr, image, seq, hitThreshold, winStride, padding, scale, finalThreshold, useMeanshiftGrouping);
            return
#if NETFX_CORE
               Extensions.
#else
               Array.
#endif
               ConvertAll(seq.ToArray(), delegate(MCvObjectDetection obj) { return obj.Rect; });
         }
      }

      /// <summary>
      /// Perfroms object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The image to search in</param>
      /// <returns>The regions where positives are found.</returns>
      public Rectangle[] DetectMultiScale(Image<Bgr, Byte> image)
      {
         return DetectMultiScale(image, 0, new Size(8, 8), new Size(32, 32), 1.05, 2, false);
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
               CvInvoke.CvHOGDescriptorCompute(_ptr, image, desc, winStride, padding, IntPtr.Zero);
            else
            {
               using (MemStorage stor = new MemStorage())
               {
                  Seq<Point> locationSeq = new Seq<Point>(stor);
                  CvInvoke.CvHOGDescriptorCompute(_ptr, image, desc, winStride, padding, locationSeq);
               }
            }
            return desc.ToArray();
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this HOGDescriptor
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvHOGDescriptorRelease(_ptr);
      }

      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public uint DescriptorSize
      {
         get
         {
            return CvInvoke.CvHOGDescriptorGetDescriptorSize(_ptr);
         }
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGDescriptorPeopleDetectorCreate(IntPtr seq);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvHOGDescriptorCreate(
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
      internal extern static void CvHOGDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGDescriptorDetectMultiScale(
         IntPtr descriptor,
         IntPtr img,
         IntPtr foundLocations,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         double finalThreshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useMeanshiftGrouping);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGDescriptorCompute(
         IntPtr descriptor,
         IntPtr img,
         IntPtr descriptors,
         Size winStride,
         Size padding,
         IntPtr locations);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static uint CvHOGDescriptorGetDescriptorSize(IntPtr descriptor);
   }
}