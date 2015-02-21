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
   public class CudaHOG : UnmanagedObject
   {
 
      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters
      /// </summary>
      /// <param name="blockSize">Block size in cells. Use (16, 16) for default.</param>
      /// <param name="cellSize">Cell size. Use (8, 8) for default.</param>
      /// <param name="blockStride">Block stride. Must be a multiple of cell size. Use (8,8) for default.</param>
      /// <param name="nbins">Number of bins.</param>
      /// <param name="winSize">Detection window size. Must be aligned to block size and block stride. Must match the size of the training image. Use (64, 128) for default.</param>
      public CudaHOG(
         Size winSize,
         Size blockSize,
         Size blockStride,
         Size cellSize,
         int nbins = 9)
      {
         _ptr = CudaInvoke.cudaHOGCreate(
            ref winSize,
            ref blockSize,
            ref blockStride,
            ref cellSize,
            nbins);
      }

      /// <summary>
      /// Returns coefficients of the classifier trained for people detection (for default window size).
      /// </summary>
      /// <returns>The default people detector</returns>
      public Mat GetDefaultPeopleDetector()
      {
         Mat m = new Mat();
         CudaInvoke.cudaHOGGetDefaultPeopleDetector(_ptr, m);
         return m;
      }

      /// <summary>
      /// Set the SVM detector 
      /// </summary>
      /// <param name="detector">The SVM detector</param>
      public void SetSVMDetector(IInputArray detector)
      {
         using (InputArray iaDetector = detector.GetInputArray())
         {
            CudaInvoke.cudaHOGSetSVMDetector(_ptr, iaDetector);
         }
      }

      /*
      /// <summary>
      /// Performs object detection with increasing detection window.
      /// </summary>
      /// <param name="image">The CudaImage to search in</param>
      /// <returns>The regions where positives are found</returns>
      public MCvObjectDetection[] DetectMultiScale(IInputArray image)
      {
         using (Util.VectorOfRect vr = new VectorOfRect())
         using (Util.VectorOfDouble vd = new VectorOfDouble())
         {
            DetectMultiScale(image, vr, vd);
            Rectangle[] location = vr.ToArray();
            double[] weight = vd.ToArray();
            MCvObjectDetection[] result = new MCvObjectDetection[location.Length];
            for (int i = 0; i < result.Length; i++)
            {
               MCvObjectDetection od = new MCvObjectDetection();
               od.Rect = location[i];
               od.Score = (float)weight[i];
               result[i] = od;
            }
            return result;
         }
      }*/

      public void DetectMultiScale(IInputArray image, VectorOfRect objects, VectorOfDouble confident = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         {
            CudaInvoke.cudaHOGDetectMultiScale(_ptr, iaImage, objects, confident);
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this HOGDescriptor
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaHOGRelease(ref _ptr);
      }

      public double WinSigma
      {
         get { return CudaInvoke.cudaHOGGetWinSigma(_ptr); }
         set { CudaInvoke.cudaHOGSetWinSigma(_ptr, value); }
      }

      public int NumLevels
      {
         get { return CudaInvoke.cudaHOGGetNumLevels(_ptr); }
         set { CudaInvoke.cudaHOGSetNumLevels(_ptr, value);}
      }

      public int GroupThreshold
      {
         get { return CudaInvoke.cudaHOGGetGroupThreshold(_ptr); }
         set
         {
            CudaInvoke.cudaHOGSetGroupThreshold(_ptr, value);
         }
      }

      public double HitThreshold
      {
         get { return CudaInvoke.cudaHOGGetHitThreshold(_ptr); }
         set { CudaInvoke.cudaHOGSetHitThreshold(_ptr, value);}
      }

      public double ScaleFactor
      {
         get { return CudaInvoke.cudaHOGGetScaleFactor(_ptr); }
         set {  CudaInvoke.cudaHOGSetScaleFactor(_ptr, value);}
      }

      public bool GammaCorrection
      {
         get { return CudaInvoke.cudaHOGGetGammaCorrection(_ptr); }
         set { CudaInvoke.cudaHOGSetGammaCorrection(_ptr, value);}
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGGetDefaultPeopleDetector(IntPtr hog, IntPtr detector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaHOGCreate(
         ref Size winSize,
         ref Size blockSize,
         ref Size blockStride,
         ref Size cellSize,
         int nbins);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGRelease(ref IntPtr descriptor);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGDetectMultiScale(
         IntPtr descriptor,
         IntPtr img,
         IntPtr foundLocations,
         IntPtr confidents);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static  double cudaHOGGetWinSigma(IntPtr descriptor);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static  void cudaHOGSetWinSigma(IntPtr descriptor, double winSigma);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static  int cudaHOGGetNumLevels(IntPtr descriptor);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetNumLevels(IntPtr descriptor, int numLevels);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cudaHOGGetGroupThreshold(IntPtr descriptor);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetGroupThreshold(IntPtr descriptor, int groupThreshold);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static double cudaHOGGetHitThreshold(IntPtr descriptor);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetHitThreshold(IntPtr descriptor, double hitThreshold);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static double cudaHOGGetScaleFactor(IntPtr descriptor);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetScaleFactor(IntPtr descriptor, double scaleFactor); 

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGGetWinStride(IntPtr descriptor, ref Size winStride);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetWinStride(IntPtr descriptor, ref Size winStride);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return:MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cudaHOGGetGammaCorrection(IntPtr descriptor);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetGammaCorrection(
         IntPtr descriptor, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool gammaCorrection);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static double cudaHOGGetL2HysThreshold(IntPtr descriptor);
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaHOGSetL2HysThreshold(IntPtr descriptor, double l2HysThreshold);
   }
}
