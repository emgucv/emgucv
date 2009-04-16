using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A forground detector
   /// </summary>
   public class ForgroundDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a forground detector of the specific type
      /// </summary>
      /// <param name="type">The type of the detectir to be created</param>
      public ForgroundDetector(CvEnum.FORGROUND_DETECTOR_TYPE type)
      {
         _ptr = CvInvoke.CvCreateFGDetectorBase(type, IntPtr.Zero);
      }

      /// <summary>
      /// Get the forground mask from the detector
      /// </summary>
      /// <returns></returns>
      public Image<Gray, Byte> GetForgroundMask()
      {
         IntPtr forground = CvInvoke.CvFGDetectorGetMask(_ptr);
         if (forground == IntPtr.Zero) return null;
         MIplImage iplImage = (MIplImage) Marshal.PtrToStructure(forground, typeof(MIplImage));
         return new Image<Gray, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
      }

      /// <summary>
      /// Release the forground detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvFGDetectorRelease(_ptr);
      }
   }
}
