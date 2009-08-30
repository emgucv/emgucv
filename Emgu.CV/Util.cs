using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Utilities class
   /// </summary>
   public static class Util
   {
      /// <summary>
      /// The ColorPalette of Grayscale for Bitmap Format8bppIndexed
      /// </summary>
      public static readonly ColorPalette GrayscalePalette = GenerateGrayscalePalette();

      private static ColorPalette GenerateGrayscalePalette()
      {
         using (Bitmap image = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
         {
            ColorPalette palette = image.Palette;
            for (int i = 0; i < 256; i++)
            {
               palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            return palette;
         }
      }

      /// <summary>
      /// Convert the color pallette to four lookup tables
      /// </summary>
      /// <param name="pallette">The color pallette to transform</param>
      /// <param name="bTable">Lookup table for the B channel</param>
      /// <param name="gTable">Lookup table for the G channel</param>
      /// <param name="rTable">Lookup table for the R channel</param>
      /// <param name="aTable">Lookup table for the A channel</param>
      public static void ColorPaletteToLookupTable(ColorPalette pallette, out Matrix<Byte> bTable, out Matrix<byte> gTable, out Matrix<Byte> rTable, out Matrix<Byte> aTable)
      {
         bTable = new Matrix<byte>(256, 1);
         gTable = new Matrix<byte>(256, 1);
         rTable = new Matrix<byte>(256, 1);
         aTable = new Matrix<byte>(256, 1);
         byte[,] bData = bTable.Data;
         byte[,] gData = gTable.Data;
         byte[,] rData = rTable.Data;
         byte[,] aData = aTable.Data;

         Color[] colors = pallette.Entries;
         for (int i = 0; i < colors.Length; i++)
         {
            Color c = colors[i];
            bData[i,0] = c.B;
            gData[i,0] = c.G;
            rData[i,0] = c.R;
            aData[i,0] = c.A;
         }
      }

      /// <summary>
      /// Returns information about one of or all of the registered modules
      /// </summary>
      /// <param name="pluginName">The list of names and versions of the optimized plugins that CXCORE was able to find and load</param>
      /// <param name="versionName">Information about the module(s), including version</param>
      public static void GetModuleInfo(out String pluginName, out String versionName)
      {
         IntPtr version = IntPtr.Zero;
         IntPtr pluginInfo = IntPtr.Zero;
         CvInvoke.cvGetModuleInfo(IntPtr.Zero, ref version, ref pluginInfo);

         pluginName = Marshal.PtrToStringAnsi(pluginInfo);
         versionName = Marshal.PtrToStringAnsi(version);
      }
      
      /// <summary>
      /// Enable or diable IPL optimization for opencv
      /// </summary>
      /// <param name="enable">true to enable optimization, false to disable</param>
      public static void OptimizeCV(bool enable)
      {
         CvInvoke.cvUseOptimized(enable);
      }

      /// <summary>
      /// Get the OpenCV matrix depth enumeration from depth type
      /// </summary>
      /// <param name="typeOfDepth">The depth of type</param>
      /// <returns>OpenCV Matrix depth</returns>
      public static CvEnum.MAT_DEPTH GetMatrixDepth(Type typeOfDepth)
      {
         if (typeOfDepth == typeof(Single))
            return CvEnum.MAT_DEPTH.CV_32F;
         if (typeOfDepth == typeof(Int32))
            return Emgu.CV.CvEnum.MAT_DEPTH.CV_32S;
         if (typeOfDepth == typeof(SByte))
            return Emgu.CV.CvEnum.MAT_DEPTH.CV_8S;
         if (typeOfDepth == typeof(Byte))
            return CvEnum.MAT_DEPTH.CV_8U;
         if (typeOfDepth == typeof(Double))
            return CvEnum.MAT_DEPTH.CV_64F;
         if (typeOfDepth == typeof(UInt16))
            return CvEnum.MAT_DEPTH.CV_16U;
         if (typeOfDepth == typeof(Int16))
            return CvEnum.MAT_DEPTH.CV_16S;
         throw new NotImplementedException("Unsupported matrix depth");
      }

      /// <summary>
      /// Convert an array of descriptors to row by row matrix
      /// </summary>
      /// <param name="descriptors">An array of descriptors</param>
      /// <returns>A matrix where each row is a descriptor</returns>
      public static Matrix<float> GetMatrixFromDescriptors(float[][] descriptors)
      {
         int rows = descriptors.Length;
         int cols = descriptors[0].Length;
         Matrix<float> res = new Matrix<float>(rows, cols);
         MCvMat mat = res.MCvMat;
         long dataPos = mat.data.ToInt64();

         for (int i = 0; i < rows; i++)
         {
            Marshal.Copy(descriptors[i], 0, new IntPtr(dataPos), cols);
            dataPos += mat.step;
         }

         return res;
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      internal static extern IntPtr cvGetImageSubRect(IntPtr imagePtr, ref Rectangle rect);
   }
}
