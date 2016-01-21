//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if (NETFX_CORE || UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN) 

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