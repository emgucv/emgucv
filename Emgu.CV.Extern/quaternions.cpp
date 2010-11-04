/**
 * @file quaternions.cpp
 *
 * @brief   Implement functions for Quaternions structure. 
**/
#pragma warning( disable: 4251 )
#include "quaternions.h"

void eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
{
   double 
      halfX = x *0.5,
      halfY = y *0.5,
      halfZ = z *0.5;

#if EMGU_SSE2
   __m128d cosSinY = _mm_set_pd(cos(halfY), sin(halfY));
   __m128d cosSinZ = _mm_set_pd(cos(halfZ), sin(halfZ));
   __m128d cosSinX = _mm_set_pd(cos(halfX), sin(halfX));  
   __m128d sinCosX = _mm_shuffle_pd(cosSinX, cosSinX, 1);
   __m128d tmp1 = _mm_mul_pd(cosSinY, cosSinZ);
   __m128d tmp2 = _mm_mul_pd(cosSinY, _mm_shuffle_pd(cosSinZ, cosSinZ, 1));

   _mm_store_sd(&quaternions->w, _dot_product(tmp1, cosSinX));
   _mm_store_sd(&quaternions->x, _cross_product(cosSinX,tmp1)); 
   _mm_store_sd(&quaternions->y, _dot_product(tmp2, sinCosX));
   _mm_store_sd(&quaternions->z, _cross_product(sinCosX, tmp2)); 
#else
	double
      sinX = sin(halfX), 
      cosX = cos(halfX),
      sinY = sin(halfY),
      cosY = cos(halfY),
      sinZ = sin(halfZ),
      cosZ = cos(halfZ);
   double cosYcosZ = cosY*cosZ;
   double sinYsinZ = sinY*sinZ;
   
   quaternions->w = cosX*cosYcosZ + sinX*sinYsinZ;
   quaternions->x = sinX*cosYcosZ - cosX*sinYsinZ;

   double cosYsinZ = cosY*sinZ;
   double sinYcosZ = sinY*cosZ;

   quaternions->y = cosX*sinYcosZ + sinX*cosYsinZ;
   quaternions->z = cosX*cosYsinZ - sinX*sinYcosZ;
#endif
}

void quaternionsToEuler(const Quaternions* quaternions, double* xDst, double* yDst, double* zDst)
{
#if EMGU_SSE2
   double buffer[4];
   __m128d _yx = _mm_loadu_pd(&quaternions->x);
   __m128d _zy = _mm_loadu_pd(&quaternions->y);
   _mm_storeu_pd(buffer+2, _mm_sub_pd(_mm_set1_pd(1.0), _mm_mul_pd(_mm_set1_pd(2.0), _mm_add_pd( _mm_mul_pd(_yx, _yx), _mm_mul_pd(_zy, _zy)))));
   _mm_storeu_pd(buffer, _mm_mul_pd(_mm_set1_pd(2.0), _mm_add_pd(_mm_mul_pd(_mm_load1_pd(&quaternions->w), _mm_shuffle_pd( _zy,_yx, _MM_SHUFFLE2(0,1)) ), _mm_mul_pd(_yx, _zy))));
   *xDst = atan2(buffer[1], buffer[2]);
   *yDst = asin(2.0 * (quaternions->w * quaternions->y - quaternions->z * quaternions->x));
   *zDst = atan2(buffer[0], buffer[3]);
#else
   double w = quaternions->w;
   double x = quaternions->x;
   double y = quaternions->y;
   double z = quaternions->z;
   *xDst = atan2(2.0 * (w * x + y * z), 1.0 - 2.0 * (x*x + y*y));
   *yDst = asin(2.0 * (w * y - z * x));
   *zDst = atan2(2.0 * (w * z + x * y), 1.0 - 2.0 * (y*y + z*z));
#endif
}

void quaternionsToRotationMatrix(const Quaternions* quaternions, CvMat* rotation)
{
   double w = quaternions->w;
   double x = quaternions->x;
   double y = quaternions->y;
   double z = quaternions->z;

   cv::Mat r = cv::cvarrToMat(rotation);
   CV_Assert(r.rows == 3 && r.cols == 3);
   cv::MatIterator_<double> rIter = r.begin<double>();
   *rIter++ = w*w+x*x-y*y-z*z; *rIter++ = 2.0*(x*y-w*z); *rIter++ = 2.0*(x*z+w*y);
   *rIter++ = 2.0*(x*y+w*z); *rIter++ = w*w-x*x+y*y-z*z; *rIter++ = 2.0*(y*z-w*x);
   *rIter++ = 2.0*(x*z-w*y); *rIter++ = 2.0*(y*z+w*x); *rIter++ = w*w-x*x-y*y+z*z;
}

