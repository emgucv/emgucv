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

/* convert axis angle vector to quaternions. (x,y,z) is the rotatation axis and |(x,y,z)| is the rotation angle  */
CVAPI(void) axisAngleToQuaternions(CvPoint3D64f* axisAngle, Quaternions* quaternions);

/* convert quaternions to axis angle vector. (x,y,z) is the rotatation matrix and |(x,y,z)| is the rotation angle  */
CVAPI(void) quaternionsToAxisAngle(Quaternions* quaternions, CvPoint3D64f* axisAngle);

/* convert quaternions to (3x3) rotation matrix  */
CVAPI(void) quaternionsToRotationMatrix(Quaternions* quaternions, CvMat* rotation);

/* rotate a single point using the quaternions */
CVAPI(void) quaternionsRotatePoint(Quaternions* quaternions, CvPoint3D64f* point, CvPoint3D64f* pointDst);

/* rotate the (3x1 or nx3) matrix of points using the quaternions */
CVAPI(void) quaternionsRotatePoints(Quaternions* quaternions, CvMat* pointSrc, CvMat* pointDst);

/* quaternionsDst = quaternions1 * quaternions2 */
CVAPI(void) quaternionsMultiply(Quaternions* quaternions1, Quaternions* quaternions2, Quaternions* quaternionsDst);

/* renormalize the quaternions */ 
CVAPI(void) quaternionsRenorm(Quaternions* quaternions);