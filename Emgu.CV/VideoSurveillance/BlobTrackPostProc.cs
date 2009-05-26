using System;
using Emgu.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A blob tracking post process module
   /// </summary>
   public class BlobTrackPostProc : UnmanagedObject
   {
      /// <summary>
      /// Create a blob tracking post process module of the specific type
      /// </summary>
      /// <param name="type"></param>
      public BlobTrackPostProc(CvEnum.BLOB_POST_PROCESS_TYPE type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BLOB_POST_PROCESS_TYPE.Kalman:
               _ptr = CvInvoke.CvCreateModuleBlobTrackPostProcKalman();
               break;
            case Emgu.CV.CvEnum.BLOB_POST_PROCESS_TYPE.TimeAverExp:
               _ptr = CvInvoke.CvCreateModuleBlobTrackPostProcTimeAverExp();
               break;
            case Emgu.CV.CvEnum.BLOB_POST_PROCESS_TYPE.TimeAverRect:
               _ptr = CvInvoke.CvCreateModuleBlobTrackPostProcTimeAverRect();
               break;
         }
      }

      /// <summary>
      /// Release the post process module
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobTrackPostProcRelease(_ptr);
      }
   }
}
