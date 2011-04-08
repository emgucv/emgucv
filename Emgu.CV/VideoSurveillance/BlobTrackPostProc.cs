//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A blob tracking post process module
   /// </summary>
   public class BlobTrackPostProc : UnmanagedObject
   {
      #region PInvoke
      /// <summary>
      /// Returns a Kalman blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcKalman();

      /// <summary>
      /// Returns a TimeAverRect blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverRect();

      /// <summary>
      /// Returns a TimeAverExp blob tracking post process module
      /// </summary>
      /// <returns>Pointer to the tracking module</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvCreateModuleBlobTrackPostProcTimeAverExp();

      /// <summary>
      /// Release the blob tracking post process module
      /// </summary>
      /// <param name="postProc">The post process module to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobTrackPostProcRelease(ref IntPtr postProc);
      #endregion

      /// <summary>
      /// Create a blob tracking post process module of the specific type
      /// </summary>
      /// <param name="type"></param>
      public BlobTrackPostProc(CvEnum.BLOB_POST_PROCESS_TYPE type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BLOB_POST_PROCESS_TYPE.Kalman:
               _ptr = CvCreateModuleBlobTrackPostProcKalman();
               break;
            case Emgu.CV.CvEnum.BLOB_POST_PROCESS_TYPE.TimeAverExp:
               _ptr = CvCreateModuleBlobTrackPostProcTimeAverExp();
               break;
            case Emgu.CV.CvEnum.BLOB_POST_PROCESS_TYPE.TimeAverRect:
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
   }
}