inline void quaternionsRotate(double w, double x, double y, double z, double v1, double v2, double v3, double*vr)
{
   double
      t2 =   w*x,
      t3 =   w*y,
      t4 =   w*z,
      t5 =  -x*x,
      t6 =   x*y,
      t7 =   x*z,
      t8 =  -y*y,
      t9 =   y*z,
      t10 = -z*z;

   *vr++  = 2.0*( (t8 + t10)* v1 + (t6 -  t4)* v2 + (t3 + t7)* v3 ) + v1,
   *vr++ = 2.0*( (t4 +  t6)* v1 + (t5 + t10)* v2 + (t9 - t2)* v3 ) + v2,
   *vr = 2.0*( (t7 -  t3)* v1 + (t2 +  t9)* v2 + (t5 + t8)* v3 ) + v3;
}

void quaternionsRotatePoint(const Quaternions* quaternions, const CvPoint3D64f* point, CvPoint3D64f* pointDst)
{
   quaternionsRotate(quaternions->w, quaternions->x, quaternions->y, quaternions->z, point->x, point->y, point->z, (double*) pointDst);
}

void quaternionsRotatePoints(const Quaternions* quaternions, const CvMat* pointSrc, CvMat* pointDst)
{
   cv::Mat p = cv::cvarrToMat(pointSrc);
   cv::Mat pDst = cv::cvarrToMat(pointDst);
   CV_Assert((p.rows == 3 && p.cols == 1) || p.cols ==3);
   CV_Assert(pDst.rows == p.rows && pDst.cols == p.cols);

   double *v;

   cv::MatIterator_<double> pIter = p.begin<double>();
   cv::MatIterator_<double> pDstIter = pDst.begin<double>();

   if ((p.rows == 3 && p.cols == 1))
   {  
      v = (double*)pIter.ptr;
      quaternionsRotate(quaternions->w, quaternions->x, quaternions->y, quaternions->z, *v, *(v+1), *(v+2), (double*)pDstIter.ptr);
   } else 
   {
      for(int i = 0; i < p.rows; i++, pIter+=3, pDstIter+=3)
      {
         v = (double*)pIter.ptr;
         quaternionsRotate(quaternions->w, quaternions->x, quaternions->y, quaternions->z, *v, *(v+1), *(v+2), (double*)pDstIter.ptr);
      }
   }
}

void quaternionsMultiply(const Quaternions* quaternions1, const Quaternions* quaternions2, Quaternions* quaternionsDst)
{
#if EMGU_SSE2
   __m128d _w1w1 = _mm_load1_pd(&quaternions1->w);
   __m128d _x1x1 = _mm_load1_pd(&quaternions1->x);
   __m128d _y1y1 = _mm_load1_pd(&quaternions1->y);
   __m128d _z1z1 = _mm_load1_pd(&quaternions1->z);
   __m128d _w2x2 = _mm_set_pd(quaternions2->w, quaternions2->x);
   __m128d _nx2w2 = _mm_set_pd(-quaternions2->x, quaternions2->w);
   __m128d _z2y2 = _mm_loadu_pd(&quaternions2->y);
   __m128d _ny2z2 = _mm_set_pd(-quaternions2->y, quaternions2->z);

   __m128d _wx = _mm_add_pd(
      _mm_add_pd(_mm_mul_pd(_w1w1, _w2x2), _mm_mul_pd(_x1x1, _nx2w2)),
      _mm_sub_pd(_mm_mul_pd(_y1y1, _ny2z2), _mm_mul_pd(_z1z1, _z2y2)));
   __m128d _xw = _mm_shuffle_pd(_wx, _wx, 1);

   __m128d _zy = _mm_add_pd(
      _mm_sub_pd(_mm_mul_pd(_w1w1, _z2y2), _mm_mul_pd(_x1x1, _ny2z2)),
      _mm_add_pd(_mm_mul_pd(_y1y1, _nx2w2), _mm_mul_pd(_z1z1, _w2x2)));
   
   //this is done to improve the numerical stability
   __m128d denorm = _mm_add_pd( _dot_product(_xw, _xw), _dot_product(_zy, _zy) );
   _mm_storeu_pd(&quaternionsDst->w, _mm_div_pd(_xw, denorm));
   _mm_storeu_pd(&quaternionsDst->y, _mm_div_pd(_zy, denorm));

#else
   double w1 = quaternions1->w;
   double x1 = quaternions1->x;
   double y1 = quaternions1->y;
   double z1 = quaternions1->z;

   double w2 = quaternions2->w;
   double x2 = quaternions2->x;
   double y2 = quaternions2->y;
   double z2 = quaternions2->z;

   quaternionsDst->w = (w1*w2 - x1*x2 - y1*y2 - z1*z2);
   quaternionsDst->x = (w1*x2 + x1*w2 + y1*z2 - z1*y2);
   quaternionsDst->y = (w1*y2 - x1*z2 + y1*w2 + z1*x2);
   quaternionsDst->z = (w1*z2 + x1*y2 - y1*x2 + z1*w2);

   //this is done to improve the numerical stability
   quaternionsDst->renorm(); 
#endif
}

