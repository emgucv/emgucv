//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      /// <summary>
      /// The grab cut algorithm for segmentation
      /// </summary>
      /// <param name="img">The 8-bit 3-channel image to be segmented</param>
      /// <param name="mask">Input/output 8-bit single-channel mask. The mask is initialized by the function
      /// when mode is set to GC_INIT_WITH_RECT. Its elements may have one of following values:
      /// 0 (GC_BGD) defines an obvious background pixels.
      /// 1 (GC_FGD) defines an obvious foreground (object) pixel.
      /// 2 (GC_PR_BGR) defines a possible background pixel.
      /// 3 (GC_PR_FGD) defines a possible foreground pixel.
      ///</param>
      /// <param name="rect">The rectangle to initialize the segmentation</param>
      /// <param name="bgdModel">
      /// Temporary array for the background model. Do not modify it while you are
      /// processing the same image.
      /// </param>
      /// <param name="fgdModel">
      /// Temporary arrays for the foreground model. Do not modify it while you are
      /// processing the same image.
      /// </param>
      /// <param name="iterCount">The number of iternations</param>
      /// <param name="type">The initilization type</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvGrabCut(
         IntPtr img,
         IntPtr mask,
         ref Rectangle rect,
         IntPtr bgdModel,
         IntPtr fgdModel,
         int iterCount,
         CvEnum.GRABCUT_INIT_TYPE type);

      /// <summary>
      /// Calculate square root of each source array element. in the case of multichannel
      /// arrays each channel is processed independently. The function accuracy is approximately
      /// the same as of the built-in std::sqrt.
      /// </summary>
      /// <param name="src">The source floating-point array</param>
      /// <param name="dst">The destination array; will have the same size and the same type as src</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="cvArrSqrt")]
      public extern static void cvSqrt(IntPtr src, IntPtr dst);

      /// <summary>
      /// Apply color map to the image
      /// </summary>
      /// <param name="src">
      /// The source image.         
      /// This function expects Image&lt;Bgr, Byte&gt; or Image&lt;Gray, Byte&gt;. If the wrong image type is given, the original image
      /// will be returned.</param>
      /// <param name="dst">The destination image</param>
      /// <param name="colorMapType">The type of color map</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "CvApplyColorMap")]
      public extern static void ApplyColorMap(IntPtr src, IntPtr dst, CvEnum.ColorMapType colorMapType);

      /// <summary>
      /// Check that every array element is neither NaN nor +- inf. The functions also check that each value
      /// is between minVal and maxVal. in the case of multi-channel arrays each channel is processed
      /// independently. If some values are out of range, position of the first outlier is stored in pos, 
      /// and then the functions either return false (when quiet=true) or throw an exception.
      /// </summary>
      /// <param name="arr">The array to check</param>
      /// <param name="quiet">The flag indicating whether the functions quietly return false when the array elements are
      /// out of range, or they throw an exception</param>
      /// <param name="pos">This will be filled with the position of the first outlier</param>
      /// <param name="minVal">The inclusive lower boundary of valid values range</param>
      /// <param name="maxVal">The exclusive upper boundary of valid values range</param>
      /// <returns>If quiet, return true if all values are in range</returns>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvCheckRange(
         IntPtr arr,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool quiet,
         ref Point pos,
         double minVal,
         double maxVal);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeatureDetectorDetectKeyPoints(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints);
   }
}
