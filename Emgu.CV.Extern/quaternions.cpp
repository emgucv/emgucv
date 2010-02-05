#include "quaternions.h"

CVAPI(void) eulerToQuaternions(double x, double y, double z, Quaternions* quaternions)
{
   double 
      halfX = x *0.5,
      halfY = y *0.5,
      halfZ = z *0.5,
      sinX = sin(halfX), 
      cosX = cos(halfX),
      sinY = sin(halfY),
      cosY = cos(halfY),
      sinZ = sin(halfZ),
      cosZ = cos(halfZ);

   quaternions->w = cosX*cosY*cosZ + sinX*sinY*sinZ;
   quaternions->x = sinX*cosY*cosZ - cosX*sinY*sinZ;
   quaternions->y = cosX*sinY*cosZ + sinX*cosY*sinZ;
   quaternions->z = cosX*cosY*sinZ - sinX*sinY*cosZ;

}

CVAPI(void) quaternionsToEuler(Quaternions* quaternions, double* x, double* y, double* z)
{
   double q0 = quaternions->w;
   double q1 = quaternions->x;
   double q2 = quaternions->y;
   double q3 = quaternions->z;

   *x = atan2(2.0 * (q0 * q1 + q2 * q3), 1.0 - 2.0 * (q1*q1 + q2*q2));
   *y = asin(2.0 * (q0 * q2 - q3 * q1));
   *z = atan2(2.0 * (q0 * q3 + q1 * q2), 1.0 - 2.0 * (q2*q2 + q3*q3));
}

CVAPI(void) quaternionsToRotationMatrix(Quaternions* quaternions, CvMat* rotation)
{
   double a = quaternions->w;
   double b = quaternions->x;
   double c = quaternions->y;
   double d = quaternions->z;

   cv::Mat r = cv::cvarrToMat(rotation);
   CV_Assert(r.rows == 3 && r.cols == 3);
   cv::MatIterator_<double> rIter = r.begin<double>();
   *rIter++ = a*a+b*b-c*c-d*d; *rIter++ = 2.0*b*c-2.0*a*d; *rIter++ = 2.0*b*d+2.0*a*c;
   *rIter++ = 2.0*b*c+2.0*a*d; *rIter++ = a*a-b*b+c*c-d*d; *rIter++ = 2.0*c*d-2.0*a*b;
   *rIter++ = 2.0*b*d-2.0*a*c; *rIter++ = 2.0*c*d+2.0*a*b; *rIter++ = a*a-b*b-c*c+d*d;
}

void quaternionsRotate(double w, double x, double y, double z, double v1, double v2, double v3, double*vr)
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

CVAPI(void) quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point, CvPoint3D64f* pointDst)
{
   quaternionsRotate(quaternions->w, quaternions->x, quaternions->y, quaternions->z, point->x, point->y, point->z, (double*) pointDst);
}

CVAPI(void) quaternionsRotatePoints(Quaternions* quaternions,  CvMat* pointSrc, CvMat* pointDst)
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
      v = pIter.ptr;
      quaternionsRotate(quaternions->w, quaternions->x, quaternions->y, quaternions->z, *v, *(v+1), *(v+2), pDstIter.ptr);
   } else 
   {
      for(int i = 0; i < p.rows; i++, pIter+=3, pDstIter+=3)
      {
         v = pIter.ptr;
         quaternionsRotate(quaternions->w, quaternions->x, quaternions->y, quaternions->z, *v, *(v+1), *(v+2),  pDstIter.ptr);
      }
   }
}

CVAPI(void) quaternionsMultiply(Quaternions* quaternions1, Quaternions* quaternions2, Quaternions* quaternionsDst)
{
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
}


