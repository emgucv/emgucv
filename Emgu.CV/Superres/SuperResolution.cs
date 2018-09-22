//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || NETFX_CORE || NETSTANDARD1_4 || UNITY_IOS )

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
       private IntPtr _sharedPtr;
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
         _ptr = SuperresInvoke.cveSuperResolutionCreate(type, frameSource, ref _frameSourcePtr, ref _sharedPtr);
      }

      /// <summary>
      /// Release all the unmanaged memory associated to this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            SuperresInvoke.cveSuperResolutionRelease(ref _ptr, ref _sharedPtr);
         }
      }
   }

   internal static partial class SuperresInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSuperResolutionCreate(SuperResolution.OpticalFlowType type, IntPtr frameSource, ref IntPtr frameSourceOut, ref IntPtr sharedPtr);

      /*
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperResolutionNextFrame(IntPtr superres, IntPtr frame);
      */
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperResolutionRelease(ref IntPtr superres, ref IntPtr sharedPtr);
   }
}
#endif