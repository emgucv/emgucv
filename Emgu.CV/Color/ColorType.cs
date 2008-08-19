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
