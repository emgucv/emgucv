//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Util;

namespace Emgu.CV
{
    /// <summary>
    /// This is the proxy class for passing read-only input arrays into OpenCV functions.
    /// </summary>
    public interface IInputArray : IDisposable
    {
        /// <summary>
        /// The unmanaged pointer to the input array.
        /// </summary>
        /// <returns>The input array</returns>
        InputArray GetInputArray();
    }

    /// <summary>
    /// InputArrayOfArrays
    /// </summary>
    public interface IInputArrayOfArrays : IInputArray
    {

    }

    /// <summary>
    /// Extension methods for IInputArrays
    /// </summary>
    public static class IInputArrayExtensions
    {
        /// <summary>
        /// Determines whether the specified input array is umat.
        /// </summary>
        /// <param name="arr">The array</param>
        /// <returns>True if it is a umat</returns>
        public static bool IsUmat(this IInputArray arr)
        {
            using (InputArray ia = arr.GetInputArray())
                return ia.IsUMat;
        }

        /// <summary>
        /// Apply converter and compute result for each channel of the image, for single channel image, apply converter directly, for multiple channel image, make a copy of each channel to a temperary image and apply the convertor
        /// </summary>
        /// <typeparam name="TReturn">The return type</typeparam>
        /// <param name="image">The source image</param>
        /// <param name="conv">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        public static TReturn[] ForEachDuplicateChannel<TReturn>(this IInputArray image, Func<IInputArray, int, TReturn> conv)
        {
            using (InputArray ia = image.GetInputArray())
            {
                int channels = ia.GetChannels();
                TReturn[] res = new TReturn[channels];
                if (channels == 1)
                    res[0] = conv(image, 0);
                else
                {
                    using (Mat tmp = new Mat())
                        for (int i = 0; i < channels; i++)
                        {
                            CvInvoke.ExtractChannel(image, tmp, i);
                            res[i] = conv(tmp, i);
                        }
                }

                return res;
            }
        }

        /// <summary>
        /// Apply converter and compute result for each channel of the image, for single channel image, apply converter directly, for multiple channel image, make a copy of each channel to a temperary image and apply the convertor
        /// </summary>
        /// <param name="image">The source image</param>
        /// <param name="action">The converter such that accept the IntPtr of a single channel IplImage, and image channel index which returning result of type R</param>
        /// <returns>An array which contains result for each channel</returns>
        public static void ForEachDuplicateChannel(this IInputArray image, Action<IInputArray, int> action)
        {
            using (InputArray ia = image.GetInputArray())
            {
                int channels = ia.GetChannels();
                if (channels == 1)
                    action(image, 0);
                else
                {
                    using (Mat tmp = new Mat())
                        for (int i = 0; i < channels; i++)
                        {
                            CvInvoke.ExtractChannel(image, tmp, i);
                            action(tmp, i);
                        }
                }
            }
        }
    }
}
