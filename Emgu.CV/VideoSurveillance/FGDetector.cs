using System;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A foreground detector
   /// </summary>
   public class FGDetector<TColor> : UnmanagedObject, IBGFGDetector<TColor>
      where TColor : struct, IColor
   {
      /// <summary>
      /// Create a foreground detector of the specific type
      /// </summary>
      /// <param name="type">The type of the detector to be created</param>
      public FGDetector(CvEnum.FORGROUND_DETECTOR_TYPE type)
      {
         _ptr = CvInvoke.CvCreateFGDetectorBase(type, IntPtr.Zero);
      }

      /// <summary>
      /// Update the foreground detector using the specific image
      /// </summary>
      /// <param name="image">The image which will be used to update the FGDetector</param>
      public void Update(Image<TColor, Byte> image)
      {
         CvInvoke.CvFGDetectorProcess(_ptr, image.Ptr);
      }

      /// <summary>
      /// Get the foreground mask from the detector
      /// </summary>
      public Image<Gray, Byte> ForgroundMask
      {
         get
         {
            IntPtr foreground = CvInvoke.CvFGDetectorGetMask(_ptr);
            if (foreground == IntPtr.Zero) return null;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(foreground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
         }
      }

      /// <summary>
      /// Get the background mask
      /// </summary>
      public Image<Gray, Byte> BackgroundMask
      {
         get
         {
            return ForgroundMask.Not();
         }
      }

      /// <summary>
      /// Release the foreground detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvFGDetectorRelease(_ptr);
      }
   }
}
