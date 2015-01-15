/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// A Planar object detector
   /// </summary>
   public class PlanarObjectDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a planar Object detector
      /// </summary>
      public PlanarObjectDetector()
      {
         _ptr = CvInvoke.CvPlanarObjectDetectorDefaultCreate();
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvPlanarObjectDetectorRelease(_ptr);
      }

      /// <summary>
      /// Train the planar object detector using the specific image
      /// </summary>
      /// <param name="image">The training image</param>
      /// <param name="npoints">Use 300 for default</param>
      /// <param name="patchSize">Use 31 for default</param>
      /// <param name="nstructs">Use 50 for default</param>
      /// <param name="structSize">Use 9 for default</param>
      /// <param name="nviews">Use 5000 for default</param>
      /// <param name="keyPointDetector">The keypoint detector to be used</param>
      /// <param name="patchGenerator">The patch generator to be used</param>
      public void Train(Image<Gray, byte> image,
         int npoints,
         int patchSize,
         int nstructs,
         int structSize,
         int nviews,
         ref LDetector keyPointDetector,
         ref PatchGenerator patchGenerator)
      {
         CvInvoke.CvPlanarObjectDetectorTrain(Ptr, image, npoints, patchSize, nstructs, structSize, nviews, ref keyPointDetector, ref patchGenerator);
      }

      /// <summary>
      /// Detect planar object from the specific image
      /// </summary>
      /// <param name="image">The image where the planar object will be detected</param>
      /// <param name="h">The homography matrix which will be updated</param>
      /// <returns>The four corners of the detected region</returns>
      public PointF[] Detect(Image<Gray, Byte> image, HomographyMatrix h)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<PointF> corners = new Seq<PointF>(stor);
            CvInvoke.CvPlanarObjectDetectorDetect(_ptr, image, h, corners);
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
            CvInvoke.CvPlanarObjectDetectorGetModelPoints(_ptr, modelPoints);
            return modelPoints.ToArray();
         }
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvPlanarObjectDetectorDefaultCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvPlanarObjectDetectorRelease(IntPtr detector);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvPlanarObjectDetectorTrain(
         IntPtr objectDetector,
         IntPtr image,
         int npoints,
         int patchSize,
         int nstructs,
         int structSize,
         int nviews,
         ref LDetector keyPointDetector,
         ref PatchGenerator patchGenerator);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvPlanarObjectDetectorDetect(IntPtr detector, IntPtr image, IntPtr homography, IntPtr corners);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvPlanarObjectDetectorGetModelPoints(IntPtr detector, IntPtr modelPoints);
   }
}*/