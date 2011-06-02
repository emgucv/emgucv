//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#include "datum.h"

Datum* datumCreateWGS84() { return new Datum; }

Datum* datumCreate(double a, double b) { return new Datum(a, b); }

void datumRelease(Datum** d) { delete *d; }

void transformGeodetic2ECEF(const Datum* d, const GeodeticCoordinate* coordinate, CvPoint3D64f* ecef)
{
   d->transformGeodetic2ECEF(coordinate, ecef);
}

void transformECEF2Geodetic(const Datum* d, const CvPoint3D64f* ecef, GeodeticCoordinate* coordinate)
{
   d->transformECEF2Geodetic(ecef, coordinate);
}

void transformGeodetic2ENU(const Datum* d, const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu)
{
   d->transformGeodetic2ENU(coor, refCoor, refEcef, enu);
}

void transformENU2Geodetic(const Datum* d, const CvPoint3D64f* enu, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
   d->transformENU2Geodetic(enu, refCoor, refEcef, coor);
}

void transformGeodetic2NED(const Datum* d, const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned)
{
   d->transformGeodetic2NED(coor, refCoor, refEcef, ned);
}

void transformNED2Geodetic(const Datum* d, const CvPoint3D64f* ned, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
   d->transformNED2Geodetic(ned, refCoor, refEcef, coor);
}
