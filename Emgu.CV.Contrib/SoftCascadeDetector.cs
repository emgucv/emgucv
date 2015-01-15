/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      /// <summary>
      /// Algorithm used for non maximum suppression.
      /// </summary>
      public enum RejectionCriteria
      {
         /// <summary>
         /// No rejection
         /// </summary>
         NoReject = 1,
         /// <summary>
         /// Dollar
         /// </summary>
         Dollar = 2,
         /// <summary>
         /// Default
         /// </summary>
         Default = NoReject,
         /// <summary>
         /// NMS mask
         /// </summary>
         NmsMask = 0xF
      };

      /// <summary>
      /// Detection result
      /// </summary>
      public struct Detection
      {
         /// <summary>
         /// Create detection result from the given data
         /// </summary>
         /// <param name="boundingBox">The bounding box</param>
         /// <param name="confident">The confident level</param>
         public Detection(Rectangle boundingBox, float confident)
         {
            _boundingBox = boundingBox;
            _confident = confident;
         }

         private Rectangle _boundingBox;
         private float _confident;

         /// <summary>
         /// Get or set the bounding box
         /// </summary>
         public Rectangle BoundingBox
         {
            get { return _boundingBox; }
            set { _boundingBox = value; }
         }

         /// <summary>
         /// Get or set the confident
         /// </summary>
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
      /// <param name="minScale">A minimum scale relative to the original size of the image on which cascade will be applied.</param>
      /// <param name="maxScale">A maximum scale relative to the original size of the image on which cascade will be applied.</param>
      /// <param name="scales">Number of scales from minScale to maxScale.</param>
      /// <param name="rejCriteria">Algorithm used for non maximum suppression.</param>
      public SoftCascadeDetector(String trainedCascadeFileName, double minScale = 0.4, double maxScale = 5.0, int scales = 55, RejectionCriteria rejCriteria = RejectionCriteria.NoReject)
      {
         using (CvString s = new CvString(trainedCascadeFileName))
            _ptr = SoftCascadeInvoke.cveSoftCascadeDetectorCreate(s, minScale, maxScale, scales, rejCriteria);
      }

      /// <summary>
      /// Apply cascade to an input frame and return the vector of Detection objects.
      /// </summary>
      /// <param name="image">A frame on which detector will be applied.</param>
      /// <param name="rois">A vector of regions of interest. Only the objects that fall into one of the regions will be returned.</param>
      /// <returns>An output array of Detections.</returns>
      public Detection[] Detect(IInputArray image, Rectangle[] rois = null)
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
            using (InputArray iaImage = image.GetInputArray())
            SoftCascadeInvoke.cveSoftCascadeDetectorDetect(_ptr, iaImage, roisPtr, regions, confidents);

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
            SoftCascadeInvoke.cveSoftCascadeDetectorRelease(ref _ptr);
      }
   }

   internal static partial class SoftCascadeInvoke
   {
      #if !IOS
      static SoftCascadeInvoke()
      {
         //Dummy code to make sure the static constructor of CudaInvoke (and CvInvoke) has been called and the error handler has been registered.
         bool hasCuda = Cuda.CudaInvoke.HasCuda;
      }
      #endif

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveSoftCascadeDetectorCreate(
         IntPtr fileName, 
         double minScale, double maxScale, int scales, SoftCascadeDetector.RejectionCriteria rejCriteria);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveSoftCascadeDetectorDetect(IntPtr detector, IntPtr image, IntPtr rois, IntPtr rects, IntPtr confidents);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveSoftCascadeDetectorRelease(ref IntPtr detector);
   }
}
*/