//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "quaternions.h"

void eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
{
   quaternions->setEuler(x, y, z);
}

void quaternionsToEuler(const Quaternions* quaternions, double* x, double* y, double* z)
{
   quaternions->getEuler(x, y, z);
}

void quaternionsRotatePoint(const Quaternions* quaternions, const CvPoint3D64f* point, CvPoint3D64f* pointDst)
{
   quaternions->rotatePoint(point, pointDst);
}

void quaternionsRotatePoints(const Quaternions* quaternions, const CvMat* pointSrc, CvMat* pointDst)
{
   cv::Mat p = cv::cvarrToMat(pointSrc);
   cv::Mat pDst = cv::cvarrToMat(pointDst);
   CV_Assert((p.rows == 3 && p.cols == 1) || p.cols ==3);
   CV_Assert(pDst.rows == p.rows && pDst.cols == p.cols);

   cv::MatIterator_<double> pIter = p.begin<double>();
   cv::MatIterator_<double> pDstIter = pDst.begin<double>();

   if ((p.rows == 3 && p.cols == 1))
   {  
      quaternionsRotatePoint( quaternions, (CvPoint3D64f*) pIter.ptr, (CvPoint3D64f*) pDstIter.ptr);
   } else 
   {
      for(int i = 0; i < p.rows; i++, pIter+=3, pDstIter+=3)
      {
         quaternionsRotatePoint(quaternions, (CvPoint3D64f*) pIter.ptr, (CvPoint3D64f*)pDstIter.ptr);
      }
   }
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

void quaternionsMultiply(const Quaternions* quaternions1, const Quaternions* quaternions2, Quaternions* quaternionsDst)
{
   quaternions1->multiply(quaternions2, quaternionsDst);
}

void axisAngleToQuaternions(const CvPoint3D64f* axisAngle, Quaternions* quaternions)
{
   quaternions->setAxisAngle(axisAngle);
}

void quaternionsToAxisAngle(const Quaternions* quaternions, CvPoint3D64f* axisAngle)
{
   quaternions->getAxisAngle(axisAngle);
}

void quaternionsRenorm(Quaternions* quaternions) { quaternions->renorm(); }

void quaternionsSlerp(const Quaternions* qa, const Quaternions* qb, double t, Quaternions* qm)
{
   qa->slerp(qb, t, qm);
}