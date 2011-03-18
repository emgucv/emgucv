//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Use zlib included in OpenCV to perform in-memory binary compression and decompression
   /// </summary>
   internal static class ZlibCompression
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int zlib_compress_bound(int length);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void zlib_compress2(IntPtr dataCompressed, ref int sizeDataCompressed, IntPtr dataOriginal, int sizeDataOriginal, int compressionLevel);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void zlib_uncompress(IntPtr dataUncompressed, ref int sizeDataUncompressed, IntPtr compressedData, int sizeDataCompressed);
      #endregion

      /// <summary>
      /// Compress the data using the specific compression level
      /// </summary>
      /// <param name="original">The data to be compressed</param>
      /// <param name="compressionLevel">The compression level, 0-9 where 0 mean no compression at all</param>
      /// <returns>The compressed bytes</returns>
      public static Byte[] Compress(Byte[] original, int compressionLevel)
      {
         Byte[] result = new Byte[zlib_compress_bound(original.Length)];
         GCHandle originalHandle = GCHandle.Alloc(original, GCHandleType.Pinned);
         GCHandle resultHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
         int compressDataSize = result.Length;
         zlib_compress2(resultHandle.AddrOfPinnedObject(), ref compressDataSize, originalHandle.AddrOfPinnedObject(), original.Length, compressionLevel);

         originalHandle.Free();
         resultHandle.Free();

         Array.Resize(ref result, compressDataSize);
         return result;
      }

      /// <summary>
      /// Uncompress the data 
      /// </summary>
      /// <param name="compressedData">The compressed data</param>
      /// <param name="estimatedUncompressedSize">The estimated size fo the uncompress data. Must be large enough to hold the decompressed data.</param>
      /// <returns>The decompressed data</returns>
      public static Byte[] Uncompress(Byte[] compressedData, int estimatedUncompressedSize)
      {
         Byte[] result = new Byte[estimatedUncompressedSize];
         GCHandle originalHandle = GCHandle.Alloc(compressedData, GCHandleType.Pinned);
         GCHandle resultHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
         int uncompressDataSize = estimatedUncompressedSize;
         zlib_uncompress(resultHandle.AddrOfPinnedObject(), ref uncompressDataSize, originalHandle.AddrOfPinnedObject(), compressedData.Length);

         originalHandle.Free();
         resultHandle.Free();

         if (uncompressDataSize != estimatedUncompressedSize)
            Array.Resize(ref result, uncompressDataSize);

         return result;
      }
   }
}
