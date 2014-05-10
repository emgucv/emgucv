//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Hu invariants
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvHuMoments
   {
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double Hu1;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double Hu2;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double Hu3;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double Hu4;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double Hu5;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double Hu6;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double Hu7;
   }
}
