//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if (NETFX_CORE || UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE)
using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
#if !(NETFX_CORE || NETSTANDARD1_4)
   [Serializable]
#endif
   [StructLayout(LayoutKind.Sequential)]
   public struct Point
   {
      public int X;
      public int Y;

      public Point(int x, int y)
      {
         X = x;
         Y = y;
      }

      public static Point Empty
      {
         get
         {
            return new Point(0, 0);
         }
      }

      public static Point Round(PointF pointf)
      {
         return new Point((int)Math.Round(pointf.X), (int)Math.Round(pointf.Y));
      }

      public static implicit operator PointF(Point p)
      {
         return new PointF(p.X, p.Y);
      }
   }
}

#endif