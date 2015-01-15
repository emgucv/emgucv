//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


#if !NETFX_CORE
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
      public static IEnumerable<KeyValuePair<String, MethodInfo>> GetImageMethods(IImage image)
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
                     yield return new KeyValuePair<String, MethodInfo>(att.Category, mi as MethodInfo);
               }
            }
         }
      }

      /// <summary>
      /// Get the color type of the image
      /// </summary>
      /// <param name="image">The image to apply reflection on</param>
      /// <returns>The color type of the image</returns>
      public static Type GetTypeOfColor(IImage image)
      {
         Type baseType =  Toolbox.GetBaseType(image.GetType(), "Image`2");
         if (baseType == null)
            baseType = Toolbox.GetBaseType(image.GetType(), "CudaImage`2");

         if (baseType != null)
            return baseType.GetGenericArguments()[0];
         else
         {
            baseType = Toolbox.GetBaseType(image.GetType(), "Mat");
            return
               baseType == null ? null :
               image.NumberOfChannels == 1 ? typeof(Gray) :
               image.NumberOfChannels == 3 ? typeof(Bgr) :
               image.NumberOfChannels == 4 ? typeof(Bgra) :
               null;
         }
      }

      /// <summary>
      /// Get the depth type of the image
      /// </summary>
      /// <param name="image">The image to apply reflection on</param>
      /// <returns>The depth type of the image</returns>
      public static Type GetTypeOfDepth(IImage image)
      {
         Type baseType = Toolbox.GetBaseType(image.GetType(), "Image`2");
         if (baseType == null)
            baseType = Toolbox.GetBaseType(image.GetType(), "CudaImage`2");

         if (baseType != null)
            return baseType.GetGenericArguments()[1];
         else
         {
            baseType = Toolbox.GetBaseType(image.GetType(), "Mat");
            return
               baseType == null ? null :
               CvInvoke.GetDepthType((image as Mat).Depth);
         }
      }


      /// <summary>
      /// Get the color at the specific location of the image
      /// </summary>
      /// <param name="image">The image to obtain pixel value from</param>
      /// <param name="location">The location to sample a pixel</param>
      /// <returns>The color at the specific location</returns>
      public static IColor GetPixelColor(IImage image, Point location)
      {
         Size size = image.Size;
         location.X = Math.Min(location.X, size.Width - 1);
         location.Y = Math.Min(location.Y, size.Height - 1);

         MethodInfo indexers =
            image.GetType()
            .GetMethod("get_Item", new Type[2] { typeof(int), typeof(int) });

         return indexers == null ?
            new Bgra()
            : indexers.Invoke(image, new object[2] { location.Y, location.X }) as IColor;
      }
   }
}
#endif
