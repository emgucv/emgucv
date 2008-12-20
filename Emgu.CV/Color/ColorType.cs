using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary>
   /// A color type
   ///</summary>
   public abstract class ColorType
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      protected MCvScalar _scalar;

      /// <summary>
      /// Create a color type of default intensity
      /// </summary>
      protected ColorType()
      {
      }

      /// <summary>
      /// Create a ColorType of the specific intensity
      /// </summary>
      /// <param name="scalar">The intensity of the color</param>
      protected ColorType(MCvScalar scalar)
      {
         _scalar = scalar;
      }

      /// <summary>
      /// Get the names for each channel
      /// </summary>
      public virtual String[] ChannelNames
      {
         get
         {
            List<String> channelNames = new List<string>();
            foreach (System.Reflection.PropertyInfo pInfo in GetType().GetProperties())
            {
               Object[] displayAtts = pInfo.GetCustomAttributes(typeof(DisplayColorAttribute), true);
               if (displayAtts.Length > 0)
               {
                  channelNames.Add(pInfo.Name);
               }
            }
            if (channelNames.Count > 0) return channelNames.ToArray();

            String[] res = new string[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
               res[i] = "Channel " + i;
            }
            return res;
         }
      }

      /// <summary>
      /// The equivalent of MCvScalar value
      /// </summary>
      public MCvScalar MCvScalar
      {
         get
         {
            return _scalar;
         }
         set
         {
            _scalar = value;
         }
      }

      /// <summary>
      /// Get the dimension of the color type
      /// </summary>
      public abstract int Dimension
      {
         get;
      }

      /// <summary>
      /// implicit operator to MCvScalar
      /// </summary>
      /// <param name="color">The color</param>
      /// <returns>MCvScalar</returns>
      public static implicit operator MCvScalar(ColorType color)
      {
         return color.MCvScalar;
      }

      /// <summary>
      /// Return the color intensity as a string
      /// </summary>
      /// <returns>the color intensity as a string</returns>
      public override string ToString()
      {
         double[] intensity = _scalar.ToArray();
         return String.Join(",", Array.ConvertAll<double, String>(intensity, System.Convert.ToString));
      }
   }
}
