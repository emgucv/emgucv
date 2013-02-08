//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using NUnit.Framework;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestOcr
   {
      [Test]
      public void TestOCREngGrayText()
      {
         using (Tesseract ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED))
         using (Image<Gray, Byte> img = new Image<Gray, byte>(480, 200))
         {
            ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");
            MCvFont font = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
            String message = "Hello, World";
            img.Draw(message, ref font, new Point(50, 100), new Gray(255.0));
            //ImageViewer.Show(img);
            ocr.Recognize(img);

            String messageOcr = ocr.GetText().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
            EmguAssert.AreEqual(message, messageOcr, String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

            Tesseract.Charactor[] results = ocr.GetCharactors();
         }
      }

      [Test]
      public void TestOCRBgrText()
      {
         using (Tesseract ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED))
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 200))
         {
            ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");
            MCvFont font = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
            String message = "Hello, World";
            img.Draw(message, ref font, new Point(50, 100), new Bgr(Color.Pink));
            //ImageViewer.Show(img);
            ocr.Recognize(img);

            String messageOcr = ocr.GetText().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
            EmguAssert.AreEqual(message, messageOcr, String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

            Tesseract.Charactor[] results = ocr.GetCharactors();
         }
      }

      [Test]
      public void TestOCREngBlankPage()
      {
         Version version = Tesseract.Version;
         int i = version.Major;
         using (Tesseract ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED))
         using (Image<Gray, Byte> img = new Image<Gray, byte>(1024, 960))
         {
            ocr.Recognize(img);
            Tesseract.Charactor[] results = ocr.GetCharactors();
            EmguAssert.IsTrue(results.Length == 0);
         }
      }

      [Test]
      public void TestOCREngConstructor()
      {
         using (Tesseract ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED))
         {
         }
      }
   }
}
