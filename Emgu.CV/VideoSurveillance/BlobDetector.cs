using System;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A blob detector
   /// </summary>
   public class BlobDetector :UnmanagedObject
   {
      /// <summary>
      /// Create a blob detector of specific type
      /// </summary>
      /// <param name="type">The type of the detector</param>
      public BlobDetector(CvEnum.BLOB_DETECTOR_TYPE type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BLOB_DETECTOR_TYPE.Simple:
               _ptr = CvInvoke.CvCreateBlobDetectorSimple();
               break;
            case Emgu.CV.CvEnum.BLOB_DETECTOR_TYPE.CC:
               _ptr = CvInvoke.CvCreateBlobDetectorCC();
               break;
         }
      }

      /// <summary>
      /// Detect new blobs
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="imageForground">The forground mask</param>
      /// <param name="newBlob">The new blob list</param>
      /// <param name="oldBlob">The old blob list</param>
      public int DetectNewBlob(IImage image, Image<Gray, Byte> imageForground, BlobSeq newBlob, BlobSeq oldBlob)
      {
         return CvInvoke.CvBlobDetectorDetectNewBlob(_ptr, image.Ptr, imageForground.Ptr, newBlob.Ptr, oldBlob.Ptr);
      }

      /// <summary>
      /// Release the detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobDetectorRelease(_ptr);
      }
   }
}
