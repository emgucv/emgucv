//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// A solid resembling a cube, with the rectangular faces not all equal; a rectangular parallelepiped.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Cuboid
   {
      /// <summary>
      /// The coordinate of the upper corner
      /// </summary>
      public MCvPoint3D64f Min;

      /// <summary>
      /// The coordinate of the lower corner
      /// </summary>
      public MCvPoint3D64f Max;

      /// <summary>
      /// Check if the specific point is in the Cuboid
      /// </summary>
      /// <param name="point">The point to be checked</param>
      /// <returns>True if the point is in the cuboid</returns>
      public bool Contains(MCvPoint3D64f point)
      {
         return point.X >= Min.X && point.Y >= Min.Y && point.Z >= Min.Z
            && point.X <= Max.X && point.Y <= Max.Y && point.Z <= Max.Z;
      }

      /// <summary>
      /// Get the centroid of this cuboid
      /// </summary>
      public MCvPoint3D64f Centroid
      {
         get
         {
            return (Min + Max) * 0.5;
         }
      }
   }
}
