//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
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
      static Tesseract()
      {
         //dummy code that is used to involve the static constructor of CvInvoke, if it has not already been called.
         CvInvoke.CV_MAKETYPE(0, 0);
      }

      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr TessBaseAPICreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int TessBaseAPIInit(
         IntPtr ocr,
         [MarshalAs(CvInvoke.StringMarshalType)]
         String dataPath,
         [MarshalAs(CvInvoke.StringMarshalType)]
         String language,
         OcrEngineMode mode);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void TessBaseAPIRelease(ref IntPtr ocr);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void TessBaseAPIRecognizeImage(IntPtr ocr, IntPtr image);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void TessBaseAPIGetUTF8Text(
         IntPtr ocr,
         IntPtr text);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void TessBaseAPIExtractResult(IntPtr ocr, IntPtr charSeq, IntPtr resultSeq);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool TessBaseAPISetVariable(
         IntPtr ocr,
         [MarshalAs(CvInvoke.StringMarshalType)]
         String varName,
         [MarshalAs(CvInvoke.StringMarshalType)]
         String value);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr TesseractGetVersion();
      #endregion

      private UTF8Encoding _utf8 = new UTF8Encoding();

      /// <summary>
      /// Get the tesseract version
      /// </summary>
      public static Version Version
      {
         get
         {
            IntPtr ptr = TesseractGetVersion();
            return new Version(ptr == IntPtr.Zero ? "0.0" : Marshal.PtrToStringAnsi(ptr));
         }
      }

      /// <summary>
      /// Create a default tesseract engine. Needed to Call Init function to load language files in a later stage.
      /// </summary>
      public Tesseract()
      {
         _ptr = TessBaseAPICreate();
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
         if (mode == OcrEngineMode.OEM_CUBE_ONLY || mode == OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED)
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
               //there is a direcotry separator, get the path up to the separator, the same way tesseract-ocr calculate the folder
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
         int initResult = TessBaseAPIInit(_ptr, dataPath, language, mode);
         if (initResult != 0) throw new ArgumentException(String.Format("Unable to create ocr model using Path {0} and language {1}.", dataPath, language));
      }

      /// <summary>
      /// Release the unmanaged resource associated with this class
      /// </summary>
      protected override void DisposeObject()
      {
         TessBaseAPIRelease(ref _ptr);
      }

      /// <summary>
      /// Set the image for optical charater recognition
      /// </summary>
      /// <typeparam name="TColor">The color type of the image</typeparam>
      /// <param name="image">The image where detection took place</param>
      public void Recognize<TColor>(Image<TColor, Byte> image) where TColor : struct, IColor
      {
         TessBaseAPIRecognizeImage(_ptr, image);
      }

      /// <summary>
      /// Set the variable to the specific value.
      /// </summary>
      /// <param name="variableName">The name of the tesseract variable. e.g. use "tessedit_char_blacklist" to black list characters and ""tessedit_char_whitelist" to white list characters</param>
      /// <param name="value">The value to be set</param>
      public void SetVariable(String variableName, String value)
      {
         if (!TessBaseAPISetVariable(_ptr, variableName, value))
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
            TessBaseAPIGetUTF8Text(_ptr, bytes);
            return _utf8.GetString(bytes.ToArray()).Replace("\n", Environment.NewLine);
         }
      }

      /// <summary>
      /// Detect all the charactors in the image.
      /// </summary>
      /// <returns>All the charactors in the image</returns>
      public Charactor[] GetCharactors()
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<byte> textSeq = new Seq<byte>(stor);
            Seq<TesseractResult> results = new Seq<TesseractResult>(stor);
            TessBaseAPIExtractResult(_ptr, textSeq, results);

            byte[] bytes = textSeq.ToArray();
            TesseractResult[] trs = results.ToArray();

            Charactor[] res = new Charactor[trs.Length];
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
      /// This represent a charactor that is detected by the OCR engine
      /// </summary>
      public struct Charactor
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
         /// The region where the charactor is detected.
         /// </summary>
         public Rectangle Region;
      }

      /// <summary>
      /// This structure is primary used for PInvoke
      /// </summary>
      private struct TesseractResult
      {
#pragma warning disable 0649
         public int Length;
         public float Cost;
         public Rectangle Region;
#pragma warning restore 0649
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
         OEM_TESSERACT_ONLY,
         /// <summary>
         /// Run Cube only - better accuracy, but slower
         /// </summary>
         OEM_CUBE_ONLY,
         /// <summary>
         /// Run both and combine results - best accuracy
         /// </summary>
         OEM_TESSERACT_CUBE_COMBINED,
         /// <summary>
         /// Specify this mode to indicate that any of the above modes
         /// should be automatically inferred from the variables in the 
         /// language-specific config, or if not specified in any of 
         /// the above should be set to the default OEM_TESSERACT_ONLY.
         /// </summary>
         OEM_DEFAULT
      };
   }
}
