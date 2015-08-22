//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if NETFX_CORE

namespace System.Drawing
{
   public struct SizeF
   {
      public float Width;
      public float Height;

      public SizeF(float width, float height)
      {
         Width = width;
         Height = height;
      }
   }
}

#endif