//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
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
      /// <param name="img">The image to be segmented</param>
      /// <param name="mask">The mask to initialize segmentation</param>
      /// <param name="rect">The rectangle to initialize the segmentation</param>
      /// <param name="bgdModel">The background model</param>
      /// <param name="fgdModel">The foreground model</param>
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
