//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.OCR;
using Emgu.CV.Structure;

#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using NUnit.Framework;
#endif

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestOcr
   {
      [Test]
      public void TestOCREngGrayText()
      {
         using (Tesseract ocr = GetTesseract())
         using (Image<Gray, Byte> img = new Image<Gray, byte>(480, 200))
         
         {

            ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");
            IntPtr oclDevice = new IntPtr();
            int deviceId = ocr.GetOpenCLDevice(ref oclDevice);

            String message = "Hello, World";
            CvInvoke.PutText(img, message, new Point(50, 100), CvEnum.FontFace.HersheySimplex, 1.0, new MCvScalar(255));

            //
            //ocr.Recognize(img);
            using (Image<Gray, Byte> rotatedImg = img.Rotate(10, new Gray(), false))
            {
              

               ocr.PageSegMode = PageSegMode.AutoOsd;

               ocr.Recognize(rotatedImg);
               using (PageIterator pi = ocr.AnalyseLayout())
               {
                  Orientation or = pi.Orientation;
                  LineSegment2D? baseLine = pi.GetBaseLine(PageIteratorLevel.Textline);
                  if (baseLine.HasValue)
                  {
                     CvInvoke.Line(rotatedImg, baseLine.Value.P1, baseLine.Value.P2, new MCvScalar(255));
                     //Emgu.CV.UI.ImageViewer.Show(rotatedImg);       
                  }
               }


               /*
               String messageOcr = ocr.GetText().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
               EmguAssert.AreEqual(message, messageOcr,
                  String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

               Tesseract.Character[] results = ocr.GetCharacters();*/
            }
         }
      }

      [Test]
      public void TestOCRBgrText()
      {
         using (Tesseract ocr = GetTesseract())
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 200))
         {
            ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");

            String message = "Hello, World";
            CvInvoke.PutText(img, message, new Point(50, 100), CvEnum.FontFace.HersheySimplex, 1.0, new MCvScalar(0, 0, 255));
            //ImageViewer.Show(img);
            ocr.Recognize(img);

            String messageOcr = ocr.GetText().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
            EmguAssert.AreEqual(message, messageOcr, String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

            Tesseract.Character[] results = ocr.GetCharacters();
         }
      }

      [Test]
      public void TestOCREngBlankPage()
      {
         Version version = Tesseract.Version;
         int i = version.Major;
         using (Tesseract ocr = GetTesseract())
         using (Image<Gray, Byte> img = new Image<Gray, byte>(1024, 960))
         {
            ocr.Recognize(img);
            Tesseract.Character[] results = ocr.GetCharacters();
            EmguAssert.IsTrue(results.Length == 0);
         }
      }

      private static Tesseract GetTesseract()
      {
#if __ANDROID__
         Emgu.Util.AndroidFileAsset.OverwriteMethod overwriteMethod = Emgu.Util.AndroidFileAsset.OverwriteMethod.AlwaysOverwrite;
         System.IO.FileInfo a8 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context , "tessdata/eng.traineddata", "tmp", overwriteMethod);
         System.IO.FileInfo a0 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.cube.bigrams", "tmp", overwriteMethod);
         System.IO.FileInfo a1 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.cube.fold", "tmp", overwriteMethod);
         System.IO.FileInfo a2 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.cube.lm", "tmp", overwriteMethod);
         System.IO.FileInfo a3 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.cube.nn", "tmp", overwriteMethod);
         System.IO.FileInfo a4 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.cube.params", "tmp", overwriteMethod);
         System.IO.FileInfo a5 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.cube.size", "tmp", overwriteMethod);
         System.IO.FileInfo a6 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.cube.word-freq", "tmp", overwriteMethod);
         System.IO.FileInfo a7 = Emgu.Util.AndroidFileAsset.WritePermanantFileAsset(AssetsUtil.Context, "tessdata/eng.tesseract_cube.nn", "tmp", overwriteMethod);
         String path = System.IO.Path.Combine(a0.DirectoryName, "..") + System.IO.Path.DirectorySeparatorChar;
         return new Tesseract(path, "eng", OcrEngineMode.TesseractCubeCombined);
#else
         return new Tesseract("./", "eng", OcrEngineMode.TesseractCubeCombined);
#endif
      }

      [Test]
      public void TestOCREngConstructor()
      {
         using (Tesseract ocr = GetTesseract())
         {
         }
      }
   }
}
