//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   /// Dense Optical flow
   /// </summary>
   public abstract class DenseOpticalFlow : UnmanagedObject, IAlgorithm
   {
      static DenseOpticalFlow()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Calculates an optical flow.
      /// </summary>
      /// <param name="i0">First 8-bit single-channel input image.</param>
      /// <param name="i1">Second input image of the same size and the same type as prev.</param>
      /// <param name="flow">Computed flow image that has the same size as prev and type CV_32FC2 </param>
      public void Calc(IInputArray i0, IInputArray i1, IInputOutputArray flow)
      {
         using (InputArray iaI0 = i0.GetInputArray())
         using (InputArray iaI1 = i1.GetInputArray())
         using (InputOutputArray ioaFlow = flow.GetInputOutputArray())
            cveDenseOpticalFlowCalc(_ptr, iaI0, iaI1, ioaFlow);
      }

      /// <summary>
      /// Release the unmanaged object related to this dense optical flow
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            cveDenseOpticalFlowRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDenseOpticalFlowRelease(ref IntPtr flow);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDenseOpticalFlowCalc(IntPtr dof, IntPtr i0, IntPtr i1, IntPtr flow);


      IntPtr IAlgorithm.AlgorithmPtr
      {
         get { return _ptr; }
      }
   }

   /// <summary>
   /// Dual TV L1 Optical Flow Algorithm.
   /// </summary>
   public class OpticalFlowDualTVL1 : DenseOpticalFlow
   {
      /// <summary>
      /// Create Dual TV L1 Optical Flow.
      /// </summary>
      public OpticalFlowDualTVL1()
      {
         _ptr = cveDenseOpticalFlowCreateDualTVL1();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr cveDenseOpticalFlowCreateDualTVL1();
   }
}
