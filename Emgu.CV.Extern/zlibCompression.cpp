//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "zlibCompression.h"

int zlib_compress_bound(int length) { return (int) compressBound(length); };
void zlib_compress2(Byte* dataCompressed, int* sizeDataCompressed, Byte* dataOriginal, int sizeDataOriginal, int compressionLevel)
{
   uLongf sizeCompressed = *sizeDataCompressed;
   int z_result = compress2(dataCompressed, &sizeCompressed, dataOriginal, sizeDataOriginal, compressionLevel);
   switch(z_result)
   {
   case Z_BUF_ERROR:
      CV_Error( CV_StsError, "Output buffer wasn't large enough\n");
      break;
   case Z_MEM_ERROR:
      CV_Error( CV_StsError, "Out of memory\n");
      break;
   default:
      *sizeDataCompressed = (int) sizeCompressed;
   }
}

void zlib_uncompress(Byte* dataUncompressed, int* sizeDataUncompressed, Byte* compressedData, int sizeDataCompressed)
{
   uLongf sizeUncompressed = *sizeDataUncompressed;
   int z_result = uncompress(dataUncompressed, &sizeUncompressed, compressedData, sizeDataCompressed);
   switch(z_result)
   {
   case Z_BUF_ERROR:
      CV_Error( CV_StsError, "Output buffer wasn't large enough\n");
      break;
   case Z_MEM_ERROR:
      CV_Error( CV_StsError, "Out of memory\n");
      break;
   default:
      *sizeDataUncompressed = (int) sizeUncompressed;
   }
}