#pragma once
#ifndef TRANSFORMATION_WGS84_H
#define TRANSFORMATION_WGS84_H

#include "opencv2/core/core_c.h"
#include <math.h>
#include "sse.h"

/**
 * @struct  GeodeticCoordinate
 *
 * @brief   A Geodetic Coordinate is defined by its latitude, longitude and altitude
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
typedef struct GeodeticCoordinate
{

   /// The latitude (phi) in radian
   double latitude;
   
   /// The longitude (lambda) in radian
   double longitude;

   /// The altitude (height) in meters
   double altitude;
} 
GeodeticCoordinate;

/**
 * @fn   void transformGeodetic2ECEF(const GeodeticCoordinate* coordinate,
 * CvPoint3D64f* ecef)
 *
 * @brief   Convert geodetic coordinate to ECEF coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformGeodetic2ECEF(const GeodeticCoordinate* coordinate, CvPoint3D64f* ecef);

/**
 * @fn   void transformECEF2Geodetic(const CvPoint3D64f* ecef,
 * GeodeticCoordinate* coordinate)
 *
 * @brief   Convert ECEF coordinate to geodetic coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformECEF2Geodetic(const CvPoint3D64f* ecef, GeodeticCoordinate* coordinate);

/**
 * @fn   void transformGeodetic2ENU(const GeodeticCoordinate* coor,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu)
 *
 * @brief   Convert geodetic coordinate to ENU (East North UP) using the reference coordinate
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformGeodetic2ENU(const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu);

/**
 * @fn   void transformENU2Geodetic(const CvPoint3D64f* enu,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
 *
 * @brief   Convert ENU (East North UP) to geodetic coordinate using the reference coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformENU2Geodetic(const CvPoint3D64f* enu, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor);

/**
 * @fn   void transformGeodetic2NED(const GeodeticCoordinate* coor,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned)
 *
 * @brief   Convert geodetic coordinate to NED (North East Down) using the reference coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformGeodetic2NED(const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned);

/**
 * @fn   void transformNED2Geodetic(const CvPoint3D64f* ned,
 * const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
 *
 * @brief   Convert NED (North East Down) to geodetic coordinate using the reference coordinate 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) transformNED2Geodetic(const CvPoint3D64f* ned, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor);

#endif