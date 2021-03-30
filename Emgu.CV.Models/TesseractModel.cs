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
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Models;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Models
{
    public class TesseractModel : DisposableObject, IProcessAndRenderModel
    {
        private String _modelFolderName = "tessdata";
        private Tesseract _ocr;
        private String _lang;
        private OcrEngineMode _mode;
        private async Task InitTesseract(String lang, OcrEngineMode mode, System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_ocr == null)
            {
                FileDownloadManager manager = new FileDownloadManager();
                manager.AddFile(Emgu.CV.OCR.Tesseract.GetLangFileUrl(lang), _modelFolderName);
                manager.AddFile(Emgu.CV.OCR.Tesseract.GetLangFileUrl("osd"), _modelFolderName); //script orientation detection

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
                await manager.Download();

                if (manager.AllFilesDownloaded)
                {
                    _lang = lang;
                    _mode = mode;
                    FileInfo fi = new FileInfo(manager.Files[0].LocalFile);
                    _ocr = new Tesseract(fi.DirectoryName, _lang, _mode);
                }
            }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_ocr != null)
            {
                _ocr.Dispose();
                _ocr = null;
            }
        }
        protected override void DisposeObject()
        {
            Clear();
        }

        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _ocr.SetImage(imageIn);
            if (_ocr.Recognize() != 0)
                throw new Exception("Failed to recognize image");
            String ocrResult = _ocr.GetUTF8Text();
            watch.Stop();

            if (imageOut != imageIn)
            {
                using (InputArray iaImageIn = imageIn.GetInputArray())
                {
                    iaImageIn.CopyTo(imageOut);
                }
            }

            return String.Format(
                "tesseract version {2}; lang: {0}; mode: {1}{3}Text Detected:{3}{4}",
                _lang,
                _mode.ToString(),
                Emgu.CV.OCR.Tesseract.VersionString,
                System.Environment.NewLine, ocrResult);

        }

        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
        {
            await InitTesseract("eng", OcrEngineMode.TesseractOnly, onDownloadProgressChanged);
        }


    }
}
