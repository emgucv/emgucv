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
   /// The ERStat structure represents a class-specific Extremal Region (ER).
   /// An ER is a 4-connected set of pixels with all its grey-level values smaller than the values in its outer boundary. 
   /// A class-specific ER is selected (using a classifier) from all the ER’s in the component tree of the image.
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvERStat
   {
      /// <summary>
      /// Seed point
      /// </summary>
      public int Pixel;

      /// <summary>
      /// Threshold (max grey-level value)
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
      /// Bounding box
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
      /// Probability that the ER belongs to the class we are looking for
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
   /// <summary>
   /// Base class for 1st and 2nd stages of Neumann and Matas scene text detection algorithm
   /// </summary>
   public abstract class ERFilter : Emgu.Util.UnmanagedObject
   {
      /// <summary>
      /// Release all the unmanaged memory associate with this ERFilter
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.CvERFilterRelease(ref _ptr);
      }

      /// <summary>
      /// Takes image on input and returns the selected regions in a vector of ERStat only distinctive ERs which correspond to characters are selected by a sequential classifier
      /// </summary>
      /// <param name="image">Sinle channel image CV_8UC1</param>
      /// <param name="regions">Output for the 1st stage and Input/Output for the 2nd. The selected Extremal Regions are stored here.</param>
      public void Run(Image<Gray, Byte> image, VectorOfERStat regions)
      {
         CvInvoke.CvERFilterRun(_ptr, image, regions);
      }

      /// <summary>
      /// Find groups of Extremal Regions that are organized as text blocks.
      /// </summary>
      /// <param name="channels">Array of sinle channel images from wich the regions were extracted</param>
      /// <param name="erstats">Vector of ER’s retreived from the ERFilter algorithm from each channel</param>
      /// <param name="groupingTrainedFileName">The XML or YAML file with the classifier model (e.g. trained_classifier_erGrouping.xml)</param>
      /// <param name="minProbability">The minimum probability for accepting a group. Use 0.5 for default.</param>
      /// <returns>The output of the algorithm that indicates the text regions</returns>
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

   /// <summary>
   /// Extremal Region Filter for the 1st stage classifier of N&amp;M algorithm
   /// </summary>
   public class ERFilterNM1 : ERFilter
   {
      /// <summary>
      /// Create an Extremal Region Filter for the 1st stage classifier of N&amp;M algorithm
      /// </summary>
      /// <param name="classifierFileName">The file name of the classifier</param>
      /// <param name="thresholdDelta">Threshold step in subsequent thresholds when extracting the component tree. Use 1 for default.</param>
      /// <param name="minArea">The minimum area (% of image size) allowed for retreived ER’s. Use 0.00025 for default.</param>
      /// <param name="maxArea">The maximum area (% of image size) allowed for retreived ER’s. Use 0.13 for default.</param>
      /// <param name="minProbability">The minimum probability P(er|character) allowed for retreived ER’s. Use 0.4 for default</param>
      /// <param name="nonMaxSuppression">Whenever non-maximum suppression is done over the branch probabilities. User true for default.</param>
      /// <param name="minProbabilityDiff">The minimum probability difference between local maxima and local minima ERs. Use 0.1 for default.</param>
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

   /// <summary>
   /// Extremal Region Filter for the 2nd stage classifier of N&amp;M algorithm
   /// </summary>
   public class ERFilterNM2 : ERFilter
   {
      /// <summary>
      /// Create an Extremal Region Filter for the 2nd stage classifier of N&amp;M algorithm
      /// </summary>
      /// <param name="classifierFileName">The file name of the classifier</param>
      /// <param name="minProbability">The minimum probability P(er|character) allowed for retreived ER’s. Use 0.3 for default.</param>
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