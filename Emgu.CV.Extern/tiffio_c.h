//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------


#pragma once
#ifndef EMGU_TIFFIO_H
#define EMGU_TIFFIO_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"

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

CVAPI(void) tiffWriteImageSize(TIFF* pTiff, CvSize* imageSize);

CVAPI(void) tiffWriteImageInfo(TIFF* pTiff, int bitsPerSample, int samplesPerPixel);

CVAPI(void) tiffWriteImage(TIFF* pTiff, IplImage* image);

CVAPI(void) tiffWriteTile(TIFF* pTiff, int row, int col, IplImage* tileImage);

CVAPI(void) tiffWriteTileInfo(TIFF* pTiff, CvSize* tileSize);

CVAPI(void) tiffWriterClose(TIFF** pTiff);

CVAPI(void) tiffWriteGeoTag(TIFF* pTiff, double* ModelTiepoint, double* ModelPixelScale);

#endif