const double THETA_EPS = 1.0e-30;

/* convert axis angle vector to quaternions. (x,y,z) is the rotatation axis and |(x,y,z)| is the rotation angle  */
void axisAngleToQuaternions(const CvPoint3D64f* axisAngle, Quaternions* quaternions)
{
   double theta = sqrt(cvPoint3D64fDotProduct(axisAngle, axisAngle));
   if (theta < THETA_EPS) 
   {
      quaternions->w = 1.0;
      quaternions->x = 0.0;
      quaternions->y = 0.0;
      quaternions->z = 0.0;
      return;
   }
   double halfAngle = theta * 0.5;
   double scale = sin(halfAngle) / theta;
   quaternions->w = cos(halfAngle);
   quaternions->x = axisAngle->x * scale;
   quaternions->y = axisAngle->y * scale;
   quaternions->z = axisAngle->z * scale;

   quaternions->renorm();
}

/* convert quaternions to axis angle vector. The vector (x,y,z) defines the rotatation axis and the norm |(x,y,z)| defines the rotation angle  */
void quaternionsToAxisAngle(const Quaternions* quaternions, CvPoint3D64f* axisAngle)
{
   double theta = 2.0 * acos(quaternions->w);
   if (theta < THETA_EPS)
   {
      axisAngle->x = axisAngle->y = axisAngle->z = 0.0;
   } else
   {
      double norm = sqrt(quaternions->x * quaternions->x + quaternions->y * quaternions->y + quaternions->z * quaternions->z);
      double scale = theta / norm;
      axisAngle->x = quaternions->x * scale;
      axisAngle->y = quaternions->y * scale;
      axisAngle->z = quaternions->z * scale;
   }
}

/**
 * @fn   void quaternionsRenorm(Quaternions* quaternions)
 *
 * @brief   Renormalize the given quaternions such that the norm becomes 1
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsRenorm(Quaternions* quaternions) { quaternions->renorm(); }

void quaternionsSlerp(Quaternions* qa, Quaternions* qb, double t, Quaternions* qm)
{
   //making sure t is in the valid range
   t = t < 0 ? 0 : ( t > 1.0 ? 1.0 : t );

   // Calculate angle between them.
   double cosHalfTheta = qa->dotProduct(qb);

   // if qa=qb or qa=-qb then theta = 0 and we can return qa
   if (abs(cosHalfTheta) >= 1.0){
      memcpy(qm, qa, sizeof(Quaternions));// qm->w = qa->w;qm->x = qa->x;qm->y = qa->y;qm->z = qa->z;
      return;
   }
   
   // Calculate temporary values.
   double sinHalfTheta = sqrt(1.0 - cosHalfTheta*cosHalfTheta);
   
   // if theta = 180 degrees then result is not fully defined
   // we could rotate around any axis normal to qa or qb
   if (fabs(sinHalfTheta) < 1.0e-4)
   { 
      doubleOps::weightedSum((double*)qa, (double*)qb, 4, 1.0-t, t, (double*)qm);
   } else
   {
      double halfTheta = acos(cosHalfTheta);
      double ratioA = sin((1.0 - t) * halfTheta) / sinHalfTheta;
      double ratioB = sin(t * halfTheta) / sinHalfTheta; 
      //calculate Quaternion.
      doubleOps::weightedSum((double*)qa, (double*)qb, 4, ratioA, ratioB, (double*)qm);
   }
   qm->renorm();
}