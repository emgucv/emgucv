/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// The interface for a background foreground Detector
   /// </summary>
   /// <typeparam name="TColor">The type of color image to be analyzed</typeparam>
   public interface IBGFGDetector<TColor> 
      where TColor : struct, IColor
   {
      /// <summary>
      /// Update the FG / BG detector 
      /// </summary>
      /// <param name="image">The image which will be used to update the BGFG detector</param>
      void Update(Image<TColor, Byte> image);

      /// <summary>
      /// Get the mask of the forground
      /// </summary>
      Image<Gray, Byte> ForegroundMask
      {
         get;
      }

      /// <summary>
      /// Get the mask of the background
      /// </summary>
      Image<Gray, Byte> BackgroundMask
      {
         get;
      }
   }
}
*/