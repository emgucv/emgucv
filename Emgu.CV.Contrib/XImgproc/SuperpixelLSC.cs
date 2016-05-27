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
   /// <summary>
   /// Class implementing the LSC (Linear Spectral Clustering) superpixels algorithm described in "Zhengqin Li and Jiansheng Chen. Superpixel segmentation using linear spectral clustering. June 2015."
   /// </summary>
   /// <remarks>LSC (Linear Spectral Clustering) produces compact and uniform superpixels with low computational costs. Basically, a normalized cuts formulation of the superpixel segmentation is adopted based on a similarity metric that measures the color similarity and space proximity between image pixels. LSC is of linear computational complexity and high memory efficiency and is able to preserve global properties of images</remarks>
   public class SuperpixelLSC : UnmanagedObject
   {
      /// <summary>
      /// The function initializes a SuperpixelLSC object for the input image. 
      /// </summary>
      /// <param name="image">Image to segment</param>
      /// <param name="regionSize">Chooses an average superpixel size measured in pixels</param>
      /// <param name="ratio">Chooses the enforcement of superpixel compactness factor of superpixel</param>
      public SuperpixelLSC(IInputArray image, int regionSize, float ratio)
      {
         using (InputArray iaImage = image.GetInputArray())
            _ptr = XimgprocInvoke.cveSuperpixelLSCCreate(iaImage, regionSize, ratio);
      }

      /// <summary>
      /// Calculates the actual amount of superpixels on a given segmentation computed and stored in SuperpixelLSC object
      /// </summary>
      public int NumberOfSuperpixels
      {
         get { return XimgprocInvoke.cveSuperpixelLSCGetNumberOfSuperpixels(_ptr); }
      }

      /// <summary>
      /// Returns the segmentation labeling of the image.
      /// Each label represents a superpixel, and each pixel is assigned to one superpixel label.
      /// </summary>
      /// <param name="labels">A CV_32SC1 integer array containing the labels of the superpixel segmentation. The labels are in the range [0, NumberOfSuperpixels].</param>
      public void GetLabels(IOutputArray labels)
      {
         using (OutputArray oaLabels = labels.GetOutputArray())
            XimgprocInvoke.cveSuperpixelLSCGetLabels(_ptr, oaLabels);
      }

      /// <summary>
      /// Returns the mask of the superpixel segmentation stored in SuperpixelLSC object.
      /// </summary>
      /// <param name="image">Return: CV_8U1 image mask where -1 indicates that the pixel is a superpixel border, and 0 otherwise.</param>
      /// <param name="thickLine">If false, the border is only one pixel wide, otherwise all pixels at the border are masked.</param>
      public void GetLabelContourMask(IOutputArray image, bool thickLine = true)
      {
         using (OutputArray oaImage = image.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSLICGetLabelContourMask(_ptr, oaImage, thickLine);
      }

      /// <summary>
      /// Calculates the superpixel segmentation on a given image with the initialized parameters in the SuperpixelLSC object.
      /// This function can be called again without the need of initializing the algorithm with createSuperpixelLSC(). This save the computational cost of allocating memory for all the structures of the algorithm.
      /// </summary>
      /// <param name="numIterations">Number of iterations. Higher number improves the result.</param>
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

