//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !NETFX_CORE

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Preferences;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Util;
using FaceDetection;

namespace Emgu.CV.XamarinForms
{
    public class OcrPage : ButtonTextImagePage
    {
        private Tesseract _ocr;
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


        public OcrPage()
         : base()
        {

            var button = this.GetButton();
            button.Text = "Perform Text Detection";
            button.Clicked += OnButtonClicked;

            OnImagesLoaded += async (sender, image) =>
            {
                GetLabel().Text = "Please wait...";
                SetImage(null);

                Task<Tuple<Mat, String, long>> t = new Task<Tuple<Mat, String, long>>(
                  () =>
                  {
                      String lang = "eng";
#if __ANDROID__
                      String path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                             Android.OS.Environment.DirectoryDownloads);
#else
                      String path = ".";
#endif

                      TesseractDownloadLangFile(path, lang);
                      TesseractDownloadLangFile(path, "osd"); //script orientation detection
                      String pathFinal = path.Length == 0 || path.Substring(path.Length - 1, 1).Equals(System.IO.Path.DirectorySeparatorChar.ToString())
                             ? path
                             : String.Format("{0}{1}", path, System.IO.Path.DirectorySeparatorChar);
                      if (_ocr == null)
                          _ocr = new Tesseract(pathFinal, lang, OcrEngineMode.TesseractOnly);
                      _ocr.SetImage(image[0]);
                      _ocr.Recognize();
                      //GetLabel().Text = _ocr.GetUTF8Text();
                      long time = 0;

                      return new Tuple<Mat, String, long>(image[0], _ocr.GetUTF8Text(), time);
                  });
                t.Start();

                var result = await t;
                SetImage(t.Result.Item1);
                String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
                GetLabel().Text = t.Result.Item2;
            };
        }

        private void OnButtonClicked(Object sender, EventArgs args)
        {
            LoadImages(new string[] { "test_image.png" });
        }

    }
}

#endif