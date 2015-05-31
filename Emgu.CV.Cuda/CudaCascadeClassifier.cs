//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
 using Emgu.CV.CvEnum;
 using Emgu.CV.Structure;
 using Emgu.CV.Util;
 using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Cascade Classifier for object detection using Cuda
   /// </summary>
   public class CudaCascadeClassifier : UnmanagedObject
   {
      private GpuMat _buffer;

      /// <summary>
      /// Create a Cuda cascade classifier using the specific file
      /// </summary>
      /// <param name="fileName">The file to create the classifier from</param>
      public CudaCascadeClassifier(String fileName)
      {
#if !NETFX_CORE
         Debug.Assert(File.Exists(fileName), String.Format("The Cascade file {0} does not exist.", fileName));
#endif
         using (CvString s = new CvString(fileName))
            _ptr = CudaInvoke.cudaCascadeClassifierCreate(s);
         _buffer = new GpuMat(1, 100, DepthType.Cv32S, 4);
      }

      /// <summary>
      /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles.
      /// </summary>
      /// <param name="image">The image where search will take place</param>
      /// <returns>An array of regions for the detected objects</returns>
      public void DetectMultiScale(IInputArray image, IOutputArray objects, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaObjects = objects.GetOutputArray())
            CudaInvoke.cudaCascadeClassifierDetectMultiScale(_ptr, iaImage, oaObjects,
               stream == null ? IntPtr.Zero : stream.Ptr);
      }

      public Rectangle[] Convert(IOutputArray objects)
      {
         using (OutputArray oaObjects = objects.GetOutputArray())
         using (VectorOfRect vr = new VectorOfRect())
         {
            CudaInvoke.cudaCascadeClassifierConvert(_ptr, oaObjects, vr);
            return vr.ToArray();
         }
      }

      /// <summary>
      /// Release all unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaCascadeClassifierRelease(ref _ptr);
         if (_buffer != null)
            _buffer.Dispose();
      }

      public double ScaleFactor
      {
         get { return CudaInvoke.cudaCascadeClassifierGetScaleFactor(_ptr); }
         set
         {
            CudaInvoke.cudaCascadeClassifierSetScaleFactor(_ptr, value);
         }
      }

      public int MinNeighbors
      {
         get { return CudaInvoke.cudaCascadeClassifierGetMinNeighbors(_ptr); }
         set {  CudaInvoke.cudaCascadeClassifierSetMinNeighbors(_ptr, value);}
      }

      public Size MinObjectSize
      {
         get
         {
            Size s = new Size();
            CudaInvoke.cudaCascadeClassifierGetMinObjectSize(_ptr, ref s);
            return s;
         }
         set
         {
            CudaInvoke.cudaCascadeClassifierSetMinObjectSize(_ptr, ref value);
         }
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaCascadeClassifierCreate(IntPtr filename);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCascadeClassifierRelease(ref IntPtr classified);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cudaCascadeClassifierDetectMultiScale(IntPtr classifier, IntPtr image, IntPtr objects, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCascadeClassifierConvert(IntPtr classifier, IntPtr gpuObjects, IntPtr objects);

           [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static double cudaCascadeClassifierGetScaleFactor(IntPtr classifier);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCascadeClassifierSetScaleFactor(IntPtr classifier, double scaleFactor);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int cudaCascadeClassifierGetMinNeighbors(IntPtr classifier);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCascadeClassifierSetMinNeighbors(IntPtr classifier, int minNeighbours);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCascadeClassifierGetMinObjectSize(IntPtr classifier, ref Size minObjectSize);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCascadeClassifierSetMinObjectSize(IntPtr classifier, ref Size minObjectSize);
   }
}
