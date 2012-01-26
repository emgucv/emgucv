//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
      public double hu1;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double hu2;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double hu3;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double hu4;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double hu5;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double hu6;
      /// <summary>
      /// Hu invariants
      /// </summary>
      public double hu7;
   }
}
