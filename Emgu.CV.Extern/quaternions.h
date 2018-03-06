//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_QUATERNIONS_H
#define EMGU_QUATERNIONS_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"
#include "sse.h"
#include "doubleOps.h"
#include "pointUtil.h"

const double QUATERNIONS_THETA_EPS = 1.0e-30;

/**
* @struct  Quaternions
*
* @brief   An unit quaternions that defines rotation in 3D
*
**/
typedef struct Quaternions
{
   /// The value for cos(rotation angle / 2)
   double w;

   /// The x component of the vector: rotation axis * sin(rotation angle / 2)
   double x;

   /// The y component of the vector: rotation axis * sin(rotation angle / 2)
   double y;

   /// The z component of the vector: rotation axis * sin(rotation angle / 2)
   double z;

   void renorm()
   {
#if EMGU_SSE2
      __m128d _xw = _mm_loadu_pd(&w);
      __m128d _zy = _mm_loadu_pd(&y);
#if EMGU_SSE4_1
      if(simdSSE4_1)
      {
         __m128d _denorm = _mm_sqrt_pd( _mm_add_pd( _mm_dp_pd(_xw, _xw, 0x33), _mm_dp_pd(_zy, _zy, 0x33) ));
         _mm_storeu_pd(&w, _mm_div_pd(_xw, _denorm));
         _mm_storeu_pd(&y, _mm_div_pd(_zy, _denorm));
         return;
      }
#endif
      __m128d _tmp = _mm_add_pd( _mm_mul_pd(_xw, _xw),  _mm_mul_pd(_zy, _zy)); //x*x + z*z, w*w + y*y
      __m128d _denorm = _mm_sqrt_pd(_mm_add_pd(_tmp, _mm_shuffle_pd(_tmp, _tmp, 1))); 
      _mm_storeu_pd(&w, _mm_div_pd(_xw, _denorm));
      _mm_storeu_pd(&y, _mm_div_pd(_zy, _denorm));
#else
      double scale = 1.0 / sqrt(w * w + x * x + y * y + z * z);
      w /= scale;
      x /= scale;
      y /= scale;
      z /= scale;
#endif
   }

   double dotProduct (const Quaternions* q1) const
   {
#if EMGU_SSE2
      double result;
#if EMGU_SSE4_1
      if(simdSSE4_1)
      {
         _mm_store_sd(&result, _mm_add_pd(
            _mm_dp_pd(_mm_loadu_pd(&w), _mm_loadu_pd(&q1->w), 0x33),
            _mm_dp_pd(_mm_loadu_pd(&y), _mm_loadu_pd(&q1->y), 0x33)));
         return result;
      }
#endif
      __m128d _sum = _mm_add_pd(
         _mm_mul_pd(_mm_loadu_pd(&w), _mm_loadu_pd(&q1->w)), //x0 * x1, w0 * w1
         _mm_mul_pd(_mm_loadu_pd(&y), _mm_loadu_pd(&q1->y)) //z0 * z1, y0 * y1 
         ); //x0x1 + z0z1, w0w1 + y0y1
      _mm_store_sd(&result, _mm_add_pd(_sum, _mm_shuffle_pd(_sum, _sum, 1)));

      return result;
#else
      return w * q1->w + x * q1->x + y * q1->y + z * q1->z;
#endif
   }

   void multiply(const Quaternions* quaternions2, Quaternions* quaternionsDst) const
   {
#if EMGU_SSE2
      __m128d _w1w1 = _mm_load1_pd(&w);
      __m128d _x1x1 = _mm_load1_pd(&x);
      __m128d _y1y1 = _mm_load1_pd(&y);
      __m128d _z1z1 = _mm_load1_pd(&z);
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

      double w2 = quaternions2->w;
      double x2 = quaternions2->x;
      double y2 = quaternions2->y;
      double z2 = quaternions2->z;

      quaternionsDst->w = (w*w2 - x*x2 - y*y2 - z*z2);
      quaternionsDst->x = (w*x2 + x*w2 + y*z2 - z*y2);
      quaternionsDst->y = (w*y2 - x*z2 + y*w2 + z*x2);
      quaternionsDst->z = (w*z2 + x*y2 - y*x2 + z*w2);

      //this is done to improve the numerical stability
      quaternionsDst->renorm(); 
#endif
   }

   Quaternions operator* (const Quaternions& other)
   {
      Quaternions result;
      multiply(&other, &result);
      return result;
   }

   void rotatePoint(const CvPoint3D64f* point, CvPoint3D64f* pointDst) const
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

      double xx = 2.0*( (t8 + t10)* point->x + (t6 -  t4)* point->y + (t3 + t7)* point->z ) + point->x;
      double yy = 2.0*( (t4 +  t6)* point->x + (t5 + t10)* point->y + (t9 - t2)* point->z ) + point->y;
      double zz = 2.0*( (t7 -  t3)* point->x + (t2 +  t9)* point->y + (t5 + t8)* point->z ) + point->z;
      pointDst->x = xx; pointDst->y = yy; pointDst->z = zz;
   }

   void setEuler(double roll, double pitch, double raw)
   {
      double 
         halfX = roll *0.5,
         halfY = pitch *0.5,
         halfZ = raw *0.5;

#if EMGU_SSE2
      __m128d cosSinY = _mm_set_pd(cos(halfY), sin(halfY));
      __m128d cosSinZ = _mm_set_pd(cos(halfZ), sin(halfZ));
      __m128d cosSinX = _mm_set_pd(cos(halfX), sin(halfX));  
      __m128d sinCosX = _mm_shuffle_pd(cosSinX, cosSinX, 1);
      __m128d tmp1 = _mm_mul_pd(cosSinY, cosSinZ);
      __m128d tmp2 = _mm_mul_pd(cosSinY, _mm_shuffle_pd(cosSinZ, cosSinZ, 1));

      _mm_store_sd(&w, _dot_product(tmp1, cosSinX));
      _mm_store_sd(&x, _cross_product(cosSinX,tmp1)); 
      _mm_store_sd(&y, _dot_product(tmp2, sinCosX));
      _mm_store_sd(&z, _cross_product(sinCosX, tmp2)); 
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

      w = cosX*cosYcosZ + sinX*sinYsinZ;
      x = sinX*cosYcosZ - cosX*sinYsinZ;

      double cosYsinZ = cosY*sinZ;
      double sinYcosZ = sinY*cosZ;

      y = cosX*sinYcosZ + sinX*cosYsinZ;
      z = cosX*cosYsinZ - sinX*sinYcosZ;
#endif
   }

   void getEuler(double* xDst, double* yDst, double* zDst) const
   {
#if EMGU_SSE2
      double buffer[4];
      __m128d _yx = _mm_loadu_pd(&x);
      __m128d _zy = _mm_loadu_pd(&y);
      _mm_storeu_pd(buffer+2, _mm_sub_pd(_mm_set1_pd(1.0), _mm_mul_pd(_mm_set1_pd(2.0), _mm_add_pd( _mm_mul_pd(_yx, _yx), _mm_mul_pd(_zy, _zy)))));
      _mm_storeu_pd(buffer, _mm_mul_pd(_mm_set1_pd(2.0), _mm_add_pd(_mm_mul_pd(_mm_load1_pd(&w), _mm_shuffle_pd( _zy,_yx, _MM_SHUFFLE2(0,1)) ), _mm_mul_pd(_yx, _zy))));
      *xDst = atan2(buffer[1], buffer[2]);
      double v = 2.0 * (w * y - z * x);
      *yDst = asin(v > 1.0 ? 1.0 : (v < -1.0? -1.0 : v)); //extra step to enhance the numerical stability.
      *zDst = atan2(buffer[0], buffer[3]);
#else
      *xDst = atan2(2.0 * (w * x + y * z), 1.0 - 2.0 * (x*x + y*y));
      double v = 2.0 * (w * y - z * x);
      *yDst = asin(v > 1.0 ? 1.0 : (v < -1.0? -1.0 : v)); //extra step to enhance the numerical stability.
      *zDst = atan2(2.0 * (w * z + x * y), 1.0 - 2.0 * (y*y + z*z));
#endif
   }

   void getAxisAngle(CvPoint3D64f* axisAngle) const
   {
      double theta = 2.0 * acos(w);
      if (theta < QUATERNIONS_THETA_EPS)
      {
         axisAngle->x = axisAngle->y = axisAngle->z = 0.0;
      } else
      {
         double norm = sqrt(x * x + y * y + z * z);
         double scale = theta / norm;
         axisAngle->x = x * scale;
         axisAngle->y = y * scale;
         axisAngle->z = z * scale;
      }
   }

   void setAxisAngle(const CvPoint3D64f* axisAngle)
   {
      double theta = sqrt(cvPoint3D64fDotProduct(axisAngle, axisAngle));
      if (theta < QUATERNIONS_THETA_EPS) 
      {
         w = 1.0;
         x = 0.0;
         y = 0.0;
         z = 0.0;
         return;
      }
      double halfAngle = theta * 0.5;
      double scale = sin(halfAngle) / theta;
      w = cos(halfAngle);
      x = axisAngle->x * scale;
      y = axisAngle->y * scale;
      z = axisAngle->z * scale;

      renorm();
   }

   void slerp(const Quaternions* qb, double t, Quaternions* qm) const
   {
      //making sure t is in the valid range
      t = t < 0 ? 0 : ( t > 1.0 ? 1.0 : t );

      // Calculate angle between them.
      double cosHalfTheta =  dotProduct(qb);

      // if qa=qb or qa=-qb then theta = 0 and we can return qa
      if (fabs(cosHalfTheta) >= 1.0){
         memcpy(qm, &w, sizeof(Quaternions));// qm->w = qa->w;qm->x = qa->x;qm->y = qa->y;qm->z = qa->z;
         return;
      }

      // Calculate temporary values.
      double sinHalfTheta = sqrt(1.0 - cosHalfTheta*cosHalfTheta);

      // if theta = 180 degrees then result is not fully defined
      // we could rotate around any axis normal to qa or qb
      if (fabs(sinHalfTheta) < 1.0e-4)
      { 
         doubleOps::weightedSum((double*)&w, (double*)qb, 4, 1.0-t, t, (double*)qm);
      } else
      {
         double halfTheta = acos(cosHalfTheta);
         double ratioA = sin((1.0 - t) * halfTheta) / sinHalfTheta;
         double ratioB = sin(t * halfTheta) / sinHalfTheta; 
         //calculate Quaternion.
         doubleOps::weightedSum((double*)&w, (double*)qb, 4, ratioA, ratioB, (double*)qm);
      }
      qm->renorm();
   }

   void conjugate()
   {
      x = -x;
      y = -y; 
      z = -z;
   }

} Quaternions;

