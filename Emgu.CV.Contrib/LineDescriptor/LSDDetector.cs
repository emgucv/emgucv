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
using System.Diagnostics;


namespace Emgu.CV.LineDescriptor
{
   public class LSDDetector : UnmanagedObject
   {
      public LSDDetector()
      {
         _ptr = LineDescriptorInvoke.cveLineDescriptorLSDDetectorCreate();
      }

      public void Detect(Mat image, VectorOfKeyLine keylines, int scale, int numOctaves, Mat mask = null)
      {
         LineDescriptorInvoke.cveLineDescriptorLSDDetectorDetect(_ptr, image, keylines, scale, numOctaves, mask);
      }
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            LineDescriptorInvoke.cveLineDescriptorLSDDetectorRelease(ref _ptr);
      }
   }

   public static partial class LineDescriptorInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveLineDescriptorLSDDetectorCreate();
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveLineDescriptorLSDDetectorDetect(IntPtr detector, IntPtr image, IntPtr keypoints, int scale, int numOctaves, IntPtr mask);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveLineDescriptorLSDDetectorRelease(ref IntPtr detector);
   }

}