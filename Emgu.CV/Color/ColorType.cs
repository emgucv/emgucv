using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   ///<summary>
   /// A color type
   ///</summary>
   public abstract class ColorType : Point<double>
   {
      /// <summary>
      /// Create a color type of certain dimension
      /// </summary>
      /// <param name="dimension"></param>
      protected ColorType(int dimension) : base(dimension) { }

      /// <summary>
      /// Get the names for each channel
      /// </summary>
      public virtual String[] ChannelName
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
      /// Get the display color for each channel
      /// </summary>
      public virtual System.Drawing.Color[] ChannelColor
      {
         get
         {
            List<System.Drawing.Color> channelColor = new List<System.Drawing.Color>();
            foreach (System.Reflection.PropertyInfo pInfo in GetType().GetProperties())
            {
               Object[] displayAtts = pInfo.GetCustomAttributes(typeof(DisplayColorAttribute), true);
               if (displayAtts.Length > 0)
               {
                  channelColor.Add( ((DisplayColorAttribute) displayAtts[0]).DisplayColor );
               }
            }
            if (channelColor.Count > 0) return channelColor.ToArray();

            System.Drawing.Color[] res = new System.Drawing.Color[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
               res[i] = System.Drawing.Color.Gray;
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
            double[] v = Resize(4).Coordinate;
            return new MCvScalar(v[0], v[1], v[2], v[3]);
         }
         set
         {
            int size = Math.Min(Dimension, 4);
            for (int i = 0; i < size; i++)
               _coordinate[i] = value.v[i];
         }
      }

      /// <summary>
      /// implicit operator to MCvScalar
      /// </summary>
      /// <param name="point">The color</param>
      /// <returns>MCvScalar</returns>
      public static implicit operator MCvScalar(ColorType point)
      {
         return point.MCvScalar;
      }
   }
}
