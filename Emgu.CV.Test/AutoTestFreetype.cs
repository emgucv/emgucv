//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Freetype;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
    public class AutoTestFreetype
    {
#if !(__ANDROID__ || __IOS__ || NETFX_CORE)
        [Test]
        public void TestFreetype()
        {
            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveFreetype = (openCVConfigDict["HAVE_OPENCV_FREETYPE"] != 0);
            if (haveFreetype)
            {
                using (Mat m = new Mat(new Size(640, 480), DepthType.Cv8U, 3))
                using (Freetype.Freetype2 freetype = new Freetype2())
                {
                    m.SetTo(new MCvScalar(0, 0, 0, 0));
                    freetype.LoadFontData("NotoSansCJK-Regular.ttc", 0);

                    freetype.PutText(m, "测试", new Point(100, 100), 36, new MCvScalar(255, 255, 0), 1,
                        LineType.EightConnected, false);
                    //CvInvoke.NamedWindow("test");
                    //CvInvoke.Imshow("test", m);
                    //CvInvoke.WaitKey();
                }
            }
        }

        [Test]
        public void TestGenerateLogo()
        {
            String productName = "CV";
            Mat logo = GenerateLogo(800, 800, productName);
            logo.Save(String.Format("Emgu{0}Logo.png", productName == null ? String.Empty : productName));
        }

        /*
        public void GenerateLogo(String productName = null)
        {
            Image<Bgra, Byte> logo = GenerateLogo(860, 389, productName);
            logo.Save(String.Format("Emgu{0}Logo.png", productName == null ? String.Empty : productName));
        }*/

        public Mat GenerateLogo(int width, int height = -1, String productName = null)
        {
            int heightShift = 0;
            int textHeight = (int)(width / 160.0 * 72.0);
            if (height <= 0)
                height = textHeight;
            else
            {
                heightShift = Math.Max((height - textHeight) / 2, 0);
            }
            double scale = width / 160.0;
            Mat semgu = new Mat(width, height, DepthType.Cv8U, 3);
            Mat scv = new Mat(width, height, DepthType.Cv8U, 3);
            semgu.SetTo(new MCvScalar(0,0,0));
            scv.SetTo(new MCvScalar(0,0,0));
            //MCvFont f1 = new MCvFont(CvEnum.HersheyFonts.Triplex, 1.5 * scale, 1.5 * scale);
            //MCvFont f2 = new MCvFont(CvEnum.HersheyFonts.Complex, 1.6 * scale, 2.2 * scale);
            CvInvoke.PutText(
                semgu, 
                "Emgu", 
                Point.Round(new PointF((float)(6 * scale), (float)(50 * scale + heightShift))), 
                CvEnum.HersheyFonts.Triplex, 
                1.5 * scale, 
                new MCvScalar(55, 155, 255),
                (int)Math.Round(1.5 * scale));
            CvInvoke.Dilate(semgu, semgu, null, new Point(-1, -1), (int) (1 * scale), BorderType.Default, new MCvScalar(0,0,0));
            //semgu._Dilate((int)(1 * scale));
            if (productName != null)
            {
                CvInvoke.PutText(
                    scv, 
                    productName, 
                    Point.Round(new PointF((float) (50 * scale), (float) (60 * scale + heightShift))),
                    CvEnum.HersheyFonts.Simplex, 
                    1.6 * scale, 
                    new MCvScalar(255, 55, 255),
                    (int) Math.Round(2.2 * scale));
                CvInvoke.Dilate(scv, scv, null, new Point(-1, -1), (int)(2 * scale), BorderType.Default, new MCvScalar());
            }

            using (Mat logoBgr = new Mat())
            //using (Mat logoA = new Mat(semgu.Size, DepthType.Cv8U, 1))
            using (Mat logoMask = new Mat())
            {
                CvInvoke.BitwiseOr(semgu, scv, logoBgr);
                CvInvoke.CvtColor(logoBgr, logoMask, ColorConversion.Bgra2Gray);
                
                Mat logoBgra = new Mat();
                CvInvoke.CvtColor(logoBgr, logoBgra, ColorConversion.Bgr2Bgra);
                CvInvoke.BitwiseNot(logoMask, logoMask);

                logoBgra.SetTo(new MCvScalar(0.0, 0.0, 0.0, 0.0), logoMask);
                
                return logoBgra;
                /*
                Image<Bgr, Byte> bg_header = new Image<Bgr, byte>(1, 92);
                for (int i = 0; i < 92; i++)
                   bg_header[i, 0] = new Bgr(210, 210 - i * 0.4, 210 - i * 0.9);
                bg_header.Save("bg_header.gif");*/
            }
        }

        public void CreateUnityIcons(String productName = null)
        {
            //128x128
            Mat imgSmall = GenerateLogo(128, 128, productName);

            //200x258
            Mat imgMediumTop = GenerateLogo(200, 120, productName);
            Mat imgMediumBottom = new Mat(200, 138, DepthType.Cv8U, 4);
            imgMediumBottom.SetTo(new MCvScalar(0,0,0,255));
            Mat imgMedium = new Mat();
            CvInvoke.VConcat(imgMediumTop, imgMediumBottom, imgMedium);

            //Image<Bgra, Byte> imgMedium = .ConcateVertical(new Image<Bgra, byte>(200, 138));

            //860x389
            int screenShotWidth = 400;
            int rightPadding = 40;
            Mat unity_screenshot = EmguAssert.LoadMat("unity_screenshot.png", ImreadModes.ColorBgr);
            Mat resized_unity_screenshot = new Mat();
            CvInvoke.Resize(unity_screenshot, resized_unity_screenshot, new Size(screenShotWidth, 209 ));
            Mat screenShot = new Mat();
            
            if (screenShot.Width < screenShotWidth)
            {
                using (Mat blank = new Mat(
                           (screenShotWidth - screenShot.Width) / 2, 
                           screenShot.Height, 
                           DepthType.Cv8U,
                           4))
                using (Mat tmp = new Mat())
                {
                    blank.SetTo(new MCvScalar(0, 0, 0, 255));
                    
                    CvInvoke.CvtColor(resized_unity_screenshot, tmp, ColorConversion.Bgr2Bgra);
                    CvInvoke.HConcat(blank, tmp, screenShot);
                }
            }
            else
            {
                CvInvoke.CvtColor(resized_unity_screenshot, screenShot, ColorConversion.Bgr2Bgra);
            }

            Mat blankLarge = new Mat(860 - (screenShotWidth + rightPadding), 389, DepthType.Cv8U, 4);
            blankLarge.SetTo(new MCvScalar(255, 255, 255, 0));
            Mat logoLarge = GenerateLogo(screenShotWidth, 389 - screenShot.Height);
            Mat paddedVScreenShot = new Mat();
            CvInvoke.VConcat(logoLarge, screenShot, paddedVScreenShot);
            Mat paddedHVScreenShot = new Mat();
            CvInvoke.HConcat(blankLarge, paddedVScreenShot, paddedHVScreenShot);
            Mat rightPaddingMat = new Mat(rightPadding, 389, DepthType.Cv8U, 4);
            rightPaddingMat.SetTo(new MCvScalar(255, 255, 255, 0));
            Mat imgLarge = new Mat();
            CvInvoke.HConcat(paddedHVScreenShot, rightPaddingMat, imgLarge);

            imgSmall.Save(String.Format("Emgu{0}Logo_128x128.png", productName == null ? String.Empty : productName));
            imgMedium.Save(String.Format("Emgu{0}Logo_200x258.png", productName == null ? String.Empty : productName));
            imgLarge.Save(String.Format("Emgu{0}Logo_860x389.png", productName == null ? String.Empty : productName));


            //Image<Bgra, Byte> result = imgSmall.ConcateVertical(imgMedium).ConcateVertical(imgLarge);
            //result.Draw(new LineSegment2D(new Point(0, imgSmall.Height), new Point(result.Width, imgSmall.Height) ), new Bgra(0, 0, 0, 255), 1  );
            //result.Draw(new LineSegment2D(new Point(0, imgSmall.Height + imgMedium.Height), new Point(result.Width, imgSmall.Height + imgMedium.Height)), new Bgra(0, 0, 0, 255), 1);
            //ImageViewer.Show(result);
        }
#endif
    }
}
