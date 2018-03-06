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

#endif