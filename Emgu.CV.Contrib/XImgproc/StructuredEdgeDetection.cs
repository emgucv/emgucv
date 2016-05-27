//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Ximgproc
{
   /// <summary>
   /// Class implementing edge detection algorithm from Piotr Dollár and C Lawrence Zitnick. Structured forests for fast edge detection. In Computer Vision (ICCV), 2013 IEEE International Conference on, pages 1841–1848. IEEE, 2013.
   /// </summary>
   public class StructuredEdgeDetection : UnmanagedObject
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="model">name of the file where the model is stored</param>
      /// <param name="howToGetFeatures">optional object inheriting from RFFeatureGetter. You need it only if you would like to train your own forest, pass NULL otherwise</param>
      public StructuredEdgeDetection(String model, RFFeatureGetter howToGetFeatures)
      {
         using (CvString sModel = new CvString(model))
            _ptr = XimgprocInvoke.cveStructuredEdgeDetectionCreate(sModel, howToGetFeatures);
      }

      /// <summary>
      /// The function detects edges in src and draw them to dst. The algorithm underlies this function is much more robust to texture presence, than common approaches, e.g. Sobel
      /// </summary>
      /// <param name="src">source image (RGB, float, in [0;1]) to detect edges</param>
      /// <param name="dst">destination image (grayscale, float, in [0;1]) where edges are drawn</param>
      public void DetectEdges(Mat src, Mat dst)
      {
         XimgprocInvoke.cveStructuredEdgeDetectionDetectEdges(_ptr, src, dst);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveStructuredEdgeDetectionRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Helper class for training part of [P. Dollar and C. L. Zitnick. Structured Forests for Fast Edge Detection, 2013].
   /// </summary>
   public class RFFeatureGetter : UnmanagedObject
   {
      /// <summary>
      /// Create a default RFFeatureGetter
      /// </summary>
      public RFFeatureGetter()
      {
         _ptr = XimgprocInvoke.cveRFFeatureGetterCreate();
      }

      /// <summary>
      /// Release the unmanaged memory associated with this RFFeatureGetter.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveRFFeatureGetterRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// Library to invoke Ximgproc functions
   /// </summary>
   public static partial class XimgprocInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveStructuredEdgeDetectionCreate(IntPtr model, IntPtr howToGetFeatures);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveStructuredEdgeDetectionDetectEdges(IntPtr detection, IntPtr src, IntPtr dst);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveStructuredEdgeDetectionRelease(ref IntPtr detection);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveRFFeatureGetterCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveRFFeatureGetterRelease(ref IntPtr getter);

   }
}
