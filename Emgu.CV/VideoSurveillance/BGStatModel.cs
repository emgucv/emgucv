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
   /// Background statistics model
   /// </summary>
   public class BGStatModel<TColor> : UnmanagedObject, IBGFGDetector<TColor>
      where TColor : struct, IColor
   {
      /// <summary>
      /// Create a BGStatModel
      /// </summary>
      /// <param name="image">The image used for initiating the statistic model</param>
      /// <param name="type">The type of the statistics model</param>
      public BGStatModel(Image<TColor, Byte> image, Emgu.CV.CvEnum.BgStatType type)
      {
         _ptr = (type == Emgu.CV.CvEnum.BgStatType.FgdStatModel) ?
            CvInvoke.cvCreateFGDStatModel(image, IntPtr.Zero)
            : CvInvoke.cvCreateGaussianBGModel(image, IntPtr.Zero);
      }

      /// <summary>
      /// Create a foreground statistic model using the given parameters
      /// </summary>
      /// <param name="image">The image used for initiating the statistic model</param>
      /// <param name="parameters">FGDStatModel</param>
      public BGStatModel(Image<TColor, Byte> image, ref MCvFGDStatModelParams parameters)
      {
         _ptr = CvInvoke.cvCreateFGDStatModel(image, ref parameters);
      }

      /// <summary>
      /// Create a Gaussian Background statistic model using the given parameters
      /// </summary>
      /// <param name="image">The image used for initiating the statistic model</param>
      /// <param name="parameters">GaussStatModel</param>
      public BGStatModel(Image<TColor, Byte> image, ref MCvGaussBGStatModelParams parameters)
      {
         _ptr = CvInvoke.cvCreateGaussianBGModel(image, ref parameters);
      }

      /// <summary>
      /// Update the statistic model
      /// </summary>
      /// <param name="image">The image that is used to update the background model</param>
      /// <param name="learningRate">Use -1 for default</param>
      /// <returns>The number of found foreground regions</returns>
      public virtual int Update(Image<TColor, Byte> image, double learningRate)
      {
         return CvInvoke.cvUpdateBGStatModel(image, _ptr, learningRate);
      }

      /// <summary>
      /// Update the statistic model
      /// </summary>
      /// <param name="image"></param>
      public virtual void Update(Image<TColor, Byte> image)
      {
         Update(image, -1);
      }

      /// <summary>
      /// Get the MCvBGStatModel structure
      /// </summary>
      public MCvBGStatModel MCvBGStatModel
      {
         get
         {
            return (MCvBGStatModel)Marshal.PtrToStructure(_ptr, typeof(MCvBGStatModel));
         }
      }

      /// <summary>
      /// Get a copy of the current background
      /// </summary>
      public Image<Gray, Byte> BackgroundMask
      {
         get
         {
            IntPtr statModelbackground = MCvBGStatModel.Background;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(statModelbackground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.Width, iplImage.Height, iplImage.WidthStep, iplImage.ImageData);
         }
      }

      /// <summary>
      /// Get a copy of the mask for the current foreground
      /// </summary>
      public Image<Gray, Byte> ForegroundMask
      {
         get
         {
            IntPtr statModelforeground = MCvBGStatModel.Foreground;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(statModelforeground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.Width, iplImage.Height, iplImage.WidthStep, iplImage.ImageData);
         }
      }

      /// <summary>
      /// Release the BGStatModel and all the unmanaged memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseBGStatModel(ref _ptr);
      }
   }
}
*/