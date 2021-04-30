//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
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
    /// <summary>
    /// Tesseract Ocr model.
    /// </summary>
    public class TesseractModel : DisposableObject, IProcessAndRenderModel
    {
        private String _modelFolderName = "tessdata";
        private Tesseract _ocr;
        private String _lang;
        private OcrEngineMode _mode;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        private IEnumerator
#else
        private async Task 
#endif
            InitTesseract(String lang, OcrEngineMode mode, System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            if (_ocr == null)
            {
                FileDownloadManager manager = new FileDownloadManager();
                manager.AddFile(Emgu.CV.OCR.Tesseract.GetLangFileUrl(lang), _modelFolderName);
                manager.AddFile(Emgu.CV.OCR.Tesseract.GetLangFileUrl("osd"), _modelFolderName); //script orientation detection

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

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

        /// <summary>
        /// Release all the unmanaged memory associated to this tesseract OCR model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">The output image, can be the same as imageIn, in which case we will render directly into the input image</param>
        /// <returns>The messages that we want to display.</returns>
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

            Tesseract.Character[] characters = _ocr.GetCharacters();
            foreach (Tesseract.Character c in characters)
            {
                CvInvoke.Rectangle(imageOut, c.Region, new MCvScalar(255, 0, 0));
            }

            return String.Format(
                "tesseract version {2}; lang: {0}; mode: {1}{3}Text Detected:{3}{4}",
                _lang,
                _mode.ToString(),
                Emgu.CV.OCR.Tesseract.VersionString,
                System.Environment.NewLine, ocrResult);

        }

        /// <summary>
        /// Initialize the tesseract ocr model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <param name="initOptions">Initialization options. None supported at the moment, any value passed will be ignored.</param>
        /// <returns>Asyn task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator
#else
        public async Task 
#endif
            Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
        {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
            yield return
#else
            await 
#endif
                InitTesseract("eng", OcrEngineMode.TesseractOnly, onDownloadProgressChanged);
        }

    }
}
