//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.OCR
{
   /// <summary>
   /// The tesseract OCR engine
   /// </summary>
   public class Tesseract : UnmanagedObject
   {
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
      private static extern void TessBaseAPISetImage(IntPtr ocr, IntPtr image);

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
      #endregion

      private UTF8Encoding _utf8 = new UTF8Encoding();

      /// <summary>
      /// Create an tesseract OCR engine.
      /// </summary>
      /// <param name="dataPath">The path where the language file is located</param>
      /// <param name="language">The 3 letter language code </param>
      /// <param name="mode">OCR engine mode</param>
      public Tesseract(String dataPath, String language, OcrEngineMode mode)
      {
         _ptr = TessBaseAPICreate();
         Init(dataPath, language, mode);
      }

      /// <summary>
      /// Initialize the OCR engine using the specific dataPath and language name.
      /// </summary>
      /// <param name="dataPath">The path where the language file is located</param>
      /// <param name="language">The 3 letter language code </param>
      /// <param name="mode">OCR engine mode</param>
      private void Init(String dataPath, String language, OcrEngineMode mode)
      {
         int initResult= TessBaseAPIInit(_ptr, dataPath, language, mode);
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
      public void SetImage<TColor>(Image<TColor, Byte> image) where TColor : struct, IColor
      {
         TessBaseAPISetImage(_ptr, image);
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

      public Charactor[] FindCharactors()
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
                  res[i].Region = new Rectangle(tr.X0, tr.Y0, tr.X1 - tr.X0, tr.Y1 - tr.Y0);
            }
            return res;
         }
      }

      public struct Charactor
      {
         public String Text;
         public float Cost;
         public Rectangle Region;
      }

      private struct TesseractResult
      {
         public int Length;
         public float Cost;
         public int X0;
         public int X1;
         public int Y0;
         public int Y1;
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
