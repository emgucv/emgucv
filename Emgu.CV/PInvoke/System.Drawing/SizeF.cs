//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if (UNITY_WSA)
using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
#if !(NETFX_CORE || NETSTANDARD1_4)
   [Serializable]
#endif
   [StructLayout(LayoutKind.Sequential)]
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