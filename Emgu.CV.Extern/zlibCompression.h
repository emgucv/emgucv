//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ZLIB_H
#define EMGU_ZLIB_H

#include "zlib.h"
#include "opencv2/core/core.hpp"
#include "opencv2/core/core_c.h"

CVAPI(int) zlib_compress_bound(int length);
CVAPI(void) zlib_compress2(Byte* dataCompressed, int* sizeDataCompressed, Byte* dataOriginal, int sizeDataOriginal, int compressionLevel);
CVAPI(void) zlib_uncompress(Byte* dataUncompressed, int* sizeDataUncompressed, Byte* compressedData, int sizeDataCompressed);

#endif