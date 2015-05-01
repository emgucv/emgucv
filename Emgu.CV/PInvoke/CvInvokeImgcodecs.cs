//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      /// <summary>
      /// Loads an image from the specified file and returns the pointer to the loaded image. Currently the following file formats are supported: 
      /// Windows bitmaps - BMP, DIB; 
      /// JPEG files - JPEG, JPG, JPE; 
      /// Portable Network Graphics - PNG; 
      /// Portable image format - PBM, PGM, PPM; 
      /// Sun rasters - SR, RAS; 
      /// TIFF files - TIFF, TIF; 
      /// OpenEXR HDR images - EXR; 
      /// JPEG 2000 images - jp2. 
      /// </summary>
      /// <param name="filename">The name of the file to be loaded</param>
      /// <param name="loadType">The image loading type</param>
      /// <returns>The loaded image</returns>
      public static Mat Imread(String filename, CvEnum.LoadImageType loadType)
      {
         return new Mat(filename, loadType);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveImread(
         IntPtr filename,
         CvEnum.LoadImageType loadType,
         IntPtr result);

      /// <summary>
      /// Saves the image to the specified file. The image format is chosen depending on the filename extension, see cvLoadImage. Only 8-bit single-channel or 3-channel (with 'BGR' channel order) images can be saved using this function. If the format, depth or channel order is different, use cvCvtScale and cvCvtColor to convert it before saving, or use universal cvSave to save the image to XML or YAML format
      /// </summary>
      /// <param name="filename">The name of the file to be saved to</param>
      /// <param name="image">The image to be saved</param>
      /// <param name="parameters">The parameters</param>
      /// <returns>true if success</returns>
      public static bool Imwrite(String filename, IInputArray image, params int[] parameters)
      {
         using (Util.VectorOfInt vec = new Util.VectorOfInt())
         {
            if (parameters.Length > 0)
               vec.Push(parameters);
            using (CvString s = new CvString(filename))
            using (InputArray iaImage = image.GetInputArray())
               return cveImwrite(s, iaImage, vec);
         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool cveImwrite(IntPtr filename, IntPtr image, IntPtr parameters);

      /// <summary>
      /// Decode image stored in the buffer
      /// </summary>
      /// <param name="buf">The buffer</param>
      /// <param name="loadType">The image loading type</param>
      /// <param name="dst">The output placeholder for the decoded matrix.</param>
      public static void Imdecode(byte[] buf, CvEnum.LoadImageType loadType, Mat dst)
      {
         using (VectorOfByte vb = new VectorOfByte(buf))
         {
            Imdecode(vb, loadType, dst);
         }
      }

      /// <summary>
      /// Decode image stored in the buffer
      /// </summary>
      /// <param name="buf">The buffer</param>
      /// <param name="loadType">The image loading type</param>
      /// <param name="dst">The output placeholder for the decoded matrix.</param>
      public static void Imdecode(IInputArray buf, CvEnum.LoadImageType loadType, Mat dst)
      {
         using (InputArray iaBuffer = buf.GetInputArray())
            cveImdecode(iaBuffer, loadType, dst);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveImdecode(IntPtr buf, CvEnum.LoadImageType loadType, IntPtr dst);

      /// <summary>
      /// encode image and store the result as a byte vector.
      /// </summary>
      /// <param name="ext">The image format</param>
      /// <param name="image">The image</param>
      /// <param name="buf">Output buffer resized to fit the compressed image.</param>
      /// <param name="parameters">The pointer to the array of intergers, which contains the parameter for encoding, use IntPtr.Zero for default</param>
      public static void Imencode(String ext, IInputArray image, VectorOfByte buf, params int[] parameters)
      {
         using (CvString extStr = new CvString(ext))
         using (VectorOfInt p = new VectorOfInt())
         {
            if (parameters.Length > 0)
               p.Push(parameters);
            using (InputArray iaImage = image.GetInputArray())
               cveImencode(extStr, iaImage, buf, p);
         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveImencode(IntPtr ext, IntPtr image, IntPtr buffer, IntPtr parameters);


   }
}
