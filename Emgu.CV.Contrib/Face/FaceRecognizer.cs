//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Face
{
   /// <summary>
   /// Face Recognizer
   /// </summary>
   public abstract class FaceRecognizer : UnmanagedObject
   {
      static FaceRecognizer()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Train the face recognizer with the specific images and labels
      /// </summary>
      /// <param name="images">The images used in the training. This can be a VectorOfMat</param>
      /// <param name="labels">The labels of the images. This can be a VectorOfInt</param>
      public void Train(IInputArray images, IInputArray labels)
      {
         using (InputArray iaImage = images.GetInputArray())
         using (InputArray iaLabels = labels.GetInputArray())
            CvFaceRecognizerTrain(_ptr, iaImage, iaLabels);
      }

      /// <summary>
      /// Train the face recognizer with the specific images and labels
      /// </summary>
      /// <param name="images">The images used in the training.</param>
      /// <param name="labels">The labels of the images.</param>
      public void Train<TColor, TDepth>(Image<TColor, TDepth>[] images, int[] labels)
         where TColor : struct, IColor
         where TDepth : new()
      {
         using (VectorOfMat imgVec = new VectorOfMat())
         using (VectorOfInt labelVec = new VectorOfInt(labels))
         {
            imgVec.Push<TDepth>(images);
            Train(imgVec, labelVec);
         }
      }

      /// <summary>
      /// Predict the label of the image
      /// </summary>
      /// <param name="image">The image where prediction will be based on</param>
      /// <returns>The prediction label</returns>
      public PredictionResult Predict(IInputArray image)
      {
         int label = -1;
         double distance = -1;
         using (InputArray iaImage = image.GetInputArray())
            CvFaceRecognizerPredict(_ptr, iaImage, ref label, ref distance);
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
         using (CvString s = new CvString(fileName))
            CvFaceRecognizerSave(_ptr, s);
      }

      /// <summary>
      /// Load the FaceRecognizer from the file
      /// </summary>
      /// <param name="fileName">The file where the FaceRecognizer will be loaded from</param>
      public void Load(String fileName)
      {
         using (CvString s = new CvString(fileName))
            CvFaceRecognizerLoad(_ptr, s);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this FaceRecognizer
      /// </summary>
      protected override void DisposeObject()
      {
         CvFaceRecognizerRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerTrain(IntPtr recognizer, IntPtr images, IntPtr labels);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerPredict(IntPtr recognizer, IntPtr image, ref int label, ref double distance);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerSave(
         IntPtr recognizer,
         IntPtr fileName);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerLoad(
         IntPtr recognizer,
         IntPtr fileName);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerRelease(ref IntPtr recognizer);
   }

   /// <summary>
   /// Eigen face recognizer
   /// </summary>
   public class EigenFaceRecognizer : FaceRecognizer
   {
      /// <summary>
      /// Create an EigenFaceRecognizer
      /// </summary>
      /// <param name="numComponents">The number of components</param>
      /// <param name="threshold">The distance threshold</param>
      public EigenFaceRecognizer(int numComponents = 0, double threshold = double.MaxValue)
      {
         _ptr = CvEigenFaceRecognizerCreate(numComponents, threshold);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvEigenFaceRecognizerCreate(int numComponents, double threshold);
   }

   /// <summary>
   /// Fisher face recognizer
   /// </summary>
   public class FisherFaceRecognizer : FaceRecognizer
   {
      /// <summary>
      /// Create a FisherFaceRecognizer
      /// </summary>
      /// <param name="numComponents">The number of components</param>
      /// <param name="threshold">The distance threshold</param>
      public FisherFaceRecognizer(int numComponents = 0, double threshold = double.MaxValue)
      {
         _ptr = CvFisherFaceRecognizerCreate(numComponents, threshold);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFisherFaceRecognizerCreate(int numComponents, double threshold);
   }

   /// <summary>
   /// LBPH face recognizer
   /// </summary>
   public class LBPHFaceRecognizer : FaceRecognizer
   {
      /// <summary>
      /// Create a LBPH face recognizer
      /// </summary>
      /// <param name="radius">Radius</param>
      /// <param name="neighbors">Neighbors</param>
      /// <param name="gridX">Grid X</param>
      /// <param name="gridY">Grid Y</param>
      /// <param name="threshold">The distance threshold</param>
      public LBPHFaceRecognizer(int radius = 1, int neighbors = 8, int gridX = 8, int gridY = 8, double threshold = Double.MaxValue)
      {
         _ptr = CvLBPHFaceRecognizerCreate(radius, neighbors, gridX, gridY, threshold);
      }

      /// <summary>
      /// Updates a FaceRecognizer with given data and associated labels.
      /// </summary>
      /// <param name="images">The training images, that means the faces you want to learn. The data has to be given as a VectorOfMat.</param>
      /// <param name="labels">The labels corresponding to the images</param>
      public void Update(IInputArray images, IInputArray labels)
      {
         using (InputArray iaImages = images.GetInputArray())
         using (InputArray iaLabels = labels.GetInputArray())
         CvFaceRecognizerUpdate(_ptr, iaImages, iaLabels);
      }

      /// <summary>
      /// Update the face recognizer with the specific images and labels
      /// </summary>
      /// <param name="images">The images used for updating the face recognizer</param>
      /// <param name="labels">The labels of the images</param>
      public void Update<TColor, TDepth>(Image<TColor, TDepth>[] images, int[] labels)
         where TColor : struct, IColor
         where TDepth : new()
      {
         Debug.Assert(images.Length == labels.Length, "The number of labels must equals the number of images");
         using (VectorOfMat imgVec = new VectorOfMat())
         using (VectorOfInt labelVec = new VectorOfInt(labels))
         {
            imgVec.Push(images);
            Update(imgVec, labelVec);
         }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvLBPHFaceRecognizerCreate(int radius, int neighbors, int gridX, int gridY, double threshold);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFaceRecognizerUpdate(IntPtr recognizer, IntPtr images, IntPtr labels);
   }

}
