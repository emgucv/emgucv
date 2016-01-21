//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XImgproc
{
   public static partial class XImgprocInvoke
   {
      static XImgprocInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      public static void JointBilateralFilter(
         IInputArray joint, IInputArray src, IOutputArray dst, int d,
         double sigmaColor, double sigmaSpace, CvEnum.BorderType borderType = BorderType.Reflect101)
      {
         using (InputArray iaJoint = joint.GetInputArray())
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveJointBilateralFilter(iaJoint, iaSrc,
               oaDst, d, sigmaColor, sigmaSpace, borderType);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveJointBilateralFilter(IntPtr joint, IntPtr src, IntPtr dst, int d, double sigmaColor, double sigmaSpace, CvEnum.BorderType borderType);

      public static void FastGlobalSmootherFilter(IInputArray guide, IInputArray src, IOutputArray dst, double lambda,
         double sigmaColor, double lambdaAttenuation, int numIter)
      {
         using (InputArray iaGuide = guide.GetInputArray())
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         {
            cveFastGlobalSmootherFilter(iaGuide, iaSrc, oaDst, lambda, sigmaColor, lambdaAttenuation, numIter);
         }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFastGlobalSmootherFilter(IntPtr guide, IntPtr src, IntPtr dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter);

      public static void AmFilter(IInputArray joint, IInputArray src, IOutputArray dst, double sigmaS, double sigmaR,
         bool adjustOutliers)
      {
         using (InputArray iaJoint = joint.GetInputArray())
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveAmFilter(iaJoint, iaSrc, oaDst, sigmaS, sigmaR, adjustOutliers);
      }


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveAmFilter(
         IntPtr joint,
         IntPtr src,
         IntPtr dst,
         double sigmaS,
         double sigmaR,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool adjustOutliers);

      public static void GuidedFilter(IInputArray guide, IInputArray src, IOutputArray dst, int radius, double eps,
         int dDepth)
      {
         using (InputArray iaGuide = guide.GetInputArray())
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveGuidedFilter(iaGuide, iaSrc, oaDst, radius, eps, dDepth);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveGuidedFilter(IntPtr guide, IntPtr src, IntPtr dst, int radius, double eps, int dDepth);

      public static void DtFilter(IInputArray guide, IInputArray src, IOutputArray dst,
         double sigmaSpatial, double sigmaColor, int mode, int numIters)
      {
         using (InputArray iaGuide = guide.GetInputArray())
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveDtFilter(iaGuide, iaSrc, oaDst, sigmaSpatial, sigmaColor, mode, numIters);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDtFilter(IntPtr guide, IntPtr src, IntPtr dst, double sigmaSpatial, double sigmaColor, int mode, int numIters);

   }
}
