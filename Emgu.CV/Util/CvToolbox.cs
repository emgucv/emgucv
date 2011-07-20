//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Utilities class
   /// </summary>
   public static class CvToolbox
   {
      #region Color Pallette
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
      public static void ColorPaletteToLookupTable(ColorPalette pallette, out Matrix<Byte> bTable, out Matrix<Byte> gTable, out Matrix<Byte> rTable, out Matrix<Byte> aTable)
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
            bData[i, 0] = c.B;
            gData[i, 0] = c.G;
            rData[i, 0] = c.R;
            aData[i, 0] = c.A;
         }
      }
      #endregion

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

      /// <summary>
      /// Compute the minimum and maximum value from the points
      /// </summary>
      /// <param name="points">The points</param>
      /// <param name="min">The minimum x,y,z values</param>
      /// <param name="max">The maximum x,y,z values</param>
      public static void GetMinMax(IEnumerable<MCvPoint3D64f> points, out MCvPoint3D64f min, out MCvPoint3D64f max)
      {
         min = new MCvPoint3D64f();
         min.x = min.y = min.z = double.MaxValue;
         max = new MCvPoint3D64f();
         max.x = max.y = max.z = double.MinValue;

         foreach (MCvPoint3D64f p in points)
         {
            min.x = Math.Min(min.x, p.x);
            min.y = Math.Min(min.y, p.y);
            min.z = Math.Min(min.z, p.z);
            max.x = Math.Max(max.x, p.x);
            max.y = Math.Max(max.y, p.y);
            max.z = Math.Max(max.z, p.z);
         }
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvGetImageSubRect(IntPtr imagePtr, ref Rectangle rect);

      /*
      #region FFMPEG
      private static bool _hasFFMPEG;
      private static bool _ffmpegChecked = false;

      /// <summary>
      /// Indicates if opencv_ffmpeg is presented
      /// </summary>
      internal static bool HasFFMPEG
      {
         get
         {
            if (!_ffmpegChecked)
            {
               String tempFile = Path.GetTempFileName();
               File.Delete(tempFile);
               String fileName = Path.Combine(Path.GetDirectoryName(tempFile), Path.GetFileNameWithoutExtension(tempFile)) + ".avi";
               try
               {
                  IntPtr capture = CvInvoke.cvCreateVideoWriter_FFMPEG(fileName, CvInvoke.CV_FOURCC('U', '2', '6', '3'), 10, new Size(480, 320), true);
                  _hasFFMPEG = (capture != IntPtr.Zero);
                  if (_hasFFMPEG)
                     CvInvoke.cvReleaseVideoWriter_FFMPEG(ref capture);
               }
               catch (Exception e)
               {
                  String msg = e.Message;
                  _hasFFMPEG = false;
               }
               finally
               {
                  if (File.Exists(fileName))
                     File.Delete(fileName);
                  _ffmpegChecked = true;
               }
            }
            return _hasFFMPEG;
         }
      }
      #endregion
      */
      #region color conversion
      private static Dictionary<Type, Dictionary<Type, CvEnum.COLOR_CONVERSION>> _lookupTable
         = new Dictionary<Type, Dictionary<Type, CvEnum.COLOR_CONVERSION>>();

      private static CvEnum.COLOR_CONVERSION GetCode(Type srcType, Type destType)
      {
         ColorInfoAttribute srcInfo = (ColorInfoAttribute)srcType.GetCustomAttributes(typeof(ColorInfoAttribute), true)[0];
         ColorInfoAttribute destInfo = (ColorInfoAttribute)destType.GetCustomAttributes(typeof(ColorInfoAttribute), true)[0];

         String key = String.Format("CV_{0}2{1}", srcInfo.ConversionCodename, destInfo.ConversionCodename);
         return (CvEnum.COLOR_CONVERSION)Enum.Parse(typeof(CvEnum.COLOR_CONVERSION), key, true);
      }

      /// <summary>
      /// Given the source and destination color type, compute the color conversion code for CvInvoke.cvCvtColor function
      /// </summary>
      /// <param name="srcColorType">The source color type. Must be a type inherited from IColor</param>
      /// <param name="destColorType">The dest color type. Must be a type inherited from IColor</param>
      /// <returns>The color conversion code for CvInvoke.cvCvtColor function</returns>
      public static CvEnum.COLOR_CONVERSION GetColorCvtCode(Type srcColorType, Type destColorType)
      {
         CvEnum.COLOR_CONVERSION conversion;
         lock (_lookupTable)
         {
            Dictionary<Type, CvEnum.COLOR_CONVERSION> table = _lookupTable.ContainsKey(srcColorType) ?
               _lookupTable[srcColorType] : (_lookupTable[srcColorType] = new Dictionary<Type, Emgu.CV.CvEnum.COLOR_CONVERSION>());
            conversion = table.ContainsKey(destColorType) ?
                          table[destColorType] : (table[destColorType] = GetCode(srcColorType, destColorType)); ;
         }
         return conversion;
      }
      #endregion
   }
}
