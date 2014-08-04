/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Laten SVM detector
   /// </summary>
   public class LatentSvmDetector : UnmanagedObject
   {
      static LatentSvmDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Load the trained detector from file
      /// </summary>
      /// <param name="fileName">The trained laten svm file</param>
      public LatentSvmDetector(String fileName)
      {
         _ptr = cvLoadLatentSvmDetector(fileName);
         if (_ptr == IntPtr.Zero)
            throw new ArgumentException(String.Format("Unable to load latent svm model from the file {0}.", fileName));
      }

      /// <summary>
      /// Find rectangular regions in the given image that are likely to contain objects and corresponding confidence levels
      /// </summary>
      /// <param name="image">The image to detect objects in</param>
      /// <param name="overlapThreshold">Threshold for the non-maximum suppression algorithm, Use default value of 0.5</param>
      /// <returns>Array of detected objects</returns>
      public MCvObjectDetection[] Detect(Image<Bgr, Byte> image, float overlapThreshold)
      {
         using (MemStorage stor = new MemStorage())
         {
            IntPtr seqPtr = cvLatentSvmDetectObjects(image, Ptr, stor, overlapThreshold, -1);
            if (seqPtr == IntPtr.Zero)
               return new MCvObjectDetection[0];
            Seq<MCvObjectDetection> seq = new Seq<MCvObjectDetection>(seqPtr, stor);
            return seq.ToArray();
         }
      }
      /// <summary>
      /// Release the unmanaged memory associated with the LatenSvnDetector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            cvReleaseLatentSvmDetector(ref _ptr);
            _ptr = IntPtr.Zero;
         }
      }

      /// <summary>
      /// Load trained detector from a file
      /// </summary>
      /// <param name="filename">Path to the file containing the parameters of trained Latent SVM detector</param>
      /// <returns>Trained Latent SVM detector in internal representation</returns>
      [DllImport(CvInvoke.OpencvObjdetectLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr cvLoadLatentSvmDetector(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String filename);

      /// <summary>
      /// Release memory allocated for CvLatentSvmDetector structure
      /// </summary>
      /// <param name="detector">Pointer to the trained Latent SVM detector</param>
      [DllImport(CvInvoke.OpencvObjdetectLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvReleaseLatentSvmDetector(ref IntPtr detector);

      /// <summary>
      /// Find rectangular regions in the given image that are likely to contain objects and corresponding confidence levels
      /// </summary>
      /// <param name="image">Image to detect objects in</param>
      /// <param name="detector">Latent SVM detector in internal representation</param>
      /// <param name="storage">Memory storage to store the resultant sequence of the object candidate rectangles</param>
      /// <param name="overlapThreshold">Threshold for the non-maximum suppression algorithm, use 0.5f for default</param>
      /// <param name="numThreads">Use -1 for default</param>
      /// <returns>Sequence of detected objects (bounding boxes and confidence levels stored in MCvObjectDetection structures</returns>
      [DllImport(CvInvoke.OpencvObjdetectLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr cvLatentSvmDetectObjects(
         IntPtr image,
         IntPtr detector,
         IntPtr storage,
         float overlapThreshold,
         int numThreads);
   }
}
*/