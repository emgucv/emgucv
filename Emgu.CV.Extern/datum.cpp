//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "datum.h"

datum* datumCreateWGS84() { return new datum; }

datum* datumCreate(double a, double b) { return new datum(a, b); }

void datumRelease(datum** d) { delete *d; }

void transformGeodetic2ECEF(const datum* d, const GeodeticCoordinate* coordinate, CvPoint3D64f* ecef)
{
   d->transformGeodetic2ECEF(coordinate, ecef);
}

void transformECEF2Geodetic(const datum* d, const CvPoint3D64f* ecef, GeodeticCoordinate* coordinate)
{
   d->transformECEF2Geodetic(ecef, coordinate);
}

void transformGeodetic2ENU(const datum* d, const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu)
{
   d->transformGeodetic2ENU(coor, refCoor, refEcef, enu);
}

void transformENU2Geodetic(const datum* d, const CvPoint3D64f* enu, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
   d->transformENU2Geodetic(enu, refCoor, refEcef, coor);
}

void transformGeodetic2NED(const datum* d, const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned)
{
   d->transformGeodetic2NED(coor, refCoor, refEcef, ned);
}

void transformNED2Geodetic(const datum* d, const CvPoint3D64f* ned, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
   d->transformNED2Geodetic(ned, refCoor, refEcef, coor);
}
