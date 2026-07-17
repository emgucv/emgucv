//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Util
{
    /// <summary>
    /// Use zlib included in OpenCV to perform in-memory binary compression and decompression
    /// </summary>
    public static class ZlibCompression
    {
        /// <summary>
        /// Get the maximum size of the buffer required to hold the result of compressing <paramref name="length"/> bytes of data
        /// </summary>
        /// <param name="length">The size of the uncompressed data, in bytes</param>
        /// <returns>The maximum size of the buffer required to hold the compressed data</returns>
        public static int CompressBound(int length)
        {
            return CvInvoke.zlib_compress_bound(length);
        }

        /// <summary>
        /// Compress the data using the specific compression level
        /// </summary>
        /// <param name="original">The data to be compressed</param>
        /// <param name="compressionLevel">The compression level, 0-9 where 0 mean no compression at all</param>
        /// <returns>The compressed bytes</returns>
        public static Byte[] Compress(Byte[] original, int compressionLevel)
        {
            Byte[] result = new Byte[CompressBound(original.Length)];
            int compressDataSize;
            GCHandle originalHandle = GCHandle.Alloc(original, GCHandleType.Pinned);
            GCHandle resultHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
            try
            {
                compressDataSize = Compress(
                    originalHandle.AddrOfPinnedObject(),
                    original.Length,
                    resultHandle.AddrOfPinnedObject(),
                    result.Length,
                    compressionLevel);
            }
            finally
            {
                originalHandle.Free();
                resultHandle.Free();
            }

            Array.Resize(ref result, compressDataSize);
            return result;
        }

        /// <summary>
        /// Compress the data in an unmanaged buffer (e.g. Mat.DataPointer) using the specific compression level, without copying the data into a managed array
        /// </summary>
        /// <param name="original">Pointer to the data to be compressed</param>
        /// <param name="originalSize">The size of the data to be compressed, in bytes</param>
        /// <param name="compressed">Pointer to the buffer that receives the compressed data</param>
        /// <param name="compressedSize">The size of the buffer that receives the compressed data. Use CompressBound to compute a size that is always large enough.</param>
        /// <param name="compressionLevel">The compression level, 0-9 where 0 mean no compression at all</param>
        /// <returns>The number of bytes written to <paramref name="compressed"/></returns>
        public static int Compress(IntPtr original, int originalSize, IntPtr compressed, int compressedSize, int compressionLevel)
        {
            CvInvoke.zlib_compress2(compressed, ref compressedSize, original, originalSize, compressionLevel);
            return compressedSize;
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
            int uncompressDataSize;
            GCHandle originalHandle = GCHandle.Alloc(compressedData, GCHandleType.Pinned);
            GCHandle resultHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
            try
            {
                uncompressDataSize = Uncompress(
                    originalHandle.AddrOfPinnedObject(),
                    compressedData.Length,
                    resultHandle.AddrOfPinnedObject(),
                    estimatedUncompressedSize);
            }
            finally
            {
                originalHandle.Free();
                resultHandle.Free();
            }

            if (uncompressDataSize != estimatedUncompressedSize)
                Array.Resize(ref result, uncompressDataSize);

            return result;
        }

        /// <summary>
        /// Uncompress the data into an unmanaged buffer (e.g. Mat.DataPointer), without copying the data into a managed array
        /// </summary>
        /// <param name="compressed">Pointer to the compressed data</param>
        /// <param name="compressedSize">The size of the compressed data, in bytes</param>
        /// <param name="uncompressed">Pointer to the buffer that receives the decompressed data. Must be large enough to hold the decompressed data.</param>
        /// <param name="uncompressedSize">The size of the buffer that receives the decompressed data</param>
        /// <returns>The number of bytes written to <paramref name="uncompressed"/></returns>
        public static int Uncompress(IntPtr compressed, int compressedSize, IntPtr uncompressed, int uncompressedSize)
        {
            CvInvoke.zlib_uncompress(uncompressed, ref uncompressedSize, compressed, compressedSize);
            return uncompressedSize;
        }
    }
}
