/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A blob tracking post process module
   /// </summary>
   public class BlobTrackPostProc : UnmanagedObject
   {
      static BlobTrackPostProc()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a blob tracking post process module of the specific type
      /// </summary>
      /// <param name="type"></param>
      public BlobTrackPostProc(CvEnum.BlobPostProcessType type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BlobPostProcessType.Kalman:
               _ptr = CvCreateModuleBlobTrackPostProcKalman();
               break;
            case Emgu.CV.CvEnum.BlobPostProcessType.TimeAverExp:
               _ptr = CvCreateModuleBlobTrackPostProcTimeAverExp();
               break;
            case Emgu.CV.CvEnum.BlobPostProcessType.TimeAverRect:
               _ptr = CvCreateModuleBlobTrackPostProcTimeAverRect();
               break;
         }
      }

      /// <summary>
      /// Release the post process module
      /// </summary>
      protected override void DisposeObject()
      {
         CvBlobTrackPostProcRelease(ref _ptr);
      }

      #region PInvoke
      /// <summary>
      /// Returns a Kalman blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateModuleBlobTrackPostProcKalman();

      /// <summary>
      /// Returns a TimeAverRect blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverRect();

      /// <summary>
      /// Returns a TimeAverExp blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverExp();

      /// <summary>
      /// Release the blob tracking post process module
      /// </summary>
      /// <param name="postProc">The post process module to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBlobTrackPostProcRelease(ref IntPtr postProc);
      #endregion
   }
}
*/