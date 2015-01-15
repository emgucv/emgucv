//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

 using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /*
      /// <summary>
      /// Deallocates the cascade that has been created manually or loaded using cvLoadHaarClassifierCascade or cvLoad
      /// </summary>
      /// <param name="cascade">Double pointer to the released cascade. The pointer is cleared by the function. </param>
      [DllImport(OpencvObjdetectLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseHaarClassifierCascade(ref IntPtr cascade);

      /// <summary>
      /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles. The function scans the image several times at different scales (see cvSetImagesForHaarClassifierCascade). Each time it considers overlapping regions in the image and applies the classifiers to the regions using cvRunHaarClassifierCascade. It may also apply some heuristics to reduce number of analyzed regions, such as Canny prunning. After it has proceeded and collected the candidate rectangles (regions that passed the classifier cascade), it groups them and returns a sequence of average rectangles for each large enough group. The default parameters (scale_factor=1.1, min_neighbors=3, flags=0) are tuned for accurate yet slow object detection. For a faster operation on real video images the settings are: scale_factor=1.2, min_neighbors=2, flags=CV_HAAR_DO_CANNY_PRUNING, min_size=&lt;minimum possible face size&gt; (for example, ~1/4 to 1/16 of the image area in case of video conferencing). 
      /// </summary>
      /// <param name="image">Image to detect objects in.</param>
      /// <param name="cascade">Haar classifier cascade in internal representation</param>
      /// <param name="storage">Memory storage to store the resultant sequence of the object candidate rectangles</param>
      /// <param name="scaleFactor">Use 1.1 as default. The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%</param>
      /// <param name="minNeighbors">Use 3 as default. Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure</param>
      /// <param name="flags">Mode of operation. Currently the only flag that may be specified is CV_HAAR_DO_CANNY_PRUNING. If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing</param>
      /// <param name="minSize">Use Size.Empty as default. Minimum window size. By default, it is set to the size of samples the classifier has been trained on (~20x20 for face detection). </param>
      /// <param name="maxSize">Use Size.Empty to ignor the parameter. </param>
      /// <returns>Rectangular regions in the given image that are likely to contain objects the cascade has been trained for</returns>
      [DllImport(OpencvObjdetectLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvHaarDetectObjects(
         IntPtr image,
         IntPtr cascade,
         IntPtr storage,
         double scaleFactor,
         int minNeighbors,
         CvEnum.HAAR_DETECTION_TYPE flags,
         Size minSize, 
         Size maxSize);*/


   }
}
