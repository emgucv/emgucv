//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
         BTVL = 0,
         /// <summary>
         /// BTVL using gpu
         /// </summary>
         BTVL1_GPU = 1,
         /// <summary>
         /// BTVL using opencl
         /// </summary>
         BTVL1_OCL = 2
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

      /*
      /// <summary>
      /// Get the next frame
      /// </summary>
      /// <returns>The next frame, this image should not be release by the user. I will be reused over frames. If there are no more frames available, null will be returned</returns>
      public Image<Gray, Byte> NextFrame()
      {
         SuperresInvoke.cvSuperResolutionNextFrame(_ptr, _frame);

         if (_frame.IsEmpty)
            return null;

         if (_image == null || _image.Ptr == IntPtr.Zero)
            _image = new Image<Gray, byte>(_frame.Size);

         _frame.CopyTo(_image);

         return _image;
      }*/

      /// <summary>
      /// Release all the unmanaged memory associated to this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            SuperresInvoke.cvSuperResolutionRelease(ref _ptr);
         }
         ReleaseCache();
      }
   }

   internal static partial class SuperresInvoke
   {

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvSuperResolutionCreate(SuperResolution.OpticalFlowType type, IntPtr frameSource, ref IntPtr frameSourceOut);

      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperResolutionNextFrame(IntPtr superres, IntPtr frame);
      */
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cvSuperResolutionRelease(ref IntPtr superres);
   }
}
