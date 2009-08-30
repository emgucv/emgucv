using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// A Planar object detector
   /// </summary>
   public class PlanarObjectDetector : UnmanagedObject
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static IntPtr CvPlanarObjectDetectorDefaultCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPlanarObjectDetectorRelease(IntPtr detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPlanarObjectDetectorTrain(
         IntPtr objectDetector,
         IntPtr image,
         int npoints,
         int patchSize,
         int nstructs,
         int structSize,
         int nviews,
         ref LDetector keyPointDetector,
         IntPtr patchGenerator);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPlanarObjectDetectorDetect(IntPtr detector, IntPtr image, IntPtr homography, IntPtr corners);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvPlanarObjectDetectorGetModelPoints(IntPtr detector, IntPtr modelPoints);

      /// <summary>
      /// Create a planar Object detector
      /// </summary>
      public PlanarObjectDetector()
      {
         _ptr = CvPlanarObjectDetectorDefaultCreate();
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvPlanarObjectDetectorRelease(_ptr);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <param name="npoints">Use 300 for default</param>
      /// <param name="patchSize">Use 31 for default</param>
      /// <param name="nstructs">Use 50 for default</param>
      /// <param name="structSize">Use 9 for default</param>
      /// <param name="nviews">Use 5000 for default</param>
      /// <param name="keyPointDetector"></param>
      /// <param name="patchGenerator"></param>
      public void Train(Image<Gray, byte> image, 
         int npoints,
         int patchSize,
         int nstructs,
         int structSize,
         int nviews,
         LDetector keyPointDetector,
         PatchGenerator patchGenerator)
      {
         CvPlanarObjectDetectorTrain(Ptr, image, npoints, patchSize, nstructs, structSize, nviews, ref keyPointDetector, patchGenerator);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <param name="h"></param>
      /// <returns></returns>
      public PointF[] Detect(Image<Gray, Byte> image, HomographyMatrix h)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<PointF> corners = new Seq<PointF>(stor);
            CvPlanarObjectDetectorDetect(_ptr, image, h, corners);
            return corners.ToArray();
         }
      }

      /// <summary>
      /// Get the model points stored in this detector
      /// </summary>
      /// <returns>The model points stored in this detector</returns>
      public MKeyPoint[] GetModelPoints()
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<MKeyPoint> modelPoints = new Seq<MKeyPoint>(stor);
            CvPlanarObjectDetectorGetModelPoints(_ptr, modelPoints);
            return modelPoints.ToArray();
         }
      }
   }
}
