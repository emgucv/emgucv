using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.OCR
{
   public class Tesseract : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr TessBaseAPICreate(
         [MarshalAs(CvInvoke.StringMarshalType)]
         String dataPath,
         [MarshalAs(CvInvoke.StringMarshalType)]
         String language,
         ref int initResult);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void TessBaseAPIRelease(ref IntPtr ocr);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void TessBaseAPISetImage(IntPtr ocr, IntPtr image);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void TessBaseAPIGetUTF8Text(
         IntPtr ocr,
         [MarshalAs(UnmanagedType.LPStr)]
         StringBuilder text, 
         int maxSizeInBytes);
      #endregion

      public Tesseract(String dataPath, String language)
      {
         int initResult = 0;
         _ptr = TessBaseAPICreate(dataPath, language, ref initResult);
         if (initResult != 0) throw new ArgumentException(String.Format("Unable to create ocr model using Path {0} and language {1}.", dataPath, language));
      }

      protected override void DisposeObject()
      {
         TessBaseAPIRelease(ref _ptr);
      }

      public void SetImage<TColor>(Image<TColor, Byte> image) where TColor : struct, IColor
      {
         TessBaseAPISetImage(_ptr, image);
      }

      public string GetText()
      {
         StringBuilder buffer = new StringBuilder(2048);
         TessBaseAPIGetUTF8Text(_ptr, buffer, buffer.Capacity);
         return buffer.ToString();
      }

   }
}
