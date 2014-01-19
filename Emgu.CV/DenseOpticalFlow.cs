//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public abstract class DenseOpticalFlow : UnmanagedObject
   {
      static DenseOpticalFlow()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      public void Calc(IInputArray i0, IInputArray i1, IInputOutputArray flow)
      {
         cveDenseOpticalFlowCalc(_ptr, i0.InputArrayPtr, i1.InputArrayPtr, flow.InputOutputArrayPtr);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            cveDenseOpticalFlowRelease(ref _ptr);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDenseOpticalFlowRelease(ref IntPtr flow);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDenseOpticalFlowCalc(IntPtr dof, IntPtr i0, IntPtr i1, IntPtr flow);
   }

   public class OpticalFlowDualTVL1 : DenseOpticalFlow
   {
      public OpticalFlowDualTVL1()
      {
         _ptr = cveDenseOpticalFlowCreateDualTVL1();
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr cveDenseOpticalFlowCreateDualTVL1();
   }
}
