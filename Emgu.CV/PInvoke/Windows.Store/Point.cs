//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------


namespace System.Drawing
{
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
