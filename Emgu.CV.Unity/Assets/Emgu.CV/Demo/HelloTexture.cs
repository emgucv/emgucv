//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System.IO;
using Emgu.CV.CvEnum;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Drawing;
using System.Collections;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using Emgu.CV.Ocl;

namespace Emgu.CV.Demo
{
    public class HelloTexture : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            // Tall enough for the header/OpenCL lines plus all 5 SelfTest lines
            // drawn up to y=260 in SelfTest() below, with margin for descenders.
            Mat img = new Mat(new Size(640, 300), DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar());
            String openclStr = "None";
            if (CvInvoke.HaveOpenCL)
            {
                //StringBuilder builder = new StringBuilder();
                using (VectorOfOclPlatformInfo oclPlatformInfos = OclInvoke.GetPlatformsInfo())
                {
                    if (oclPlatformInfos.Size > 0)
                    {
                        PlatformInfo platformInfo = oclPlatformInfos[0];
                        openclStr = platformInfo.ToString();
                    }
                }
            }

            CvInvoke.PutText(img, String.Format("Emgu CV for Unity {0}", Emgu.Util.Platform.OperationSystem),
                new System.Drawing.Point(10, 60), Emgu.CV.CvEnum.HersheyFonts.Duplex,
                1.0, new MCvScalar(0, 255, 0));

            CvInvoke.PutText(img, String.Format("OpenCL: {0}", openclStr), new System.Drawing.Point(10, 120),
                Emgu.CV.CvEnum.HersheyFonts.Duplex,
                1.0, new MCvScalar(0, 0, 255));

            SelfTest(img);

            Texture2D texture = img.ToTexture2D();

            RenderTexture(texture);
            ResizeTexture(texture);
        }

        /// <summary>
        /// Exercises a broader slice of the Emgu CV API surface than the plain
        /// PutText/ToTexture2D path above -- imgproc, imgcodecs (JPEG), core
        /// Features, core Objdetect, and one Contrib module (XImgproc) -- so a
        /// native rebuild regression shows up as a visible FAIL line rather
        /// than only failing silently deep in an unrelated feature. Each check
        /// is independent and wrapped so one failure doesn't hide the rest.
        /// </summary>
        private void SelfTest(Mat img)
        {
            int y = 160;
            foreach (var check in new Func<string>[]
            {
                SelfTestImgprocRoundTrip,
                SelfTestJpegRoundTrip,
                SelfTestOrbFeatureDetection,
                SelfTestQrCodeDetector,
                SelfTestContribThinning,
            })
            {
                string result = check();
                Debug.Log("EmguCV_SelfTest:" + result);
                CvInvoke.PutText(img, result, new System.Drawing.Point(10, y),
                    Emgu.CV.CvEnum.HersheyFonts.Duplex, 0.6,
                    result.Contains("FAIL") ? new MCvScalar(0, 0, 255) : new MCvScalar(0, 255, 0));
                y += 25;
            }
        }

        private string SelfTestImgprocRoundTrip()
        {
            try
            {
                using (Mat gray = new Mat())
                using (Mat edges = new Mat())
                using (Mat src = new Mat(64, 64, DepthType.Cv8U, 3))
                {
                    src.SetTo(new MCvScalar(50, 100, 150));
                    CvInvoke.CvtColor(src, gray, ColorConversion.Bgr2Gray);
                    CvInvoke.Canny(gray, edges, 50, 150);
                    return "imgproc (CvtColor/Canny): PASS";
                }
            }
            catch (Exception e)
            {
                return "imgproc (CvtColor/Canny): FAIL " + e.Message;
            }
        }

        private string SelfTestJpegRoundTrip()
        {
            try
            {
                using (Mat src = new Mat(64, 64, DepthType.Cv8U, 3))
                using (VectorOfByte buf = new VectorOfByte())
                using (Mat decoded = new Mat())
                {
                    src.SetTo(new MCvScalar(10, 20, 30));
                    bool encoded = CvInvoke.Imencode(".jpg", src, buf);
                    CvInvoke.Imdecode(buf.ToArray(), ImreadModes.ColorBgr, decoded);
                    return (encoded && !decoded.IsEmpty)
                        ? "imgcodecs (Imencode/Imdecode JPEG): PASS"
                        : "imgcodecs (Imencode/Imdecode JPEG): FAIL empty result";
                }
            }
            catch (Exception e)
            {
                return "imgcodecs (Imencode/Imdecode JPEG): FAIL " + e.Message;
            }
        }

        private string SelfTestOrbFeatureDetection()
        {
            try
            {
                using (Mat src = new Mat(64, 64, DepthType.Cv8U, 1))
                using (Emgu.CV.Features.ORB orb = new Emgu.CV.Features.ORB())
                {
                    CvInvoke.Randu(src, new MCvScalar(0), new MCvScalar(255));
                    MKeyPoint[] keyPoints = orb.Detect(src);
                    return "Features (ORB.Detect): PASS (" + keyPoints.Length + " keypoints)";
                }
            }
            catch (Exception e)
            {
                return "Features (ORB.Detect): FAIL " + e.Message;
            }
        }

        private string SelfTestQrCodeDetector()
        {
            try
            {
                using (Mat src = new Mat(64, 64, DepthType.Cv8U, 1))
                using (Mat points = new Mat())
                using (QRCodeDetector detector = new QRCodeDetector())
                {
                    src.SetTo(new MCvScalar(255));
                    // A blank image has no QR code -- Detect should return false,
                    // not throw. The point is exercising the P/Invoke call itself.
                    detector.Detect(src, points);
                    return "Objdetect (QRCodeDetector.Detect): PASS";
                }
            }
            catch (Exception e)
            {
                return "Objdetect (QRCodeDetector.Detect): FAIL " + e.Message;
            }
        }

        private string SelfTestContribThinning()
        {
            try
            {
                using (Mat src = new Mat(64, 64, DepthType.Cv8U, 1))
                using (Mat dst = new Mat())
                {
                    src.SetTo(new MCvScalar(255));
                    Emgu.CV.XImgproc.XImgprocInvoke.Thinning(src, dst, Emgu.CV.XImgproc.ThinningTypes.ZhangSuen);
                    return "Contrib XImgproc (Thinning): PASS";
                }
            }
            catch (Exception e)
            {
                return "Contrib XImgproc (Thinning): FAIL " + e.Message;
            }
        }

        private void RenderTexture(Texture2D texture)
        {
            Image image = this.GetComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }

        private void ResizeTexture(Texture2D texture)
        {
            Image image = this.GetComponent<Image>();
            var transform = image.rectTransform;
            transform.sizeDelta = new Vector2(texture.width, texture.height);
            transform.position = new Vector3(-texture.width / 2, -texture.height / 2);
            transform.anchoredPosition = new Vector2(0, 0);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}