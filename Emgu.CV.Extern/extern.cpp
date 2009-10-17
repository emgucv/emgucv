#include "extern.h"

void CvAngleToRotationMatrix3D(double Rx, double Ry, double Rz, CvMat* rotMat)
{
   cv::Mat mat = cv::cvarrToMat(rotMat);
   assert(mat.rows == 3 && mat.cols == 3);

   double sinRx = sin(Rx), cosRx = cos(Rx);
   double sinRy = sin(Ry), cosRy = cos(Ry);
   double sinRz = sin(Rz), cosRz = cos(Rz);

   double* row = mat.ptr<double>(0);
   *row++ = cosRy*cosRz;
   *row++ = -sinRz*cosRy;
   *row = sinRy;
   
   row = mat.ptr<double>(1);
   *row++ = sinRx*sinRy*cosRz + sinRz*cosRx;
   *row++ = -sinRx*sinRy*sinRz + cosRx*cosRz;
   *row = -sinRx*cosRy;

   row = mat.ptr<double>(2);
   *row++ = -sinRy*cosRx*cosRz + sinRx*sinRz;
   *row++ = sinRy*sinRz*cosRx + sinRx*cosRz;
   *row = cosRx*cosRy;
}