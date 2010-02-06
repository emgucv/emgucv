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
CVAPI(void) transformGeodetic2ENU(geodeticCoordinate* coor, geodeticCoordinate* refCoor, CvPoint3D64f* refEcef, CvPoint3D64f* enu);
CVAPI(void) transformENU2Geodetic(CvPoint3D64f* enu, geodeticCoordinate* refCoor, CvPoint3D64f* refEcef, geodeticCoordinate* coor);
CVAPI(void) transformGeodetic2NED(geodeticCoordinate* coor, geodeticCoordinate* refCoor, CvPoint3D64f* refEcef, CvPoint3D64f* ned);
CVAPI(void) transformNED2Geodetic(CvPoint3D64f* ned, geodeticCoordinate* refCoor, CvPoint3D64f* refEcef, geodeticCoordinate* coor);
