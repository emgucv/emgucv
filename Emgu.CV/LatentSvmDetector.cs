//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Latent SVM detector
   /// </summary>
   public class LatentSvmDetector : UnmanagedObject
   {
      static LatentSvmDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Load the trained detector from files
      /// </summary>
      /// <param name="fileNames">The names of the trained latent svm file</param>
      /// <param name="classNames">The names of the class</param>
      public LatentSvmDetector(String[] fileNames, String[] classNames = null)
      {
         CvString[] fileNameStrings = new CvString[fileNames.Length];
         for (int i = 0; i < fileNames.Length ;i++)
            fileNameStrings[i] = new CvString(fileNames[i]);

         CvString[] classNameStrings = null;
         if (classNames != null)
         {
            classNameStrings = new CvString[classNames.Length];
            for (int i = 0; i < classNames.Length; i++)
               classNameStrings[i] = new CvString(classNames[i]);
         }
         try
         {
            using (VectorOfCvString fvcs = new VectorOfCvString(fileNameStrings))
            using (VectorOfCvString cvcs = new VectorOfCvString())
            {
               if (classNameStrings != null)
                  cvcs.Push(classNameStrings);

               _ptr = cveLSVMDetectorCreate(fvcs, cvcs);
            }
         }
         finally
         {
            for (int i =  0; i < fileNameStrings.Length; i++)
               fileNameStrings[i].Dispose();

            if (classNameStrings != null)
               for (int i = 0; i < classNameStrings.Length; i++)
                  classNameStrings[i].Dispose();
         }
      }

      /// <summary>
      /// Find rectangular regions in the given image that are likely to contain objects and corresponding confidence levels
      /// </summary>
      /// <param name="image">The image to detect objects in</param>
      /// <param name="overlapThreshold">Threshold for the non-maximum suppression algorithm</param>
      /// <returns>Array of detected objects</returns>
      public MCvObjectDetection[] Detect(Mat image, float overlapThreshold = 0.5f)
      {
         using (VectorOfObjectDetection vod = new VectorOfObjectDetection())
         {
            cveLSVMDetectorDetect(_ptr, image, vod, overlapThreshold);
            return vod.ToArray();
         }
      }

      public int ClassCount
      {
         get { return cveLSVMGetClassCount(_ptr); }
      }

      public String[] ClassNames
      {
         get
         {
            using (VectorOfCvString vcs = new VectorOfCvString())
            {
               cveLSVMGetClassNames(_ptr, vcs);
              
               String[] r = new String[vcs.Size];
               for (int i = 0; i < r.Length; i++)
               {
                  CvString s = vcs[i];
                  r[i] = s.ToString();
                  s.Dispose();
               }
               return r;
            }
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with the LatenSvnDetector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            cveLSVMDetectorRelease(ref _ptr);
         }
      }

      


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveLSVMDetectorCreate(IntPtr filenames, IntPtr classNames);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveLSVMDetectorDetect(IntPtr detector, IntPtr image, IntPtr objects, float overlapThreshold);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveLSVMDetectorRelease(ref IntPtr detector);
      

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cveLSVMGetClassCount(IntPtr detector);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveLSVMGetClassNames(IntPtr detector, IntPtr classNames);
   }
}
