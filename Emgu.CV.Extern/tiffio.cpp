//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "tiffio_c.h"

TIFF* tiffWriterOpen(char* fileName)
{
#ifdef EMGU_CV_WITH_TIFF
	return XTIFFOpen(fileName, "w");
#else
	throw_no_tiff();
#endif
}

int tiffTileRowSize(TIFF* pTiff)
{
#ifdef EMGU_CV_WITH_TIFF
	return TIFFTileRowSize(pTiff);
#else
	throw_no_tiff();
#endif
}

int tiffTileSize(TIFF* pTiff)
{
#ifdef EMGU_CV_WITH_TIFF
	return TIFFTileSize(pTiff);
#else
	throw_no_tiff();
#endif
}

void tiffWriteImageSize(TIFF* pTiff, CvSize* imageSize)
{
#ifdef EMGU_CV_WITH_TIFF
	TIFFSetField(pTiff, TIFFTAG_IMAGEWIDTH, imageSize->width);
	TIFFSetField(pTiff, TIFFTAG_IMAGELENGTH, imageSize->height);
#else
	throw_no_tiff();
#endif
}

void tiffWriteImageInfo(TIFF* pTiff, int bitsPerSample, int samplesPerPixel)
{
#ifdef EMGU_CV_WITH_TIFF
	TIFFSetField(pTiff, TIFFTAG_COMPRESSION, COMPRESSION_NONE);
	TIFFSetField(pTiff, TIFFTAG_ORIENTATION, ORIENTATION_TOPLEFT);
	TIFFSetField(pTiff, TIFFTAG_PLANARCONFIG, PLANARCONFIG_CONTIG);

	TIFFSetField(pTiff, TIFFTAG_BITSPERSAMPLE, bitsPerSample);
	TIFFSetField(pTiff, TIFFTAG_SAMPLESPERPIXEL, samplesPerPixel);

	TIFFSetField(pTiff, TIFFTAG_PHOTOMETRIC,
		samplesPerPixel == 1 ? 1 //BlackIsZero. For bilevel and grayscale images: 0 is imaged as black.
		: 2 //RGB. RGB value of (0,0,0) represents black, and (255,255,255) represents white, assuming 8-bit components. The components are stored in the indicated order: first Red, then Green, then Blue.
	);

	//for RGBA, define the fourth channel as alpha
	if (samplesPerPixel == 4)
	{
		uint16 extraSampleType[] = { EXTRASAMPLE_UNASSALPHA };
		TIFFSetField(pTiff, TIFFTAG_EXTRASAMPLES, 1, extraSampleType);
	}
#else
	throw_no_tiff();
#endif
}

void tiffWriteImage(TIFF* pTiff, IplImage* image)
{
#ifdef EMGU_CV_WITH_TIFF
	cv::Mat mat = cv::cvarrToMat(image);
	CvSize imageSize = cvSize(image->width, image->height);
	tiffWriteImageSize(pTiff, &imageSize);

	//write scaneline image data
	for (int row = 0; row < mat.rows; row++)
	{
		TIFFWriteScanline(pTiff, mat.ptr(row), row, 0);
	}
	//end writing image data
#else
	throw_no_tiff();
#endif
}

void tiffWriteTile(TIFF* pTiff, int row, int col, IplImage* tileImage)
{
#ifdef EMGU_CV_WITH_TIFF
	cv::Mat tile = cv::cvarrToMat(tileImage);

	int bufferStride = tile.cols * tile.elemSize();
	unsigned char* buffer = (unsigned char*)malloc(tile.rows * bufferStride);
	unsigned char* ptr = buffer;

	for (int i = 0; i < tile.rows; i++, ptr += bufferStride)
		memcpy(ptr, tile.ptr(i), bufferStride);

	TIFFWriteTile(pTiff, buffer, col, row, 0, 0);
	free(buffer);
#else
	throw_no_tiff();
#endif
}

void tiffWriteTileInfo(TIFF* pTiff, CvSize* tileSize)
{
#ifdef EMGU_CV_WITH_TIFF
	TIFFSetField(pTiff, TIFFTAG_TILEWIDTH, tileSize->width);
	TIFFSetField(pTiff, TIFFTAG_TILELENGTH, tileSize->height);
#else
	throw_no_tiff();
#endif
}

void tiffWriteGeoTag(TIFF* pTiff, double* ModelTiepoint, double* ModelPixelScale)
{
#ifdef EMGU_CV_WITH_TIFF
	TIFFSetField(pTiff, GTIFF_TIEPOINTS, 6, ModelTiepoint);
	TIFFSetField(pTiff, GTIFF_PIXELSCALE, 3, ModelPixelScale);

	GTIF* gTiff = GTIFNew(pTiff);
	GTIFKeySet(gTiff, GTModelTypeGeoKey, TYPE_SHORT, 1, ModelTypeGeographic);
	GTIFKeySet(gTiff, GTRasterTypeGeoKey, TYPE_SHORT, 1, RasterPixelIsArea);
	GTIFKeySet(gTiff, GeographicTypeGeoKey, TYPE_SHORT, 1, GCS_WGS_84);
	GTIFKeySet(gTiff, GeogAngularUnitsGeoKey, TYPE_SHORT, 1, Angular_Degree);
	GTIFWriteKeys(gTiff);
	GTIFFree(gTiff);
#else
	throw_no_tiff();
#endif
}

void tiffWriterClose(TIFF** pTiff)
{
#ifdef EMGU_CV_WITH_TIFF
	TIFFWriteDirectory(*pTiff);
	XTIFFClose(*pTiff);
	*pTiff = 0;
#else
	throw_no_tiff();
#endif
}

