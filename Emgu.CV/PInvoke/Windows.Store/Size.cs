//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------


namespace System.Drawing
{
   public struct Size
   {
      public int Width;
      public int Height;

      public Size(int width, int height)
      {
         Width = width;
         Height = height;
      }

      public static Size Empty
      {
         get
         {
            return new Size(0, 0);
         }
      }
   }
}
