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
   public class StructuredEdgeDetection : UnmanagedObject
   {
      public StructuredEdgeDetection(String model, RFFeatureGetter howToGetFeatures)
      {
         using (CvString sModel = new CvString(model))
            _ptr = XimgprocInvoke.cveStructuredEdgeDetectionCreate(sModel, howToGetFeatures);
      }

      public void DetectEdges(Mat src, Mat dst)
      {
         XimgprocInvoke.cveStructuredEdgeDetectionDetectEdges(_ptr, src, dst);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveStructuredEdgeDetectionRelease(ref _ptr);
         }
      }
   }

   public class RFFeatureGetter : UnmanagedObject
   {

      public RFFeatureGetter()
      {
         _ptr = XimgprocInvoke.cveRFFeatureGetterCreate();
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveRFFeatureGetterRelease(ref _ptr);
         }
      }
   }

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
