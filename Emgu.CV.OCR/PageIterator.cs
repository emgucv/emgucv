//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !NETFX_CORE

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

      protected override void DisposeObject()
      {
         OcrInvoke.TessPageIteratorRelease(ref _ptr);
      }
   }

   public struct Orientation
   {
      public PageOrientation PageOrientation;
      public WritingDirection WritingDirection;
      public TextlineOrder TextlineOrder;
      public float DeskewAngle;
   }

   public enum PageOrientation
   {
      Up = 0,
      Right = 1,
      Down = 2,
      Left = 3,
   }

   public enum WritingDirection
   {
      LeftToRight = 0,
      RightToLeft = 1,
      TopToBottom = 2,
   }

   public enum TextlineOrder
   {
      LeftToRight = 0,
      RightToLeft = 1,
      TopToBottom = 2,

   }

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
#endif