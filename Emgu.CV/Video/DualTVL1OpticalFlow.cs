//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
 
   /// <summary>
   /// Dual TV L1 Optical Flow Algorithm.
   /// </summary>
   public partial class DualTVL1OpticalFlow :  UnmanagedObject, IDenseOpticalFlow
   {
      private IntPtr _algorithm;
      private IntPtr _denseOpticalFlow;

      /// <summary>
      /// Create Dual TV L1 Optical Flow.
      /// </summary>
      public DualTVL1OpticalFlow()
      {
         _ptr = CvInvoke.cveDenseOpticalFlowCreateDualTVL1(ref _denseOpticalFlow, ref _algorithm);
      }

      /// <summary>
      /// Release the unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            CvInvoke.cveDualTVL1OpticalFlowRelease(ref _ptr);
            _algorithm = IntPtr.Zero;
            _denseOpticalFlow = IntPtr.Zero;
         }
      }

      /// <summary>
      /// Gets the dense optical flow pointer.
      /// </summary>
      /// <value>
      /// The pointer to the dense optical flow object.
      /// </value>
      public IntPtr DenseOpticalFlowPtr
      {
         get { return _denseOpticalFlow; }
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
      internal static extern void cveDualTVL1OpticalFlowRelease(ref IntPtr flow);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDenseOpticalFlowCreateDualTVL1(ref IntPtr denseOpticalFlow, ref IntPtr algorithm);
   }
}
