using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A Blob Tracker
   /// </summary>
   public class BlobTracker : UnmanagedObject
   {
      /// <summary>
      /// Create a blob trakcer of the specific type
      /// </summary>
      /// <param name="type">The type of the blob tracker</param>
      public BlobTracker(CvEnum.BLOBTRACKER_TYPE type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.CC:
               _ptr = CvInvoke.CvCreateBlobDetectorCC();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.CCMSPF:
               _ptr = CvInvoke.CvCreateBlobTrackerCCMSPF();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MS:
               _ptr = CvInvoke.CvCreateBlobTrackerMS();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFG:
               _ptr = CvInvoke.CvCreateBlobTrackerMSFG();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFGS:
               _ptr = CvInvoke.CvCreateBlobTrackerMSFGS();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSPF:
               _ptr = CvInvoke.CvCreateBlobTrackerMSPF();
               break;
         }
      }

      /// <summary>
      /// Release the blob trakcer
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobTrackerRealease(_ptr);
      }
   }
}
