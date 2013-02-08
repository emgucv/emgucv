//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Face Recognizer
   /// </summary>
   public abstract class FaceRecognizer : UnmanagedObject
   {
      /// <summary>
      /// Train the face recognizer with the specific images and labels
      /// </summary>
      /// <param name="images">The images used in the training</param>
      /// <param name="labels">The labels of the images</param>
      public void Train(IImage[] images, int[] labels)
      {
         Debug.Assert(images.Length == labels.Length, "The number of labels must equals the number of images");

         IntPtr[] ptrs = new IntPtr[images.Length];
         for (int i = 0; i < images.Length; i++)
         {
            ptrs[i] = images[i].Ptr;
         }

         GCHandle imagesHandle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
         GCHandle labelsHandle = GCHandle.Alloc(labels, GCHandleType.Pinned);

         try
         {
            CvInvoke.CvFaceRecognizerTrain(_ptr, imagesHandle.AddrOfPinnedObject(), labelsHandle.AddrOfPinnedObject(), images.Length);
         }
         finally
         {
            imagesHandle.Free();
            labelsHandle.Free();
         }
      }

      /// <summary>
      /// Predict the label of the image
      /// </summary>
      /// <param name="image">The image where prediction will be based on</param>
      /// <returns>The prediction label</returns>
      public PredictionResult Predict(IImage image)
      {
         int label = -1;
         double distance = -1;
         CvInvoke.CvFaceRecognizerPredict(_ptr, image.Ptr, ref label, ref distance);
         return new PredictionResult() { Label = label, Distance = distance };
      }

      /// <summary>
      /// The prediction result
      /// </summary>
      public struct PredictionResult
      {
         /// <summary>
         /// The label
         /// </summary>
         public int Label;
         /// <summary>
         /// The distance
         /// </summary>
         public double Distance;
      }

      /// <summary>
      /// Save the FaceRecognizer to a file
      /// </summary>
      /// <param name="fileName">The file name to be saved to</param>
      public void Save(String fileName)
      {
         CvInvoke.CvFaceRecognizerSave(_ptr, fileName);
      }

      /// <summary>
      /// Load the FaceRecognizer from the file
      /// </summary>
      /// <param name="fileName">The file where the FaceRecognizer will be loaded from</param>
      public void Load(String fileName)
      {
         CvInvoke.CvFaceRecognizerLoad(_ptr, fileName);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this FaceRecognizer
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvFaceRecognizerRelease(ref _ptr);
      }
   }

   /// <summary>
   /// Eigen face recognizer
   /// </summary>
   public class EigenFaceRecognizer : FaceRecognizer
   {
      /// <summary>
      /// Create an EigenFaceRecognizer
      /// </summary>
      /// <param name="numComponents">The number of components, use 0 for default</param>
      /// <param name="threshold">The distance threshold</param>
      public EigenFaceRecognizer(int numComponents, double threshold)
      {
         _ptr = CvInvoke.CvEigenFaceRecognizerCreate(numComponents, threshold);
      }
   }

   /// <summary>
   /// Fisher face recognizer
   /// </summary>
   public class FisherFaceRecognizer : FaceRecognizer
   {
      /// <summary>
      /// Create a FisherFaceRecognizer
      /// </summary>
      /// <param name="numComponents">The number of components, use 0 for default</param>
      /// <param name="threshold">The distance threshold</param>
      public FisherFaceRecognizer(int numComponents, double threshold)
      {
         _ptr = CvInvoke.CvFisherFaceRecognizerCreate(numComponents, threshold);
      }
   }

   /// <summary>
   /// LBPH face recognizer
   /// </summary>
   public class LBPHFaceRecognizer : FaceRecognizer
   {
      /// <summary>
      /// Create a LBPH face recognizer
      /// </summary>
      /// <param name="radius">Radius, use 1 for default</param>
      /// <param name="neighbors">Neighbors, use 8 for default</param>
      /// <param name="gridX">Grid X, use 8 for default</param>
      /// <param name="gridY">Grid Y, use 9 for default</param>
      /// <param name="threshold">The distance threshold</param>
      public LBPHFaceRecognizer(int radius, int neighbors, int gridX, int gridY, double threshold)
      {
         _ptr = CvInvoke.CvLBPHFaceRecognizerCreate(radius, neighbors, gridX, gridY, threshold);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvEigenFaceRecognizerCreate(int numComponents, double threshold);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFisherFaceRecognizerCreate(int numComponents, double threshold);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerTrain(IntPtr recognizer, IntPtr images, IntPtr labels, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerPredict(IntPtr recognizer, IntPtr image, ref int label, ref double distance);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerSave(
         IntPtr recognizer,
         [MarshalAs(CvInvoke.StringMarshalType)]
         String fileName);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerLoad(
         IntPtr recognizer,
         [MarshalAs(CvInvoke.StringMarshalType)]
         String fileName);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerRelease(ref IntPtr recognizer);
   }
}
