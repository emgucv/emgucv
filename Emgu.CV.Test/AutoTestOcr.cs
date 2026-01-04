//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Orientation = Emgu.CV.OCR.Orientation;
//using static System.Net.WebRequestMethods;


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

#if !NETFX_CORE
namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestOcr
    {
#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestOCREngGrayText()
        {
            String tesseractVersion = Emgu.CV.OCR.Tesseract.VersionString;
            using (TesseractModel tm = new TesseractModel())
            using (Mat img = new Mat(new Size(480, 200), DepthType.Cv8U, 1))
            {
                await tm.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                Tesseract ocr = tm.Model;
                ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");
                IntPtr oclDevice = new IntPtr();
                int deviceId = ocr.GetOpenCLDevice(ref oclDevice);

                img.SetTo(new MCvScalar(0, 0, 0));
                String message = "Hello, World";
                CvInvoke.PutText(img, message, new Point(50, 100), CvEnum.FontFace.HersheySimplex, 1.0, new MCvScalar(255));

                ocr.PageSegMode = PageSegMode.AutoOsd;
                ocr.SetImage(img);
                ocr.Recognize();
                using (PageIterator pi = ocr.AnalyseLayout())
                {
                    Orientation or = pi.Orientation;
                    LineSegment2D? baseLine = pi.GetBaseLine(PageIteratorLevel.Textline);
                    //if (baseLine.HasValue)
                    //{
                    //CvInvoke.Line(img, baseLine.Value.P1, baseLine.Value.P2, new MCvScalar(255));
                    //}
                }


                String messageOcr = ocr.GetUTF8Text().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
                EmguAssert.AreEqual(message.Replace(" ", String.Empty), messageOcr.Replace(" ", String.Empty),
                   String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

                Tesseract.Word[] results = ocr.GetWords();

                String s1 = ocr.GetBoxText();
                //String s2 = ocr.GetOsdText();
                String s3 = ocr.GetTSVText();
                String s4 = ocr.GetUNLVText();

                bool success = true;
                using (PDFRenderer pdfRenderer = new PDFRenderer("abc", tm.TessDataDirectory, false))
                {
                    success &= pdfRenderer.BeginDocument("testing");
                    success &= pdfRenderer.AddImage(ocr);
                    success &= pdfRenderer.EndDocument();
                    EmguAssert.IsTrue(success, "failed to export pdf");
                }
                if (success)
                {
                    FileInfo fi1 = new FileInfo("abc.pdf");
                }

            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestOCRBgrText()
        {
            using (TesseractModel ocr = new TesseractModel())
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 200))
            {
                await ocr.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                Tesseract tesseract = ocr.Model;
                tesseract.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");

                String message = "Hello, World";
                CvInvoke.PutText(img, message, new Point(50, 100), CvEnum.FontFace.HersheySimplex, 1.0, new MCvScalar(0, 0, 255));
                //ImageViewer.Show(img);
                tesseract.SetImage(img);
                tesseract.Recognize();

                String messageOcr = tesseract.GetUTF8Text().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
                EmguAssert.AreEqual(message.Replace(" ", ""), messageOcr.Replace(" ", ""), String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

                Tesseract.Word[] results = tesseract.GetWords();
            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestOCREngBlankPage()
        {
            Version version = Tesseract.Version;
            int i = version.Major;
            using (TesseractModel tm = new TesseractModel())
            using (Mat img = new Mat(new Size(1024, 960), DepthType.Cv8U, 3))
            {
                await tm.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                Tesseract ocr = tm.Model;
                img.SetTo(new MCvScalar()); //Set to a black image
                ocr.SetImage(img);
                bool success = ocr.Recognize() == 0;
                if (success)
                {
                    Tesseract.Word[] results = ocr.GetWords();
                    EmguAssert.IsTrue(results.Length == 0);
                }
            }
        }


#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestTesseractUnicodePath()
        {
            using (TesseractModel tm = new TesseractModel(modelFolderName: Path.Combine("emgu", "데이터")))
            {
                await tm.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                Tesseract ocr = tm.Model;
                
            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestOCREngConstructor()
        {
            using (TesseractModel tm = new TesseractModel())
            {
                await tm.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                Tesseract ocr = tm.Model;
                int isValid1 = ocr.IsValidWord("1123");
                int isValid2 = ocr.IsValidWord("hello");
            }
        }
    }
}
#endif