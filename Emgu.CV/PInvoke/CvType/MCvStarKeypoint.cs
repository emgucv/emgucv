/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// CvStarKeypoint
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvStarKeypoint
   {
      /// <summary>
      /// The location of the keypoint
      /// </summary>
      public Point pt;
      /// <summary>
      /// The size of the key point
      /// </summary>
      public int size;
      /// <summary>
      /// The response of the key point
      /// </summary>
      public float response;
   }
}
*/