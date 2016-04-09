//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Ximgproc
{

   public class SupperpixelSLIC : UnmanagedObject
   {
      public enum Algorithm
      {
         SLIC = 100,
         SLICO = 101
      }
   

      public SupperpixelSLIC(IInputArray image, Algorithm algorithm, int regionSize, float ruler)
      {
         using (InputArray iaImage = image.GetInputArray())
            _ptr = XimgprocInvoke.cveSuperpixelSLICCreate(iaImage, algorithm, regionSize, ruler);
      }


      public int NumberOfSuperpixels
      {
         get { return XimgprocInvoke.cveSuperpixelSLICGetNumberOfSuperpixels(_ptr); }
      }


      public void GetLabels(IOutputArray labels)
      {
         using (OutputArray oaLabels = labels.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSLICGetLabels(_ptr, oaLabels);
      }


      public void GetLabelContourMask(IOutputArray image, bool thickLine = true)
      {
         using (OutputArray oaImage = image.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSLICGetLabelContourMask(_ptr, oaImage, thickLine);
      }


      public void Iterate(int numIterations = 10)
      {

         XimgprocInvoke.cveSuperpixelSLICIterate(_ptr, numIterations);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveSuperpixelSLICRelease(ref _ptr);
         }
      }
   }


   public static partial class XimgprocInvoke
   {


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperixelSLICEnforceLabelConnectivity(IntPtr slic, int minElementSize);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSuperpixelSLICCreate(IntPtr image, SupperpixelSLIC.Algorithm algorithm, int regionSize, float ruler);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveSuperpixelSLICGetNumberOfSuperpixels(IntPtr slic);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSLICGetLabels(IntPtr slic, IntPtr labelsOut);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSLICGetLabelContourMask(
         IntPtr slic,
         IntPtr image,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool thickLine);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSLICRelease(ref IntPtr slic);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelSLICIterate(IntPtr slic, int numIterations);
   }
}
