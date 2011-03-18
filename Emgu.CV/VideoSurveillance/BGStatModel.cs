//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

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
      public BGStatModel(Image<TColor, Byte> image, Emgu.CV.CvEnum.BG_STAT_TYPE type)
      {
         _ptr = (type == Emgu.CV.CvEnum.BG_STAT_TYPE.FGD_STAT_MODEL) ?
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
      /// A cache of the update function
      /// </summary>
      private BGStatModelDelegates.UpdateFunctionDelagate updateFunction;

      /// <summary>
      /// Update the statistic model
      /// </summary>
      /// <param name="image"></param>
      /// <param name="learningRate">Use -1 for default</param>
      /// <returns>The number of found foreground regions</returns>
      public virtual int Update(Image<TColor, Byte> image, double learningRate)
      {
         if (updateFunction == null)
         {
            updateFunction = (BGStatModelDelegates.UpdateFunctionDelagate)Marshal.GetDelegateForFunctionPointer(MCvBGStatModel.CvUpdateBGStatModel, typeof(BGStatModelDelegates.UpdateFunctionDelagate));
         }
         return updateFunction(image.Ptr, _ptr, learningRate);
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
            IntPtr statModelbackground = MCvBGStatModel.background;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(statModelbackground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
         }
      }

      /// <summary>
      /// Get a copy of the mask for the current foreground
      /// </summary>
      public Image<Gray, Byte> ForgroundMask
      {
         get
         {
            IntPtr statModelforeground = MCvBGStatModel.foreground;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(statModelforeground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
         }
      }

      /// <summary>
      /// Release the BGStatModel and all the unmanaged memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         BGStatModelDelegates.ReleaseFunction releaseFunction = (BGStatModelDelegates.ReleaseFunction)Marshal.GetDelegateForFunctionPointer(MCvBGStatModel.CvReleaseBGStatModel, typeof(BGStatModelDelegates.ReleaseFunction));
         releaseFunction(ref _ptr);
      }
   }

   internal static class BGStatModelDelegates
   {
      /// <summary>
      /// Defines an image update function
      /// </summary>
      /// <param name="img">The image to be used for update</param>
      /// <param name="statModel">The stat model to update</param>
      /// <param name="learningRate">Use -1 for default</param>
      /// <returns></returns>
      public delegate int UpdateFunctionDelagate(IntPtr img, IntPtr statModel, double learningRate);

      /// <summary>
      /// Define the Release function
      /// </summary>
      /// <param name="ptr">The background mode to be released</param>
      public delegate void ReleaseFunction(ref IntPtr ptr);
   }
}
