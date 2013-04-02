using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Drawing
{
   public struct PointF
   {
      public float X;
      public float Y;

      public PointF(float x, float y)
      {
         X = x;
         Y = y;
      }

      public static PointF Empty
      {
         get
         {
            return new PointF(0, 0);
         }
      }
   }
}
