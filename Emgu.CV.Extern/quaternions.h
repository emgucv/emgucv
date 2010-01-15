#include "cxcore.h"

typedef struct Quaternions
{
   double w;
   double x;
   double y;
   double z;
} Quaternions;

/* convert eluer angle (in radian) to quaternions */
CVAPI(void) eulerToQuaternions(double x, double y, double z, Quaternions* quaternions);

/* convert quaternions to eluer angle (in radian)  */
CVAPI(void) quaternionsToEuler(Quaternions* quaternions, double* x, double* y, double* z);

/* convert quaternions to (3x3) rotation matrix  */
CVAPI(void) quaternionsToRotationMatrix(Quaternions* quaternions, CvMat* rotation);

/* rotate the (3x1 or nx3) matrix of points using the quaternions */
CVAPI(void) quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point, CvPoint3D64f* pointDst);

/* rotate the (3x1 or nx3) matrix of points using the quaternions */
CVAPI(void) quaternionsRotatePoints(Quaternions* quaternions, CvMat* pointSrc, CvMat* pointDst);

/* quaternionsDst = quaternions1 * quaternions2 */
CVAPI(void) quaternionsMultiply(Quaternions* quaternions1, Quaternions* quaternions2, Quaternions* quaternionsDst);