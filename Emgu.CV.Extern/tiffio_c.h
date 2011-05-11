//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_TIFFIO_H
#define EMGU_TIFFIO_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"
#include "geotiff.h"
#include "geo_tiffp.h"
#include "geotiffio.h" //writing geotiff
#include "xtiffio.h"

CVAPI(TIFF*) tiffWriterOpen(char* fileName);

CVAPI(int) tiffTileRowSize(TIFF* pTiff);

CVAPI(int) tiffTileSize(TIFF* pTiff);

CVAPI(void) tiffWriteImageSize(TIFF* pTiff, CvSize* imageSize);

CVAPI(void) tiffWriteImageInfo(TIFF* pTiff, int bitsPerSample, int samplesPerPixel);

CVAPI(void) tiffWriteImage(TIFF* pTiff, IplImage* image);

CVAPI(void) tiffWriteTile(TIFF* pTiff, int row, int col, IplImage* tileImage);

CVAPI(void) tiffWriteTileInfo(TIFF* pTiff, CvSize* tileSize);

CVAPI(void) tiffWriteGeoTag(TIFF* pTiff, double* ModelTiepoint, double* ModelPixelScale);

CVAPI(void) tiffWriterClose(TIFF** pTiff);

#endif