/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using Emgu.Util;
using Emgu.CV.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// SelfSimDescriptor
   /// </summary>
   public class SelfSimDescriptor : UnmanagedObject
   {
      static SelfSimDescriptor()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="smallSize"></param>
      /// <param name="largeSize"></param>
      /// <param name="startDistanceBucket"></param>
      /// <param name="numberOfDistanceBuckets"></param>
      /// <param name="numberOfAngles"></param>
      public SelfSimDescriptor(
         int smallSize = 5,
         int largeSize = 41,
         int startDistanceBucket = 3,
         int numberOfDistanceBuckets = 7,
         int numberOfAngles = 20)
      {
         _ptr = CvSelfSimDescriptorCreate(smallSize, largeSize, startDistanceBucket, numberOfDistanceBuckets, numberOfAngles);
      }

      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return CvSelfSimDescriptorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <param name="winStride"></param>
      /// <param name="locations"></param>
      /// <returns></returns>
      public float[] Compute(Mat image, Size winStride, Point[] locations)
      {
         using (VectorOfFloat vof = new VectorOfFloat())
         using (VectorOfPoint vp = new VectorOfPoint(locations))
         {
            CvSelfSimDescriptorCompute(_ptr, image, vof, ref winStride, vp);
            return vof.ToArray();
         }
      }

      /// <summary>
      /// Release all unmanaged memory associated with this descriptor
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvSelfSimDescriptorRelease(_ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSelfSimDescriptorCreate(
         int smallSize,
         int largeSize,
         int startDistanceBucket,
         int numberOfDistanceBuckets,
         int numberOfAngles);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSelfSimDescriptorRelease(IntPtr descriptor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSelfSimDescriptorCompute(
         IntPtr descriptor,
         IntPtr image,
         IntPtr descriptors,
         ref Size winStride,
         IntPtr locations);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvSelfSimDescriptorGetDescriptorSize(IntPtr descriptor);
   }
}
*/