#ifndef TRANSFORMATION_WGS84_H
#define TRANSFORMATION_WGS84_H

#include "cxcore.h"
#include <math.h>
#include "sse.h"

typedef struct geodeticCoordinate
{
   double latitude;
   double longitude;
   double altitude;
} 
geodeticCoordinate;

CVAPI(void) transformGeodetic2ECEF(const geodeticCoordinate* coordinate, CvPoint3D64f* ecef);
CVAPI(void) transformECEF2Geodetic(const CvPoint3D64f* ecef, geodeticCoordinate* coordinate);
CVAPI(void) transformGeodetic2ENU(const geodeticCoordinate* coor, const geodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu);
CVAPI(void) transformENU2Geodetic(const CvPoint3D64f* enu, const geodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, geodeticCoordinate* coor);
CVAPI(void) transformGeodetic2NED(const geodeticCoordinate* coor, const geodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned);
CVAPI(void) transformNED2Geodetic(const CvPoint3D64f* ned, const geodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, geodeticCoordinate* coor);

#endif