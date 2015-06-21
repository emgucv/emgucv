//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   public interface IDenseOpticalFlow : IAlgorithm
   {
      /// <summary>
      /// Gets the dense optical flow pointer.
      /// </summary>
      /// <value>
      /// The dense optical flow .
      /// </value>
      IntPtr DenseOpticalFlowPtr { get; }     
   }

   /// <summary>
   /// Extension methods for IDenseOpticalFlow
   /// </summary>
   public static class DenseOpticalFlowExtensions
   {
      /// <summary>
      /// Calculates an optical flow.
      /// </summary>
      /// <param name="i0">First 8-bit single-channel input image.</param>
      /// <param name="i1">Second input image of the same size and the same type as prev.</param>
      /// <param name="flow">Computed flow image that has the same size as prev and type CV_32FC2 </param>
      /// <param name="opticalFlow">The dense optical flow object</param>
      public static void Calc(this IDenseOpticalFlow opticalFlow, IInputArray i0, IInputArray i1, IInputOutputArray flow)
      {
         using (InputArray iaI0 = i0.GetInputArray())
         using (InputArray iaI1 = i1.GetInputArray())
         using (InputOutputArray ioaFlow = flow.GetInputOutputArray())
            CvInvoke.cveDenseOpticalFlowCalc(opticalFlow.DenseOpticalFlowPtr, iaI0, iaI1, ioaFlow);
      }

   }

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
      internal static extern void cveDenseOpticalFlowCalc(IntPtr dof, IntPtr i0, IntPtr i1, IntPtr flow);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDenseOpticalFlowCreateDualTVL1(ref IntPtr denseOpticalFlow, ref IntPtr algorithm);
   }
}
