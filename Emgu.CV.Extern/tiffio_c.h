//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------


#pragma once
#ifndef EMGU_TIFFIO_H
#define EMGU_TIFFIO_H

#include "opencv2/core/core.hpp"
#include "cvapi_compat.h"

#ifdef EMGU_CV_WITH_TIFF
#include "geotiff.h"
#include "geo_tiffp.h"
#include "geotiffio.h" //writing geotiff
#include "xtiffio.h"
#else
#define TIFF void
static inline CV_NORETURN void throw_no_tiff() { CV_Error(cv::Error::StsBadFunc, "cvextern is compiled without tiff support"); }
#endif

CVAPI(TIFF*) tiffWriterOpen(char* fileName);

CVAPI(int) tiffTileRowSize(TIFF* pTiff);

CVAPI(int) tiffTileSize(TIFF* pTiff);

CVAPI(void) tiffWriteImageSize(TIFF* pTiff, cv::Size* imageSize);

CVAPI(void) tiffWriteImageInfo(TIFF* pTiff, int bitsPerSample, int samplesPerPixel);

CVAPI(void) tiffWriteImage(TIFF* pTiff, cv::Mat mat);

CVAPI(void) tiffWriteTile(TIFF* pTiff, int row, int col, cv::Mat* tileImage);

CVAPI(void) tiffWriteTileInfo(TIFF* pTiff, cv::Size* tileSize);

CVAPI(void) tiffWriterClose(TIFF** pTiff);

CVAPI(void) tiffWriteGeoTag(TIFF* pTiff, double* ModelTiepoint, double* ModelPixelScale);

#endif

