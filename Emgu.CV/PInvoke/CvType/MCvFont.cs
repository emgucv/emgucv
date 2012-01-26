//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvFont
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvFont
   {
      /// <summary>
      /// For QT
      /// </summary>
      public IntPtr fontName;
      /// <summary>
      /// For QT
      /// </summary>
      public MCvScalar color;
      
      ///<summary>
      /// Font type
      ///</summary>
      public CvEnum.FONT font_face;
      ///<summary>
      /// font data and metrics 
      ///</summary>
      public IntPtr ascii;
      /// <summary>
      /// 
      /// </summary>
      public IntPtr greek;
      /// <summary>
      /// 
      /// </summary>
      public IntPtr cyrillic;
      /// <summary>
      /// hscale
      /// </summary>
      public float hscale;
      /// <summary>
      /// vscale
      /// </summary>
      public float vscale;
      ///<summary>
      /// slope coefficient: 0 - normal, >0 - italic 
      ///</summary>
      public float shear;
      ///<summary>
      /// letters thickness 
      ///</summary>
      public int thickness;
      ///<summary>
      /// horizontal interval between letters 
      ///</summary>
      public float dx;
      /// <summary>
      /// type of line
      /// </summary>
      public int line_type;

      /// <summary>
      /// Create a Font of the specific type, horizontal scale and vertical scale
      /// </summary>
      /// <param name="type">The type of the font</param>
      /// <param name="hscale">The horizontal scale of the font</param>
      /// <param name="vscale">the vertical scale of the fonr</param>
      public MCvFont(CvEnum.FONT type, double hscale, double vscale)
         : this()
      {
         CvInvoke.cvInitFont(ref this, type, hscale, vscale, 0, 1, CvEnum.LINE_TYPE.EIGHT_CONNECTED);
      }

      /// <summary>
      /// Calculates the binding rectangle for the given text string when the font is used
      /// </summary>
      /// <param name="text">Input string</param>
      /// <param name="baseline">y-coordinate of the baseline relatively to the bottom-most text point</param>
      /// <returns>size of the text string. Height of the text does not include the height of character parts that are below the baseline</returns>
      public System.Drawing.Size GetTextSize(string text, int baseline)
      {
         System.Drawing.Size size = new System.Drawing.Size();
         CvInvoke.cvGetTextSize(text, ref this, ref size, ref baseline);
         return size;
      }
   }
}
