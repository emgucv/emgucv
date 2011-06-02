//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Cascade Classifier for object detection using GPU
   /// </summary>
   public class GpuCascadeClassifier : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr gpuCascadeClassifierCreate(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String filename);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuCascadeClassifierRelease(ref IntPtr classified);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static int gpuCascadeClassifierDetectMultiScale(IntPtr classifier, IntPtr image, IntPtr objectsBuf, double scaleFactor, int minNeighbors, Size minSize, IntPtr resultSeq);
      #endregion

      private GpuMat<int> _buffer;
      private MemStorage _stor;

      /// <summary>
      /// Create a GPU cascade classifier using the specific file
      /// </summary>
      /// <param name="fileName">The file to create the classifier from</param>
      public GpuCascadeClassifier(String fileName)
      {
         Debug.Assert(File.Exists(fileName), String.Format("The Cascade file {0} does not exist.", fileName));
         _ptr = gpuCascadeClassifierCreate(fileName);
         _buffer = new GpuMat<int>(1, 100, 4);
         _stor = new MemStorage();
      }

      /// <summary>
      /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles.
      /// </summary>
      /// <param name="image">The image where search will take place</param>
      /// <param name="scaleFactor">The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%. Use 1.2 for default.</param>
      /// <param name="minNeighbors">Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure. Use 4 for default.</param>
      /// <param name="minSize">Minimum window size. By default, it is set to the size of samples the classifier has been trained on (~20x20 for face detection). Use Size.Empty for default</param>
      /// <returns>An array of regions for the detected objects</returns>
      public Rectangle[] DetectMultiScale<TColor>(GpuImage<TColor, Byte> image, double scaleFactor, int minNeighbors, Size minSize) where TColor : struct, IColor
      {
         try
         {
            Seq<Rectangle> regions = new Seq<Rectangle>(_stor);
            int count = gpuCascadeClassifierDetectMultiScale(_ptr, image, _buffer, scaleFactor, minNeighbors, minSize, regions);
            if (count == 0) return new Rectangle[0];
            Rectangle[] result = regions.ToArray();
            return result;
         }
         finally
         {
            _stor.Clear();
         }
      }

      /// <summary>
      /// Release all unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         gpuCascadeClassifierRelease(ref _ptr);
         _buffer.Dispose();
         _stor.Dispose();
      }

      /*
      public static void test()
      {
         Trace.WriteLine(Marshal.SizeOf(typeof(Rectangle)) == 4 * sizeof(int));
      }*/
   }
}
