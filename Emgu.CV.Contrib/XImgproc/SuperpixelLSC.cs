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

   public class SupperpixelLSC : UnmanagedObject
   {


      public SupperpixelLSC(IInputArray image, int regionSize, float ratio)
      {
         using (InputArray iaImage = image.GetInputArray())
            _ptr = XimgprocInvoke.cveSuperpixelLSCCreate(iaImage, regionSize, ratio);
      }


      public int NumberOfSuperpixels
      {
         get { return XimgprocInvoke.cveSuperpixelLSCGetNumberOfSuperpixels(_ptr); }
      }


      public void GetLabels(IOutputArray labels)
      {
         using (OutputArray oaLabels = labels.GetOutputArray())
            XimgprocInvoke.cveSuperpixelLSCGetLabels(_ptr, oaLabels);
      }


      public void GetLabelContourMask(IOutputArray image, bool thickLine = true)
      {
         using (OutputArray oaImage = image.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSLICGetLabelContourMask(_ptr, oaImage, thickLine);
      }


      public void Iterate(int numIterations = 10)
      {

         XimgprocInvoke.cveSuperpixelLSCIterate(_ptr, numIterations);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this object.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveSuperpixelLSCRelease(ref _ptr);
         }
      }
   }



   public static partial class XimgprocInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveSuperpixelLSCCreate(IntPtr image, int regionSize, float ratio);



      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveSuperpixelLSCGetNumberOfSuperpixels(IntPtr lsc);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelLSCGetLabels(IntPtr lsc, IntPtr labelsOut);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelLSCGetLabelContourMask(
         IntPtr slic,
         IntPtr image,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool thickLine);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelLSCRelease(ref IntPtr lsc);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveSuperpixelLSCIterate(IntPtr lsc, int numIterations);
   }
}

