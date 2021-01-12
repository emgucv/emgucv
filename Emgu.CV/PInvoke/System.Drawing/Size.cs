//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if (UNITY_WSA)
using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
   [Serializable]
   [StructLayout(LayoutKind.Sequential)]
   public struct Size : IEquatable<Size>
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

      public bool Equals(Size other)
      {
         return (Width == other.Width) && (Height == other.Height); 
      }

      public static bool operator ==(Size s1, Size s2)
      {
         return s1.Equals(s2);
      }

      public static bool operator !=(Size s1, Size s2)
      {
         return !s1.Equals(s2);
      }

      public override bool Equals(object obj)
      {
         Size? s2 = obj as Size?;
         if (!s2.HasValue)
            return false;
         return Equals(s2.Value);
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }
   }
}

#endif