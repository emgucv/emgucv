//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
