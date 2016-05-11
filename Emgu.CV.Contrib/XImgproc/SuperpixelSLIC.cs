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
   /// Class implementing the SLIC (Simple Linear Iterative Clustering) superpixels algorithm described in Radhakrishna Achanta, Appu Shaji, Kevin Smith, Aurelien Lucchi, Pascal Fua, and Sabine Susstrunk. Slic superpixels compared to state-of-the-art superpixel methods. IEEE Trans. Pattern Anal. Mach. Intell., 34(11):2274–2282, nov 2012.
   /// </summary>
   public class SupperpixelSLIC : UnmanagedObject
   {
      /// <summary>
      /// The algorithm to use
      /// </summary>
      public enum Algorithm
      {
         /// <summary>
         /// SLIC segments image using a desired region_size
         /// </summary>
         SLIC = 100,
         /// <summary>
         /// SLICO will choose an adaptive compactness factor.
         /// </summary>
         SLICO = 101
      }

      /// <summary>
      /// The function initializes a SuperpixelSLIC object for the input image. It sets the parameters of choosed superpixel algorithm, which are: region_size and ruler. It preallocate some buffers for future computing iterations over the given image. 
      /// </summary>
      /// <param name="image">Image to segment</param>
      /// <param name="algorithm">Chooses the algorithm variant to use</param>
      /// <param name="regionSize">Chooses an average superpixel size measured in pixels</param>
      /// <param name="ruler">Chooses the enforcement of superpixel smoothness factor of superpixel</param>
      public SupperpixelSLIC(IInputArray image, Algorithm algorithm, int regionSize, float ruler)
      {
         using (InputArray iaImage = image.GetInputArray())
            _ptr = XimgprocInvoke.cveSuperpixelSLICCreate(iaImage, algorithm, regionSize, ruler);
      }

      /// <summary>
      /// Calculates the actual amount of superpixels on a given segmentation computed and stored in SuperpixelSLIC object. 
      /// </summary>
      public int NumberOfSuperpixels
      {
         get { return XimgprocInvoke.cveSuperpixelSLICGetNumberOfSuperpixels(_ptr); }
      }

      /// <summary>
      /// Returns the segmentation labeling of the image. Each label represents a superpixel, and each pixel is assigned to one superpixel label.
      /// </summary>
      /// <param name="labels">A CV_32SC1 integer array containing the labels of the superpixel segmentation. The labels are in the range [0, NumberOfSuperpixels].</param>
      public void GetLabels(IOutputArray labels)
      {
         using (OutputArray oaLabels = labels.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSLICGetLabels(_ptr, oaLabels);
      }

      /// <summary>
      /// Returns the mask of the superpixel segmentation stored in SuperpixelSLIC object.
      /// </summary>
      /// <param name="image">CV_8U1 image mask where -1 indicates that the pixel is a superpixel border, and 0 otherwise.</param>
      /// <param name="thickLine">If false, the border is only one pixel wide, otherwise all pixels at the border are masked.</param>
      public void GetLabelContourMask(IOutputArray image, bool thickLine = true)
      {
         using (OutputArray oaImage = image.GetOutputArray())
            XimgprocInvoke.cveSuperpixelSLICGetLabelContourMask(_ptr, oaImage, thickLine);
      }

      /// <summary>
      /// Calculates the superpixel segmentation on a given image with the initialized parameters in the SuperpixelSLIC object.
      /// This function can be called again without the need of initializing the algorithm with createSuperpixelSLIC(). This save the computational cost of allocating memory for all the structures of the algorithm.
      /// </summary>
      /// <param name="numIterations">Number of iterations. Higher number improves the result.</param>
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
      internal static extern void cveSuperpixelSLICEnforceLabelConnectivity(IntPtr slic, int minElementSize);


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
