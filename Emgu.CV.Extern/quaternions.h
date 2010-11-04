#pragma once
#ifndef QUATERNIONS_H
#define QUATERNIONS_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"
#include "sse.h"
#include "doubleOps.h"
#include "pointUtil.h"

/**
 * @struct  Quaternions
 *
 * @brief   An unit quaternions that defines rotation in 3D
 *
 * @author  Canming Huang
 * @date 8/31/2010
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
      __m128d _denorm = _mm_sqrt_pd( _mm_add_pd( _mm_dp_pd(_xw, _xw, 0x33), _mm_dp_pd(_zy, _zy, 0x33) ));
#else
      __m128d _tmp = _mm_add_pd( _mm_mul_pd(_xw, _xw),  _mm_mul_pd(_zy, _zy)); //x*x + z*z, w*w + y*y
      __m128d _denorm = _mm_sqrt_pd(_mm_add_pd(_tmp, _mm_shuffle_pd(_tmp, _tmp, 1))); 
#endif
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
      _mm_store_sd(&result, _mm_add_pd(
         _mm_dp_pd(_mm_loadu_pd(&w), _mm_loadu_pd(&q1->w), 0x33),
         _mm_dp_pd(_mm_loadu_pd(&y), _mm_loadu_pd(&q1->y), 0x33)));
#else
      __m128d _sum = _mm_add_pd(
         _mm_mul_pd(_mm_loadu_pd(&w), _mm_loadu_pd(&q1->w)), //x0 * x1, w0 * w1
         _mm_mul_pd(_mm_loadu_pd(&y), _mm_loadu_pd(&q1->y)) //z0 * z1, y0 * y1 
         ); //x0x1 + z0z1, w0w1 + y0y1
      _mm_store_sd(&result, _mm_add_pd(_sum, _mm_shuffle_pd(_sum, _sum, 1)));
#endif
      return result;
#else
      return w * q1->w + x * q1->x + y * q1->y + z * q1->z;
#endif
   }
} Quaternions;

/**
 * @fn   void eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
 *
 * @brief   Convert eluer angle (in radian) to quaternions. 
 *
 * @author  Canming
 * @date 8/31/2010
**/
CVAPI(void) eulerToQuaternions(double x, double y, double z, Quaternions* quaternions);

/**
 * @fn   void quaternionsToEuler(Quaternions* quaternions, double* x, double* y, double* z)
 * 
 * @brief   Convert quaternions to eluer angle (in radian) 
 *
 * @author  Canming
 * @date 8/31/2010
**/
CVAPI(void) quaternionsToEuler(const Quaternions* quaternions, double* x, double* y, double* z);

/**
 * @fn   void axisAngleToQuaternions(CvPoint3D64f* axisAngle, Quaternions* quaternions)
 *
 * @brief   Convert axis angle vector to quaternions. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) axisAngleToQuaternions(const CvPoint3D64f* axisAngle, Quaternions* quaternions);

/**
 * @fn   void quaternionsToAxisAngle(Quaternions* quaternions, CvPoint3D64f* axisAngle)
 *
 * @brief   Convert quaternions to axis angle vector. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsToAxisAngle(const Quaternions* quaternions, CvPoint3D64f* axisAngle);

/**
 * @fn   void quaternionsToRotationMatrix(Quaternions* quaternions, CvMat* rotation)
 *
 * @brief   Convert quaternions to (3x3) rotation matrix 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsToRotationMatrix(const Quaternions* quaternions, CvMat* rotation);

/**
 * @fn   void quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point,
 * CvPoint3D64f* pointDst)
 *
 * @brief   Rotate a single point using the quaternions. 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsRotatePoint(const Quaternions* quaternions, const CvPoint3D64f* point, CvPoint3D64f* pointDst);

/**
 * @fn   void quaternionsRotatePoints(Quaternions* quaternions, CvMat* pointSrc,
 * CvMat* pointDst)
 *
 * @brief   Rotate the (3x1 or nx3) matrix of points using the quaternions
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsRotatePoints(const Quaternions* quaternions, const CvMat* pointSrc, CvMat* pointDst);

/**
 * @fn   void quaternionsMultiply(Quaternions* quaternions1, Quaternions* quaternions2,
 * Quaternions* quaternionsDst)
 *
 * @brief   Multiply two quaternions. The result is a rotation by quaternions2 follows by
 * quaternions1. 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsMultiply(const Quaternions* quaternions1, const Quaternions* quaternions2, Quaternions* quaternionsDst);

/**
 * @fn   void quaternionsSlerp(Quaternions* qa, Quaternions* qb, double t, Quaternions* qm)
 *
 * @brief   Use Slerp to interpolate the quaternions. 
 *
 * @param qa The first Quaternions
 * @param qb The second Quaternions
 * @param t This is the weight for qb. 0<=t<=1; if t=0, qm=qa; if t=1, qm - qb;
 * @param qm The result of slerp
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsSlerp(Quaternions* qa, Quaternions* qb, double t, Quaternions* qm);


#endif