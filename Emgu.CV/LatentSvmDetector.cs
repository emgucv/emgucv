//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
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
      /// <summary>
      /// Load the trained detector from file
      /// </summary>
      /// <param name="fileName">The trained laten svm file</param>
      public LatentSvmDetector(String fileName)
      {
         _ptr = CvInvoke.cvLoadLatentSvmDetector(fileName);
         if (_ptr == IntPtr.Zero)
            throw new ArgumentException(String.Format("Unable to create latent svm model from the file {0}.", fileName));
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
            IntPtr seqPtr = CvInvoke.cvLatentSvmDetectObjects(image, Ptr, stor, overlapThreshold, -1);
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
         CvInvoke.cvReleaseLatentSvmDetector(ref _ptr);
      }
   }
}
