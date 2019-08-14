//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

//#if !NETFX_CORE

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
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
#elif NETFX_CORE
using Windows.Storage;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Util;
using FaceDetection;
using Xamarin.Forms;

namespace Emgu.CV.XamarinForms
{
    public class OcrPage : ButtonTextImagePage
    {
        private Tesseract _ocr;
        private static void TesseractDownloadLangFile(String folder, String lang)
        {
            String folderName = folder;
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
            String dest = System.IO.Path.Combine(folderName, String.Format("{0}.traineddata", lang));
            if (!System.IO.File.Exists(dest))
                using (System.Net.WebClient webclient = new System.Net.WebClient())
                {
                    String source = Emgu.CV.OCR.Tesseract.GetLangFileUrl(lang);

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
                if (image == null || image[0] == null)
                    return;
                SetMessage("Please wait...");
                SetImage(null);

                Task<Tuple<Mat, String, long>> t = new Task<Tuple<Mat, String, long>>(
                  () =>
                  {

                      String lang = "eng";
#if NETFX_CORE
                      String path = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "tessdata");
#elif __ANDROID__
                      String path = System.IO.Path.Combine(
                         Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                         Android.OS.Environment.DirectoryDownloads,
                         "tessdata");
#elif __IOS__
                     String path = System.IO.Path.Combine (
                        Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments ),
                        "tessdata");
#else
                      String path = "./tessdata/";
#endif

                      TesseractDownloadLangFile(path, lang);
                      TesseractDownloadLangFile(path, "osd"); //script orientation detection

                      if (_ocr == null)
                      {
                          _ocr = new Tesseract(path, lang, OcrEngineMode.TesseractOnly);
                      }

                      _ocr.SetImage(image[0]);
                      if (_ocr.Recognize() != 0)
                          throw new Exception("Failed to recognize image");
                      String text = _ocr.GetUTF8Text();
                      long time = 0;

                      return new Tuple<Mat, String, long>(image[0], text, time);
                  });
                t.Start();

                var result = await t;
                SetImage(t.Result.Item1);
                String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
                string ocrResult = t.Result.Item2;
                if (Device.RuntimePlatform.Equals("WPF"))
                {
                    ocrResult = ocrResult.Replace(System.Environment.NewLine, " ");
                }

                SetMessage(ocrResult);
            };
        }

        private void OnButtonClicked(Object sender, EventArgs args)
        {
            LoadImages(new string[] { "test_image.png" });
        }

    }
}

//#endif