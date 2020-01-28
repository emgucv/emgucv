//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV.Reflection
{
    /// <summary>
    /// A collection of reflection function that can be applied to IImage object
    /// </summary>
    public static class ReflectIImage
    {

        /// <summary>
        /// Get all the methods that belongs to the IImage and Image class with ExposableMethodAttribute set true.
        /// </summary>
        /// <param name="image">The IImage object to be refelected for methods marked with ExposableMethodAttribute</param>
        /// <returns>All the methods that belongs to the IImage and Image class with ExposableMethodAttribute set true</returns>
        public static IEnumerable<KeyValuePair<String, MethodInfo>> GetImageMethods(IInputArray image)
        {
            if (image != null)
            {
                foreach (MethodInfo mi in image.GetType().GetMethods())
                {
                    Object[] atts = mi.GetCustomAttributes(typeof(ExposableMethodAttribute), false);
                    if (atts.Length > 0)
                    {
                        ExposableMethodAttribute att = (ExposableMethodAttribute)atts[0];
                        if (att.Exposable)
                            yield return new KeyValuePair<String, MethodInfo>(att.Category, mi);
                    }
                }
            }
        }

        /// <summary>
        /// Get the color type of the image
        /// </summary>
        /// <param name="image">The image to apply reflection on</param>
        /// <returns>The color type of the image</returns>
        public static Type GetTypeOfColor(IInputArray image)
        {
            Type baseType = Toolbox.GetBaseType(image.GetType(), "Image`2") ??
                             Toolbox.GetBaseType(image.GetType(), "CudaImage`2");

            if (baseType != null)
                return baseType.GetGenericArguments()[0];
            else
            {
                using (InputArray ia = image.GetInputArray())
                {
                    int numberOfChannels = ia.GetChannels();
                    return
                        numberOfChannels == 1 ? typeof(Gray) :
                        numberOfChannels == 3 ? typeof(Bgr) :
                        numberOfChannels == 4 ? typeof(Bgra) :
                        null;
                }
            }
        }

        /// <summary>
        /// Get the depth type of the image
        /// </summary>
        /// <param name="image">The image to apply reflection on</param>
        /// <returns>The depth type of the image</returns>
        public static Type GetTypeOfDepth(IInputArray image)
        {
            Type baseType = Toolbox.GetBaseType(image.GetType(), "Image`2") ??
                            Toolbox.GetBaseType(image.GetType(), "CudaImage`2");

            if (baseType != null)
                return baseType.GetGenericArguments()[1];
            else
            {
                using (InputArray iaImage = image.GetInputArray())
                {
                    return CvInvoke.GetDepthType(iaImage.GetDepth());
                }
                //baseType = Toolbox.GetBaseType(image.GetType(), "Mat");
                //return
                //baseType == null ? null :
                //CvInvoke.GetDepthType((image as Mat).Depth);
            }
        }


        /// <summary>
        /// Get the color at the specific location of the image
        /// </summary>
        /// <param name="image">The image to obtain pixel value from</param>
        /// <param name="location">The location to sample a pixel</param>
        /// <returns>The color at the specific location</returns>
        public static IColor GetPixelColor(IInputArray image, Point location)
        {
            using (InputArray ia = image.GetInputArray())
            {
                Size size = ia.GetSize();
                location.X = Math.Min(location.X, size.Width - 1);
                location.Y = Math.Min(location.Y, size.Height - 1);

                MethodInfo indexers =
                    image.GetType()
                        .GetMethod("get_Item", new Type[2] {typeof(int), typeof(int)});

                return indexers == null
                    ? new Bgra()
                    : indexers.Invoke(image, new object[2] {location.Y, location.X}) as IColor;
            }
        }
    }
}

