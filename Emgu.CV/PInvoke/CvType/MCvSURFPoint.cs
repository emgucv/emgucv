using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Wrapped CvSURFPoint structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSURFPoint
   {
      /// <summary>
      /// Position of the feature within the image
      /// </summary>
      public MCvPoint2D32f pt;

      /// <summary>
      /// -1, 0 or +1. sign of the laplacian at the point.
      /// can be used to speedup feature comparison
      /// (normally features with laplacians of different signs can not match)
      /// </summary>
      public int laplacian;

      /// <summary>
      /// Size of the feature
      /// </summary>
      public int size;

      /// <summary>
      /// Orientation of the feature: 0..360 degrees
      /// </summary>
      public float dir;

      /// <summary>
      /// value of the hessian (can be used to approximately estimate the feature strengths;
      /// see also params.hessianThreshold.
      /// </summary>
      public float hessian;
   }

   /*
   /// <summary>
   /// The basic SURF descriptor (64 elements)
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSURFDescriptor
   {
      /// <summary>
      /// The 64 elements of the descriptor
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
      public float[] values;
   }

   /// <summary>
   /// The Extended SURF descriptor (128 elements)
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSURFDescriptorExtended
   {
      /// <summary>
      /// The 128 elements of the descriptor
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
      public float[] values;
   }*/
}
