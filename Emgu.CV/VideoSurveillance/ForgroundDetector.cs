using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

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
      /// Release the forground detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvFGDetectorRelease(_ptr);
      }
   }
}
