#pragma once
#ifndef QUATERNIONS_H
#define QUATERNIONS_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"
#include "sse.h"
#include "doubleOps.h"

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
 * @fn   void quaternionsRenorm(Quaternions* quaternions)
 *
 * @brief   Renormalize the given quaternions such that the norm becomes 1
 *
 * @author  Canming Huang
 * @date 8/31/2010
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
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsSlerp(Quaternions* qa, Quaternions* qb, double t, Quaternions* qm);

inline double quaternionsDotProduct(const Quaternions* q0, const Quaternions* q1)
{
#if EMGU_SSE2
   __m128d _w0x0 = _mm_set_pd(q0->w, q0->x);
   __m128d _w1x1 = _mm_set_pd(q1->w, q1->x);
   __m128d _y0z0 = _mm_set_pd(q0->y, q0->z);
   __m128d _y1z1 = _mm_set_pd(q1->y, q1->z);

   __m128d _wx = _mm_mul_pd(_w0x0, _w1x1); //w0 * w1, x0 * x1
   __m128d _yz = _mm_mul_pd(_y0z0, _y1z1); //y0 * y1, z0 * z1
   __m128d _sum = _mm_add_pd(_wx, _yz); //w0w1 + y0y1, x0x1 + z0z1
   return _sum.m128d_f64[1] + _sum.m128d_f64[0];
#else
   return q0->w * q1->w + q0->x * q1->x + q0->y * q1->y + q0->z * q1->z;
#endif
}
#endif