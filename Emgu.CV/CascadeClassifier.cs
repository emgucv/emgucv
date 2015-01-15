//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      static CascadeClassifier()
      {
         CvInvoke.CheckLibraryLoaded();
      }

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
#if ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
            throw new FileNotFoundException("File '{0}' not found", file.FullName);
#else
            throw new FileNotFoundException(Properties.StringTable.FileNotFound, file.FullName);
#endif
#endif
         using (CvString s = new CvString(fileName))
            _ptr = CvCascadeClassifierCreate(s);

         if (_ptr == IntPtr.Zero)
         {
#if NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
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
      /// <param name="maxSize">Maximum window size. Use Size.Empty for default, where the parameter will be ignored.</param>
      /// <returns>The objects detected, one array per channel</returns>
      public Rectangle[] DetectMultiScale(IInputArray image, double scaleFactor = 1.1, int minNeighbors = 3, Size minSize = new Size(), Size maxSize = new Size())
      {
         using (Util.VectorOfRect rectangles = new Util.VectorOfRect())
         using (InputArray iaImage = image.GetInputArray())
         {
            CvCascadeClassifierDetectMultiScale(_ptr, iaImage, rectangles, scaleFactor, minNeighbors, 0, ref minSize, ref maxSize);
            return rectangles.ToArray();
         }
      }

      /// <summary>
      /// Get if the cascade is old format
      /// </summary>
      public bool IsOldFormatCascade
      {
         get
         {
            return CvCascadeClassifierIsOldFormatCascade(_ptr);
         }
      }

      /// <summary>
      /// Get the original window size
      /// </summary>
      public Size OriginalWindowSize
      {
         get
         {
            Size s = new Size();
            CvCascadeClassifierGetOriginalWindowSize(_ptr, ref s);
            return s;
         }
      }

      /// <summary>
      /// Release the CascadeClassifier Object and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvCascadeClassifierRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCascadeClassifierCreate(IntPtr fileName);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvCascadeClassifierRelease(ref IntPtr classifier);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvCascadeClassifierDetectMultiScale(
         IntPtr classifier,
         IntPtr image,
         IntPtr objects,
         double scaleFactor,
         int minNeighbors, int flags,
         ref Size minSize,
         ref Size maxSize);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool CvCascadeClassifierIsOldFormatCascade(IntPtr classifier);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvCascadeClassifierGetOriginalWindowSize(IntPtr classifier, ref Size size);
   }

}
