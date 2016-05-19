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
   /// <summary>
   /// The lines extraction methodology described in the following is mainly based on: R Grompone Von Gioi, Jeremie Jakubowicz, Jean-Michel Morel, and Gregory Randall. Lsd: A fast line segment detector with a false detection control. IEEE Transactions on Pattern Analysis and Machine Intelligence, 32(4):722–732, 2010.
   /// </summary>
   public class LSDDetector : UnmanagedObject
   {
      /// <summary>
      /// Default constructor
      /// </summary>
      public LSDDetector()
      {
         _ptr = LineDescriptorInvoke.cveLineDescriptorLSDDetectorCreate();
      }

      /// <summary>
      /// Detect lines inside an image.
      /// </summary>
      /// <param name="image">	input image</param>
      /// <param name="keylines">vector that will store extracted lines for one or more images</param>
      /// <param name="scale">scale factor used in pyramids generation</param>
      /// <param name="numOctaves">number of octaves inside pyramid</param>
      /// <param name="mask">	mask matrix to detect only KeyLines of interest</param>
      public void Detect(Mat image, VectorOfKeyLine keylines, int scale, int numOctaves, Mat mask = null)
      {
         LineDescriptorInvoke.cveLineDescriptorLSDDetectorDetect(_ptr, image, keylines, scale, numOctaves, mask);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this object.
      /// </summary>
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