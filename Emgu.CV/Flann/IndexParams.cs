//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Flann
{
   /// <summary>
   /// When passing an object of this type, the index will perform a linear, brute-force search.
   /// </summary>
   public class LinearIndexParams : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      public LinearIndexParams()
      {
         _ptr = CvInvoke.cveLinearIndexParamsCreate(ref _indexParamPtr);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveLinearIndexParamsRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// When passing an object of this type the index constructed will consist of a set of randomized kd-trees which will be searched in parallel.
   /// </summary>
   public class KdTreeIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      /// <summary>
      /// Initializes a new instance of the <see cref="KdTreeIndexParamses"/> class.
      /// </summary>
      /// <param name="trees">The number of parallel kd-trees to use. Good values are in the range [1..16]</param>
      public KdTreeIndexParamses(int trees)
      {
         _ptr = CvInvoke.cveKDTreeIndexParamsCreate(ref _indexParamPtr, trees);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveKDTreeIndexParamsRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// When using a parameters object of this type the index created uses multi-probe LSH (by Multi-Probe LSH: Efficient Indexing for High-Dimensional Similarity Search by Qin Lv, William Josephson, Zhe Wang, Moses Charikar, Kai Li., Proceedings of the 33rd International Conference on Very Large Data Bases (VLDB). Vienna, Austria. September 2007)
   /// </summary>
   public class LshIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      /// <summary>
      /// Initializes a new instance of the <see cref="LshIndexParamses"/> class.
      /// </summary>
      /// <param name="tableNumber">The number of hash tables to use (between 10 and 30 usually).</param>
      /// <param name="keySize">The size of the hash key in bits (between 10 and 20 usually).</param>
      /// <param name="multiProbeLevel">The number of bits to shift to check for neighboring buckets (0 is regular LSH, 2 is recommended).</param>
      public LshIndexParamses(int tableNumber, int keySize, int multiProbeLevel)
      {
         _ptr = CvInvoke.cveLshIndexParamsCreate(ref _indexParamPtr, tableNumber, keySize, multiProbeLevel);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveLshIndexParamsRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// When passing an object of this type the index constructed will be a hierarchical k-means tree.
   /// </summary>
   public class KMeansIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      /// <summary>
      /// Initializes a new instance of the <see cref="KMeansIndexParamses"/> class.
      /// </summary>
      /// <param name="branching">The branching factor to use for the hierarchical k-means tree</param>
      /// <param name="iterations"> The maximum number of iterations to use in the k-means clustering stage when building the k-means tree. A value of -1 used here means that the k-means clustering should be iterated until convergence</param>
      /// <param name="centersInit">The algorithm to use for selecting the initial centers when performing a k-means clustering step. The possible values are CENTERS_RANDOM (picks the initial cluster centers randomly), CENTERS_GONZALES (picks the initial centers using Gonzales’ algorithm) and CENTERS_KMEANSPP (picks the initial centers using the algorithm suggested in arthur_kmeanspp_2007 )</param>
      /// <param name="cbIndex">This parameter (cluster boundary index) influences the way exploration is performed in the hierarchical kmeans tree. When cb_index is zero the next kmeans domain to be explored is chosen to be the one with the closest center. A value greater then zero also takes into account the size of the domain.</param>
      public KMeansIndexParamses(int branching = 32, int iterations = 11, Flann.CenterInitType centersInit = CenterInitType.Random, float cbIndex = 0.2f)
      {
         _ptr = CvInvoke.cveKMeansIndexParamsCreate(ref _indexParamPtr, branching, iterations, centersInit, cbIndex);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveKMeansIndexParamsRelease(ref _ptr);
         }
      }
   }

   /// <summary>
   /// When using a parameters object of this type the index created combines the randomized kd-trees and the hierarchical k-means tree.
   /// </summary>
   public class CompositeIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      /// <summary>
      /// Initializes a new instance of the <see cref="CompositeIndexParamses"/> class.
      /// </summary>
      /// <param name="trees">The number of parallel kd-trees to use. Good values are in the range [1..16]</param>
      /// <param name="branching">The branching factor to use for the hierarchical k-means tree</param>
      /// <param name="iterations"> The maximum number of iterations to use in the k-means clustering stage when building the k-means tree. A value of -1 used here means that the k-means clustering should be iterated until convergence</param>
      /// <param name="centersInit">The algorithm to use for selecting the initial centers when performing a k-means clustering step. The possible values are CENTERS_RANDOM (picks the initial cluster centers randomly), CENTERS_GONZALES (picks the initial centers using Gonzales’ algorithm) and CENTERS_KMEANSPP (picks the initial centers using the algorithm suggested in arthur_kmeanspp_2007 )</param>
      /// <param name="cbIndex">This parameter (cluster boundary index) influences the way exploration is performed in the hierarchical kmeans tree. When cb_index is zero the next kmeans domain to be explored is chosen to be the one with the closest center. A value greater then zero also takes into account the size of the domain.</param>
      public CompositeIndexParamses(int trees = 4, int branching = 32, int iterations = 11, Flann.CenterInitType centersInit = CenterInitType.Random, float cbIndex = 0.2f)
      {
         _ptr = CvInvoke.cveCompositeIndexParamsCreate(ref _indexParamPtr, trees, branching, iterations, centersInit, cbIndex);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveCompositeIndexParamsRelease(ref _ptr);
         }
      }
   }

   public class AutotunedIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      public AutotunedIndexParamses(float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction)
      {
         _ptr = CvInvoke.cveAutotunedIndexParamsCreate(ref _indexParamPtr, targetPrecision, buildWeight, memoryWeight, sampleFraction);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveAutotunedIndexParamsRelease(ref _ptr);
         }
      }
   }

   public class HierarchicalClusteringIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      public HierarchicalClusteringIndexParamses(int branching, Flann.CenterInitType centersInit, int trees, int leafSize)
      {
         _ptr = CvInvoke.cveHierarchicalClusteringIndexParamsCreate(ref _indexParamPtr, branching, centersInit, trees, leafSize);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveHierarchicalClusteringIndexParamsRelease(ref _ptr);
         }
      }
   }

   public class SearchParams : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      public SearchParams(int checks, float eps, bool sorted)
      {
         _ptr = CvInvoke.cveSearchParamsCreate(ref _indexParamPtr, checks, eps, sorted);
      }

      IntPtr IIndexParams.IndexParamPtr
      {
         get { return _indexParamPtr; }
      }

      /// <summary>
      /// Release all the memory associated with this IndexParam
      /// </summary>
      protected override void DisposeObject()
      {
         if (IntPtr.Zero != _ptr)
         {
            CvInvoke.cveSearchParamsRelease(ref _ptr);
         }
      }
   }
}

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveLinearIndexParamsCreate(ref IntPtr ip);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveLinearIndexParamsRelease(ref IntPtr p);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveKDTreeIndexParamsCreate(ref IntPtr ip, int trees);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveKDTreeIndexParamsRelease(ref IntPtr p);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveLshIndexParamsCreate(ref IntPtr ip, int tableNumber, int keySize, int multiProbeLevel);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveLshIndexParamsRelease(ref IntPtr p);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveKMeansIndexParamsCreate(ref IntPtr ip, int branching, int iterations, Flann.CenterInitType centersInit, float cbIndex);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveKMeansIndexParamsRelease(ref IntPtr p);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveCompositeIndexParamsCreate(ref IntPtr ip, int trees, int branching, int iterations, Flann.CenterInitType centersInit, float cbIndex);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCompositeIndexParamsRelease(ref IntPtr p);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveAutotunedIndexParamsCreate(ref IntPtr ip, float targetPrecision, float buildWeight, float memoryWeight, float sampleFraction);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveAutotunedIndexParamsRelease(ref IntPtr p);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveHierarchicalClusteringIndexParamsCreate(ref IntPtr ip, int branching, Flann.CenterInitType centersInit, int trees, int leafSize);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveHierarchicalClusteringIndexParamsRelease(ref IntPtr p);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveSearchParamsCreate(
         ref IntPtr ip, int checks, float eps, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool sorted);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveSearchParamsRelease( ref IntPtr p);
   }
}
