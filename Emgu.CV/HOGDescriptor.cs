using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// A HOG discriptor
   /// </summary>
   public class HOGDescriptor : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvHOGDescriptorPeopleDetectorCreate(IntPtr seq);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static IntPtr CvHOGDescriptorCreateDefault();

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
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
         [MarshalAs(UnmanagedType.I1)]
         bool gammaCorrection);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvHOGDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvHOGSetSVMDetector(IntPtr descriptor, IntPtr svmDetector);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvHOGDescriptorDetectMultiScale(
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
      public HOGDescriptor()
      {
         _ptr = CvHOGDescriptorCreateDefault();
         _rectStorage = new MemStorage();
         _rectSeq = new Seq<Rectangle>(_rectStorage);
         _vector = new VectorOfFloat();
      }

      /// <summary>
      /// Create a new HOGDescriptor using the specific parameters
      /// </summary>
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

         _rectStorage = new MemStorage();
         _rectSeq = new Seq<Rectangle>(_rectStorage);
      }

      /// <summary>
      /// Return the default people detector
      /// </summary>
      /// <returns>the default people detector</returns>
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
         Image<Bgr, Byte> image,
         double hitThreshold,
         Size winStride,
         Size padding,
         double scale,
         int groupThreshold)
      {
         CvHOGDescriptorDetectMultiScale(_ptr, image, _rectSeq, hitThreshold, winStride, padding, scale, groupThreshold);
         return _rectSeq.ToArray();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <returns></returns>
      public Rectangle[] DetectMultiScale(Image<Bgr, Byte> image)
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
         CvHOGDescriptorRelease(_ptr);
      }
   }
}
