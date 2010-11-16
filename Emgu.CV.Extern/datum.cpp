/**
* @file datum.cpp
*
* @brief   Implement functions for datum
**/
#include "datum.h"

CVAPI(datum*) datumCreateWGS84() { return new datum; }

CVAPI(datum*) datumCreate(double a, double b) { return new datum(a, b); }

CVAPI(void) datumRelease(datum** d) { delete *d; }

/**
 * @fn   void transformGeodetic2ECEF(const datum* d, const GeodeticCoordinate* coordinate,
 * CvPoint3D64f* ecef)
 *
 * @brief   Convert geodetic coordinate to ECEF coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformGeodetic2ECEF(const datum* d, const GeodeticCoordinate* coordinate, CvPoint3D64f* ecef)
{
   d->transformGeodetic2ECEF(coordinate, ecef);
}

/**
 * @fn   void transformECEF2Geodetic(const datum* d, const CvPoint3D64f* ecef,
 * GeodeticCoordinate* coordinate)
 *
 * @brief   Convert ECEF coordinate to geodetic coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformECEF2Geodetic(const datum* d, const CvPoint3D64f* ecef, GeodeticCoordinate* coordinate)
{
   d->transformECEF2Geodetic(ecef, coordinate);
}

/**
 * @fn   void transformGeodetic2ENU(const datum* d, const GeodeticCoordinate* coor,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu)
 *
 * @brief   Convert geodetic coordinate to ENU (East North UP) using the reference coordinate
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformGeodetic2ENU(const datum* d, const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu)
{
   d->transformGeodetic2ENU(coor, refCoor, refEcef, enu);
}

/**
 * @fn   void transformENU2Geodetic(const datum* d, const CvPoint3D64f* enu,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
 *
 * @brief   Convert ENU (East North UP) to geodetic coordinate using the reference coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformENU2Geodetic(const datum* d, const CvPoint3D64f* enu, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
   d->transformENU2Geodetic(enu, refCoor, refEcef, coor);
}

/**
 * @fn   void transformGeodetic2NED(const datum* d, const GeodeticCoordinate* coor,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned)
 *
 * @brief   Convert geodetic coordinate to NED (North East Down) using the reference coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformGeodetic2NED(const datum* d, const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned)
{
   d->transformGeodetic2NED(coor, refCoor, refEcef, ned);
}

/**
 * @fn   void transformNED2Geodetic(const datum* d, const CvPoint3D64f* ned,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
 *
 * @brief   Convert NED (North East Down) to geodetic coordinate using the reference coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformNED2Geodetic(const datum* d, const CvPoint3D64f* ned, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
   d->transformNED2Geodetic(ned, refCoor, refEcef, coor);
}






