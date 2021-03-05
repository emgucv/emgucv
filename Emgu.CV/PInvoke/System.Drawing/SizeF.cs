//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if UNITY_WSA || (UNITY_STANDALONE_WIN && NET_4_6)

using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
   [Serializable]
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