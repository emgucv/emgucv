//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

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
            using(CvString s = new CvString(filename))
               return cveImwrite(s, image.InputArrayPtr, vec);
         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool cveImwrite(IntPtr filename, IntPtr image, IntPtr parameters);

      /// <summary>
      /// Decode image stored in the buffer
      /// </summary>
      /// <param name="bufMat">A pointer to the CvMat that holds the buffer</param>
      /// <param name="loadType">The image loading type</param>
      /// <returns>A pointer to the Image decoded.</returns>
      [DllImport(OpencvHighguiLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvDecodeImage(IntPtr bufMat, CvEnum.LoadImageType loadType);

      /// <summary>
      /// encode image and store the result as a byte vector.
      /// </summary>
      /// <param name="ext">The image format</param>
      /// <param name="image">The image</param>
      /// <param name="parameters">The pointer to the array of intergers, which contains the parameter for encoding, use IntPtr.Zero for default</param>
      /// <returns>A pointer to single-row 8uC1 CvMat that represent the encoded image.</returns>
      [DllImport(OpencvHighguiLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvEncodeImage([MarshalAs(StringMarshalType)] String ext, IntPtr image, IntPtr parameters);

      /// <summary>
      /// Decode image stored in the buffer
      /// </summary>
      /// <param name="bufMat">The buffer</param>
      /// <param name="loadType">The image loading type</param>
      /// <returns>A pointer to the Image decoded.</returns>
      public static IntPtr cvDecodeImage(byte[] bufMat, CvEnum.LoadImageType loadType)
      {
         GCHandle handle = GCHandle.Alloc(bufMat, GCHandleType.Pinned);
         try
         {
            using (Matrix<byte> mat = new Matrix<byte>(bufMat.Length, 1, 1, handle.AddrOfPinnedObject(), bufMat.Length))
            {
#region set the continute flag for the mat
               int CV_MAT_CONT_FLAG = 1<< 14;
               int type = Marshal.ReadInt32(mat.Ptr, MCvMatConstants.TypeOffset );
               Marshal.WriteInt32(mat.Ptr, MCvMatConstants.TypeOffset, type | CV_MAT_CONT_FLAG);
#endregion

               return cvDecodeImage(mat, loadType);
            }
         }
         finally
         {
            handle.Free();
         }
      }

      [DllImport(OpencvHighguiLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvNamedWindow")]
      private static extern int _cvNamedWindow([MarshalAs(StringMarshalType)] String name, int flags);

      /// <summary>
      /// Creates a window which can be used as a placeholder for images and trackbars. Created windows are reffered by their names. 
      /// If the window with such a name already exists, the function does nothing.
      /// </summary>
      /// <param name="name">Name of the window which is used as window identifier and appears in the window caption</param>
      public static int cvNamedWindow(String name)
      {
         return _cvNamedWindow(name, 1);
      }

      /// <summary>
      /// Waits for key event infinitely (delay &lt;= 0) or for "delay" milliseconds. 
      /// </summary>
      /// <param name="delay">Delay in milliseconds.</param>
      /// <returns>The code of the pressed key or -1 if no key were pressed until the specified timeout has elapsed</returns>
      [DllImport(OpencvHighguiLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvWaitKey(int delay);

      /// <summary>
      /// Shows the image in the specified window
      /// </summary>
      /// <param name="name">Name of the window</param>
      /// <param name="image">Image to be shown</param>
      [DllImport(OpencvHighguiLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvShowImage([MarshalAs(StringMarshalType)] String name, IntPtr image);

      /// <summary>
      /// Destroys the window with a given name
      /// </summary>
      /// <param name="name">Name of the window to be destroyed</param>
      [DllImport(OpencvHighguiLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDestroyWindow([MarshalAs(StringMarshalType)] String name);

   }
}
