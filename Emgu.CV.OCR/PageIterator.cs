//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.OCR
{
   /// <summary>
   /// The tesseract page iterator
   /// </summary>
   public class PageIterator : UnmanagedObject
   {

      internal PageIterator(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Returns orientation for the block the iterator points to. 
      /// </summary>
      public Orientation Orientation
      {
         get
         {
            Orientation o = new Orientation();
            PageOrientation po = PageOrientation.Up;
            WritingDirection wd = WritingDirection.LeftToRight;
            TextlineOrder tp = TextlineOrder.TopToBottom;
            float deskewAngle = 0;
            OcrInvoke.TessPageIteratorGetOrientation(_ptr, ref po, ref wd, ref tp, ref deskewAngle);
            o.PageOrientation = po;
            o.WritingDirection = wd;
            o.TextlineOrder = tp;
            o.DeskewAngle = deskewAngle;
            return o;
         }
      }

      /// <summary>
      /// Returns the baseline of the current object at the given level. The baseline is the line that passes through (x1, y1) and (x2, y2). WARNING: with vertical text, baselines may be vertical! Returns null if there is no baseline at the current position.
      /// </summary>
      /// <param name="level">Page iterator level</param>
      /// <returns>The baseline of the current object at the given level</returns>
      public LineSegment2D? GetBaseLine(PageIteratorLevel level)
      {
         int x1 = 0;
         int y1 = 0;
         int x2 = 0;
         int y2 = 0;

         bool found = OcrInvoke.TessPageIteratorGetBaseLine(_ptr, level, ref x1, ref y1, ref x2, ref y2);
         if (!found)
            return null;
         else
            return new LineSegment2D(new Point(x1, y1), new Point(x2, y2) );
      }

      /// <summary>
      /// Release the page iterator
      /// </summary>
      protected override void DisposeObject()
      {
         OcrInvoke.TessPageIteratorRelease(ref _ptr);
      }
   }

   /// <summary>
   /// The orientation
   /// </summary>
   public struct Orientation
   {
      /// <summary>
      /// Page orientation
      /// </summary>
      public PageOrientation PageOrientation;
      /// <summary>
      /// Writing direction
      /// </summary>
      public WritingDirection WritingDirection;
      /// <summary>
      /// Textline order
      /// </summary>
      public TextlineOrder TextlineOrder;

      /// <summary>
      /// after rotating the block so the text orientation is upright, how many radians does one have to rotate the block anti-clockwise for it to be level? -Pi/4 &lt;= deskew_angle &lt;= Pi/4
      /// </summary>
      public float DeskewAngle;
   }

   /// <summary>
   /// Page orientation
   /// </summary>
   public enum PageOrientation
   {
      /// <summary>
      /// Up
      /// </summary>
      Up = 0,
      /// <summary>
      /// Right
      /// </summary>
      Right = 1,
      /// <summary>
      /// Down
      /// </summary>
      Down = 2,
      /// <summary>
      /// Left
      /// </summary>
      Left = 3,
   }

   /// <summary>
   /// Writing direction
   /// </summary>
   public enum WritingDirection
   {
      /// <summary>
      /// Left to right
      /// </summary>
      LeftToRight = 0,
      /// <summary>
      /// Right to left
      /// </summary>
      RightToLeft = 1,
      /// <summary>
      /// Top to bottom
      /// </summary>
      TopToBottom = 2,
   }

   /// <summary>
   /// Textline order
   /// </summary>
   public enum TextlineOrder
   {
      /// <summary>
      /// Left to right
      /// </summary>
      LeftToRight = 0,
      /// <summary>
      /// Right to left
      /// </summary>
      RightToLeft = 1,
      /// <summary>
      /// Top to bottom
      /// </summary>
      TopToBottom = 2,
   }

   /// <summary>
   /// Page iterator level
   /// </summary>
   public enum PageIteratorLevel
   {
      /// <summary>
      /// Block of text/image/separator line.
      /// </summary>
      Block,
      /// <summary>
      /// Paragraph within a block.
      /// </summary>
      Para,
      /// <summary>
      /// Line within a paragraph.
      /// </summary>
      Textline,
      /// <summary>
      /// Word within a textline.
      /// </summary>
      Word,
      /// <summary>
      /// Symbol/character within a word.
      /// </summary>
      Symbol
   }
}
