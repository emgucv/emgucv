//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !NETFX_CORE
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
      /// Get the tesseract version
      /// </summary>
      public static Version Version
      {
         get
         {
            IntPtr ptr = OcrInvoke.TesseractGetVersion();
            return new Version(ptr == IntPtr.Zero ? "0.0" : Marshal.PtrToStringAnsi(ptr));
         }
      }

      /// <summary>
      /// Create a default tesseract engine. Needed to Call Init function to load language files in a later stage.
      /// </summary>
      public Tesseract()
      {
         _ptr = OcrInvoke.TessBaseAPICreate();
      }

      public int GetOpenCLDevice(ref IntPtr device)
      {
         return OcrInvoke.TessBaseAPIGetOpenCLDevice(_ptr, ref device);
      }

      /// <summary>
      /// Create an tesseract OCR engine.
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
      public Tesseract(String dataPath, String language, OcrEngineMode mode)
         : this()
      {
         Init(dataPath, language, mode);
      }

      /// <summary>
      /// Create an tesseract OCR engine.
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
      /// <param name="whiteList">This can be used to specify a white list for OCR. e.g. specify "1234567890" to recognize digits only. Note that the white list currently seems to only work with OcrEngineMode.OEM_TESSERACT_ONLY</param>
      public Tesseract(String dataPath, String language, OcrEngineMode mode, String whiteList)
         : this(dataPath, language, mode)
      {
         if (mode == OcrEngineMode.CubeOnly || mode == OcrEngineMode.TesseractCubeCombined)
            throw new ArgumentException("White list is not supported by CUBE engine");

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
         
         if (!(dataPath.Length > 0 && dataPath.Substring(dataPath.Length - 1).ToCharArray()[0] == System.IO.Path.DirectorySeparatorChar))
         {  //if the data path end in slash
            int lastSlash = dataPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
            if (lastSlash != -1)
            {  
               //there is a directory separator, get the path up to the separator, the same way tesseract-ocr calculate the folder
               dataPath = dataPath.Substring(0, lastSlash + 1);
            }
         }
         
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
         int initResult = OcrInvoke.TessBaseAPIInit(_ptr, dataPath, language, mode);
         if (initResult != 0) throw new ArgumentException(String.Format("Unable to create ocr model using Path {0} and language {1}.", dataPath, language));
      }

      /// <summary>
      /// Release the unmanaged resource associated with this class
      /// </summary>
      protected override void DisposeObject()
      {
         OcrInvoke.TessBaseAPIRelease(ref _ptr);
      }

      /// <summary>
      /// Set the image for optical character recognition
      /// </summary>
      
      /// <param name="image">The image where detection took place</param>
      public void Recognize(IInputArray image)
      {
         using (InputArray iaImage = image.GetInputArray())
            OcrInvoke.TessBaseAPIRecognizeArray(_ptr, iaImage);
      }

      /// <summary>
      /// Set the variable to the specific value.
      /// </summary>
      /// <param name="variableName">The name of the tesseract variable. e.g. use "tessedit_char_blacklist" to black list characters and ""tessedit_char_whitelist" to white list characters</param>
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
      public string GetText()
      {
         using (Util.VectorOfByte bytes = new Util.VectorOfByte())
         {
            OcrInvoke.TessBaseAPIGetUTF8Text(_ptr, bytes);
            return _utf8.GetString(bytes.ToArray()).Replace("\n", Environment.NewLine);
         }
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
            //Seq<byte> textSeq = new Seq<byte>(stor);
            //Seq<TesseractResult> results = new Seq<TesseractResult>(stor);
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

      public PageIterator AnalyseLayout(bool mergeSimilarWords = false)
      {
         return  new PageIterator(OcrInvoke.TessBaseAPIAnalyseLayout(_ptr, mergeSimilarWords));
      }
   }

   /// <summary>
   /// When Tesseract/Cube is initialized we can choose to instantiate/load/run
   /// only the Tesseract part, only the Cube part or both along with the combiner.
   /// The preference of which engine to use is stored in tessedit_ocr_engine_mode.
   /// </summary>
   public enum OcrEngineMode
   {
      /// <summary>
      /// Run Tesseract only - fastest
      /// </summary>
      TesseractOnly,
      /// <summary>
      /// Run Cube only - better accuracy, but slower
      /// </summary>
      CubeOnly,
      /// <summary>
      /// Run both and combine results - best accuracy
      /// </summary>
      TesseractCubeCombined,
      /// <summary>
      /// Specify this mode to indicate that any of the above modes
      /// should be automatically inferred from the variables in the 
      /// language-specific config, or if not specified in any of 
      /// the above should be set to the default OEM_TESSERACT_ONLY.
      /// </summary>
      Default
   };

   /// <summary>
   /// Tesseract page segmentation mode
   /// </summary>
   public enum PageSegMode
   {
      /// <summary>
      /// PageOrientation and script detection only.
      /// </summary>
      OsdOnly,
      /// <summary>
      /// Automatic page segmentation with orientation and script detection. (OSD)
      /// </summary>
      AutoOsd,
      /// <summary>
      /// Automatic page segmentation, but no OSD, or OCR.
      /// </summary>
      AutoOnly,
      /// <summary>
      /// Fully automatic page segmentation, but no OSD.
      /// </summary>
      Auto,
      /// <summary>
      /// Assume a single column of text of variable sizes.
      /// </summary>
      SingleColumn,
      /// <summary>
      /// Assume a single uniform block of vertically aligned text.
      /// </summary>
      SingleBlockVertText,


      /// <summary>
      /// Assume a single uniform block of text. (Default.)
      /// </summary>
      SingleBlock,
      /// <summary>
      /// Treat the image as a single text line.
      /// </summary>
      SingleLine,
      /// <summary>
      /// Treat the image as a single word.
      /// </summary>
      SingleWord,
      /// <summary>
      /// Treat the image as a single word in a circle.
      /// </summary>
      CircleWord,
      /// <summary>
      /// Treat the image as a single character.
      /// </summary>
      SingleChar,
      /// <summary>
      /// Find as much text as possible in no particular order.
      /// </summary>
      SparseText,
      /// <summary>
      /// Sparse text with orientation and script det.
      /// </summary>
      SparseTextOsd,
      /// <summary>
      /// Treat the image as a single text line, bypassing hacks that are Tesseract-specific.
      /// </summary>
      RawLine,

      /// <summary>
      /// Number of enum entries.
      /// </summary>
      Count
   };
}

#endif