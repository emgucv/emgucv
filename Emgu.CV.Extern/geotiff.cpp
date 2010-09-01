#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"
#include "geotiff.h"
#include "geo_tiffp.h"
#include "geotiffio.h" //writing geotiff
#include "xtiffio.h"
#include "transformationWGS84.h"

CVAPI(void) geotiffWriteImage(char* fileSpec, IplImage* image, GeodeticCoordinate* coor, CvPoint3D64f* pixelSize)
{
   double latitude = coor->latitude * (180.0 / CV_PI);
   double longitude = coor->longitude * (180.0 / CV_PI);
   double ModelTiepoint[6] = { 
      0, 0, 0, 
      longitude, latitude, 0 };
   double ModelPixelScale[3] = { pixelSize->x, pixelSize->y, pixelSize->z };

   cv::Mat mat = cv::cvarrToMat(image);
   TIFF *pTiff = XTIFFOpen(fileSpec, "w");
   TIFFSetField(pTiff, TIFFTAG_ORIENTATION, ORIENTATION_TOPLEFT);
   TIFFSetField(pTiff, TIFFTAG_PLANARCONFIG, PLANARCONFIG_CONTIG);
   TIFFSetField(pTiff, TIFFTAG_COMPRESSION, COMPRESSION_NONE);
   TIFFSetField(pTiff, TIFFTAG_IMAGEWIDTH, mat.cols);
   TIFFSetField(pTiff, TIFFTAG_IMAGELENGTH, mat.rows);
   //int esize1 = mat.elemSize1();
   //int esize = mat.elemSize();
   //int counte = esize1 + esize;
   TIFFSetField(pTiff, TIFFTAG_BITSPERSAMPLE, mat.elemSize1()*8); 
   TIFFSetField(pTiff, TIFFTAG_SAMPLESPERPIXEL, mat.channels());
   
   TIFFSetField(pTiff, TIFFTAG_PHOTOMETRIC, 
      mat.channels() == 1 ? 1 //BlackIsZero. For bilevel and grayscale images: 0 is imaged as black.
      : 2 //RGB. RGB value of (0,0,0) represents black, and (255,255,255) represents white, assuming 8-bit components. The components are stored in the indicated order: first Red, then Green, then Blue.
      );

   //for RGBA, define the fourth channel as alpha
   if (mat.channels() == 4)
   {
      uint16 extraSampleType[] = {EXTRASAMPLE_UNASSALPHA};
      TIFFSetField(pTiff, TIFFTAG_EXTRASAMPLES, 1, extraSampleType);
   }

   //write scaneline image data
   for (int row = 0; row < mat.rows; row++)
   {
      TIFFWriteScanline(pTiff, mat.ptr(row), row, 0);
   }
   //end writing image data

   TIFFSetField(pTiff, GTIFF_TIEPOINTS,  6, ModelTiepoint);
   TIFFSetField(pTiff, GTIFF_PIXELSCALE, 3, ModelPixelScale);

   GTIF* gTiff = GTIFNew(pTiff);

   GTIFKeySet(gTiff, GTModelTypeGeoKey, TYPE_SHORT, 1, ModelTypeGeographic);
   GTIFKeySet(gTiff, GTRasterTypeGeoKey, TYPE_SHORT, 1, RasterPixelIsArea);
   GTIFKeySet(gTiff, GeographicTypeGeoKey, TYPE_SHORT, 1, GCS_WGS_84);
   GTIFKeySet(gTiff, GeogAngularUnitsGeoKey, TYPE_SHORT, 1, Angular_Degree);
   GTIFWriteKeys(gTiff);
   GTIFFree(gTiff);

   TIFFWriteDirectory(pTiff);
   XTIFFClose(pTiff);
}