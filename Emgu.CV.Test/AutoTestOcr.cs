//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
                    ocr.SetImage(rotatedImg);
                    ocr.Recognize();
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


                    String messageOcr = ocr.GetUTF8Text().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
                    //EmguAssert.AreEqual(message, messageOcr,
                    //   String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

                    Tesseract.Character[] results = ocr.GetCharacters();

                    String s1 = ocr.GetBoxText();
                    //String s2 = ocr.GetOsdText();
                    String s3 = ocr.GetTSVText();
                    String s4 = ocr.GetUNLVText();

                    using (PDFRenderer pdfRenderer = new PDFRenderer("abc.pdf", "./", false))
                    using (Pix imgPix = new Pix(img.Mat))
                    {
                        //bool success = ocr.ProcessPage(imgPix, 1, "img", null, 100000, pdfRenderer);
                        //EmguAssert.IsTrue(success, "failed to export pdf");
                    }


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
                ocr.SetImage(img);
                ocr.Recognize();

                String messageOcr = ocr.GetUTF8Text().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
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
                ocr.SetImage(img);
                ocr.Recognize();
                Tesseract.Character[] results = ocr.GetCharacters();
                EmguAssert.IsTrue(results.Length == 0);
            }
        }

        private static void TesseractDownloadLangFile(String folder, String lang)
        {
            String subfolderName = "tessdata";
            String folderName = System.IO.Path.Combine(folder, subfolderName);
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
            String dest = System.IO.Path.Combine(folderName, String.Format("{0}.traineddata", lang));
            if (!System.IO.File.Exists(dest))
                using (System.Net.WebClient webclient = new System.Net.WebClient())
                {
                    String source =
                        String.Format("https://github.com/tesseract-ocr/tessdata/blob/4592b8d453889181e01982d22328b5846765eaad/{0}.traineddata?raw=true", lang);

                    Console.WriteLine(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
                    webclient.DownloadFile(source, dest);
                    Console.WriteLine(String.Format("Download completed"));
                }
        }

        private static Tesseract GetTesseract(String lang = "eng")
        {
            TesseractDownloadLangFile(".", lang);
            TesseractDownloadLangFile(".", "osd"); //script orientation detection
            return new Tesseract("./", lang, OcrEngineMode.TesseractLstmCombined);
        }

        [Test]
        public void TestOCREngConstructor()
        {
            using (Tesseract ocr = GetTesseract())
            {
                int isValid1 = ocr.IsValidWord("1123");
                int isValid2 = ocr.IsValidWord("hello");
            }
        }
    }
}
