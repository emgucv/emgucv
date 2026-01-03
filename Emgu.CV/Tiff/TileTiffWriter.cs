//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Tiff
{
    internal static partial class TIFFInvoke
    {
        #region PInvoke
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void tiffWriteTileInfo(IntPtr pTiff, ref Size tileSize);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void tiffWriteTile(IntPtr pTiff, int row, int col, IntPtr tileImage);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern int tiffTileRowSize(IntPtr pTiff);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern int tiffTileSize(IntPtr pTiff);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void tiffWriteImageSize(IntPtr pTiff, ref Size imageSize);
        #endregion
    }

    /// <summary>
    /// A writer for writing GeoTiff
    /// </summary>
    public class TileTiffWriter : TiffWriter
    {

        /// <summary>
        /// Create a TitleTiffWriter.
        /// </summary>
        /// <param name="fileName">The name of the file to be written to</param>
        /// <param name="imageSize">The size of the image</param>
        /// <param name="tileSize">The tile size in pixels</param>
        public TileTiffWriter(String fileName, Size imageSize, Size tileSize)
           : base(fileName)
        {
            TIFFInvoke.tiffWriteImageSize(_ptr, ref imageSize);
            TIFFInvoke.tiffWriteTileInfo(_ptr, ref tileSize);
        }

        /// <summary>
        /// Write a tile into the tile tiff
        /// </summary>
        /// <param name="rowNumber">The starting row for the tile</param>
        /// <param name="colNumber">The starting col for the tile</param>
        /// <param name="tile">The tile to be written</param>
        public void WriteTile(int rowNumber, int colNumber, Mat tile)
        {
            TIFFInvoke.tiffWriteTile(_ptr, rowNumber, colNumber, tile);
        }

        /// <summary>
        /// Get the equivalent size for a tile of data as it would be returned in a call to TIFFReadTile or as it would be expected in a call to TIFFWriteTile. 
        /// </summary>
        public int TileSizeInBytes
        {
            get
            {
                return TIFFInvoke.tiffTileSize(_ptr);
            }
        }

        /// <summary>
        /// Get the number of bytes of a row of data in a tile. 
        /// </summary>
        public int TileRowSizeInBytes
        {
            get
            {
                return TIFFInvoke.tiffTileRowSize(_ptr);
            }
        }

        /*
        /// <summary>
        /// Get tile size in pixels.
        /// </summary>
        public Size TileSize
        {
           get
           {
              int width = TileRowSizeInBytes / (Image<TColor, TDepth>.SizeOfElement * (new TColor().Dimension));
              return new Size(width, TileSizeInBytes / TileRowSizeInBytes);
           }
        }*/

        /// <summary>
        /// Write the whole image as tile tiff
        /// </summary>
        /// <param name="image">The image to be written</param>
        public override void WriteImage(Mat image)
        {
            if (!_imageInfoWritten)
            {
                TIFFInvoke.tiffWriteImageInfo(_ptr, image.ElementSize * 8, image.NumberOfChannels);
                _imageInfoWritten = true;
            }

            Rectangle originalROI = new Rectangle(Point.Empty, image.Size);
            int sizeOfElement = image.ElementSize;
            int tileRowSizeInBytes = TileRowSizeInBytes;

            int width = TileRowSizeInBytes / (image.ElementSize * image.NumberOfChannels);
            Size tileSize = new Size(width, TileSizeInBytes / tileRowSizeInBytes);
            Size imageSize = originalROI.Equals(Rectangle.Empty) ? image.Size : originalROI.Size;

            for (int row = 0; row < image.Height; row += tileSize.Height)
            {
                int rowsToCopy = tileSize.Height < (imageSize.Height - row) ? tileSize.Height : (imageSize.Height - row);

                for (int col = 0; col < imageSize.Width; col += tileSize.Width)
                {
                    int actualRowLength = ((col + tileSize.Width) <= imageSize.Width) ? tileRowSizeInBytes : (imageSize.Width % tileSize.Width) * sizeOfElement;

                    Rectangle tileROI = new Rectangle(originalROI.Y + col, originalROI.X + row, actualRowLength / sizeOfElement, rowsToCopy);

                    using (Mat title = new Mat(image, tileROI))
                        WriteTile(row, col, image);
                }
            }

        }
    }
}
