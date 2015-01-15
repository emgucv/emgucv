//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Drawing;

#if ANDROID
using Bitmap =  Android.Graphics.Bitmap;
#elif IOS
using CoreGraphics;
using UIKit;
#endif

namespace Emgu.CV
{
   internal static class IImageExtensions
   {
      /// <summary>
      /// Apply converter and compute result for each channel of the image, for single channel image, apply converter directly, for multiple channel image, make a copy of each channel to a temperary image and apply the convertor
      /// </summary>
      /// <typeparam name="TReturn">The return type</typeparam>
      /// <param name="image">The source image</param>
      /// <param name="conv">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
      /// <returns>An array which contains result for each channel</returns>
      public static TReturn[] ForEachDuplicateChannel<TReturn>(this IImage image, Func<IImage, int, TReturn> conv)
      {
         TReturn[] res = new TReturn[image.NumberOfChannels];
         if (image.NumberOfChannels == 1)
            res[0] = conv(image, 0);
         else
         {
            using (Mat tmp = new Mat())
               for (int i = 0; i < image.NumberOfChannels; i++)
               {
                  CvInvoke.ExtractChannel(image, tmp, i);
                  res[i] = conv(tmp, i);
               }
         }
         return res;
      }

      /// <summary>
      /// Apply converter and compute result for each channel of the image, for single channel image, apply converter directly, for multiple channel image, make a copy of each channel to a temperary image and apply the convertor
      /// </summary>
      /// <param name="image">The source image</param>
      /// <param name="action">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
      /// <returns>An array which contains result for each channel</returns>
      public static void ForEachDuplicateChannel(this IImage image, Action<IImage, int> action)
      {
         if (image.NumberOfChannels == 1)
            action(image, 0);
         else
         {
            using (Mat tmp = new Mat())
               for (int i = 0; i < image.NumberOfChannels; i++)
               {
                  CvInvoke.ExtractChannel(image, tmp, i);
                  action(tmp, i);
               }
         }
      }


   }

   /// <summary>
   /// IImage interface
   /// </summary>
   public interface IImage : IDisposable, ICloneable, IInputOutputArray
   {
#if NETFX_CORE || ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
#elif IOS 
      /// <summary>
      /// Convert this image to UIImage
      /// </summary>
      /// <returns>
      /// The UIImage
      /// </returns>
      UIImage ToUIImage();
#else
      /// <summary>
      /// Convert this image into Bitmap, when available, data is shared with this image.
      /// </summary>
      /// <returns>The Bitmap, when available, data is shared with this image</returns>
      Bitmap Bitmap
      {
         get;
      }
#endif

      /// <summary>
      /// The size of this image
      /// </summary>
      Size Size
      {
         get;
      }

      /// <summary>
      /// Returns the min / max location and values for the image
      /// </summary>
      /// <returns>
      /// Returns the min / max location and values for the image
      /// </returns>
      void MinMax(out double[] minValues, out double[] maxValues, out Point[] minLocations, out Point[] maxLocations);

      ///<summary> 
      /// Split current IImage into an array of gray scale images where each element 
      /// in the array represent a single color channel of the original image
      ///</summary>
      ///<returns> 
      /// An array of gray scale images where each element 
      /// in the array represent a single color channel of the original image 
      ///</returns>
      IImage[] Split();

      /// <summary>
      /// Get the pointer to the unmanaged memory
      /// </summary>
      IntPtr Ptr
      {
         get;
      }

      /// <summary>
      /// Save the image to the specific <paramref name="fileName"/> 
      /// </summary>
      /// <param name="fileName">The file name of the image</param>
      void Save(String fileName);

      /// <summary>
      /// Get the number of channels for this image
      /// </summary>
      int NumberOfChannels
      {
         get;
      }
   }
}
