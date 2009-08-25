using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV
{
   public class PlanarObjectDetector : UnmanagedObject
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static IntPtr CvPlanarObjectDetectorDefaultCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPlanarObjectDetectorRelease(IntPtr detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPlanarObjectDetectorTrain(IntPtr detector, IntPtr image);

      public PlanarObjectDetector()
      {
         _ptr = CvPlanarObjectDetectorDefaultCreate();
      }

      protected override void DisposeObject()
      {
         CvPlanarObjectDetectorRelease(_ptr);
      }

      public void Train(Image<Gray, byte> image)
      {
         CvPlanarObjectDetectorTrain(Ptr, image);
      }
   }
}
