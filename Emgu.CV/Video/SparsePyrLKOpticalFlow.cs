//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{

   public partial class SparsePyrLKOpticalFlow :  UnmanagedObject, ISparseOpticalFlow
   {
      private IntPtr _algorithm;
      private IntPtr _sparseOpticalFlow;


      public SparsePyrLKOpticalFlow(
          Size winSize,
          int maxLevel,
          MCvTermCriteria crit,
          CvEnum.LKFlowFlag flags,
          double minEigThreshold)
      {
          _ptr = CvInvoke.cveSparsePyrLKOpticalFlowCreate(
              ref winSize,
              maxLevel,
              ref crit,
              flags,
              minEigThreshold,
              ref _sparseOpticalFlow,
              ref _algorithm);
      }

      /// <summary>
      /// Release the unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            CvInvoke.cveSparsePyrLKOpticalFlowRelease(ref _ptr);
            _algorithm = IntPtr.Zero;
            _sparseOpticalFlow = IntPtr.Zero;
         }
      }

      
      public IntPtr SparseOpticalFlowPtr
      {
         get { return _sparseOpticalFlow; }
      }

      /// <summary>
      /// Return the pointer to the algorithm object
      /// </summary>
      public IntPtr AlgorithmPtr
      {
         get { return _algorithm; }
      }
   }

   public static partial class CvInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSparsePyrLKOpticalFlowCreate(
          ref Size winSize,
          int maxLevel,
          ref MCvTermCriteria crit,
          CvEnum.LKFlowFlag flags,
          double minEigThreshold,
          ref IntPtr sparseOpticalFlow,
          ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSparsePyrLKOpticalFlowRelease(ref IntPtr flow);
    }
}
