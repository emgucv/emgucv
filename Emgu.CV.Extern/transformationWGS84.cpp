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

CVAPI(void) transformGeodetic2ECEF(geodeticCoordinate* coordinate, CvPoint3D64f* ecef)
{
   double sinPhi = sin(coordinate->latitude);

   double N = A / sqrt(1.0 - E * E * sinPhi * sinPhi);

   double tmp1 = (N + coordinate->altitude) * cos(coordinate->latitude);

   ecef->x = tmp1 * cos(coordinate->longitude);
   ecef->y = tmp1 * sin(coordinate->longitude);
   ecef->z = ((B * B) / (A * A) * N + coordinate->altitude) * sinPhi;
}

#if CV_SSE2
const __m128d _ECEF2GeodeticConst0 = _mm_set_pd(A, B);
const __m128d _ECEF2GeodeticConst1 = _mm_set_pd(EP*EP*B, -E*E*A);
#endif

CVAPI(void) transformECEF2Geodetic(CvPoint3D64f* ecef, geodeticCoordinate* coor)
{
   coor->longitude = atan2(ecef->y, ecef->x);
   double p = sqrt(ecef->x * ecef->x + ecef->y * ecef->y);

#if CV_SSE2
   __m128d _t2 = _mm_set_pd(ecef->z, p);
   __m128d _t3 = _mm_mul_pd(_t2, _ECEF2GeodeticConst0);
   double theta = atan2(_t3.m128d_f64[1], _t3.m128d_f64[0]);
   __m128d _t1 = _mm_set_pd(sin(theta), cos(theta)); //1:sinTheta, 0:cosTheta

   _t2 = _mm_add_pd(_t2, _mm_mul_pd(_ECEF2GeodeticConst1, _mm_mul_pd(_t1,_mm_mul_pd(_t1, _t1)))); 
   coor->latitude = atan2(_t2.m128d_f64[1] , _t2.m128d_f64[0]);
#else
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
