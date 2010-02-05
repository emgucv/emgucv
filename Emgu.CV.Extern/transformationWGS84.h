#include "cxcore.h"
#include <math.h>
typedef struct geodeticCoordinate
{
   double latitude;
   double longitude;
   double altitude;
} 
geodeticCoordinate;

CVAPI(void) transformGeodetic2ECEF(geodeticCoordinate* coordinate, CvPoint3D64f* ecef);
CVAPI(void) transformECEF2Geodetic(CvPoint3D64f* ecef, geodeticCoordinate* coordinate);
