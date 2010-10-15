#pragma warning( disable: 4251 )

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"
#include "geotiff.h"
#include "geo_tiffp.h"
#include "geotiffio.h" //writing geotiff
#include "xtiffio.h"
#include "datum.h"

CVAPI(TIFF*) tiffWriterOpen(char* fileName)
{
   return XTIFFOpen(fileName, "w");
}

CVAPI(int) tiffTileRowSize(TIFF* pTiff)
{
   return TIFFTileRowSize(pTiff);
}

CVAPI(int) tiffTileSize(TIFF* pTiff)
{
   return TIFFTileSize(pTiff);
}

CVAPI(void) tiffWriteImageSize(TIFF* pTiff, CvSize* imageSize)
{
   TIFFSetField(pTiff, TIFFTAG_IMAGEWIDTH, imageSize->width);
   TIFFSetField(pTiff, TIFFTAG_IMAGELENGTH, imageSize->height);

}

CVAPI(void) tiffWriteImageInfo(TIFF* pTiff, int bitsPerSample, int samplesPerPixel)
{
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
      uint16 extraSampleType[] = {EXTRASAMPLE_UNASSALPHA};
      TIFFSetField(pTiff, TIFFTAG_EXTRASAMPLES, 1, extraSampleType);
   }
}

CVAPI(void) tiffWriteImage(TIFF* pTiff, IplImage* image)
{
   cv::Mat mat = cv::cvarrToMat(image);
   CvSize imageSize = cvSize(image->width, image->height);
   tiffWriteImageSize(pTiff, &imageSize);

   //write scaneline image data
   for (int row = 0; row < mat.rows; row++)
   {
      TIFFWriteScanline(pTiff, mat.ptr(row), row, 0);
   }
   //end writing image data
}

CVAPI(void) tiffWriteTile(TIFF* pTiff, int row, int col, IplImage* tileImage)
{
   cv::Mat tile = cv::cvarrToMat(tileImage);
   
   int bufferStride = tile.cols * tile.elemSize();
   unsigned char* buffer = (unsigned char*) malloc(tile.rows * bufferStride);
   unsigned char* ptr = buffer;
   
   for (int i = 0; i < tile.rows; i++, ptr += bufferStride)
      memcpy(ptr, tile.ptr(i), bufferStride);

   TIFFWriteTile(pTiff, buffer, col, row, 0, 0);
   free(buffer);
}

CVAPI(void) tiffWriteTileInfo(TIFF* pTiff, CvSize* tileSize)
{
   TIFFSetField(pTiff, TIFFTAG_TILEWIDTH, tileSize->width);
   TIFFSetField(pTiff, TIFFTAG_TILELENGTH, tileSize->height);
}

CVAPI(void) tiffWriteGeoTag(TIFF* pTiff, double* ModelTiepoint, double* ModelPixelScale)
{
   TIFFSetField(pTiff, GTIFF_TIEPOINTS,  6, ModelTiepoint);
   TIFFSetField(pTiff, GTIFF_PIXELSCALE, 3, ModelPixelScale);

   GTIF* gTiff = GTIFNew(pTiff);
   GTIFKeySet(gTiff, GTModelTypeGeoKey, TYPE_SHORT, 1, ModelTypeGeographic);
   GTIFKeySet(gTiff, GTRasterTypeGeoKey, TYPE_SHORT, 1, RasterPixelIsArea);
   GTIFKeySet(gTiff, GeographicTypeGeoKey, TYPE_SHORT, 1, GCS_WGS_84);
   GTIFKeySet(gTiff, GeogAngularUnitsGeoKey, TYPE_SHORT, 1, Angular_Degree);
   GTIFWriteKeys(gTiff);
   GTIFFree(gTiff);
}

CVAPI(void) tiffWriterClose(TIFF** pTiff)
{
   TIFFWriteDirectory(*pTiff);
   XTIFFClose(*pTiff);
   *pTiff = 0;
}