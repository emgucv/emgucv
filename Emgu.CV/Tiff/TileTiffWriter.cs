//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
      public extern static void tiffWriteTileInfo(IntPtr pTiff, ref Size tileSize);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void tiffWriteTile(IntPtr pTiff, int row, int col, IntPtr tileImage);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int tiffTileRowSize(IntPtr pTiff);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int tiffTileSize(IntPtr pTiff);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void tiffWriteImageSize(IntPtr pTiff, ref Size imageSize);
      #endregion
   }

   /// <summary>
   /// A writer for writing GeoTiff
   /// </summary>
   /// <typeparam name="TColor">The color type of the image to be written</typeparam>
   /// <typeparam name="TDepth">The depth type of the image to be written</typeparam>
   public class TileTiffWriter<TColor, TDepth> : TiffWriter<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
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
      public void WriteTile(int rowNumber, int colNumber, Image<TColor, TDepth> tile)
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
      }

      /// <summary>
      /// Write the whole image as tile tiff
      /// </summary>
      /// <param name="image">The image to be written</param>
      public override void WriteImage(Image<TColor, TDepth> image)
      {
         Rectangle originalROI = image.ROI;
         int sizeOfElement = Image<TColor, TDepth>.SizeOfElement;
         int tileRowSizeInBytes = TileRowSizeInBytes;
         Size tileSize = TileSize;
         Size imageSize = originalROI.Equals(Rectangle.Empty) ? image.Size : originalROI.Size;

         for (int row = 0; row < image.Height; row += tileSize.Height)
         {
            int rowsToCopy = tileSize.Height < (imageSize.Height - row) ? tileSize.Height : (imageSize.Height - row);

            for (int col = 0; col < imageSize.Width; col += tileSize.Width)
            {
               int actualRowLength = ((col + tileSize.Width) <= imageSize.Width) ? tileRowSizeInBytes : (imageSize.Width % tileSize.Width) * sizeOfElement;

               image.ROI = new Rectangle(originalROI.Y + col, originalROI.X + row, actualRowLength / sizeOfElement, rowsToCopy);

               WriteTile(row, col, image);
            }
         }

         image.ROI = originalROI;
      }
   }
}
