//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Softcascade
{
   /// <summary>
   /// Implementation of soft (stageless) cascaded detector.
   /// </summary>
   public class SoftCascadeDetector : UnmanagedObject
   {
      public enum RejectionCriteria
      {
         NoReject = 1,
         Dollar = 2,
         Default = NoReject,
         NmsMask = 0xF
      };

      public struct Detection
      {
         public Detection(Rectangle boundingBox, float confident)
         {
            _boundingBox = boundingBox;
            _confident = confident;
         }

         private Rectangle _boundingBox;
         private float _confident;

         public Rectangle BoundingBox
         {
            get { return _boundingBox; }
            set { _boundingBox = value; }
         }

         public float Confident
         {
            get { return _confident; }
            set { _confident = value; }
         }
      }

      /// <summary>
      /// Create a soft (stageless) cascaded detector.
      /// </summary>
      /// <param name="trainedCascadeFileName">File name of the trained soft cascade detector</param>
      /// <param name="minScale">A minimum scale relative to the original size of the image on which cascade will be applied. Use 0.4 for default.</param>
      /// <param name="maxScale">A maximum scale relative to the original size of the image on which cascade will be applied. Use 5.0 for default</param>
      /// <param name="scales">Number of scales from minScale to maxScale. Use 55 for default</param>
      /// <param name="rejCriteria">Algorithm used for non maximum suppression.</param>
      public SoftCascadeDetector(String trainedCascadeFileName, double minScale, double maxScale, int scales, RejectionCriteria rejCriteria)
      {
         _ptr = SoftCascadeInvoke.cvSoftCascadeDetectorCreate(trainedCascadeFileName, minScale, maxScale, scales, rejCriteria);
      }

      public Detection[] Detect(Image<Bgr, Byte> image, Rectangle[] rois)
      {
         using (VectorOfRect roiRects = new VectorOfRect())
         using (VectorOfRect regions = new VectorOfRect())
         using (VectorOfFloat confidents = new VectorOfFloat())
         {
            IntPtr roisPtr;
            if (rois == null || rois.Length == 0)
            {
               roisPtr = IntPtr.Zero;
            }
            else
            {
               roiRects.Push(rois);
               roisPtr = roiRects.Ptr;
            }
            SoftCascadeInvoke.cvSoftCascadeDetectorDetect(_ptr, image, roisPtr, regions, confidents);

            if (regions.Size == 0)
               return new Detection[0];
            else
            {
               Rectangle[] regionArr = regions.ToArray();
               float[] confidentArr = confidents.ToArray();
               Detection[] results = new Detection[regionArr.Length];
               for (int i = 0; i < results.Length; i++)
               {
                  results[i] = new Detection(regionArr[i], confidentArr[i]);
               }
               return results;
            }
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this detector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            SoftCascadeInvoke.cvSoftCascadeDetectorRelease(ref _ptr);
      }
   }

   public static partial class SoftCascadeInvoke
   {
      static SoftCascadeInvoke()
      {
         //Dummy code to make sure the static constructor of CudaInvoke (and CvInvoke) has been called and the error handler has been registered.
         bool hasCuda = Cuda.CudaInvoke.HasCuda;
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvSoftCascadeDetectorCreate(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String fileName, 
         double minScale, double maxScale, int scales, SoftCascadeDetector.RejectionCriteria rejCriteria);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvSoftCascadeDetectorDetect(IntPtr detector, IntPtr image, IntPtr rois, IntPtr rects, IntPtr confidents);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvSoftCascadeDetectorRelease(ref IntPtr detector);
   }
}
