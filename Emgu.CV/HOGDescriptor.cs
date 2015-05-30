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

namespace Emgu.CV
{
   /// <summary>
   /// A HOG descriptor
   /// </summary>
   public class HOGDescriptor : UnmanagedObject
   {
      static HOGDescriptor()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a new HOGDescriptor
      /// </summary>
      public HOGDescriptor()
      {
         _ptr = CvHOGDescriptorCreateDefault();
      }

      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters.
      /// </summary>
      /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
      /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
      /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
      /// <param name="gammaCorrection">Do gamma correction preprocessing or not. Use true for default.</param>
      /// <param name="L2HysThreshold">L2-Hys normalization method shrinkage.</param>
      /// <param name="nbins">Number of bins.</param>
      /// <param name="winSigma">Gaussian smoothing window parameter.</param>
      /// <param name="winSize">Detection window size. Must be aligned to block size and block stride. Must match the size of the training image. Use (64, 128) for default.</param>
      /// <param name="derivAperture"></param>
      public HOGDescriptor(
         Size winSize,
         Size blockSize,
         Size blockStride,
         Size cellSize,
         int nbins = 9,
         int derivAperture = 1,
         double winSigma = -1,
         double L2HysThreshold = 0.2,
         bool gammaCorrection = true)
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
      }

      private static Size InputArrGetSize(IInputArray arr)
      {
         using (InputArray ia = arr.GetInputArray())
            return ia.GetSize();
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
         IInputArray template,
         Size blockSize,
         Size blockStride,
         Size cellSize,
         int nbins = 9,
         int derivAperture = 1,
         double winSigma = -1,
         double L2HysThreshold = 0.2,
         bool gammaCorrection = true)
         : this(InputArrGetSize(template), blockSize, blockStride, cellSize, nbins, derivAperture, winSigma, L2HysThreshold, gammaCorrection)
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
      public HOGDescriptor(IInputArray template)
         : this(template, new Size(16, 16), new Size(8, 8), new Size(8, 8))
      {
      }

      /// <summary>
      /// Return the default people detector
      /// </summary>
      /// <returns>The default people detector</returns>
      public static float[] GetDefaultPeopleDetector()
      {
         using (Util.VectorOfFloat desc = new VectorOfFloat())
         {
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
         using (VectorOfFloat vec = new VectorOfFloat(detector))
         {
            CvHOGSetSVMDetector(_ptr, vec);
         }
      }

      /// <summary>
      /// Performs object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The image to search in</param>
      /// <param name="hitThreshold">
      /// Threshold for the distance between features and SVM classifying plane.
      /// Usually it is 0 and should be specified in the detector coefficients (as the last free coefficient).
      /// But if the free coefficient is omitted (which is allowed), you can specify it manually here.
      ///</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride.</param>
      /// <param name="padding"></param>
      /// <param name="scale">Coefficient of the detection window increase.</param>
      /// <param name="finalThreshold">After detection some objects could be covered by many rectangles. This coefficient regulates similarity threshold. 0 means don't perform grouping. Should be an integer if not using meanshift grouping. Use 2.0 for default</param>
      /// <param name="useMeanshiftGrouping">If true, it will use meanshift grouping.</param>
      /// <returns>The regions where positives are found</returns>
      public MCvObjectDetection[] DetectMultiScale(
         IInputArray image,
         double hitThreshold = 0,
         Size winStride = new Size(),
         Size padding = new Size(),
         double scale = 1.05,
         double finalThreshold = 2.0,
         bool useMeanshiftGrouping = false)
      {
         using (Util.VectorOfRect vr = new VectorOfRect())
         using (Util.VectorOfDouble vd = new VectorOfDouble())
         using (InputArray iaImage = image.GetInputArray())
         {
            CvHOGDescriptorDetectMultiScale(_ptr, iaImage, vr, vd, hitThreshold, ref winStride, ref padding, scale, finalThreshold, useMeanshiftGrouping);
            Rectangle[] location = vr.ToArray();
            double[] weight = vd.ToArray();
            MCvObjectDetection[] result = new MCvObjectDetection[location.Length];
            for (int i = 0; i < result.Length; i++)
            {
               MCvObjectDetection od = new MCvObjectDetection();
               od.Rect = location[i];
               od.Score = (float) weight[i];
               result[i] = od;
            }
            return result;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="winStride">Window stride. Must be a multiple of block stride. Use Size.Empty for default</param>
      /// <param name="padding">Padding. Use Size.Empty for default</param>
      /// <param name="locations">Locations for the computation. Can be null if not needed</param>
      /// <returns>The descriptor vector</returns>
      public float[] Compute(IInputArray image, Size winStride = new Size(), Size padding = new Size(), Point[] locations = null)
      {
         using (VectorOfFloat desc = new VectorOfFloat())
         using (InputArray iaImage = image.GetInputArray())
         {
            if (locations == null)
            {
               CvHOGDescriptorCompute(_ptr, iaImage, desc, ref winStride, ref padding, IntPtr.Zero);
            }
            else
            {
               using (VectorOfPoint vp = new VectorOfPoint(locations))
               {
                  CvHOGDescriptorCompute(_ptr, iaImage, desc, ref winStride, ref padding, vp);
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

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGDescriptorPeopleDetectorCreate(IntPtr seq);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGDescriptorDetectMultiScale(
         IntPtr descriptor,
         IntPtr img,
         IntPtr foundLocations,
         IntPtr weights,
         double hitThreshold,
         ref Size winStride,
         ref Size padding,
         double scale,
         double finalThreshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useMeanshiftGrouping);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvHOGDescriptorCompute(
         IntPtr descriptor,
         IntPtr img,
         IntPtr descriptors,
         ref Size winStride,
         ref Size padding,
         IntPtr locations);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static uint CvHOGDescriptorGetDescriptorSize(IntPtr descriptor);
   }

}