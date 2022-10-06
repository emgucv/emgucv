//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
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
        [Test]
        public async Task TestOCREngGrayText()
        {
            using (Tesseract ocr = await GetTesseract())
            using (Mat img = new Mat(new Size(480, 200), DepthType.Cv8U, 1))
            {

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
                                                                           //EmguAssert.AreEqual(message, messageOcr,
                                                                           //   String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

                Tesseract.Character[] results = ocr.GetCharacters();

                String s1 = ocr.GetBoxText();
                //String s2 = ocr.GetOsdText();
                String s3 = ocr.GetTSVText();
                String s4 = ocr.GetUNLVText();

                bool success = true;
                using (PDFRenderer pdfRenderer = new PDFRenderer("abc", Tesseract.DefaultTesseractDirectory, false))
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

        [Test]
        public async Task TestOCRBgrText()
        {
            using (Tesseract ocr = await GetTesseract())
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 200))
            {
                ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz,");

                String message = "Hello, World";
                CvInvoke.PutText(img, message, new Point(50, 100), CvEnum.FontFace.HersheySimplex, 1.0, new MCvScalar(0, 0, 255));
                //ImageViewer.Show(img);
                ocr.SetImage(img);
                ocr.Recognize();

                String messageOcr = ocr.GetUTF8Text().TrimEnd('\n', '\r'); // remove end of line from ocr-ed text
                EmguAssert.AreEqual(message.Replace(" ", ""), messageOcr.Replace(" ", ""), String.Format("'{0}' is not equal to '{1}'", message, messageOcr));

                Tesseract.Character[] results = ocr.GetCharacters();
            }
        }

        [Test]
        public async Task TestOCREngBlankPage()
        {
            Version version = Tesseract.Version;
            int i = version.Major;
            using (Tesseract ocr = await GetTesseract())
            using (Mat img = new Mat(new Size(1024, 960), DepthType.Cv8U, 3))
            {
                ocr.SetImage(img);
                bool success = ocr.Recognize() == 0;
                if (success)
                {
                    Tesseract.Character[] results = ocr.GetCharacters();
                    EmguAssert.IsTrue(results.Length == 0);
                }
            }
        }
        

        private static void TesseractDownloadLangFile(String folder, String lang)
        {
            
            String folderName = folder;
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
            String dest = System.IO.Path.Combine(folderName, String.Format("{0}.traineddata", lang));
            if ((!System.IO.File.Exists(dest)) || (new System.IO.FileInfo(dest).Length == 0))
            {
                String source = Emgu.CV.OCR.Tesseract.GetLangFileUrl(lang);

                using (System.Net.WebClient webclient = new System.Net.WebClient())
                {
                    Console.WriteLine(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
                    webclient.DownloadFile(source, dest);
                    Console.WriteLine(String.Format("Download completed"));
                }
            }
        }

        private static async Task TesseractDownloadPDFFontFile(String folder, String file)
        {

            String folderName = folder;
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
            String dest = System.IO.Path.Combine(folderName, file);
            if ((!System.IO.File.Exists(dest)) || (new System.IO.FileInfo(dest).Length == 0))
            {
                String source = "https://github.com/tesseract-ocr/tessconfigs/blob/3decf1c8252ba6dbeef0bf908f4b0aab7f18d113/pdf.ttf?raw=true";

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    Console.WriteLine(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
                    using (Stream stream = await client.GetStreamAsync(source))
                    using (Stream outStream = File.OpenWrite(dest))
                    {
                        stream.CopyTo(outStream);
                    }

                    Console.WriteLine(String.Format("Download completed"));
                }
            }
        }

        private static async Task<Tesseract> GetTesseract(String lang = "eng")
        {
            TesseractDownloadLangFile(Tesseract.DefaultTesseractDirectory, lang);
            TesseractDownloadLangFile(Tesseract.DefaultTesseractDirectory, "osd"); //script orientation detection
            await TesseractDownloadPDFFontFile(Tesseract.DefaultTesseractDirectory, "pdf.tff");
            return new Tesseract(Tesseract.DefaultTesseractDirectory, lang, OcrEngineMode.TesseractLstmCombined);
        }

        [Test]
        public void TestTesseractUnicodePath()
        {
            String filePath = Path.Combine(Tesseract.DefaultTesseractDirectory, "데이터") + Path.DirectorySeparatorChar;
            TesseractDownloadLangFile(filePath, "eng");
            var rawData = System.IO.File.ReadAllBytes(Path.Combine(filePath, "eng.traineddata"));

            using (Tesseract ocr = new Tesseract())
            {
                ocr.Init(rawData, "eng", OcrEngineMode.TesseractLstmCombined);
            }
        }

        [Test]
        public async Task TestOCREngConstructor()
        {
            using (Tesseract ocr = await GetTesseract())
            {
                int isValid1 = ocr.IsValidWord("1123");
                int isValid2 = ocr.IsValidWord("hello");
            }
        }
    }
}
#endif