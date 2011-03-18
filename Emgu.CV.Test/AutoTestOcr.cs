using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using NUnit.Framework;

namespace Emgu.CV.OCR.UnitTest
{
   [TestFixture]
   public class AutoTestOcr
   {
      [Test]
      public void TestOCREngText()
      {
         using (Tesseract ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED))
         using (Image<Gray, Byte> img = new Image<Gray, byte>(480, 200))
         {
            ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");
            MCvFont font = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_SIMPLEX, 1.0, 1.0);
            String message = "Hello, World";
            img.Draw(message, ref font, new Point(50, 100), new Gray(255.0));
            //ImageViewer.Show(img);
            ocr.SetImage(img);

            String messageOcr = ocr.GetText().TrimEnd('\n'); // remove end of line from ocr-ed text
            Assert.AreEqual(message, messageOcr);

            Tesseract.Word[] results = ocr.ExtractResults();
         }
      }

      [Test]
      public void TestOCREngBlankPage()
      {
         using (Tesseract ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED))
         using (Image<Gray, Byte> img = new Image<Gray, byte>(1024, 960))
         {
            ocr.SetImage(img);
            Tesseract.Word[] results = ocr.ExtractResults();
            Assert.AreEqual(results.Length, 0);
         }
      }
   }
}
