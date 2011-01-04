using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// A HOG discriptor
   /// </summary>
   public class GpuHOGDescriptor : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuHOGDescriptorPeopleDetectorCreate(IntPtr seq);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr gpuHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuHOGDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      /// Return the default people detector
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
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <param name="hitThreshold"></param>
      /// <param name="winStride"></param>
      /// <param name="padding"></param>
      /// <param name="scale"></param>
      /// <param name="groupThreshold"></param>
      /// <returns></returns>
      public Rectangle[] DetectMultiScale(
         GpuImage<Bgr, Byte> image,
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
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <returns></returns>
      public Rectangle[] DetectMultiScale(GpuImage<Bgr, Byte> image)
      {
         return DetectMultiScale(image, 0, new Size(8, 8), new Size(32, 32), 1.05, 2);
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