/**
* @fn   void eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
*
* @brief   Convert eluer angle (in radian) to quaternions. 
*
**/
CVAPI(void) eulerToQuaternions(double x, double y, double z, Quaternions* quaternions);

/**
* @fn   void quaternionsToEuler(Quaternions* quaternions, double* x, double* y, double* z)
* 
* @brief   Convert quaternions to eluer angle (in radian) 
*
**/
CVAPI(void) quaternionsToEuler(const Quaternions* quaternions, double* x, double* y, double* z);

/**
* @fn   void quaternionsToRotationMatrix(Quaternions* quaternions, CvMat* rotation)
*
* @brief   Convert quaternions to (3x3) rotation matrix 
*
**/
CVAPI(void) quaternionsToRotationMatrix(const Quaternions* quaternions, CvMat* rotation);

/**
* @fn   void quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point,
* CvPoint3D64f* pointDst)
*
* @brief   Rotate a single point using the quaternions. 
*
**/
CVAPI(void) quaternionsRotatePoint(const Quaternions* quaternions, const CvPoint3D64f* point, CvPoint3D64f* pointDst);


/**
* @fn   void quaternionsRotatePoints(Quaternions* quaternions, CvMat* pointSrc,
* CvMat* pointDst)
*
* @brief   Rotate the (3x1 or nx3) matrix of points using the quaternions
*
**/
CVAPI(void) quaternionsRotatePoints(const Quaternions* quaternions, const CvMat* pointSrc, CvMat* pointDst);

