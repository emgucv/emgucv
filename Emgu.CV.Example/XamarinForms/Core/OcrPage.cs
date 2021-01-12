//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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
using Emgu.CV.Dnn;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Models;
using Emgu.Util;
using FaceDetection;
using Xamarin.Forms;
using Environment = System.Environment;

namespace Emgu.CV.XamarinForms
{
    public class OcrPage : ButtonTextImagePage
    {
        private String _modelFolderName = "tessdata";
        private Tesseract _ocr;

        private async Task InitTesseract(String lang, OcrEngineMode mode)
        {
            if (_ocr == null)
            {
                FileDownloadManager manager = new FileDownloadManager();
                manager.AddFile(Emgu.CV.OCR.Tesseract.GetLangFileUrl(lang), _modelFolderName);
                manager.AddFile(Emgu.CV.OCR.Tesseract.GetLangFileUrl("osd"), _modelFolderName); //script orientation detection
                
                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
                await manager.Download();
                FileInfo fi = new FileInfo(manager.Files[0].LocalFile);
                
                _ocr = new Tesseract(fi.DirectoryName, lang, mode);
                
            }
        }

        public OcrPage()
         : base()
        {
            var button = this.GetButton();
            button.Text = "Perform Text Detection";
            button.Clicked += OnButtonClicked;

        }

        private async void OnButtonClicked(Object sender, EventArgs args)
        {
            Mat[] images = await LoadImages(new string[] { "test_image.png" });
            if (images == null || images[0] == null)
                return;
            SetMessage("Please wait...");
            SetImage(null);
            String lang = "eng";
            OcrEngineMode mode = OcrEngineMode.TesseractOnly;
            await InitTesseract(lang, OcrEngineMode.TesseractOnly);
            _ocr.SetImage(images[0]);
            if (_ocr.Recognize() != 0)
                throw new Exception("Failed to recognize image");
            String ocrResult = _ocr.GetUTF8Text();

            SetImage(images[0]);

            if (Device.RuntimePlatform.Equals("WPF"))
            {
                ocrResult = ocrResult.Replace(System.Environment.NewLine, " ");
            }
            ocrResult = String.Format(
                "tesseract version {2}; lang: {0}; mode: {1}{3}Text Detected:{3}{4}",
                lang,
                mode.ToString(),
                Emgu.CV.OCR.Tesseract.VersionString,
                System.Environment.NewLine, ocrResult);
            SetMessage(ocrResult);
        }

        private void DownloadManager_OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive <= 0)
                SetMessage(String.Format("{0} bytes downloaded.", e.BytesReceived));
            else
                SetMessage(String.Format("{0} of {1} bytes downloaded ({2}%)", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage));
        }
    }
}