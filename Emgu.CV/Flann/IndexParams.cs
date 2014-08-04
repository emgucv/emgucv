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
   public class LinearIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      public LinearIndexParamses()
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

   public class KdTreeIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

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

   public class LshIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

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

   public class KMeansIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      public KMeansIndexParamses(int branching, int iterations, Flann.CenterInitType centersInit, float cbIndex)
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

   public class CompositeIndexParamses : UnmanagedObject, IIndexParams
   {
      private IntPtr _indexParamPtr;

      public CompositeIndexParamses(int trees, int branching, int iterations, Flann.CenterInitType centersInit, float cbIndex)
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
