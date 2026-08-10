using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Provide extension method to convert IInputArray to and from Bitmap
    /// </summary>
    public static class BitmapExtension
    {
        #region Color Palette

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
        /// Convert the color palette to four lookup tables
        /// </summary>
        /// <param name="palette">The color palette to transform</param>
        /// <param name="bTable">Lookup table for the B channel</param>
        /// <param name="gTable">Lookup table for the G channel</param>
        /// <param name="rTable">Lookup table for the R channel</param>
        /// <param name="aTable">Lookup table for the A channel</param>
        public static void ColorPaletteToLookupTable(ColorPalette palette, out Mat bTable,
            out Mat gTable, out Mat rTable, out Mat aTable)
        {
            bTable = new Mat(256, 1, DepthType.Cv8U, 1);

            gTable = new Mat(256, 1, DepthType.Cv8U, 1);
            rTable = new Mat(256, 1, DepthType.Cv8U, 1);
            aTable = new Mat(256, 1, DepthType.Cv8U, 1);
            byte[] bData = new byte[256];
            byte[] gData = new byte[256];
            byte[] rData = new byte[256];
            byte[] aData = new byte[256];
            Color[] colors = palette.Entries;
            for (int i = 0; i < colors.Length; i++)
            {
                Color c = colors[i];
                bData[i] = c.B;
                gData[i] = c.G;
                rData[i] = c.R;
                aData[i] = c.A;
            }
            bTable.SetTo(bData);
            gTable.SetTo(gData);
            rTable.SetTo(rData);
            aTable.SetTo(aData);
        }

        #endregion

        /// <summary>
        /// Convert raw data to bitmap
        /// </summary>
        /// <param name="scan0">The pointer to the raw data</param>
        /// <param name="step">The step</param>
        /// <param name="size">The size of the image</param>
        /// <param name="srcColorType">The source image color type</param>
        /// <param name="numberOfChannels">The number of channels</param>
        /// <param name="srcDepthType">The source image depth type</param>
        /// <param name="tryDataSharing">Try to create Bitmap that shares the data with the image</param>
        /// <returns>A bitmap representation of the image.</returns>
        public static Bitmap RawDataToBitmap(IntPtr scan0, int step, Size size, Type srcColorType, int numberOfChannels,
            Type srcDepthType, bool tryDataSharing = false)
        {
            if (tryDataSharing)
            {
                if (srcColorType == typeof(Gray) && srcDepthType == typeof(Byte)
                    && (step & 3) == 0)
                {
                    //Grayscale of Bytes — GDI+ requires stride to be a multiple of 4
                    Bitmap bmpGray = new Bitmap(
                        size.Width,
                        size.Height,
                        step,
                        System.Drawing.Imaging.PixelFormat.Format8bppIndexed,
                        scan0
                    );

                    bmpGray.Palette = GrayscalePalette;

                    return bmpGray;
                }
                // Mono in Linux doesn't support scan0 constructor with Format24bppRgb, use ToBitmap instead
                // See https://bugzilla.novell.com/show_bug.cgi?id=363431
                // TODO: check mono buzilla Bug 363431 to see when it will be fixed 
                else if (
                    Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows &&
                    Emgu.Util.Platform.ClrType == Emgu.Util.Platform.Clr.DotNet &&
                    srcColorType == typeof(Bgr) && srcDepthType == typeof(Byte)
                    && (step & 3) == 0)
                {
                    //Bgr byte    
                    return new Bitmap(
                        size.Width,
                        size.Height,
                        step,
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                        scan0);
                }
                else if (srcColorType == typeof(Bgra) && srcDepthType == typeof(Byte))
                {
                    //Bgra byte
                    return new Bitmap(
                        size.Width,
                        size.Height,
                        step,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                        scan0);
                }

                //PixelFormat.Format16bppGrayScale is not supported in .NET
                //else if (typeof(TColor) == typeof(Gray) && typeof(TDepth) == typeof(UInt16))
                //{
                //   return new Bitmap(
                //      size.width,
                //      size.height,
                //      step,
                //      PixelFormat.Format16bppGrayScale;
                //      scan0);
                //}
            }

            System.Drawing.Imaging.PixelFormat format; //= System.Drawing.Imaging.PixelFormat.Undefined;

            if (srcColorType == typeof(Gray)) // if this is a gray scale image
            {
                format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            }
            else if (srcColorType == typeof(Bgra)) //if this is Bgra image
            {
                format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            }
            else if (srcColorType == typeof(Bgr)) //if this is a Bgr Byte image
            {
                format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            }
            else
            {
                using (Mat m = new Mat(size.Height, size.Width, CvInvoke.GetDepthType(srcDepthType), numberOfChannels,
                    scan0, step))
                using (Mat m2 = new Mat())
                {
                    CvInvoke.CvtColor(m, m2, srcColorType, typeof(Bgr));
                    return RawDataToBitmap(m2.DataPointer, m2.Step, m2.Size, typeof(Bgr), 3, srcDepthType, false);
                }
            }

            Bitmap bmp = new Bitmap(size.Width, size.Height, format);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                new Rectangle(Point.Empty, size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                format);
            using (Mat bmpMat = new Mat(size.Height, size.Width, CvEnum.DepthType.Cv8U, numberOfChannels, data.Scan0,
                data.Stride))
            using (Mat dataMat = new Mat(size.Height, size.Width, CvInvoke.GetDepthType(srcDepthType), numberOfChannels,
                scan0, step))
            {
                if (srcDepthType == typeof(Byte))
                    dataMat.CopyTo(bmpMat);
                else
                {

                    double scale = 1.0, shift = 0.0;
                    RangeF range = dataMat.GetValueRange();
                    if (range.Max > 255.0 || range.Min < 0)
                    {
                        scale = range.Max.Equals(range.Min) ? 0.0 : 255.0 / (range.Max - range.Min);
                        shift = scale.Equals(0) ? range.Min : -range.Min * scale;
                    }

                    CvInvoke.ConvertScaleAbs(dataMat, bmpMat, scale, shift);
                }
            }

            bmp.UnlockBits(data);

            if (format == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                bmp.Palette = GrayscalePalette;
            return bmp;
        }

        /// <summary>
        /// Convert the mat into Bitmap
        /// </summary>
        /// <param name="mat">The Mat to be converted into Bitmap</param>
        /// <param name="tryDataSharing">
        /// If true, the Bitmap created will try to use the same raw pixel data from the Mat if possible.
        /// In which case, do not disposed the input Mat before you disposed the Bitmap, or you will get an memory access violation.
        /// If you are not sure about it, use the default value of "false", in which case the data will be copied over to the Bitmap.</param>
        /// <returns>A bitmap representation of the image.</returns>
        public static Bitmap ToBitmap(this Mat mat, bool tryDataSharing = false)
        {
            if (mat.Dims > 3)
                return null;
            int channels = mat.NumberOfChannels;
            Size s = mat.Size;
            Type colorType;
            switch (channels)
            {
                case 1:
                    colorType = typeof(Gray);

                    if (s.Equals(Size.Empty))
                        return null;
                    if ((s.Width | 3) != 0) //handle the special case where width is not a multiple of 4
                    {
                        Bitmap bmp = new Bitmap(s.Width, s.Height, PixelFormat.Format8bppIndexed);
                        bmp.Palette = GrayscalePalette;
                        BitmapData bitmapData = bmp.LockBits(new Rectangle(Point.Empty, s), ImageLockMode.WriteOnly,
                            PixelFormat.Format8bppIndexed);
                        using (Mat m = new Mat(s.Height, s.Width, DepthType.Cv8U, 1, bitmapData.Scan0,
                            bitmapData.Stride))
                        {
                            mat.CopyTo(m);
                        }

                        bmp.UnlockBits(bitmapData);
                        return bmp;
                    }

                    break;
                case 3:
                    colorType = typeof(Bgr);
                    break;
                case 4:
                    colorType = typeof(Bgra);
                    break;
                default:
                    throw new Exception("Unknown color type");
            }

            return RawDataToBitmap(mat.DataPointer, mat.Step, s, colorType, mat.NumberOfChannels,
                CvInvoke.GetDepthType(mat.Depth), tryDataSharing);
        }


        /// <summary>
        /// Convert the umat into Bitmap, the pixel values are copied over to the Bitmap
        /// </summary>
        /// <param name="umat">The UMat to be converted to Bitmap</param>
        /// <returns>A bitmap representation of the image.</returns>
        public static Bitmap ToBitmap(this UMat umat)
        {
            using (Mat tmp = umat.GetMat(CvEnum.AccessType.Read))
            {
                return tmp.ToBitmap();
            }
        }

        /// <summary>
        /// Convert the gpuMat into Bitmap, the pixel values are copied over to the Bitmap
        /// </summary>
        /// <param name="gpuMat">The gpu mat to be converted to Bitmap</param>
        /// <returns>A bitmap representation of the image.</returns>
        public static Bitmap ToBitmap(this GpuMat gpuMat)
        {
            using (Mat tmp = new Mat())
            {
                gpuMat.Download(tmp);
                return tmp.ToBitmap();
            }
        }

        /// <summary>
        /// Create a Mat from Bitmap
        /// </summary>
        /// <param name="bitmap">The Bitmap to be converted to Mat</param>
        /// <returns>The Mat converted from Bitmap</returns>
        public static Mat ToMat(this Bitmap bitmap)
        {
            Mat m = new Mat();
            bitmap.ToMat(m);
            return m;
        }

        /// <summary>
        /// Create a Mat from Bitmap
        /// </summary>
        /// <param name="bitmap">The Bitmap to be converted to Mat</param>
        /// <param name="mat">The Mat converted from Bitmap.
        /// If Bitmap is 3-channel color image, the Mat will be in Bgr format.
        /// If Bitmap is 4-channel, the Mat will be in Bgra format.</param>
        public static void ToMat(this Bitmap bitmap, Mat mat)
        {
            Size size = bitmap.Size;

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format32bppRgb:
                    BitmapData data32bppRgb = bitmap.LockBits(
                        new Rectangle(Point.Empty, size),
                        ImageLockMode.ReadOnly,
                        bitmap.PixelFormat);
                    try
                    {
                        using (Mat tmp =
                            new Mat(bitmap.Size, DepthType.Cv8U, 4, data32bppRgb.Scan0, data32bppRgb.Stride))
                        {
                            CvInvoke.MixChannels(tmp, mat, new[] { 0, 0, 1, 1, 2, 2 });
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(data32bppRgb);
                    }
                    return;
                case PixelFormat.Format32bppArgb:
                    BitmapData data32bppArgb = bitmap.LockBits(
                        new Rectangle(Point.Empty, size),
                        ImageLockMode.ReadOnly,
                        bitmap.PixelFormat);
                    try
                    {
                        using (Mat tmp =
                            new Mat(bitmap.Size, DepthType.Cv8U, 4, data32bppArgb.Scan0, data32bppArgb.Stride))
                        {
                            tmp.CopyTo(mat);
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(data32bppArgb);
                    }

                    return;
                case PixelFormat.Format8bppIndexed:
                    //Mat imageFrom8bppIndexed = new Mat();
                    Mat bTable, gTable, rTable, aTable;
                    ColorPaletteToLookupTable(bitmap.Palette, out bTable, out gTable, out rTable, out aTable);
                    BitmapData data8bppIndexed = bitmap.LockBits(
                        new Rectangle(Point.Empty, size),
                        ImageLockMode.ReadOnly,
                        bitmap.PixelFormat);
                    try
                    {
                        using (Mat indexValue =
                            new Mat(bitmap.Size, DepthType.Cv8U, 1, data8bppIndexed.Scan0, data8bppIndexed.Stride))
                        {
                            using (Mat b = new Mat())
                            using (Mat g = new Mat())
                            using (Mat r = new Mat())
                            using (Mat a = new Mat())
                            {
                                CvInvoke.LUT(indexValue, bTable, b);
                                CvInvoke.LUT(indexValue, gTable, g);
                                CvInvoke.LUT(indexValue, rTable, r);
                                CvInvoke.LUT(indexValue, aTable, a);
                                using (VectorOfMat mv = new VectorOfMat(new Mat[] { b, g, r, a }))
                                {
                                    CvInvoke.Merge(mv, mat);
                                }
                            }
                        }
                    }
                    finally
                    {
                        bTable.Dispose();
                        gTable.Dispose();
                        rTable.Dispose();
                        aTable.Dispose();
                        bitmap.UnlockBits(data8bppIndexed);
                    }
                    return;
                case PixelFormat.Format24bppRgb:
                    //Mat imageFrom24bppRgb = new Mat();
                    BitmapData data24bppRgb = bitmap.LockBits(
                        new Rectangle(Point.Empty, size),
                        ImageLockMode.ReadOnly,
                        bitmap.PixelFormat);
                    try
                    {
                        using (Mat tmp =
                            new Mat(bitmap.Size, DepthType.Cv8U, 3, data24bppRgb.Scan0, data24bppRgb.Stride))
                        {
                            tmp.CopyTo(mat);
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(data24bppRgb);
                    }
                    return;
                case PixelFormat.Format1bppIndexed:
                    //Mat imageFrom1bppIndexed = new Mat();
                    int rows = size.Height;
                    int cols = size.Width;
                    BitmapData data1bppIndexed = bitmap.LockBits(
                        new Rectangle(Point.Empty, size),
                        ImageLockMode.ReadOnly,
                        bitmap.PixelFormat);

                    int fullByteCount = cols >> 3;
                    int partialBitCount = cols & 7;

                    int mask = 1 << 7;

                    Int64 srcAddress = data1bppIndexed.Scan0.ToInt64();
                    Byte[,] imagedata = new byte[rows, cols];

                    Byte[] row = new byte[fullByteCount + (partialBitCount == 0 ? 0 : 1)];

                    int v = 0;
                    for (int i = 0; i < rows; i++, srcAddress += data1bppIndexed.Stride)
                    {
                        Marshal.Copy((IntPtr)srcAddress, row, 0, row.Length);

                        for (int j = 0; j < cols; j++, v <<= 1)
                        {
                            if ((j & 7) == 0)
                            {
                                //fetch the next byte 
                                v = row[j >> 3];
                            }

                            imagedata[i, j] = (v & mask) == 0 ? (Byte)0 : (Byte)255;
                        }
                    }
                    GCHandle imageDataHandle = GCHandle.Alloc(imagedata, GCHandleType.Pinned);
                    try
                    {
                        using (Mat tmp = new Mat(new int[] { rows, cols }, DepthType.Cv8U,
                            imageDataHandle.AddrOfPinnedObject()))
                        {
                            tmp.CopyTo(mat);
                        }
                    }
                    finally
                    {
                        imageDataHandle.Free();
                        bitmap.UnlockBits(data1bppIndexed);
                    }
                    return;
                default:
                    #region Handle other image type
                    //Mat imageDefault = new Mat();
                    Byte[,,] data = new byte[size.Height, size.Width, 4];
                    for (int i = 0; i < size.Width; i++)
                        for (int j = 0; j < size.Height; j++)
                        {
                            Color color = bitmap.GetPixel(i, j);
                            data[j, i, 0] = color.B;
                            data[j, i, 1] = color.G;
                            data[j, i, 2] = color.R;
                            data[j, i, 3] = color.A;
                        }
                    GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    try
                    {
                        using (Mat tmp = new Mat(new int[] { size.Height, size.Width, 4 }, DepthType.Cv8U,
                            dataHandle.AddrOfPinnedObject()))
                        {
                            tmp.CopyTo(mat);
                        }
                    }
                    finally
                    {
                        dataHandle.Free();
                    }
                    return;
                    #endregion
            }
        }

    }

    /// <summary>
    /// Class that can be used to read file into Mat
    /// </summary>
    public class BitmapFileReaderMat : Emgu.CV.IFileReaderMat
    {
        /// <summary>
        /// Read the file into a Mat
        /// </summary>
        /// <param name="fileName">The name of the image file</param>
        /// <param name="mat">The Mat to read into</param>
        /// <param name="loadType">Image load type.</param>
        /// <returns>True if the file can be read into the Mat</returns>
        public bool ReadFile(String fileName, Mat mat, CvEnum.ImreadModes loadType)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(fileName))
                {
                    if (loadType.HasFlag(ImreadModes.AnyColor) || loadType.HasFlag(ImreadModes.ColorBgr))
                        bmp.ToMat(mat);
                    else if (loadType.HasFlag(ImreadModes.Grayscale))
                    {
                        using (Mat tmp = new Mat())
                        {
                            bmp.ToMat(tmp);
                            if (tmp.NumberOfChannels == 3)
                            {
                                CvInvoke.CvtColor(tmp, mat, ColorConversion.Bgr2Gray);
                            }
                            else if (tmp.NumberOfChannels == 4)
                            {
                                CvInvoke.CvtColor(tmp, mat, ColorConversion.Bgra2Gray);
                            }
                            else
                            {
                                throw new NotImplementedException(String.Format(
                                    "Converting {0} channels Bitmap to Grayscale is not supported.",
                                        tmp.NumberOfChannels));
                            }
                        }

                    }
                    else
                    {
                        throw new NotImplementedException(String.Format(
                            "Converting Bitmap image of type {0} to Mat is not implemented.", bmp.PixelFormat));
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                //throw;
                return false;
            }

        }
    }

    /// <summary>
    /// Class that can be used to write the Mat to a file
    /// </summary>
    public class BitmapFileWriterMat : Emgu.CV.IFileWriterMat
    {
        /// <summary>
        /// Write the Mat into the file
        /// </summary>
        /// <param name="mat">The Mat to write</param>
        /// <param name="fileName">The name of the file to be written into</param>
        /// <returns>True if the file has been written into Mat</returns>
        public bool WriteFile(Mat mat, String fileName)
        {
            try
            {
                //Try to save the image using .NET's Bitmap class
                String extension = System.IO.Path.GetExtension(fileName);
                if (!String.IsNullOrEmpty(extension))
                    using (Bitmap bmp = mat.ToBitmap())
                    {
                        switch (extension.ToLower())
                        {
                            case ".jpg":
                            case ".jpeg":
                                bmp.Save(fileName, ImageFormat.Jpeg);
                                break;
                            case ".bmp":
                                bmp.Save(fileName, ImageFormat.Bmp);
                                break;
                            case ".png":
                                bmp.Save(fileName, ImageFormat.Png);
                                break;
                            case ".tiff":
                            case ".tif":
                                bmp.Save(fileName, ImageFormat.Tiff);
                                break;
                            case ".gif":
                                bmp.Save(fileName, ImageFormat.Gif);
                                break;
                            default:
                                throw new NotImplementedException(String.Format("Saving to {0} format is not supported", extension));
                        }
                    }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                //throw;
                return false;
            }
        }
    }
}