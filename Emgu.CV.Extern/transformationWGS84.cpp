/**
 * @file transformationWGS84.cpp
 *
 * @brief   Implement functions for WGS84 transformation. 
**/
#include "transformationWGS84.h"

// Value of a from WGS84
const double A = 6378137.0;

// Value of f from WGS84
const double F = 1.0 / 298.257223563;

// Value of b from WGS84
const double B = 6356752.31424518;

// Value of e from WGS84
const double E = sqrt((A * A - B*B)/ (A*A));

// Value of e' from WGS84
const double EP = sqrt((A * A - B*B)/ (B*B));

#if EMGU_SSE2
const __m128d _ECEF2GeodeticConst0 = _mm_set_pd(A, B); 
const __m128d _ECEF2GeodeticConst1 = _mm_set_pd(EP*EP*B, -E*E*A);
#endif

void transformGeodetic2ECEF(const GeodeticCoordinate* coordinate, CvPoint3D64f* ecef)
{
   double sinPhi = sin(coordinate->latitude);

   double N = A / sqrt(1.0 - E * E * sinPhi * sinPhi);

#if EMGU_SSE2
   _mm_storeu_pd(&ecef->x, _mm_mul_pd(_mm_set_pd(sin(coordinate->longitude), cos(coordinate->longitude)), _mm_set1_pd( (N + coordinate->altitude) * cos(coordinate->latitude))));
#else
   double tmp1 = (N + coordinate->altitude) * cos(coordinate->latitude);
   ecef->x = tmp1 * cos(coordinate->longitude);
   ecef->y = tmp1 * sin(coordinate->longitude);
#endif

   ecef->z = ((B * B) / (A * A) * N + coordinate->altitude) * sinPhi;
}

void transformECEF2Geodetic(const CvPoint3D64f* ecef, GeodeticCoordinate* coor)
{
   coor->longitude = atan2(ecef->y, ecef->x);

#if EMGU_SSE2
   __m128d _yx = _mm_loadu_pd(&ecef->x);
   double p = sqrt(_dot_product(_yx, _yx));
   __m128d _t2 = _mm_set_pd(ecef->z, p);
   __m128d _t3 = _mm_mul_pd(_t2, _ECEF2GeodeticConst0);
   double theta = atan2(_t3.m128d_f64[1], _t3.m128d_f64[0]);
   __m128d _t1 = _mm_set_pd(sin(theta), cos(theta)); //1:sinTheta, 0:cosTheta

   __m128d _t4 = _mm_add_pd(_t2, _mm_mul_pd(_ECEF2GeodeticConst1, _mm_mul_pd(_t1,_mm_mul_pd(_t1, _t1)))); 
   coor->latitude = atan2(_t4.m128d_f64[1] , _t4.m128d_f64[0]);
#else
   double p = sqrt(ecef->x * ecef->x + ecef->y * ecef->y);
   double theta = atan2(ecef->z * A, p * B);
   double sinTheta = sin(theta);
   double cosTheta = cos(theta);
   coor->latitude = atan2(
      ecef->z + EP * EP * B * sinTheta * sinTheta * sinTheta,
      p - E * E * A * cosTheta * cosTheta * cosTheta);
#endif

   double sinLat = sin(coor->latitude);
   double N = A / sqrt(1.0 - E * E * sinLat * sinLat);

   coor->altitude = p / cos(coor->latitude) - N;
}

void transformGeodetic2ENU(const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* enu)
{
   CvPoint3D64f ecef;
   transformGeodetic2ECEF(coor, &ecef);

#if EMGU_SSE2
   __m128d deltaYX = _mm_sub_pd(_mm_loadu_pd(&ecef.x), _mm_loadu_pd(&refEcef->x));
   __m128d cosSinPhi= _mm_set_pd(cos(refCoor->latitude), sin(refCoor->latitude));
   __m128d sinCosLambda = _mm_set_pd(sin(refCoor->longitude), cos(refCoor->longitude));
   enu->x = _cross_product(deltaYX, sinCosLambda); 
   __m128d tmp = _mm_set_pd(_dot_product(sinCosLambda, deltaYX), ecef.z - refEcef->z);
   enu->y = _cross_product(cosSinPhi, tmp); 
   enu->z = _dot_product(cosSinPhi, tmp);
#else
   double sinPhi, cosPhi;
   CvPoint3D64f delta;
   delta.x = ecef.x - refEcef->x;
   delta.y = ecef.y - refEcef->y;
   delta.z = ecef.z - refEcef->z;

   sinPhi = sin(refCoor->latitude);
   cosPhi = cos(refCoor->latitude);
   double sinLambda = sin(refCoor->longitude);
   double cosLambda = cos(refCoor->longitude);

   enu->x = -sinLambda * delta.x + cosLambda * delta.y;

   double tmp = cosLambda * delta.x + sinLambda * delta.y;
   enu->y = -sinPhi * tmp + cosPhi * delta.z;
   enu->z = cosPhi * tmp + sinPhi * delta.z;
#endif
}

void transformENU2Geodetic(const CvPoint3D64f* enu, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
#if EMGU_SSE2
   __m128d sinCosPhi = _mm_set_pd(sin(refCoor->latitude), cos(refCoor->latitude));
   __m128d cosSinLambda = _mm_set_pd(cos(refCoor->longitude), sin(refCoor->longitude));

   CvPoint3D64f ecef;
   __m128d zy = _mm_loadu_pd(&enu->y);
   __m128d tmpX = _mm_set_pd( _cross_product(sinCosPhi, zy), enu->x);
   _mm_storeu_pd(&ecef.y, _mm_add_pd( _mm_loadu_pd(&refEcef->y), _mm_set_pd(_dot_product(sinCosPhi, zy), _cross_product(cosSinLambda, tmpX))));
   
   ecef.x =   refEcef->x - _dot_product(cosSinLambda, tmpX);
#else
   double sinPhi = sin(refCoor->latitude);
   double cosPhi = cos(refCoor->latitude);
   double sinLambda = sin(refCoor->longitude);
   double cosLambda = cos(refCoor->longitude);

   double tmp = sinPhi * enu->y - cosPhi * enu->z;
   CvPoint3D64f ecef;
   ecef.x =   -(cosLambda * tmp + sinLambda * enu->x ) + refEcef->x;
   ecef.y =   cosLambda * enu->x - sinLambda * tmp + refEcef->y;
   ecef.z =   cosPhi * enu->y + sinPhi * enu->z + refEcef->z;
#endif

   transformECEF2Geodetic(&ecef, coor);
}

void transformGeodetic2NED(const GeodeticCoordinate* coor, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, CvPoint3D64f* ned)
{
   CvPoint3D64f enu;
   transformGeodetic2ENU(coor, refCoor, refEcef, &enu);
   ned->x = enu.y;
   ned->y = enu.x;
   ned->z = -enu.z;
}

void transformNED2Geodetic(const CvPoint3D64f* ned, const GeodeticCoordinate* refCoor, const CvPoint3D64f* refEcef, GeodeticCoordinate* coor)
{
   CvPoint3D64f enu;
   enu.x = ned->y;
   enu.y = ned->x;
   enu.z = -ned->z;
   
   transformENU2Geodetic(&enu, refCoor, refEcef, coor);
}