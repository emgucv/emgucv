//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.LineDescriptor
{
   /// <summary>
   /// A class to represent a line.
   /// </summary>
   public struct MKeyLine
   {
      /// <summary>
      /// Orientation of the line
      /// </summary>
      public float Angle;

      /// <summary>
      /// Object ID, that can be used to cluster keylines by the line they represent
      /// </summary>
      public int ClassId;

      /// <summary>
      /// Octave (pyramid layer), from which the keyline has been extracted 
      /// </summary>
      public int Octave;

      /// <summary>
      /// Coordinates of the middlepoint
      /// </summary>
      public PointF Pt;

      /// <summary>
      /// The response, by which the strongest keylines have been selected.
      /// It's represented by the ratio between line's length and maximum between
      /// image's width and height 
      /// </summary>
      public float Response;

      /// <summary>
      /// Minimum area containing line
      /// </summary>
      public float Size;

      /// <summary>
      /// Lines's extremes in original image 
      /// </summary>
      public float StartPointX;
      /// <summary>
      /// Lines's extremes in original image 
      /// </summary>
      public float StartPointY;
      /// <summary>
      /// Lines's extremes in original image 
      /// </summary>
      public float EndPointX;
      /// <summary>
      /// Lines's extremes in original image 
      /// </summary>
      public float EndPointY;

      /// <summary>
      /// Line's extremes in image it was extracted from
      /// </summary>
      public float SPointInOctaveX;
      /// <summary>
      /// Line's extremes in image it was extracted from
      /// </summary>
      public float SPointInOctaveY;
      /// <summary>
      /// Line's extremes in image it was extracted from
      /// </summary>
      public float EPointInOctaveX;
      /// <summary>
      /// Line's extremes in image it was extracted from
      /// </summary>
      public float EPointInOctaveY;

      /// <summary>
      /// The length of line
      /// </summary>
      public float LineLength;

      /// <summary>
      /// Number of pixels covered by the line
      /// </summary>
      public int NumOfPixels;

   }
}
