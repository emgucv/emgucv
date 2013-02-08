//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvMat
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvHistogram
   {
      /// <summary>
      /// The type of histogram
      /// </summary>
      public int type;

      /// <summary>
      /// Pointer to CvArr
      /// </summary>
      public IntPtr bins;

      /// <summary>
      /// For uniform histograms 
      /// </summary>
      public RangeF thresh0;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh1;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh2;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh3;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh4;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh5;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh6;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh7;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh8;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh9;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh10;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh11;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh12;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh13;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh14;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh15;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh16;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh17;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh18;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh19;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh20;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh21;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh22;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh23;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh24;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh25;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh26;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh27;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh28;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh29;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh30;
      /// <summary>
      /// For uniform histograms
      /// </summary>
      public RangeF thresh31;

      /// <summary>
      /// Get the thresh value as an array
      /// </summary>
      public RangeF[] thresh
      {
         get
         {
            return new RangeF[] { thresh0,thresh1,thresh2,thresh3,thresh4,thresh5,thresh6,thresh7,thresh8,thresh9,thresh10,thresh11,thresh12,thresh13,thresh14,thresh15,thresh16,thresh17,thresh18,thresh19,thresh20,thresh21,thresh22,thresh23,thresh24,thresh25,thresh26,thresh27,thresh28,thresh29,thresh30,thresh31};
         }
      }

      /// <summary>
      /// For non-uniform histograms
      /// </summary>
      public IntPtr threshPtr;

      /// <summary>
      /// Embedded matrix header for array histograms
      /// </summary>
      public MCvMatND mat;
   }
}
