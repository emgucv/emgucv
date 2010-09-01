#pragma once
#ifndef QUATERNIONS_H
#define QUATERNIONS_H

#include "opencv2/core/core_c.h"
#include "opencv2/core/core.hpp"
#include "sse.h"

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
CVAPI(void) quaternionsToEuler(Quaternions* quaternions, double* x, double* y, double* z);


/**
 * @fn   void axisAngleToQuaternions(CvPoint3D64f* axisAngle, Quaternions* quaternions)
 *
 * @brief   Convert axis angle vector to quaternions. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) axisAngleToQuaternions(CvPoint3D64f* axisAngle, Quaternions* quaternions);

/**
 * @fn   void quaternionsToAxisAngle(Quaternions* quaternions, CvPoint3D64f* axisAngle)
 *
 * @brief   Convert quaternions to axis angle vector. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsToAxisAngle(Quaternions* quaternions, CvPoint3D64f* axisAngle);

/**
 * @fn   void quaternionsToRotationMatrix(Quaternions* quaternions, CvMat* rotation)
 *
 * @brief   Convert quaternions to (3x3) rotation matrix 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsToRotationMatrix(Quaternions* quaternions, CvMat* rotation);

/**
 * @fn   void quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point,
 * CvPoint3D64f* pointDst)
 *
 * @brief   Rotate a single point using the quaternions. 
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point, CvPoint3D64f* pointDst);

/**
 * @fn   void quaternionsRotatePoints(Quaternions* quaternions, CvMat* pointSrc,
 * CvMat* pointDst)
 *
 * @brief   Rotate the (3x1 or nx3) matrix of points using the quaternions
 *
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsRotatePoints(Quaternions* quaternions, CvMat* pointSrc, CvMat* pointDst);

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
CVAPI(void) quaternionsMultiply(Quaternions* quaternions1, Quaternions* quaternions2, Quaternions* quaternionsDst);

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
 * @author  Canming Huang
 * @date 8/31/2010
**/
CVAPI(void) quaternionsSlerp(Quaternions* qa, Quaternions* qb, double t, Quaternions* qm);

#endif