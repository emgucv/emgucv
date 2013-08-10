//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// The Cascade Classifier
   /// </summary>
   public class CascadeClassifier : UnmanagedObject
   {
      /// <summary>
      /// A dummy constructor that mainly aimed for those who would like to inherite this class
      /// </summary>
      protected CascadeClassifier()
      {
      }

      ///<summary> Create a CascadeClassifier from the specific file</summary>
      ///<param name="fileName"> The name of the file that contains the CascadeClassifier</param>
      public CascadeClassifier(String fileName)
      {
#if !NETFX_CORE
         FileInfo file = new FileInfo(fileName);
         if (!file.Exists)
            throw new FileNotFoundException(Properties.StringTable.FileNotFound, file.FullName);
#endif

         _ptr = CvInvoke.CvCascadeClassifierCreate(fileName);

         if (_ptr == IntPtr.Zero)
         {
#if NETFX_CORE
            throw new NullReferenceException(String.Format("Fail to create HaarCascade object: {0}", fileName));
#else
            throw new NullReferenceException(String.Format(Properties.StringTable.FailToCreateHaarCascade, file.FullName));
#endif
         }
      }

      /// <summary>
      /// Finds rectangular regions in the given image that are likely to contain objects the cascade has been trained for and returns those regions as a sequence of rectangles. 
      /// The function scans the image several times at different scales. Each time it considers overlapping regions in the image. 
      /// It may also apply some heuristics to reduce number of analyzed regions, such as Canny prunning. 
      /// After it has proceeded and collected the candidate rectangles (regions that passed the classifier cascade), it groups them and returns a sequence of average rectangles for each large enough group. 
      /// </summary>
      /// <param name="image">The image where the objects are to be detected from</param>
      /// <param name="scaleFactor">The factor by which the search window is scaled between the subsequent scans, for example, 1.1 means increasing window by 10%</param>
      /// <param name="minNeighbors">Minimum number (minus 1) of neighbor rectangles that makes up an object. All the groups of a smaller number of rectangles than min_neighbors-1 are rejected. If min_neighbors is 0, the function does not any grouping at all and returns all the detected candidate rectangles, which may be useful if the user wants to apply a customized grouping procedure. Use 3 for default.</param>
      /// <param name="minSize">Minimum window size. Use Size.Empty for default, where it is set to the size of samples the classifier has been trained on (~20x20 for face detection)</param>
      /// <param name="maxSize">Maxumum window size. Use Size.Empty for default, where the parameter will be ignored.</param>
      /// <returns>The objects detected, one array per channel</returns>
      public Rectangle[] DetectMultiScale(Image<Gray, Byte> image, double scaleFactor, int minNeighbors, Size minSize, Size maxSize)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<Rectangle> rectangles = new Seq<Rectangle>(stor);

            CvInvoke.CvCascadeClassifierDetectMultiScale(_ptr, image, rectangles, scaleFactor, minNeighbors, 0, minSize, maxSize);
            return rectangles.ToArray();
         }
      }

      /// <summary>
      /// Release the CascadeClassifier Object and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvCascadeClassifierRelease(ref _ptr);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCascadeClassifierCreate([MarshalAs(CvInvoke.StringMarshalType)] String fileName);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvCascadeClassifierRelease(ref IntPtr classifier);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvCascadeClassifierDetectMultiScale(
         IntPtr classifier,
         IntPtr image,
         IntPtr objects,
         double scaleFactor,
         int minNeighbors, int flags,
         Size minSize,
         Size maxSize);
   }
}
