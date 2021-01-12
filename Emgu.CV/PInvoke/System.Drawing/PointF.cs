//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if UNITY_WSA
using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
   [Serializable]
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