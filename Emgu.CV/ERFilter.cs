//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed cv::ERStat structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvERStat
   {
      /// <summary>
      /// Pixel
      /// </summary>
      public int Pixel;

      /// <summary>
      /// Level
      /// </summary>
      public int Level;

      /// <summary>
      /// Area
      /// </summary>
      public int Area;
      /// <summary>
      /// Perimeter
      /// </summary>
      public int Perimeter;
      /// <summary>
      /// Euler number
      /// </summary>
      public int Euler;
      /// <summary>
      /// Rectangle
      /// </summary>
      public System.Drawing.Rectangle Rect;

      /// <summary>
      /// Order 1 raw moments to derive the centroid
      /// </summary>
      public double RawMoments0;
      /// <summary>
      /// Order 1 raw moments to derive the centroid
      /// </summary>
      public double RawMoments1;
      /// <summary>
      /// Order 2 central moments to construct the covariance matrix
      /// </summary>
      public double CentralMoments0;
      /// <summary>
      /// Order 2 central moments to construct the covariance matrix
      /// </summary>
      public double CentralMoments1;
      /// <summary>
      /// Order 2 central moments to construct the covariance matrix
      /// </summary>
      public double CentralMoments2;
      /// <summary>
      /// Pointer to horizontal crossings
      /// </summary>
      public IntPtr Crossings;

      /// <summary>
      /// Median of the crossings at three different height levels
      /// </summary>
      public float MedCrossings;

      /// <summary>
      /// Hole area ratio
      /// </summary>
      public float HoleAreaRatio;
      /// <summary>
      /// Convex hull ratio
      /// </summary>
      public float ConvexHullRatio;
      /// <summary>
      /// Number of inflexion points
      /// </summary>
      public float NumInflexionPoints;

      /// <summary>
      /// Pointer to pixels
      /// </summary>
      public IntPtr Pixels;


      /// <summary>
      /// probability that the ER belongs to the class we are looking for
      /// </summary>
      public double probability;


      /// <summary>
      /// Pointer to the parent ERStat
      /// </summary>
      public IntPtr ParentPtr;
      /// <summary>
      /// Pointer to the child ERStat
      /// </summary>
      public IntPtr ChildPtr;
      /// <summary>
      /// Pointer to the next ERStat
      /// </summary>
      public IntPtr NextPtr;
      /// <summary>
      /// Pointer to the previous ERStat
      /// </summary>
      public IntPtr PrevPtr;


      /// <summary>
      /// If or not the regions is a local maxima of the probability
      /// </summary>
      //[MarshalAs(UnmanagedType.U1)]
      public Byte LocalMaxima;


      /// <summary>
      /// Pointer to the ERStat that is the max probability ancestor
      /// </summary>
      public IntPtr MaxProbabilityAncestor;
      /// <summary>
      /// Pointer to the ERStat that is the min probability ancestor
      /// </summary>
      public IntPtr MinProbabilityAncestor;

      /// <summary>
      /// Get the center of the region
      /// </summary>
      /// <param name="imageWidth">The source image width</param>
      /// <returns>The center of the region</returns>
      public System.Drawing.Point GetCenter(int imageWidth)
      {
         return new System.Drawing.Point(Pixel % imageWidth, Pixel / imageWidth);
      }
   }
}

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of ERStat.
   /// </summary>
   public class VectorOfERStat : Emgu.Util.UnmanagedObject
   {
      /// <summary>
      /// Create an empty standard vector of ERStat
      /// </summary>
      public VectorOfERStat()
      {
         _ptr = CvInvoke.VectorOfERStatCreate();
      }

      /// <summary>
      /// Create an standard vector of ERStat of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfERStat(int size)
      {
         _ptr = CvInvoke.VectorOfERStatCreateSize(size);
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return CvInvoke.VectorOfERStatGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         CvInvoke.VectorOfERStatClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return CvInvoke.VectorOfERStatGetStartAddress(_ptr);
         }
      }

      /// <summary>
      /// Convert the standard vector to an array of ERStat
      /// </summary>
      /// <returns>An array of Byte</returns>
      public MCvERStat[] ToArray()
      {
         MCvERStat[] res = new MCvERStat[Size];
         if (res.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            CvInvoke.VectorOfERStatCopyData(_ptr, handle.AddrOfPinnedObject());
            handle.Free();
         }
         return res;
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.VectorOfERStatRelease(_ptr);
      }
   }
}

namespace Emgu.CV
{
   public abstract class ERFilter : Emgu.Util.UnmanagedObject
   {
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.CvERFilterRelease(ref _ptr);
      }

      public void Run(Image<Gray, Byte> image, VectorOfERStat regions)
      {
         CvInvoke.CvERFilterRun(_ptr, image, regions);
      }

      public static System.Drawing.Rectangle[] ERGrouping(Image<Gray, Byte>[] channels, VectorOfERStat[] erstats, String groupingTrainedFileName, float minProbability)
      {
         Debug.Assert(channels.Length == erstats.Length, "Length of channels do not match length of erstats");
         IntPtr[] channelPtrs = new IntPtr[channels.Length];
         IntPtr[] erstatPtrs = new IntPtr[erstats.Length];

         for (int i = 0; i < channelPtrs.Length; i++)
         {
            channelPtrs[i] = channels[i].Ptr;
            erstatPtrs[i] = erstats[i].Ptr;
         }
         
         GCHandle channelsHandle = GCHandle.Alloc(channelPtrs, GCHandleType.Pinned);
         GCHandle erstatsHandle = GCHandle.Alloc(erstatPtrs, GCHandleType.Pinned);
         using (VectorOfRect regions = new VectorOfRect())
         {
            CvInvoke.CvERGrouping(channelsHandle.AddrOfPinnedObject(), erstatsHandle.AddrOfPinnedObject(), channelPtrs.Length, groupingTrainedFileName, minProbability, regions);
            channelsHandle.Free();
            erstatsHandle.Free();
            return regions.ToArray();
         }

      }
   }

   public class ERFilterNM1 : ERFilter
   {
      public ERFilterNM1(
         String classifierFileName,
         int thresholdDelta,
         float minArea,
         float maxArea,
         float minProbability,
         bool nonMaxSuppression,
         float minProbabilityDiff)
      {
         _ptr = CvInvoke.CvERFilterNM1Create(classifierFileName, thresholdDelta, minArea, maxArea, minProbability, nonMaxSuppression, minProbabilityDiff);
      }
   }

   public class ERFilterNM2 : ERFilter
   {
      public ERFilterNM2(String classifierFileName, float minProbability)
      {
         _ptr = CvInvoke.CvERFilterNM2Create(classifierFileName, minProbability);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvERFilterNM1Create(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String classifier,
         int thresholdDelta,
         float minArea,
         float maxArea,
         float minProbability,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonMaxSuppression,
         float minProbabilityDiff);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvERFilterNM2Create(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String classifier,
         float minProbability);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvERFilterRelease(ref IntPtr filter);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvERFilterRun(IntPtr filter, IntPtr image, IntPtr regions);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvERGrouping(
         IntPtr channels, IntPtr regions, int count, 
         [MarshalAs(CvInvoke.StringMarshalType)]
         String groupingTrainedFileName, 
         float minProbability, IntPtr groups);


      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfERStatCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfERStatCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfERStatRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfERStatGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfERStatCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfERStatGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfERStatClear(IntPtr v);
   }
}