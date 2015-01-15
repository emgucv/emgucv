//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Superres
{
   /// <summary>
   /// Supper resolution
   /// </summary>
   public class SuperResolution : FrameSource
   {
      //private Util.Mat _frame = new Util.Mat();
      //private Image<Gray, Byte> _image;

      /// <summary>
      /// The type of optical flow algorithms used for super resolution
      /// </summary>
      public enum OpticalFlowType
      {
         /// <summary>
         /// BTVL
         /// </summary>
         Btvl = 0,
         /// <summary>
         /// BTVL using gpu
         /// </summary>
         Btvl1Gpu = 1,
      }

      /// <summary>
      /// Create a super resolution solver for the given frameSource
      /// </summary>
      /// <param name="type">The type of optical flow algorithm to use</param>
      /// <param name="frameSource">The frameSource</param>
      public SuperResolution(SuperResolution.OpticalFlowType type, FrameSource frameSource)
         : base()
      {
         _ptr = SuperresInvoke.cvSuperResolutionCreate(type, frameSource, ref _frameSourcePtr);
      }

      /// <summary>
      /// Release all the unmanaged memory associated to this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            SuperresInvoke.cvSuperResolutionRelease(ref _ptr);
         }
      }
   }

   internal static partial class SuperresInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvSuperResolutionCreate(SuperResolution.OpticalFlowType type, IntPtr frameSource, ref IntPtr frameSourceOut);

      /*
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperResolutionNextFrame(IntPtr superres, IntPtr frame);
      */
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperResolutionRelease(ref IntPtr superres);
   }
}
