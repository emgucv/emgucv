/**
 * @file quaternions.cpp
 *
 * @brief   Implement functions for Quaternions structure. 
**/
#pragma warning( disable: 4251 )
#include "quaternions.h"

/**
* @fn   void eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
*
* @brief   Convert eluer angle (in radian) to quaternions. 
*
* @author  Canming
* @date 8/31/2010
**/
CVAPI(void) eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
{
   quaternions->setEuler(x, y, z);
}

/**
* @fn   void quaternionsToEuler(Quaternions* quaternions, double* x, double* y, double* z)
* 
* @brief   Convert quaternions to eluer angle (in radian) 
*
* @author  Canming
* @date 8/31/2010
**/
CVAPI(void) quaternionsToEuler(const Quaternions* quaternions, double* x, double* y, double* z)
{
   quaternions->getEuler(x, y, z);
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

/**
* @fn   void quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point,
* CvPoint3D64f* pointDst)
*
* @brief   Rotate a single point using the quaternions. 
*
* @author  Canming Huang
* @date 8/31/2010
**/
CVAPI(void) quaternionsRotatePoint(const Quaternions* quaternions, const CvPoint3D64f* point, CvPoint3D64f* pointDst)
{
   quaternions->rotatePoint(point, pointDst);
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
      quaternionsRotatePoint( quaternions, (CvPoint3D64f*) pIter.ptr, (CvPoint3D64f*) pDstIter.ptr);
   } else 
   {
      for(int i = 0; i < p.rows; i++, pIter+=3, pDstIter+=3)
      {
         quaternionsRotatePoint(quaternions, (CvPoint3D64f*) pIter.ptr, (CvPoint3D64f*)pDstIter.ptr);
      }
   }
}

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
CVAPI(void) quaternionsMultiply(const Quaternions* quaternions1, const Quaternions* quaternions2, Quaternions* quaternionsDst)
{
   quaternions1->multiply(quaternions2, quaternionsDst);
}

/**
* @fn   void axisAngleToQuaternions(CvPoint3D64f* axisAngle, Quaternions* quaternions)
*
* @brief   Convert axis angle vector to quaternions. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
*
* @author  Canming Huang
* @date 8/31/2010
**/
CVAPI(void) axisAngleToQuaternions(const CvPoint3D64f* axisAngle, Quaternions* quaternions)
{
   quaternions->setAxisAngle(axisAngle);
}

/**
* @fn   void quaternionsToAxisAngle(Quaternions* quaternions, CvPoint3D64f* axisAngle)
*
* @brief   Convert quaternions to axis angle vector. The vetor (x,y,z) is the rotatation axis and the norm |(x,y,z)| is the rotation angle
*
* @author  Canming Huang
* @date 8/31/2010
**/
CVAPI(void) quaternionsToAxisAngle(const Quaternions* quaternions, CvPoint3D64f* axisAngle)
{
   quaternions->getAxisAngle(axisAngle);
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
CVAPI(void) quaternionsSlerp(const Quaternions* qa, const Quaternions* qb, double t, Quaternions* qm)
{
   qa->slerp(qb, t, qm);
}