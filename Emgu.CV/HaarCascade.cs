//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   ///<summary> 
   /// HaarCascade for object detection
   /// </summary>
   public class HaarCascade : UnmanagedObject
   {
      ///<summary> Create a HaarCascade object from the specific file</summary>
      ///<param name="fileName"> The name of the file that contains the HaarCascade object</param>
      public HaarCascade(String fileName)
      {
         FileInfo file = new FileInfo(fileName);
         if (!file.Exists)
            throw new FileNotFoundException(Properties.StringTable.FileNotFound, file.FullName);

         _ptr = CvInvoke.cvLoad(file.FullName, IntPtr.Zero, null, IntPtr.Zero);

         if (_ptr == IntPtr.Zero)
         {
            throw new NullReferenceException(String.Format(Properties.StringTable.FailToCreateHaarCascade, file.FullName));
         }
      }

      /// <summary>
      /// Detect HaarCascade object in the given image, using predefined parameters
      /// </summary>
      /// <param name="image">The image where the objects are to be detected from</param>
      /// <returns>The objects detected</returns>
      public MCvAvgComp[] Detect(Image<Gray, Byte> image)
      {
         return Detect(image, 1.1, 3, CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, Size.Empty, Size.Empty);
      }

      /// <summary>
      /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles. 
      /// The function scans the image several times at different scales (see cvSetImagesForHaarClassifierCascade). Each time it considers overlapping regions in the image and applies the classifiers to the regions using cvRunHaarClassifierCascade. 
      /// It may also apply some heuristics to reduce number of analyzed regions, such as Canny prunning. 
      /// After it has proceeded and collected the candidate rectangles (regions that passed the classifier cascade), it groups them and returns a sequence of average rectangles for each large enough group. 
      /// The default parameters (scale_factor=1.1, min_neighbors=3, flags=0) are tuned for accurate yet slow object detection. 
      /// For a faster operation on real video images the settings are: scale_factor=1.2, min_neighbors=2, flags=CV_HAAR_DO_CANNY_PRUNING, min_size=&lt;minimum possible face size&gt; 
      /// (for example, ~1/4 to 1/16 of the image area in case of video conferencing). 
      /// </summary>
      /// <param name="image">The image where the objects are to be detected from</param>
      /// <param name="scaleFactor">The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%</param>
      /// <param name="minNeighbors">Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure</param>
      /// <param name="flag">Mode of operation. Currently the only flag that may be specified is CV_HAAR_DO_CANNY_PRUNING. If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing.</param>
      /// <param name="minSize">Minimum window size. Use Size.Empty for default, where it is set to the size of samples the classifier has been trained on (~20x20 for face detection)</param>
      /// <param name="maxSize">Maxumum window size. Use Size.Empty for default, where the parameter will be ignored.</param>
      /// <returns>The objects detected, one array per channel</returns>
      public MCvAvgComp[] Detect(Image<Gray, Byte> image, double scaleFactor, int minNeighbors, CvEnum.HAAR_DETECTION_TYPE flag, Size minSize, Size maxSize)
      {
         using (MemStorage stor = new MemStorage())
         {
            IntPtr objects = CvInvoke.cvHaarDetectObjects(
                image.Ptr,
                Ptr,
                stor.Ptr,
                scaleFactor,
                minNeighbors,
                flag,
                minSize, 
                maxSize);

            if (objects == IntPtr.Zero)
               return new MCvAvgComp[0];

            Seq<MCvAvgComp> rects = new Seq<MCvAvgComp>(objects, stor);
            return rects.ToArray();
         }
      }

      /// <summary>
      /// Release the HaarCascade Object and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseHaarClassifierCascade(ref _ptr);
      }
   }
}
