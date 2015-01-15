//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


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
