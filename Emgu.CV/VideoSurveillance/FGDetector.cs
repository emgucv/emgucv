/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A foreground detector
   /// </summary>
   public class FGDetector<TColor> : UnmanagedObject, IBGFGDetector<TColor>
      where TColor : struct, IColor
   {
      /// <summary>
      /// Create a foreground detector of the specific type
      /// </summary>
      /// <param name="type">The type of the detector to be created</param>
      public FGDetector(CvEnum.ForgroundDetectorType type)
      {
         _ptr = FGDetectorInvoke.CvCreateFGDetectorBase(type, IntPtr.Zero);
      }

      /// <summary>
      /// Create a foreground detector of the specific type
      /// </summary>
      /// <param name="type">The type of the detector to be created. Should be either FGD ot FGD_SIMPLE</param>
      /// <param name="parameter">The FGD parameters</param>
      public FGDetector(CvEnum.ForgroundDetectorType type, MCvFGDStatModelParams parameter)
      {
         if (type == CvEnum.ForgroundDetectorType.Fgd || type == CvEnum.ForgroundDetectorType.FgdSimple)
         {
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MCvFGDStatModelParams)));
            Marshal.StructureToPtr(parameter, p, false);
            _ptr = FGDetectorInvoke.CvCreateFGDetectorBase(type, p);
            Marshal.FreeHGlobal(p);
         }
         else
         {
            throw new ArgumentException("This constructor only accepts detector type of either FGD or FGD_SIMPLE");
         }
      }

      /// <summary>
      /// Create a MOG foreground detector
      /// </summary>
      /// <param name="parameter">The MOG foreground detector parameters</param>
      public FGDetector(MCvGaussBGStatModelParams parameter)
      {
         IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MCvGaussBGStatModelParams)));
         Marshal.StructureToPtr(parameter, p, false);
         _ptr = FGDetectorInvoke.CvCreateFGDetectorBase(CvEnum.ForgroundDetectorType.Mog, p);
         Marshal.FreeHGlobal(p);
      }

      /// <summary>
      /// Update the foreground detector using the specific image
      /// </summary>
      /// <param name="image">The image which will be used to update the FGDetector</param>
      public void Update(Image<TColor, Byte> image)
      {
         FGDetectorInvoke.CvFGDetectorProcess(_ptr, image.Ptr);
      }

      /// <summary>
      /// Get the foreground mask from the detector
      /// </summary>
      public Image<Gray, Byte> ForegroundMask
      {
         get
         {
            IntPtr foreground = FGDetectorInvoke.CvFGDetectorGetMask(_ptr);
            if (foreground == IntPtr.Zero) return null;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(foreground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.Width, iplImage.Height, iplImage.WidthStep, iplImage.ImageData);
         }
      }

      /// <summary>
      /// Get the background mask
      /// </summary>
      public Image<Gray, Byte> BackgroundMask
      {
         get
         {
            return ForegroundMask.Not();
         }
      }

      /// <summary>
      /// Release the foreground detector
      /// </summary>
      protected override void DisposeObject()
      {
         FGDetectorInvoke.CvFGDetectorRelease(_ptr);
      }


   }

   internal static class FGDetectorInvoke
   {
      static FGDetectorInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a simple foreground detector
      /// </summary>
      /// <param name="type">The type of the detector</param>
      /// <param name="param">Pointer to the parameters of the detector</param>
      /// <returns>Pointer the to foreground detector</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateFGDetectorBase(CvEnum.ForgroundDetectorType type, IntPtr param);

      /// <summary>
      /// Release the foreground detector
      /// </summary>
      /// <param name="detector">The foreground detector to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFGDetectorRelease(IntPtr detector);

      /// <summary>
      /// Get the foreground mask from the foreground detector
      /// </summary>
      /// <param name="detector">The foreground detector</param>
      /// <returns>The foreground mask</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFGDetectorGetMask(IntPtr detector);

      /// <summary>
      /// Update the FGDetector with new image
      /// </summary>
      /// <param name="detector">The foreground detector</param>
      /// <param name="image">The image which will be used to update the FGDetector</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFGDetectorProcess(IntPtr detector, IntPtr image);

      /// <summary>
      /// Create a simple foreground detector
      /// </summary>
      /// <param name="type">The type of the detector</param>
      /// <param name="param">The parameters of the detector</param>
      /// <returns>Pointer the to foreground detector</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateFGDetectorBase(CvEnum.ForgroundDetectorType type, ref MCvFGDStatModelParams param);
   }
}
*/