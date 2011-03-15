using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
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
         IntPtr text,
         int maxSizeInBytes);
      #endregion

      private Byte[] _utf8Buffer;
      private UTF8Encoding _utf8 = new UTF8Encoding();

      /// <summary>
      /// Create the tesseract OCR engine using the specific dataPath and language name.
      /// </summary>
      /// <param name="dataPath">The path where the language file is located</param>
      /// <param name="language">The 3 letter language code </param>
      public Tesseract(String dataPath, String language)
      {
         _utf8Buffer = new Byte[2048];

         int initResult = 0;
         _ptr = TessBaseAPICreate(dataPath, language, ref initResult);
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
      /// <typeparam name="TColor"></typeparam>
      /// <param name="image"></param>
      public void SetImage<TColor>(Image<TColor, Byte> image) where TColor : struct, IColor
      {
         TessBaseAPISetImage(_ptr, image);
      }

      /// <summary>
      /// Get all the text in the image
      /// </summary>
      /// <returns>All the text in the image</returns>
      public string GetText()
      {
         GCHandle handle = GCHandle.Alloc(_utf8Buffer, GCHandleType.Pinned);
         TessBaseAPIGetUTF8Text(_ptr, handle.AddrOfPinnedObject(), _utf8Buffer.Length);
         handle.Free();

         return _utf8.GetString(_utf8Buffer);
      }
   }
}