/**
 * @fn   void quaternionsMultiply(Quaternions* quaternions1, Quaternions* quaternions2,
 * Quaternions* quaternionsDst)
 *
 * @brief   Multiply two quaternions. The result is a rotation by quaternions2 follows by
 * quaternions1. 
 *
**/
CVAPI(void) quaternionsMultiply(const Quaternions* quaternions1, const Quaternions* quaternions2, Quaternions* quaternionsDst);

/**
* @fn   void axisAngleToQuaternions(CvPoint3D64f* axisAngle, Quaternions* quaternions)
*
* @brief   Convert axis angle vector to quaternions. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
*
**/
CVAPI(void) axisAngleToQuaternions(const CvPoint3D64f* axisAngle, Quaternions* quaternions);

/**
* @fn   void quaternionsToAxisAngle(Quaternions* quaternions, CvPoint3D64f* axisAngle)
*
* @brief   Convert quaternions to axis angle vector. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
*
**/
CVAPI(void) quaternionsToAxisAngle(const Quaternions* quaternions, CvPoint3D64f* axisAngle);

/**
 * @fn   void quaternionsRenorm(Quaternions* quaternions)
 *
 * @brief   Renormalize the given quaternions such that the norm becomes 1
 *
**/
CVAPI(void) quaternionsRenorm(Quaternions* quaternions);

/**
* @fn   void quaternionsSlerp(Quaternions* qa, Quaternions* qb, double t, Quaternions* qm)
*
* @brief   Use Slerp to interpolate the quaternions. 
*
* @param qa The first Quaternions
* @param qb The second Quaternions
* @param t This is the weight for qb. 0<=t<=1; if t=0, qm=qa; if t=1, qm - qb;
* @param qm The result of slerp
**/
CVAPI(void) quaternionsSlerp(const Quaternions* qa, const Quaternions* qb, double t, Quaternions* qm);
#endif