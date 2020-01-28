//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.OCR
{
    /// <summary>
    /// The tesseract OCR engine
    /// </summary>
    public class Tesseract : UnmanagedObject
    {

        private UTF8Encoding _utf8 = new UTF8Encoding();

        /// <summary>
        /// Get the tesseract version as String
        /// </summary>
        public static String VersionString
        {
            get
            {
                IntPtr ptr = OcrInvoke.TesseractGetVersion();
                if (ptr == IntPtr.Zero)
                    return String.Empty;
                else
                {
                    return Marshal.PtrToStringAnsi(ptr);
                }
            }
        }

        /// <summary>
        /// Get the tesseract version
        /// </summary>
        public static Version Version
        {
            get
            {
                String vStr = VersionString;
                if (String.IsNullOrEmpty(vStr))
                {
                    return new Version(0, 0);
                }
                else
                {
                    if (vStr.Contains("-"))
                    {
                        int firstDashIdx = vStr.IndexOf('-');
                        vStr = vStr.Substring(0, firstDashIdx);
                    }
                    return new Version(vStr.Replace("dev", String.Empty).Replace("alpha", String.Empty));
                }
            }
        }

        /// <summary>
        /// Create a default tesseract engine. Needed to Call Init function to load language files in a later stage.
        /// </summary>
        /// <param name="enforceLocale">If true, it will enforce "C" locale during the initialization.</param>
        public Tesseract(bool enforceLocale = true)
        {
            Emgu.CV.OCR.LocaleGuard lg = null;
            if (enforceLocale)
                lg = new Emgu.CV.OCR.LocaleGuard(Emgu.CV.OCR.LocaleGuard.LocaleCategory.All, "C");
            try
            {
                _ptr = OcrInvoke.TessBaseAPICreate();
            }
            finally
            {
                if (lg != null)
                    lg.Dispose();
            }
        }

        /// <summary>
        /// If compiled with OpenCL AND an available OpenCL
        /// device is deemed faster than serial code, then
        /// "device" is populated with the cl_device_id
        /// and returns sizeof(cl_device_id)
        /// otherwise *device=nullptr and returns 0.
        /// </summary>
        /// <param name="device">Pointer to the opencl device</param>
        /// <returns>0 if no device found. sizeof(cl_device_id) if device is found.</returns>
        public int GetOpenCLDevice(ref IntPtr device)
        {
            return OcrInvoke.TessBaseAPIGetOpenCLDevice(_ptr, ref device);
        }

        /*
        /// <summary>
        /// Create an tesseract OCR engine.
        /// </summary>
        /// <param name="dataPath">
        /// The datapath must be the name of the directory of tessdata and
        /// must end in / . Any name after the last / will be stripped.
        /// </param>
        /// <param name="language">
        /// The language is (usually) an ISO 639-3 string or NULL will default to eng.
        /// It is entirely safe (and eventually will be efficient too) to call
        /// Init multiple times on the same instance to change language, or just
        /// to reset the classifier.
        /// The language may be a string of the form [~]%lt;lang&gt;[+[~]&lt;lang&gt;]* indicating
        /// that multiple languages are to be loaded. Eg hin+eng will load Hindi and
        /// English. Languages may specify internally that they want to be loaded
        /// with one or more other languages, so the ~ sign is available to override
        /// that. Eg if hin were set to load eng by default, then hin+~eng would force
        /// loading only hin. The number of loaded languages is limited only by
        /// memory, with the caveat that loading additional languages will impact
        /// both speed and accuracy, as there is more work to do to decide on the
        /// applicable language, and there is more chance of hallucinating incorrect
        /// words.
        /// </param>
        /// <param name="mode">OCR engine mode</param>
        public Tesseract(String dataPath, String language, OcrEngineMode mode)
           : this()
        {
            Init(dataPath, language, mode);
        }*/

        /// <summary>
        /// Create a Tesseract OCR engine.
        /// </summary>
        /// <param name="dataPath">
        /// The datapath must be the name of the directory of tessdata and
        /// must end in / . Any name after the last / will be stripped.
        /// </param>
        /// <param name="language">
        /// The language is (usually) an ISO 639-3 string or NULL will default to eng.
        /// It is entirely safe (and eventually will be efficient too) to call
        /// Init multiple times on the same instance to change language, or just
        /// to reset the classifier.
        /// The language may be a string of the form [~]%lt;lang&gt;[+[~]&lt;lang&gt;]* indicating
        /// that multiple languages are to be loaded. Eg hin+eng will load Hindi and
        /// English. Languages may specify internally that they want to be loaded
        /// with one or more other languages, so the ~ sign is available to override
        /// that. Eg if hin were set to load eng by default, then hin+~eng would force
        /// loading only hin. The number of loaded languages is limited only by
        /// memory, with the caveat that loading additional languages will impact
        /// both speed and accuracy, as there is more work to do to decide on the
        /// applicable language, and there is more chance of hallucinating incorrect
        /// words.
        /// </param>
        /// <param name="mode">OCR engine mode</param>
        /// <param name="whiteList">This can be used to specify a white list for OCR. e.g. specify "1234567890" to recognize digits only. Note that the white list currently seems to only work with OcrEngineMode.OEM_TESSERACT_ONLY</param>
        /// <param name="enforceLocale">If true, we will change the locale to "C" before initializing the tesseract engine and reverting it back once the tesseract initialiation is completer. If false, it will be the user's responsibility to set the locale to "C", otherwise an exception will be thrown. See https://github.com/tesseract-ocr/tesseract/issues/1670 </param>
        public Tesseract(String dataPath, String language, OcrEngineMode mode, String whiteList = null, bool enforceLocale = true)
           : this(enforceLocale)
        {
            //if (mode == OcrEngineMode. || mode == OcrEngineMode.TesseractCubeCombined)
            //    throw new ArgumentException("White list is not supported by CUBE engine");

            Init(dataPath, language, mode);
            if (whiteList != null)
                SetVariable("tessedit_char_whitelist", whiteList);
        }

        /*
        /// <summary>
        /// Check of the specific Ocr Engine is supported for the current tesseract release
        /// </summary>
        /// <param name="mode">The Engine mode</param>
        /// <returns>True if supported, false otherwise</returns>
        public bool IsEngineModeSupported(OcrEngineMode mode)
        {
           Version v = Version;
           if ((mode == OcrEngineMode.OEM_CUBE_ONLY || mode == OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED)
              && (v.Major < 3 || (v.Major == 3 && v.Minor < 1)))
           {
              return false;
           }
           return true;
        }*/

        /// <summary>
        /// Check whether a word is valid according to Tesseract's language model
        /// </summary>
        /// <param name="word">The word to be checked.</param>
        /// <returns>0 if the word is invalid, non-zero if valid</returns>
        public int IsValidWord(String word)
        {
            return OcrInvoke.TessBaseAPIIsValidWord(_ptr, word);
        }

        /// <summary>
        /// Gets or sets the page seg mode.
        /// </summary>
        /// <value>
        /// The page seg mode.
        /// </value>
        public PageSegMode PageSegMode
        {
            get { return OcrInvoke.TessBaseAPIGetPageSegMode(_ptr); }
            set { OcrInvoke.TessBaseAPISetPageSegMode(_ptr, value); }
        }

        /// <summary>
        /// Get the default tesseract ocr directory. This should return the folder of the dll in most situations.
        /// </summary>
        public static String DefaultTesseractDirectory
        {
            get
            {
                String loadDirectory = ".";

                System.Reflection.Assembly asm = typeof(CvInvoke).Assembly; //System.Reflection.Assembly.GetExecutingAssembly();
                if (!((String.IsNullOrEmpty(asm.Location) || !System.IO.File.Exists(asm.Location)) && AppDomain.CurrentDomain.BaseDirectory != null))
                {
                    loadDirectory = System.IO.Path.GetDirectoryName(asm.Location);
                }
                return Path.Combine(loadDirectory, "tessdata") + System.IO.Path.DirectorySeparatorChar;
            }
        }

        /// <summary>
        /// Get the url to download the tessdata file for the specific language
        /// </summary>
        /// <param name="lang">The 3 letter language identifier</param>
        /// <returns>the url to download the tessdata file for the specific language</returns>
        public static String GetLangFileUrl(string lang)
        {
            return String.Format("https://github.com/tesseract-ocr/tessdata/blob/590567f20dc044f6948a8e2c61afc714c360ad0e/{0}.traineddata?raw=true", lang);
        }

        /// <summary>
        /// Initialize the OCR engine using the specific dataPath and language name.
        /// </summary>
        /// <param name="dataPath">
        /// The datapath must be the name of the parent directory of tessdata and
        /// must end in / . Any name after the last / will be stripped.
        /// </param>
        /// <param name="language">
        /// The language is (usually) an ISO 639-3 string or NULL will default to eng.
        /// It is entirely safe (and eventually will be efficient too) to call
        /// Init multiple times on the same instance to change language, or just
        /// to reset the classifier.
        /// The language may be a string of the form [~]%lt;lang&gt;[+[~]&lt;lang&gt;]* indicating
        /// that multiple languages are to be loaded. Eg hin+eng will load Hindi and
        /// English. Languages may specify internally that they want to be loaded
        /// with one or more other languages, so the ~ sign is available to override
        /// that. Eg if hin were set to load eng by default, then hin+~eng would force
        /// loading only hin. The number of loaded languages is limited only by
        /// memory, with the caveat that loading additional languages will impact
        /// both speed and accuracy, as there is more work to do to decide on the
        /// applicable language, and there is more chance of hallucinating incorrect
        /// words.
        /// </param>
        /// <param name="mode">OCR engine mode</param>
        public void Init(String dataPath, String language, OcrEngineMode mode)
        {
            /*
            #if !NETFX_CORE
                        if (!(dataPath.Length > 0 && dataPath.Substring(dataPath.Length - 1).ToCharArray()[0] == System.IO.Path.DirectorySeparatorChar))
                        {  //if the data path end in slash
                            int lastSlash = dataPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                            if (lastSlash != -1)
                            {
                                //there is a directory separator, get the path up to the separator, the same way tesseract-ocr calculate the folder
                                dataPath = dataPath.Substring(0, lastSlash + 1);
                            }
                        }
            #endif
            */
            /*
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(dataPath, "tessdata")))
            {
               throw new ArgumentException(String.Format("The directory {0} doesn't exist!", Path.Combine(dataPath, "tessdata")));
            }

            //make sure the tesseract file exist.
            if (mode == OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED || mode == OcrEngineMode.OEM_TESSERACT_ONLY)
            {
               if (!System.IO.File.Exists(System.IO.Path.Combine(dataPath, "tessdata", language + ".traineddata")))
                  throw new ArgumentException(String.Format("The required tesseract file {0}.traineddata doesn't exist", System.IO.Path.Combine(dataPath, language)));
            }*/

            /*if (!IsEngineModeSupported(mode))
               throw new ArgumentException(String.Format("The Ocr engine mode {0} is not supported in tesseract v{1}", mode, Version));*/


            using (CvString csDataPath = new CvString(dataPath))
            using (CvString csLanguage = new CvString(language))
            {
                int initResult = OcrInvoke.TessBaseAPIInit(_ptr, csDataPath, csLanguage, mode);
                if (initResult != 0)
                {
#if !NETFX_CORE
                    if (dataPath.Equals(String.Empty))
                        dataPath = Path.GetFullPath(".");
#endif
                    throw new ArgumentException(
                        String.Format("Unable to create ocr model using Path '{0}', language '{1}' and OcrEngineMode '{2}'.", dataPath,
                            language, mode));
                }
            }


        }

        /// <summary>
        /// Release the unmanaged resource associated with this class
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                OcrInvoke.TessBaseAPIRelease(ref _ptr);
        }

        /// <summary>
        /// Set the image for optical character recognition
        /// </summary>
        /// <param name="image">The image where detection took place</param>
        public void SetImage(IInputArray image)
        {
            using (InputArray iaImage = image.GetInputArray())
                OcrInvoke.TessBaseAPISetImage(_ptr, iaImage);
        }

        /// <summary>
        /// Set the image for optical character recognition
        /// </summary>
        /// <param name="image">The image where detection took place</param>
        public void SetImage(Pix image)
        {
            OcrInvoke.TessBaseAPISetImagePix(_ptr, image);
        }

        /// <summary>
        /// Recognize the image from SetAndThresholdImage, generating Tesseract
        /// internal structures.
        /// </summary>
        /// <returns>Returns 0 on success.</returns>
        public int Recognize()
        {
            return OcrInvoke.TessBaseAPIRecognize(_ptr);
        }

        /// <summary>
        /// Set the variable to the specific value.
        /// </summary>
        /// <param name="variableName">The name of the tesseract variable. e.g. use "tessedit_char_blacklist" to black list characters and "tessedit_char_whitelist" to white list characters. The full list of options can be found in the Tesseract OCR source code "tesseractclass.h"</param>
        /// <param name="value">The value to be set</param>
        public void SetVariable(String variableName, String value)
        {
            if (!OcrInvoke.TessBaseAPISetVariable(_ptr, variableName, value))
            {
                throw new System.ArgumentException(String.Format("Unable to set {0} to {1}", variableName, value));
            }
        }

        /// <summary>
        /// Get all the text in the image
        /// </summary>
        /// <returns>All the text in the image</returns>
        public string GetUTF8Text()
        {
            using (Util.VectorOfByte bytes = new Util.VectorOfByte())
            {
                OcrInvoke.TessBaseAPIGetUTF8Text(_ptr, bytes);
                return UtfByteVectorToString(bytes);
            }
        }

        /// <summary>
        /// Make a TSV-formatted string from the internal data structures.
        /// </summary>
        /// <param name="pageNumber">pageNumber is 0-based but will appear in the output as 1-based.</param>
        /// <returns>A TSV-formatted string from the internal data structures.</returns>
        public String GetTSVText(int pageNumber = 0)
        {
            using (Util.VectorOfByte bytes = new Util.VectorOfByte())
            {
                OcrInvoke.TessBaseAPIGetTSVText(_ptr, pageNumber, bytes);
                return UtfByteVectorToString(bytes);
            }
        }

        /// <summary>
        /// The recognized text is returned as coded in the same format as a box file used in training.
        /// </summary>
        /// <param name="pageNumber">pageNumber is 0-based but will appear in the output as 1-based.</param>
        /// <returns>The recognized text is returned as coded in the same format as a box file used in training.</returns>
        public String GetBoxText(int pageNumber = 0)
        {
            using (Util.VectorOfByte bytes = new Util.VectorOfByte())
            {
                OcrInvoke.TessBaseAPIGetBoxText(_ptr, pageNumber, bytes);
                return UtfByteVectorToString(bytes);
            }
        }

        /// <summary>
        /// The recognized text is returned coded as UNLV format Latin-1 with specific reject and suspect codes
        /// </summary>
        /// <param name="pageNumber">pageNumber is 0-based but will appear in the output as 1-based.</param>
        /// <returns>The recognized text is returned coded as UNLV format Latin-1 with specific reject and suspect codes</returns>
        public String GetUNLVText(int pageNumber = 0)
        {
            using (Util.VectorOfByte bytes = new Util.VectorOfByte())
            {
                OcrInvoke.TessBaseAPIGetUNLVText(_ptr, bytes);
                return UtfByteVectorToString(bytes);
            }
        }

        /// <summary>
        /// The recognized text
        /// </summary>
        /// <param name="pageNumber">pageNumber is 0-based but will appear in the output as 1-based.</param>
        /// <returns>The recognized text</returns>
        public String GetOsdText(int pageNumber = 0)
        {
            using (Util.VectorOfByte bytes = new Util.VectorOfByte())
            {
                OcrInvoke.TessBaseAPIGetOsdText(_ptr, pageNumber, bytes);
                return UtfByteVectorToString(bytes);
            }
        }

        /// <summary>
        /// Make a HTML-formatted string with hOCR markup from the internal data structures.
        /// </summary>
        /// <param name="pageNumber">pageNumber is 0-based but will appear in the output as 1-based.</param>
        /// <returns>A HTML-formatted string with hOCR markup from the internal data structures.</returns>
        public String GetHOCRText(int pageNumber = 0)
        {
            using (Util.VectorOfByte bytes = new Util.VectorOfByte())
            {
                OcrInvoke.TessBaseAPIGetHOCRText(_ptr, pageNumber, bytes);
                return UtfByteVectorToString(bytes);
            }
        }

        private String UtfByteVectorToString(VectorOfByte bytes)
        {
            //#if NETFX_CORE
            byte[] bArr = bytes.ToArray();
            return _utf8.GetString(bArr, 0, bArr.Length).Replace("\n", Environment.NewLine);
            //#else
            //            return _utf8.GetString(bytes.ToArray()).Replace("\n", Environment.NewLine);
            //#endif
        }

        /// <summary>
        /// Detect all the characters in the image.
        /// </summary>
        /// <returns>All the characters in the image</returns>
        public Character[] GetCharacters()
        {
            using (VectorOfByte textSeq = new VectorOfByte())
            using (VectorOfTesseractResult results = new VectorOfTesseractResult())
            {
                OcrInvoke.TessBaseAPIExtractResult(_ptr, textSeq, results);

                byte[] bytes = textSeq.ToArray();
                TesseractResult[] trs = results.ToArray();

                Character[] res = new Character[trs.Length];
                int idx = 0;
                for (int i = 0; i < trs.Length; i++)
                {
                    TesseractResult tr = trs[i];
                    res[i].Text = _utf8.GetString(bytes, idx, tr.Length).Replace("\n", Environment.NewLine);

                    idx += tr.Length;
                    res[i].Cost = tr.Cost;
                    if (tr.Cost == 0)
                        res[i].Region = Rectangle.Empty;
                    else
                        res[i].Region = tr.Region;
                }
                return res;
            }
        }

        /// <summary>
        /// This represent a character that is detected by the OCR engine
        /// </summary>
        public struct Character
        {
            /// <summary>
            /// The text
            /// </summary>
            public String Text;
            /// <summary>
            /// The cost. The lower it is, the more confident is the result
            /// </summary>
            public float Cost;
            /// <summary>
            /// The region where the character is detected.
            /// </summary>
            public Rectangle Region;
        }

        /// <summary>
        /// Turn a single image into symbolic text.
        /// </summary>
        /// <param name="pix">The pix is the image processed.</param>
        /// <param name="pageIndex">Metadata used by side-effect processes, such as reading a box file or formatting as hOCR.</param>
        /// <param name="filename">Metadata used by side-effect processes, such as reading a box file or formatting as hOCR.</param>
        /// <param name="retryConfig">retryConfig is useful for debugging. If not NULL, you can fall back to an alternate configuration if a page fails for some reason.</param>
        /// <param name="timeoutMillisec">terminates processing if any single page takes too long. Set to 0 for unlimited time.</param>
        /// <param name="renderer">Responsible for creating the output. For example, use the TessTextRenderer if you want plaintext output, or the TessPDFRender to produce searchable PDF.</param>
        /// <returns>Returns true if successful, false on error.</returns>
        public bool ProcessPage(
            Pix pix,
            int pageIndex,
            String filename,
            String retryConfig,
            int timeoutMillisec,
            ITessResultRenderer renderer)
        {
            using (CvString csFileName = new CvString(filename))
            using (CvString csRetryConfig = new CvString(retryConfig))
            {
                return OcrInvoke.TessBaseAPIProcessPage(
                    _ptr,
                    pix,
                    pageIndex,
                    csFileName,
                    csRetryConfig,
                    timeoutMillisec,
                    renderer.TessResultRendererPtr);
            }
        }

        /// <summary>
        /// Runs page layout analysis in the mode set by SetPageSegMode. May optionally be called prior to Recognize to get access to just the page layout results. Returns an iterator to the results. Returns NULL on error or an empty page. The returned iterator must be deleted after use. WARNING! This class points to data held within the TessBaseAPI class, and therefore can only be used while the TessBaseAPI class still exists and has not been subjected to a call of Init, SetImage, Recognize, Clear, End DetectOS, or anything else that changes the internal PAGE_RES.
        /// </summary>
        /// <param name="mergeSimilarWords">If true merge similar words</param>
        /// <returns>Page iterator</returns>
        public PageIterator AnalyseLayout(bool mergeSimilarWords = false)
        {
            return new PageIterator(OcrInvoke.TessBaseAPIAnalyseLayout(_ptr, mergeSimilarWords));
        }

        /// <summary>
        /// Get the OCR Engine Mode
        /// </summary>
        public OcrEngineMode Oem
        {
            get { return OcrInvoke.TessBaseAPIGetOem(_ptr); }
        }
    }
}

